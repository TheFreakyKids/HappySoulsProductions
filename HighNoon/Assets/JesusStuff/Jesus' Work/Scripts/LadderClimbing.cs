using UnityEngine;

public class LadderClimbing : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            if (Input.GetAxis("Left Stick Vertical") == -1)
            {
                var targetPos = other.transform.position + (Vector3.up * 10);

                other.transform.position = Vector3.Lerp(other.transform.position, targetPos, Time.deltaTime * 1.6f);
            }
        }
    }
}