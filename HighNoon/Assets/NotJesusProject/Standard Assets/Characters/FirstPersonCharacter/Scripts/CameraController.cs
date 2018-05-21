using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CameraController : MonoBehaviour {
    public Camera playerCam;
    public float verticalRotationP1;
    public float verticalRotationP2;
    public float horizontalRotation;
    public float updownrange = 80f;
    public string parentName;
    public Transform parentTransform;
    void Awake()
    {
        playerCam = this.GetComponent<Camera>();
        parentName = this.transform.parent.transform.parent.transform.parent.name;
        parentTransform = this.transform.parent.transform.parent.transform.parent;
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
            MouseLook1();
        }
        else if (parentName == "Player2")
        {
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
        if (Input.GetAxis("Right Stick Vertical") > 0.1 || Input.GetAxis("Right Stick Vertical") < 0.1)
        {
            verticalRotationP1 -= Input.GetAxis("Right Stick Vertical") * 2.4f;
            verticalRotationP1 = Mathf.Clamp(verticalRotationP1, -updownrange, updownrange);
        }
        if (Input.GetAxis("Right Stick Horizontal") > 0.1 || Input.GetAxis("Right Stick Horizontal") < 0.1)
        {
            horizontalRotation += Input.GetAxis("Right Stick Horizontal");
        }
        
        playerCam.transform.localRotation = Quaternion.Euler(verticalRotationP1, 0, 0);
        parentTransform.rotation = Quaternion.Euler(0, horizontalRotation * 4f, 0);
    }
    private void MouseLook2()
    {
        if (Input.GetAxis("P2VertLook") > 0.1 || Input.GetAxis("P2VertLook") < 0.1)
        {
            verticalRotationP2 -= Input.GetAxis("P2VertLook");
            verticalRotationP2 = Mathf.Clamp(verticalRotationP2, -updownrange, updownrange);
        }
        if (Input.GetAxis("P2HorizLook") > 0.1 || Input.GetAxis("P2HorizLook") < 0.1)
        {
            horizontalRotation += Input.GetAxis("P2HorizLook");
        }

        playerCam.transform.localRotation = Quaternion.Euler(verticalRotationP2, 0, 0);
        this.transform.parent.transform.localRotation = Quaternion.Euler(0, horizontalRotation * 1.5f, 0);
    }
}