using UnityEngine;
using UnityEngine.UI;
using BestHTTP;
using BestHTTP.WebSocket;
using System.Collections;
public class Http2Example : MonoBehaviour
{
    public Transform panelContainer;
    public GameObject panelPrefab;

    private WebSocket webSocket;

    void Start()
    {
        ConnectWebSocket();
        RefreshData();
    }

    void ConnectWebSocket()
    {
        webSocket = new WebSocket(new System.Uri("ws://localhost:5000"));
        webSocket.OnMessage += OnWebSocketMessage;
        webSocket.Open();
    }

    void OnWebSocketMessage(WebSocket ws, string message)
    {
        // Handle WebSocket message
        Debug.Log("Received message from server: " + message);

        // Parse the message and update Unity panels accordingly
        Data newData = JsonUtility.FromJson<Data>(message);
        InstantiatePanel(newData);
    }

    public void RefreshData()
    {
        // Fetch and display initial data from the server
        StartCoroutine(FetchAndDisplayData("http://localhost:5000/allData"));
    }

    IEnumerator FetchAndDisplayData(string url)
    {
        Debug.Log("Fetching data from: " + url);
        HTTPRequest request = new HTTPRequest(new System.Uri(url), HTTPMethods.Get, (originalRequest, response) =>
        {
            if (response.IsSuccess)
            {
                string responseData = response.DataAsText;
                Data[] newData = JsonHelper.FromJson<Data>(responseData);

                // Clear existing panels
                foreach (Transform child in panelContainer)
                {
                    Destroy(child.gameObject);
                }

                // Instantiate panels based on the fetched data
                foreach (var item in newData)
                {
                    InstantiatePanel(item);
                }
            }
            else
            {
                Debug.LogError("HTTP request failed: " + response.Message);
            }
        });

        request.Send();

        yield return null;
    }

    void InstantiatePanel(Data data)
    {
        // Instantiate a new panel
        GameObject panel = Instantiate(panelPrefab, panelContainer);

        // Set the text values of the instantiated panel
        panel.transform.Find("Name").GetComponent<Text>().text = "Name: " + data.name;
        panel.transform.Find("Description").GetComponent<Text>().text = "Description: " + data.description;
        panel.transform.Find("Link").GetComponent<Text>().text = "Link: " + data.url;
    }

    // Ensure that the WebSocket is closed when the application quits
    private void OnApplicationQuit()
    {
        if (webSocket != null && webSocket.IsOpen)
        {
            webSocket.Close();
        }
    }
    [System.Serializable]
    public class Data
    {
        public int id; // Unique identifier for each data entry
        public string name;
        public string description;
        public string url;
    }
    [System.Serializable]
    public class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }

}

