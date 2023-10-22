using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Http2Example : MonoBehaviour
{
    // Replace this URL with your actual endpoint
    private const string RequestUrl = "https://localhost:3000";

    void Start()
    {
        StartCoroutine(MakeHttp2Request());
    }

    IEnumerator MakeHttp2Request()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(RequestUrl))
        {
            www.useHttpContinue = false; // Ensure HTTP/2 is used

            // Send the request and wait for a response
            yield return www.SendWebRequest();

            // Check for errors
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError($"HTTP request error: {www.error}");
            }
            else
            {
                // Log the response text
                Debug.Log($"HTTP request successful. Response: {www.downloadHandler.text}");
            }
        }
    }
}
