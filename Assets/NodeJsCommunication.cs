using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NodeJsCommunication : MonoBehaviour
{
    IEnumerator Start()
    {
        Debug.Log("Sending HTTP request...");

        using (UnityWebRequest www = UnityWebRequest.Get("https://localhost:8443"))
        {
            www.certificateHandler = new BypassCertificate();

            // Add headers to handle potential CORS issues
            www.SetRequestHeader("Origin", "http://localhost:8443");


            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Request successful. Response: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Request failed. Error: " + www.error);
            }
        }
    }

    public class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            // Allow all certificates
            return true;
        }
    }
}
