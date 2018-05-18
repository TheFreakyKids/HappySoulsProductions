using UnityEngine;
using System.Collections;

public class AimScript : MonoBehaviour
{
    public string parent;
	float mouseX;
	float mouseY;
    
	Quaternion rotationSpeed;
    #region Headers
    [Header("Gun Options")]
	//How fast the gun moves on the x and y
	//axis when aiming
	public float aimSpeed = 6.5f;
	//How fast the gun moves to the new position
	public float moveSpeed = 15.0f;

	[Header("Gun Positions")]
	//Default gun position
	public Vector3 defaultPosition;
	//Aim down the sight position
	public Vector3 zoomPosition;

	[Header("Camera")]
	//Main gun camera
	public Camera gunCamera;

	[Header("Camera Options")]
	//How fast the camera field of view changes
	public float fovSpeed = 15.0f;
	//Camera FOV when zoomed in
	public float zoomFov = 30.0f;
	//Default camera FOV
	public float defaultFov = 60.0f;

	[Header("Audio")]
	public AudioSource aimSound;
	//Used to check if the audio has played
	bool soundHasPlayed = false;
    #endregion
    void Awake ()
    {
        parent = this.transform.parent.transform.parent.transform.parent.transform.parent.name;
        
	}

	void Update ()
    {
        if (this.parent == "Player1")
        {            
            FirstAim();
        }
        //When right click is held down
        else if (this.parent == "Player2")
        {
            SecondAim();
        }
	}

    void FirstAim()
    {
        if(Input.GetAxis("Left Trigger") >= 0.2)
        {
            this.transform.localPosition = Vector3.Lerp(transform.localPosition, zoomPosition, Time.deltaTime * moveSpeed);
            //Change the camera field of view
            this.gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView, zoomFov, fovSpeed * Time.deltaTime);
        }
        else
        {
            //When right click is released
            //Move the gun back to the default position
            this.transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPosition, Time.deltaTime * moveSpeed);
            //Change back the camera field of view
            this.gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView, defaultFov, fovSpeed * Time.deltaTime);
        }
    }
    void SecondAim()
    {
        float p2aim;
        p2aim = Input.GetAxis("p2 lt");
        if (p2aim > 0)
        {
            //Move the gun to the zoom position
            this.transform.localPosition = Vector3.Lerp(transform.localPosition, zoomPosition, Time.deltaTime * moveSpeed);
            //Change the camera field of view
            this.gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView, zoomFov, fovSpeed * Time.deltaTime);
        }
        else
        {
            //When right click is released
            //Move the gun back to the default position
            this.transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPosition, Time.deltaTime * moveSpeed);
            //Change back the camera field of view
            this.gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView, defaultFov, fovSpeed * Time.deltaTime);
        }
    }
}