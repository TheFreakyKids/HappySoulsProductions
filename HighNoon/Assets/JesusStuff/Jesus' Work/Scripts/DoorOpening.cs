using UnityEngine.UI;
using System.Collections;
using UnityEngine;

public class DoorOpening : MonoBehaviour
{
    public float doorOpenAngle = -90.0f;
    public float doorCloseAngle = 0.0f;
    public float doorAnimSpeed = 4.0f;
    public Quaternion doorOpen;
    public Quaternion doorOpen2;
    public Quaternion doorClose;
    public Transform playerTrans;
    public bool isClosed = true; //false is close, true is open
    public bool doorIsMoving = false; //for Coroutine, when start only one
    public GameObject[] players;
    public Material mat;
    public Image icon;

    void Awake()
    {
        isClosed = true; //door is open, maybe change
                            //Initialization your quaternions
        doorOpen = Quaternion.Euler(0, doorOpenAngle, 0);
        doorOpen2 = Quaternion.Euler(0, -doorOpenAngle, 0);
        doorClose = Quaternion.Euler(0, doorCloseAngle, 0);
        //Find only one time your player and get him reference
        playerTrans = GameObject.FindWithTag("Player").transform;

        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        for (int i = 0; i< players.Length; i++)
        {
            if (Vector3.Distance(players[i].transform.position, transform.position) < 3f)
            {
                if (Input.GetButtonDown("X" + players[i].GetComponent<Player>().playerNum) && !doorIsMoving)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(players[i].transform.position, players[i].transform.forward, out hit)) //Not complete
                    {
                        if (hit.transform == transform)
                        {
                            if (isClosed)
                            { //close door
                                if (Vector3.Dot(players[i].transform.forward, transform.forward) > 0)
                                {
                                    StartCoroutine(MoveDoor(doorOpen2));
                                }
                                else if (Vector3.Dot(players[i].transform.forward, transform.forward) < 0)
                                {
                                    StartCoroutine(MoveDoor(doorOpen));
                                }
                            }
                            else
                            { //open door
                                StartCoroutine(MoveDoor(doorClose));
                            }
                        }
                    }
                }
            }
        }
    }
    public IEnumerator MoveDoor(Quaternion dest)
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
    public void OnGUI()
    {
        
    }
}