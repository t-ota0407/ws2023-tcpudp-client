using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OthersCubeController : MonoBehaviour
{
    [SerializeField]
    private Text userNameText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUserNameText(string userName)
    {
        userNameText.text = userName;
    }
}
