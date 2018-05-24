using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnFromWin : MonoBehaviour
{
	void Update ()
    {
		if(Input.GetButton("A1") || Input.GetButton("A2"))
        {
            SceneManager.LoadScene("Menu");
        }
	}
}
