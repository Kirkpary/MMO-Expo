using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject playerCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            playerCamera.transform.Rotate(0, 0, 0.2f, Space.Self);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerCamera.transform.Rotate(0, 0, -0.2f, Space.Self);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            playerCamera.transform.Rotate(.2f, 0, 0, Space.Self);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            playerCamera.transform.Rotate(-.2f, 0, 0, Space.Self);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            playerCamera.transform.Translate(0, 0, .2f, Space.Self);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            playerCamera.transform.Translate(0, 0, -.2f, Space.Self);
        }
    }
}
