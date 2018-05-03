using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Characters.FirstPerson;

public class DesperadoDrinks : MonoBehaviour
{
    public float healthAddition = 50;
    

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (this.tag == "RootBeer")
            {
                Debug.Log("dat sho eez sum gud root beer");
                healthAddition = Mathf.Clamp(healthAddition, 0, other.gameObject.GetComponent<Player>().maxHealth);
                other.gameObject.GetComponent<Player>().currentHealth += healthAddition;
                Destroy(this.gameObject);
            }
            else if(this.tag == "Lemonade")
            {
                Debug.Log("laymohnayde");
                other.gameObject.GetComponent<Player>().infiniteAmmo = true;
                Destroy(this.gameObject);
            }
            else if(this.tag =="Fusion")
            {
                //player goes faster for 15 sec
                //other.gameObject.GetComponent<>().
                SpeedBooster(other);
            }
            else if(this.tag == "SpeedCola")
            {
                //reload faster for 15 sec
            }
            else if (this.tag == "Brisk")
            {
                //cant take damage for 10sec
            }
        }
        else
        {
            return;
        }
    }

    public IEnumerator SpeedBooster(Collision other)
    {
        other.gameObject.GetComponent<FirstPersonController>().m_WalkSpeed *= 2;
        other.gameObject.GetComponent<FirstPersonController>().m_WalkSpeed *= 2;
        yield return new WaitForSeconds(15);
        other.gameObject.GetComponent<FirstPersonController>().m_WalkSpeed /= 2;
        other.gameObject.GetComponent<FirstPersonController>().m_WalkSpeed /= 2;
    }
    
}
