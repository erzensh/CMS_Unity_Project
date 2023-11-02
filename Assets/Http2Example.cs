using System;
using UnityEngine;
using UnityEngine.UI;
using BestHTTP;
using BestHTTP.JSON;

public class Http2Example : MonoBehaviour
{
    public GameObject rowPrefab; // Reference to your row prefab in the Unity Editor
    public Button refreshButton; // Reference to your refresh button in the Unity Editor

    [Serializable]
    public class Data
    {
        public int id;
        public string name;
        public string description;
        public string url;
    }

    void Start()
    {
        // Attach the RefreshData method to the button's click event
        if (refreshButton != null)
        {
            refreshButton.onClick.AddListener(RefreshData);
        }

        string apiUrl = "http://localhost:5000/allData";
        FetchAndDisplayData(apiUrl);
    }

    void FetchAndDisplayData(string url)
    {
        HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Get, (originalRequest, response) =>
        {
            if (response.IsSuccess)
            {
                string responseData = response.DataAsText;

                // Parse the JSON array into an array of Data objects
                Data[] dataArray = JsonHelper.FromJson<Data>(responseData);

                // Display the data
                DisplayData(dataArray);
            }
            else
            {
                Debug.LogError("HTTP request failed: " + response.Message);
            }
        });

        // Send the request
        request.Send();
    }

    void DisplayData(Data[] dataArray)
    {
        // Clear previous data
        ClearRows();

        // Set the initial position
        Vector3 currentPosition = Vector3.zero;

        // Set the spacing between panels
        float panelSpacing = 10f;

        // Display the data in your UI
        foreach (Data data in dataArray)
        {
            // Instantiate a new prefab for each data entry
            GameObject newRow = Instantiate(rowPrefab, transform);

            // Set the position of the new row
            newRow.transform.localPosition = currentPosition;

            // Attach a script to the new row to handle clicks
            newRow.AddComponent<RowClickHandler>().Initialize(data);

            // Adjust the current position for the next iteration
            currentPosition.x += newRow.GetComponent<RectTransform>().rect.width + panelSpacing;

            Text rowText = newRow.GetComponentInChildren<Text>();

            if (rowText != null)
            {
                rowText.text = $"Name: {data.name}\nDescription: {data.description}\nURL: {data.url}";
            }
            else
            {
                Debug.LogError("Text component not found in the prefab!");
            }
        }
    }

    void ClearRows()
    {
        // Destroy all children (rows) under this GameObject
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void RefreshData()
    {
        string apiUrl = "http://localhost:5000/allData";
        FetchAndDisplayData(apiUrl);
    }

    // Script to handle row clicks
    public class RowClickHandler : MonoBehaviour
    {
        private Data rowData;

        public void Initialize(Data data)
        {
            rowData = data;
        }

        public void OnRowClick()
        {
            if (rowData != null)
            {
                // Trigger download with the URL from the clicked row
                DownloadData(rowData.url);
            }
        }

        public void DownloadData(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                HTTPRequest request = new HTTPRequest(new Uri(url), (originalRequest, response) =>
                {
                    if (response.IsSuccess)
                    {
                        Debug.Log("Downloaded: " + response.DataAsText);
                    }
                    else
                    {
                        Debug.LogError("Download failed: " + response.Message);
                    }
                });

                // Send the request
                request.Send();
            }
            else
            {
                Debug.LogError("URL is empty. Please enter a valid URL.");
            }
        }
    }

    [Serializable]
    public class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string wrapperJson = "{\"Items\":" + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(wrapperJson);
            return wrapper.Items;
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}
