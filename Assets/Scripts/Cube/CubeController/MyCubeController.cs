using UnityEngine;
using UnityEngine.UI;

public class MyCubeController : MonoBehaviour
{
    [SerializeField]
    private Text userNameText;

    private const float movementStep = 0.04f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 oldPosition = transform.localPosition;

        if (Input.GetKey(KeyCode.W))
        {
            transform.localPosition = new Vector3(oldPosition.x, oldPosition.y, oldPosition.z + movementStep);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.localPosition = new Vector3(oldPosition.x - movementStep, oldPosition.y, oldPosition.z);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.localPosition = new Vector3(oldPosition.x, oldPosition.y, oldPosition.z - movementStep);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.localPosition = new Vector3(oldPosition.x + movementStep, oldPosition.y, oldPosition.z);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.localPosition = new Vector3(oldPosition.x, oldPosition.y + movementStep, oldPosition.z);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.localPosition = new Vector3(oldPosition.x, oldPosition.y - movementStep, oldPosition.z);
        }
    }

    public void SetUsernameText(string userName)
    {
        userNameText.text = userName;
    }
}
