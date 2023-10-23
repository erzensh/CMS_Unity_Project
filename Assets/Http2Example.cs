using UnityEngine;
using BestHTTP;
using BestHTTP.Connections;
using BestHTTP.Connections.HTTP2;

public class Http2Example : MonoBehaviour
{
    public string ServerURL = "https://localhost:3000/";

    void Start()
    {
        // Create a new HTTPRequest and set its properties
        HTTPRequest request = new HTTPRequest(new System.Uri(ServerURL), HTTPMethods.Get, (originalRequest, response) =>
        {
            if (response.IsSuccess)
            {
                Debug.Log($"HTTP/2 Response: {response.DataAsText}");
            }
            else
            {
                Debug.LogError($"HTTP/2 Error: {response.Message}");
            }
        });

        // Configure the request to use HTTP/2
        HTTP2Settings settings = new HTTP2Settings();
        request.ProtocolHandler = new HTTP2Handler(request, settings);

        // Send the request
        request.Send();
    }
}
