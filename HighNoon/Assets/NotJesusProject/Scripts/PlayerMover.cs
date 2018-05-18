using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public bool isPlayer1;
    public bool isPlayer2;
    
	void Start ()
    {
        if(this.gameObject.tag == "Player1")
        {
            isPlayer1 = true;
        }
        else
        {
            isPlayer2 = true;
        }
    }
	
	void Update ()
    {
        #region Player1
        if(isPlayer1 == true)
        {

        }
        #endregion

        #region Player2
        else if (isPlayer2 == true)
        {

        }
        #endregion 
    }
}
