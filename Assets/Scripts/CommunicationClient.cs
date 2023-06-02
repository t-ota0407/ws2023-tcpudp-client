using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class CommunicationClient : MonoBehaviour
{
    private TcpClient tcpClient;
    private UdpClient udpClient;
    private NetworkStream networkStream;
    private bool isConnected;

    public bool sendFlag = false;
    public bool isLocal = true;

    void Awake()
    {
        try
        {
            //string url = "ws2023.an.r.appspot.com";
            //IPAddress[] addresses = Dns.GetHostAddresses(url);
            //string ipAddress = addresses[0].ToString();

            //Debug.Log(ipAddress);
            string ipAddress = "34.85.45.170";
            udpClient = new UdpClient(30000);
            udpClient.Connect(ipAddress, 3000);
            Debug.Log("�ڑ�����");
        }
        catch (SocketException e)
        {
            Debug.Log("�ڑ����s");
            Debug.Log(e.Message);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

       //IPEndPoint ep = new IPEndPoint(IPAddress.Any, 30000);
        
        udpClient.BeginReceive(new AsyncCallback(OnReceived), udpClient);
    }

    // Update is called once per frame
    void Update()
    {
        if (sendFlag)
        {
            //StartCoroutine(GET());
            StartCoroutine(POSTMessage());
            sendFlag = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Send");
            var message = System.Text.Encoding.UTF8.GetBytes("Hello World!");
            udpClient.Send(message, message.Length);
        }
    }

    private void OnReceived(System.IAsyncResult result)
    {
        UdpClient getUdp = (UdpClient)result.AsyncState;
        IPEndPoint ipEnd = null;

        byte[] getByte = getUdp.EndReceive(result, ref ipEnd);

        var message = Encoding.UTF8.GetString(getByte);
        // subject.OnNext(message);
        Debug.Log(message);

        getUdp.BeginReceive(OnReceived, getUdp);
    }

    IEnumerator POSTMessage()
    {
        User userobj = new User();
        userobj.id = "aaaa";
        string userJson = JsonUtility.ToJson(userobj);

        byte[] postData = Encoding.UTF8.GetBytes(userJson);
        string uri = (isLocal) ? "http://192.168.83.54:8080/connection/communication-start" : "http://34.85.45.170/connection/communication-start";
        var request = new UnityWebRequest(uri, "POST");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            // POST�����������ꍇ�̏���
            Debug.Log("POST succeeded!");

            // ���X�|���X�f�[�^���󂯎��A�p�[�X����
            string responseJson = request.downloadHandler.text;
            //MyResponse responseData = JsonUtility.FromJson<MyResponse>(responseJson);

            // �p�[�X�����f�[�^���g�p���ĕK�v�ȏ������s��
            Debug.Log("Response: " + responseJson);
        }
        else
        {
            // POST�����s�����ꍇ�̏���
            Debug.LogError("POST failed: " + request.error);
        }
    }

    private class User
    {
        public string id; 
    }

    private struct UdpState
    {
        public UdpClient u;
        public IPEndPoint e;
    }
}
