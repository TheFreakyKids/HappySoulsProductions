using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CameraController : MonoBehaviour {
    public Camera playerCam;
    public float verticalRotation;
    public float horizontalRotation;
    public float updownrange = 80f;
    private string parentName;
    void Awake()
    {
        playerCam = this.GetComponent<Camera>();
        parentName = this.transform.parent.name;
    }
	void Start()
    {
        // Start with cursor locked
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
		// Check mouse lock status
		MouseLock();
        if (parentName == "Player1")
        {
            Debug.Log(parentName);
            MouseLook1();
        }
        else if (parentName == "Player2")
        {
            Debug.Log(parentName);
            MouseLook2();
        }

    }

	private void MouseLock()
    {
		// Release mouse on escape key
		if (Input.GetKey(KeyCode.Escape)) {
			Cursor.lockState = CursorLockMode.None;
		}

		// Lock mouse again on left click
		if (Input.GetMouseButtonDown(0)) {
			Cursor.lockState = CursorLockMode.Locked;
		}
	}
    
	private void MouseLook1()
    {
        #region Crap
        /*float mouseInputX = Input.GetAxis("Right Stick Horizontal");
		transform.Rotate(new Vector3(0, mouseInputX * 2, 0));
        float mouseInputY = Input.GetAxis("Right Stick Vertical");
        playerCamera.transform.Rotate(new Vector3(mouseInputY * -1, 0, 0));
        if(playerCamera.transform.localEulerAngles.x >= 80)
        {
            Debug.Log("more than 80");
            playerCamera.transform.rotation = Quaternion.Euler(79, 0, 0);
        }
        else if (playerCamera.transform.localEulerAngles.x <= -80)
        {
            playerCamera.transform.rotation = Quaternion.Euler(-80, 0, 0);
        }*/
        /*Vector3 rot = transform.eulerAngles;
        float mouseY = Input.GetAxis("Mouse Y");
        rot.x = Mathf.Clamp(rot.x + mouseY, 0, 80);
        transform.eulerAngles = rot;*/
        #endregion
        if (Input.GetAxis("Right Stick Vertical") > 0.1 || Input.GetAxis("Right Stick Vertical") < 0.1)
        {
            verticalRotation -= Input.GetAxis("Right Stick Vertical");
            verticalRotation = Mathf.Clamp(verticalRotation, -updownrange, updownrange);
            Debug.Log(parentName + " is using vertical rotation");
        }
        if (Input.GetAxis("Right Stick Horizontal") > 0.1 || Input.GetAxis("Right Stick Horizontal") < 0.1)
        {
            horizontalRotation += Input.GetAxis("Right Stick Horizontal");
        }
        
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        this.transform.parent.transform.localRotation = Quaternion.Euler(0, horizontalRotation * 1.5f, 0);
    }
    private void MouseLook2()
    {
        #region Crap
        /*float mouseInputX = Input.GetAxis("Right Stick Horizontal");
		transform.Rotate(new Vector3(0, mouseInputX * 2, 0));
        float mouseInputY = Input.GetAxis("Right Stick Vertical");
        playerCamera.transform.Rotate(new Vector3(mouseInputY * -1, 0, 0));
        if(playerCamera.transform.localEulerAngles.x >= 80)
        {
            Debug.Log("more than 80");
            playerCamera.transform.rotation = Quaternion.Euler(79, 0, 0);
        }
        else if (playerCamera.transform.localEulerAngles.x <= -80)
        {
            playerCamera.transform.rotation = Quaternion.Euler(-80, 0, 0);
        }*/
        /*Vector3 rot = transform.eulerAngles;
        float mouseY = Input.GetAxis("Mouse Y");
        rot.x = Mathf.Clamp(rot.x + mouseY, 0, 80);
        transform.eulerAngles = rot;*/
        #endregion
        if (Input.GetAxis("P2VertLook") > 0.1 || Input.GetAxis("P2VertLook") < 0.1)
        {
            verticalRotation -= Input.GetAxis("P2VertLook");
            verticalRotation = Mathf.Clamp(verticalRotation, -updownrange, updownrange);
        }
        if (Input.GetAxis("P2HorizLook") > 0.1 || Input.GetAxis("P2HorizLook") < 0.1)
        {
            horizontalRotation += Input.GetAxis("P2HorizLook");
        }

        playerCam.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        this.transform.parent.transform.localRotation = Quaternion.Euler(0, horizontalRotation * 1.5f, 0);
    }
}