using UnityEngine;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;

public class Http2Example : MonoBehaviour
{
    async void Start()
    {
        await GetDataFromServer();
    }

    async Task GetDataFromServer()
    {
        string url = "http://localhost:3000"; // Change this URL to your server's endpoint

        using (HttpClient httpClient = new HttpClient())
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // Handle successful response
                string responseData = await response.Content.ReadAsStringAsync();
                Debug.Log("Received: " + responseData);
            }
            else
            {
                // Handle error
                Debug.LogError("Error: " + response.ReasonPhrase);
            }
        }
    }
}
