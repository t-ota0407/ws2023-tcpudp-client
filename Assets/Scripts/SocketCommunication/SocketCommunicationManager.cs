using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

public class SocketCommunicationManager
{
    private UdpClient udpClient;

    private bool isListening = false;
    public bool IsListening
    {
        get { return isListening; }
    }

    public SocketCommunicationManager(string serverIPAddress, int serverPort, int clientPort)
    {
        try
        {
            udpClient = new UdpClient(clientPort);
            udpClient.Connect(serverIPAddress, serverPort);
            Debug.Log("�ڑ�����");
        }
        catch (SocketException e)
        {
            Debug.Log("�ڑ����s");
            Debug.Log(e.Message);
        }
    }

    public void Send(string uuid,Vector3 position)
    {
        var message = Encoding.UTF8.GetBytes($"{{\"uuid\": \"{uuid}\", \"x\": \"{position.x}\", \"y\": \"{position.y}\", \"z\": \"{position.z}\"}}");
        udpClient.Send(message, message.Length);
    }

    public void Listen()
    {
        udpClient.BeginReceive(new AsyncCallback(OnReceived), udpClient);
        isListening = true;
    }

    private void OnReceived(IAsyncResult result)
    {
        UdpClient getUdp = (UdpClient)result.AsyncState;
        IPEndPoint ipEnd = null;

        byte[] getByte = getUdp.EndReceive(result, ref ipEnd);

        string message = Encoding.UTF8.GetString(getByte);
        Debug.Log(message);
        try
        {
            List<ResponseActiveUser> responseActiveUsers = JsonConvert.DeserializeObject<List<ResponseActiveUser>>(message);

            OthersCubesManager.SetResponseActiveUsers(responseActiveUsers);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log("parse error");
        }


        getUdp.BeginReceive(OnReceived, getUdp);
    }
}
