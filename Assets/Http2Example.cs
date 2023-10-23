using System;
using BestHTTP;
using BestHTTP.Connections;
using BestHTTP.Connections.HTTP2;
using BestHTTP.Core;
using UnityEngine;

public sealed class Http2Example : MonoBehaviour
{
    public string ServerURL;

    private Uri serverUri;
    private string key = null;
    private double lastPrintedLatency = 0;

    private void Start()
    {
        serverUri = new Uri(ServerURL);

        // Log that the script has started
        Debug.Log("Http2Example script started.");
    }

    private void Update()
    {
        // Cache the Server+Proxy unique key. It expects that the global Proxy isn't changing.
        if (string.IsNullOrEmpty(key))
            key = HostDefinition.GetKeyFor(serverUri, HTTPManager.Proxy);

        // For the given Server+Proxy combination, find an HTTPConnection that has an HTTP2Handler
        var httpConnection = HostManager.GetHost(serverUri.Host)
            .GetHostDefinition(key)
            .Find(con => con is HTTPConnection http && http.requestHandler is HTTP2Handler) as HTTPConnection;

        // No connection yet
        if (httpConnection == null)
        {
            Debug.Log("No HTTP2 connection yet.");
            return;
        }

        // Get the HTTP2Handler and print latency. If Latency is zero, no ping ack received from the server yet.
        if (httpConnection.requestHandler is HTTP2Handler http2handler &&
            http2handler.Latency > 0 &&
            lastPrintedLatency != http2handler.Latency)
        {
            lastPrintedLatency = http2handler.Latency;
            Debug.Log($"HTTP2 Latency: {lastPrintedLatency}");
        }
    }
}
