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
    public bool isGrounded;
    public bool isWalking;
    public bool isRunning;
    [SerializeField] private float walkingSpd;
    [SerializeField] private float runningSpd;

    #endregion

    #region Initialization
    private void Awake ()
    {
        playerModel = GameObject.Find("Character_Cowboy_01");
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
        if(Input.GetAxis("Left Stick Horizontal") == 1)
        {

        }
    }

    #endregion
}