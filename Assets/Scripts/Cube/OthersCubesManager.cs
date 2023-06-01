using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OthersCubesManager : MonoBehaviour
{
    private static List<(string uuid, GameObject gameObject)> uuidOthersCubePairs = new List<(string uuid, GameObject gameObject)>();

    private static List<ResponseActiveUser> responseActiveUsers;
    private static bool isResponseActiveUsersDirty = false;

    private static List<string> exclusiveUuid = new List<string>();

    void FixedUpdate()
    {
        if (isResponseActiveUsersDirty)
        {
            foreach (ResponseActiveUser responseActiveUser in responseActiveUsers)
            {
                string uuid = responseActiveUser.uuid;

                if (!exclusiveUuid.Contains(uuid))
                {
                    string userName = responseActiveUser.userName;
                    Vector3 position = new Vector3(responseActiveUser.positionX, responseActiveUser.positionY, responseActiveUser.positionZ);
                    UpdateCubePosition(uuid, userName, position);
                }

            }
            isResponseActiveUsersDirty = false;
        }
    }

    public static void SetResponseActiveUsers(List<ResponseActiveUser> responseActiveUsers)
    {
        OthersCubesManager.responseActiveUsers = responseActiveUsers;
        isResponseActiveUsersDirty = true;
    }

    public static void AddExclusiveUuid(string uuid)
    {
        if (!exclusiveUuid.Contains(uuid))
        {
            exclusiveUuid.Add(uuid);
        }
    }

    private void UpdateCubePosition(string uuid, string userName, Vector3 position)
    {
        var pair = uuidOthersCubePairs.Find((othersCube) => othersCube.uuid == uuid);
        Debug.Log(pair.uuid);

        if (pair.uuid == null)
        {
            GameObject othersCube = (GameObject)Resources.Load("Prefabs/OthersCube");
            othersCube.name = $"others_cube_{uuid}";
            othersCube = Instantiate(othersCube, this.transform);

            othersCube.transform.localPosition = position;

            othersCube.GetComponent<OthersCubeController>().SetUserNameText(userName);

            uuidOthersCubePairs.Add((uuid, othersCube));
        }
        else
        {
            pair.gameObject.transform.localPosition = position;
        }
    }
}
