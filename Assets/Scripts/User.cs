using System;
using UnityEngine;

public class User
{
    private string username;
    private Vector3 position;
    private DateTime updatedAt;

    public User (string username, Vector3 position)
    {
        this.username = username;
        this.position = position.Clone();
    }
}
