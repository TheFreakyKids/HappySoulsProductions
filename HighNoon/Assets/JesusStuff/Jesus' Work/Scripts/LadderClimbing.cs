using UnityEngine;

public class LadderClimbing : MonoBehaviour //Script doesn't work in it's current state because Unity's FPS Controller is causing problems
{
    public Vector3 climbForce = Vector3.up * 200f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            if (Input.GetAxis("Left Stick Vertical") == -1)
            {
                other.GetComponent<Rigidbody>().AddForce(climbForce);
            }
        }
        if (other.CompareTag("Player") == true)
        {
            if (Input.GetAxis("Left Stick Vertical") == 1)
            {
                other.GetComponent<Rigidbody>().AddForce(-climbForce);
            }
        }
    }
}