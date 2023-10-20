using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NodeJsCommunication : MonoBehaviour
{
    private const string serverUrl = "https://localhost:3000"; // Use the correct URL

    void Start()
    {
        StartCoroutine(SendDataToServer());
    }

    IEnumerator SendDataToServer()
    {
        // Ensure your JSON data is well-formed
        string jsonData = "{\"playerName\":\"John\",\"score\":100}";

        // Create a UnityWebRequest to send a POST request
        using (UnityWebRequest www = UnityWebRequest.Post($"{serverUrl}", jsonData))
        {
            www.SetRequestHeader("Content-Type", "application/json");

            // Send the request and wait for a response
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Server response: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + www.error);
            }
        }
    }
}
