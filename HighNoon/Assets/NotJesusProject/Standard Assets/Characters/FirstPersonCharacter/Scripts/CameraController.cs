using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CameraController : MonoBehaviour {
    public GameObject playerCamera;

	void Start()
    {
        // Start with cursor locked
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
		// Check mouse lock status
		MouseLock();
        MouseLook();
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
    
	private void MouseLook()
    {
		float mouseInputX = Input.GetAxis("Mouse X");
		transform.Rotate(new Vector3(0, mouseInputX * 2, 0));
        float mouseInputY = Input.GetAxis("Mouse Y");
        playerCamera.transform.Rotate(new Vector3(mouseInputY * -1, 0, 0));
        if(playerCamera.transform.rotation.x >= 80)
        {
            playerCamera.transform.rotation = Quaternion.Euler(80, 0, 0);
        }
        else if (playerCamera.transform.rotation.x <= -80)
        {
            playerCamera.transform.rotation = Quaternion.Euler(-80, 0, 0);
        }
    }
}