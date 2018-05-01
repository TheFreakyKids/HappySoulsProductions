using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Vars

    private Rigidbody rb;
    private GameObject playerModel;
    public Animator anim;
    private Camera playerCam;
    public bool isJumping = false;
    public bool isStrafing = false;
    public bool isGrounded;
    public bool isWalking;
    public bool isRunning;
    [SerializeField] private float walkingSpd;
    [SerializeField] private float runningSpd;
    [SerializeField]
    private float jumpSpd;
    [SerializeField]
    private Vector3 walkingMov = Vector3.forward;
    [SerializeField]
    private Vector3 runningMov = Vector3.forward;
    [SerializeField]
    private Vector3 strafingMov = Vector3.right;
    [SerializeField]
    private Vector3 jumpMov = Vector3.up;

    #endregion

    #region Initialization
    private void Awake ()
    {
        rb = GetComponent<Rigidbody>();
        playerModel = GameObject.Find("Character_Cowboy_01");
        walkingSpd = 500f;
        runningSpd = 1000f;
        jumpSpd = 500f;
        walkingMov *= walkingSpd;
        runningMov *= runningSpd;
        strafingMov *= walkingSpd;
        jumpMov *= jumpSpd;
        //playerCam = GameObject.Find("FirstPersonController").GetComponent<Camera>();
	}
    #endregion

    #region MovementUpdates
    private void Update ()
    {
        CheckForMovement();
        CheckIfGrounded();
    }
    #endregion

    #region ControllerFunctions

    private void CheckIfGrounded()
    {
        float distToGround;
        float threshold = 1.3f;

        RaycastHit hit;

        if (Physics.Raycast(playerModel.transform.position, -playerModel.transform.up, out hit))
        {
            distToGround = hit.distance;

            if (distToGround > threshold)
            {
                print("NOT TOUCHING GROUND");
                isGrounded = false;
            }
            else
                isGrounded = true;
        }
    }

    private void CheckForMovement()
    {
        if(Input.GetAxis("Left Stick Horizontal") == 1) //Walking forward
        {
            isWalking = true;
            rb.AddForce(walkingMov);
        }
        if (Input.GetAxis("Left Stick Horizontal") == 1 && Input.GetButton("Left Stick Button") == true) //Running forward
        {
            isRunning = true;
            rb.AddForce(walkingMov);
        }
        if (Input.GetButton("Left Stick Button") == false)
        {
            isRunning = false;
        }
        if (Input.GetAxis("Left Stick Horizontal") == -1) //Walking backwards
        {
            isWalking = true;
            rb.AddForce(-walkingMov);
        }
        if (Input.GetAxis("Left Stick Horizontal") == 0)
        {
            isWalking = false;
        }
        if (Input.GetAxis("Left Stick Vertical") == 1) //Strafing right
        {
            isStrafing = true;
            rb.AddForce(strafingMov);
        }
        if (Input.GetAxis("Left Stick Vertical") == -1) //Strafing left
        {
            isStrafing = true;
            rb.AddForce(-strafingMov);
        }
        if (Input.GetAxis("Left Stick Vertical") == 0)
        {
            isStrafing = false;
        }
        if (Input.GetButtonDown("A") == true) //Jumping
        {
            isJumping = true;
            rb.AddForce(jumpMov);
        }
        if (Input.GetButtonDown("A") == false)
        {
            isJumping = false;
        }
    }

    #endregion
}