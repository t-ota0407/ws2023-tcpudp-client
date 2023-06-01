using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.Networking;

public class HTTPCommunicationManager
{
    private string baseURL;

    public HTTPCommunicationManager(string baseURL)
    {
        this.baseURL = baseURL;
    }

    private struct StartCommunicationData
    {
        public string username;
    }

    public IEnumerator StartConnection(string username, Action<string> guidSetter, Action<string> tokenSetter, Action<ConnectionStatus> statusSetter)
    {
        StartConnectionRequestBody body = new StartConnectionRequestBody(username);
        string bodyJson = JsonUtility.ToJson(body);
        byte[] postData = Encoding.UTF8.GetBytes(bodyJson);
        Debug.Log(bodyJson);

        string uri = baseURL + "/connection/start";

        var request = new UnityWebRequest(uri, "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseJson = request.downloadHandler.text;
            Debug.Log(responseJson);
            StartConnectionResponseBody responseBody = JsonUtility.FromJson<StartConnectionResponseBody>(responseJson);

            Debug.Log(responseBody.uuid);
            Debug.Log(responseBody.token);
            guidSetter(responseBody.uuid);
            tokenSetter(responseBody.token);
            statusSetter(new ConnectionStatus(false, true));
        }
        else
        {
            Debug.LogError("POST failed: " + request.error);
            statusSetter(new ConnectionStatus(false, false));
        }
    }

    public IEnumerator EndConnection(string myUuid, string token, Action<ConnectionStatus> statusSetter)
    {
        EndConnectionRequestBody body = new EndConnectionRequestBody(myUuid);
        string bodyJson = JsonUtility.ToJson(body);
        byte[] postData = Encoding.UTF8.GetBytes(bodyJson);
        Debug.Log($"body : {bodyJson}");

        string uri = baseURL + "/connection/end";

        var request = new UnityWebRequest(uri, "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + token);

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            // パースしたデータを使用して必要な処理を行う
            Debug.Log("success");

            statusSetter(new ConnectionStatus(false, false));
        }
        else
        {
            // POSTが失敗した場合の処理
            Debug.LogError("POST failed: " + request.error);

            statusSetter(new ConnectionStatus(false, true));
        }
    }
}
