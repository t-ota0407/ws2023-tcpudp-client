public class ConnectionStatus
{
    public readonly bool isInCommunicationProcess;
    public readonly bool isConnected;

    public ConnectionStatus(bool isInCommunicationProcess, bool isConnected)
    {
        this.isInCommunicationProcess = isInCommunicationProcess;
        this.isConnected = isConnected;
    }
}
