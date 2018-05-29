using UnityEngine;

public class BillboardCamFacer : MonoBehaviour
{
    public Camera camToFace;
	
	void Update ()
    {
        if (camToFace != null)
        {
            transform.LookAt(camToFace.transform, camToFace.transform.up);
        }
	}
}