using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPCommunicationManager
{
    private readonly string baseURL;

    public HTTPCommunicationManager(string baseURL)
    {
        this.baseURL = baseURL;
    }

    public IEnumerator StartConnection(string username, Action<string> uuidSetter, Action<string> tokenSetter, Action<ConnectionStatus> statusSetter)
    {
        StartConnectionRequestBody body = new StartConnectionRequestBody(username);
        string bodyJson = JsonUtility.ToJson(body);
        byte[] postData = Encoding.UTF8.GetBytes(bodyJson);

        string uri = baseURL + "/connection/start";

        var request = new UnityWebRequest(uri, "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseJson = request.downloadHandler.text;
            StartConnectionResponseBody responseBody = JsonUtility.FromJson<StartConnectionResponseBody>(responseJson);

            uuidSetter(responseBody.uuid);
            tokenSetter(responseBody.token);
            statusSetter(new ConnectionStatus(false, true));
        }
        else
        {
            Debug.LogError("HTTP POST error: " + request.error);
            statusSetter(new ConnectionStatus(false, false));
        }
    }

    public IEnumerator EndConnection(string myUuid, string token, Action<ConnectionStatus> statusSetter)
    {
        EndConnectionRequestBody body = new EndConnectionRequestBody(myUuid);
        string bodyJson = JsonUtility.ToJson(body);
        byte[] postData = Encoding.UTF8.GetBytes(bodyJson);

        string uri = baseURL + "/connection/end";

        var request = new UnityWebRequest(uri, "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + token);

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            statusSetter(new ConnectionStatus(false, false));
        }
        else
        {
            Debug.LogError("HTTP POST error: " + request.error);
            statusSetter(new ConnectionStatus(false, true));
        }
    }
}
