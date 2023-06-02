using System;
using UnityEngine;

[Serializable]
public class StartConnectionRequestBody
{
    public string username;
    public StartConnectionRequestBody(string username)
    {
        this.username = username;
    }
}
