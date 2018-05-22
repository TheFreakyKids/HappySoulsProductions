using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class LadderClimbing : MonoBehaviour
{
    private Rigidbody rb;
    private FirstPersonController playerFPCON;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rb = other.GetComponent<Rigidbody>();
            playerFPCON = other.GetComponent<FirstPersonController>();

            rb.useGravity = false;
            rb.isKinematic = true;
            playerFPCON.isClimbing = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetAxis("Left Stick Vertical") == -1)
            {
                var targetPos = other.transform.position + (Vector3.up * 10);

                other.transform.position = Vector3.Lerp(other.transform.position, targetPos, Time.deltaTime * .2f);
            }
            else if (Input.GetAxis("Left Stick Vertical") == 1)
            {
                var targetPos = other.transform.position + (-Vector3.up * 10);

                other.transform.position = Vector3.Lerp(other.transform.position, targetPos, Time.deltaTime * .2f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            playerFPCON.isClimbing = false;
        }
    }
}