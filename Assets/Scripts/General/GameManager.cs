using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField]
    private bool isLocal;

    [SerializeField]
    private string userName = "user1";

    [Header("References")]
    public GameObject myCubeObj;

    private const string localIP = "127.0.0.1";
    private const string remoteIP = "34.85.45.170";

    private const int serverHttpPort = 8080;
    private const int serverUdpPort = 3000;
    private const int clientUdpPort = 30000;

    private HTTPCommunicationManager httpCommunicationManager;
    private SocketCommunicationManager socketCommunicationManager;

    private string myUuid;
    private string token;

    private ConnectionStatus connectionStatus = new ConnectionStatus(false, false);

    // Start is called before the first frame update
    void Start()
    {
        httpCommunicationManager = new HTTPCommunicationManager(isLocal ? $"http://{localIP}:{serverHttpPort}" : $"http://{remoteIP}:{serverHttpPort}");
        socketCommunicationManager = new SocketCommunicationManager((isLocal ? localIP : remoteIP), serverUdpPort, clientUdpPort);

        myCubeObj.GetComponent<MyCubeController>().SetUsernameText(userName);
    }

    void Update()
    {
        if (!connectionStatus.isInCommunicationProcess && Input.GetKeyUp(KeyCode.Space))
        {
            SetConnectionStatus(new ConnectionStatus(true, connectionStatus.isConnected));
            if (connectionStatus.isConnected)
            {
                // connection/end‚ð’@‚­
                StartCoroutine(httpCommunicationManager.EndConnection(myUuid, token, SetConnectionStatus));
                Debug.Log("end");
            }
            else
            {
                // connection/start‚ð’@‚­
                StartCoroutine(httpCommunicationManager.StartConnection(userName, SetMyUuid, SetToken, SetConnectionStatus));
                Debug.Log("start");
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (connectionStatus.isConnected)
        {
            socketCommunicationManager.Send(myUuid, myCubeObj.transform.localPosition);
            
            if (!socketCommunicationManager.IsListening)
            {
                socketCommunicationManager.Listen();
            }
        }
    }

    private void SetMyUuid(string uuid)
    {
        myUuid = uuid;
        OthersCubesManager.AddExclusiveUuid(myUuid);
    }

    private void SetToken(string token)
    {
        this.token = token;
    }

    private void SetConnectionStatus(ConnectionStatus connectionStatus)
    {
        this.connectionStatus = connectionStatus;
    }
}
