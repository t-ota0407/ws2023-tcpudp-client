using System;

[Serializable]
public class EndConnectionRequestBody
{
    public string uuid;
    public EndConnectionRequestBody(string uuid)
    {
        this.uuid = uuid.ToString();
    }
}

