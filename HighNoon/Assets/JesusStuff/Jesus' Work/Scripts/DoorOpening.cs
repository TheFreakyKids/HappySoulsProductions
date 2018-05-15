using System.Collections;
using UnityEngine;

public class DoorOpening : MonoBehaviour
{
    [SerializeField] private float doorOpenAngle = -90.0f;
    [SerializeField] private float doorCloseAngle = 0.0f;
    [SerializeField] private float doorAnimSpeed = 4.0f;
    [SerializeField] private Quaternion doorOpen;
    [SerializeField] private Quaternion doorClose;
    [SerializeField] private Transform playerTrans;
    [SerializeField] private bool isClosed = true; //false is close, true is open
    [SerializeField] private bool doorIsMoving = false; //for Coroutine, when start only one
    void Awake()
    {
        isClosed = true; //door is open, maybe change
                            //Initialization your quaternions
        doorOpen = Quaternion.Euler(0, doorOpenAngle, 0);
        doorClose = Quaternion.Euler(0, doorCloseAngle, 0);
        //Find only one time your player and get him reference
        playerTrans = GameObject.FindWithTag("Player").transform;
    }

    void OnMouseEnter()
    {

    }

    void Update()
    {
        //If press X key on Xbox One
        if (Input.GetButtonDown("X") || Input.GetButtonDown("Fire2")&& !doorIsMoving)
        {
            //Calculate distance between player and door
            if (Vector3.Distance(playerTrans.position, transform.position) < 2f)
            {
                if (isClosed)
                { //close door
                    StartCoroutine(moveDoor(doorOpen));
                }
                else
                { //open door
                    StartCoroutine(moveDoor(doorClose));
                }
            }
        }
    }
    public IEnumerator moveDoor(Quaternion dest)
    {
        doorIsMoving = true;
        //Check if close/open, if angle less 4 degree, or use another value more 0
        while (Quaternion.Angle(transform.localRotation, dest) > 4.0f)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, dest, Time.deltaTime * doorAnimSpeed);
            //UPDATE 1: add yield
            yield return null;
        }
        //Change door status
        isClosed = !isClosed;
        doorIsMoving = false;
        //UPDATE 1: add yield
        yield return null;
    }
}