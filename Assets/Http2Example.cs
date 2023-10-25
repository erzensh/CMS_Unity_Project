using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using BestHTTP;

public class Http2Example : MonoBehaviour
{
    public Text aName;
    public Text aDescription;
    public Text aTheLink;
    public Button downloadButton;

    void Start()
    {
        // Attach a click event to the download button
        downloadButton.onClick.AddListener(OnDownloadButtonClick);

        RefreshData();
    }

    public void RefreshData()
    {
        FetchAndDisplayData("http://localhost:5000/", aName);
        FetchAndDisplayData("http://localhost:5000/1", aDescription);
        FetchAndDisplayData("http://localhost:5000/2", aTheLink);
    }

    void FetchAndDisplayData(string url, Text targetText)
    {
        HTTPRequest request = new HTTPRequest(new System.Uri(url), HTTPMethods.Get, (originalRequest, response) =>
        {
            if (response.IsSuccess)
            {
                string responseData = response.DataAsText;
                targetText.text = responseData;
            }
            else
            {
                Debug.LogError("HTTP request failed: " + response.Message);
            }
        });
        request.Send();
    }

    public void OnDownloadButtonClick()
    {
        // Fetch the download link from the server
        string downloadLink = aTheLink.text;

        // Start the coroutine to download the file
        StartCoroutine(DownloadFileCoroutine(downloadLink));
    }

    IEnumerator DownloadFileCoroutine(string fileUrl)
    {
        HTTPRequest request = new HTTPRequest(new System.Uri(fileUrl), HTTPMethods.Get, (originalRequest, response) =>
        {
            if (response.IsSuccess)
            {
                // Save the downloaded file
                byte[] data = response.Data;
                string savePath = Application.persistentDataPath + "/downloadedFile";
                System.IO.File.WriteAllBytes(savePath, data);
                Debug.Log("File downloaded and saved to: " + savePath);
            }
            else
            {
                Debug.LogError("Download failed: " + response.Message);
            }
        });

        request.Send();

        yield return null; // You might want to add more sophisticated waiting logic
    }
}
