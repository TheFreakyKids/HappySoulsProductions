using UnityEngine;
using System.Text.RegularExpressions;

public partial class CameraController : MonoBehaviour
{
    public Camera playerCam;
    public float verticalRotationP1;
    public float verticalRotationP2;
    public float horizontalRotation;
    public float updownrange = 80f;
    public string parentName;
    public Transform parentTransform;

    public int playerNum;

    void Awake()
    {
        playerCam = this.GetComponent<Camera>();
        parentName = this.transform.parent.transform.parent.transform.parent.name;
        parentTransform = this.transform.parent.transform.parent.transform.parent;

        string numberOnly = Regex.Replace(parentName, "[^0-9]", "");
        playerNum = int.Parse(numberOnly);
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
        //if (parentName == "Player1")
        //{
        //    MouseLook1();
        //}
        //else if (parentName == "Player2")
        //{
        //    MouseLook2();
        //}
        NewMouseLook();
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

    private void NewMouseLook()
    {
        if (Input.GetAxis("RSVert" + playerNum) > 0.1 || Input.GetAxis("RSVert" + playerNum) < 0.1)
        {
            verticalRotationP1 -= Input.GetAxis("RSVert" + playerNum) * 2.4f;
            verticalRotationP1 = Mathf.Clamp(verticalRotationP1, -updownrange, updownrange);
        }
        if (Input.GetAxis("RSHor" + playerNum) > 0.1 || Input.GetAxis("RSHor" + playerNum) < 0.1)
        {
            horizontalRotation += Input.GetAxis("RSHor" + playerNum);
        }

        playerCam.transform.localRotation = Quaternion.Euler(verticalRotationP1, 0, 0);
        parentTransform.rotation = Quaternion.Euler(0, horizontalRotation * 4f, 0);
    }
    
	private void MouseLook1()
    {
        if (Input.GetAxis("RSVert1") > 0.1 || Input.GetAxis("RSVert1") < 0.1)
        {
            verticalRotationP1 -= Input.GetAxis("RSVert1") * 2.4f;
            verticalRotationP1 = Mathf.Clamp(verticalRotationP1, -updownrange, updownrange);
        }
        if (Input.GetAxis("RSHor1") > 0.1 || Input.GetAxis("RSHor1") < 0.1)
        {
            horizontalRotation += Input.GetAxis("RSHor1");
        }
        
        playerCam.transform.localRotation = Quaternion.Euler(verticalRotationP1, 0, 0);
        parentTransform.rotation = Quaternion.Euler(0, horizontalRotation * 4f, 0);
    }

    private void MouseLook2()
    {
        if (Input.GetAxis("RSVert2") > 0.1 || Input.GetAxis("RSVert2") < 0.1)
        {
            verticalRotationP2 -= Input.GetAxis("RSVert2");
            verticalRotationP2 = Mathf.Clamp(verticalRotationP2, -updownrange, updownrange);
        }
        if (Input.GetAxis("RSHor2") > 0.1 || Input.GetAxis("RSHor2") < 0.1)
        {
            horizontalRotation += Input.GetAxis("RSHor2");
        }
        playerCam.transform.localRotation = Quaternion.Euler(verticalRotationP2, 0, 0);
        this.transform.parent.transform.localRotation = Quaternion.Euler(0, horizontalRotation * 1.5f, 0);
    }
}