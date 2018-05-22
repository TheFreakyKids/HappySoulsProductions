using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Characters.FirstPerson;

public class DesperadoDrinks : MonoBehaviour
{
    public float healthAddition = 50;
    private MeshRenderer rndr;
    private BoxCollider boxCollider;
    private void Awake()
    {
        rndr = this.gameObject.GetComponent<MeshRenderer>();
        boxCollider = this.gameObject.GetComponent<BoxCollider>();
    }

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (this.tag == "RootBeer")
            {
                Debug.Log("dat sho eez sum gud root beer");
                healthAddition = Mathf.Clamp(healthAddition, 0, other.gameObject.GetComponent<Player>().maxHealth);
                other.gameObject.GetComponent<Player>().currentHealth += healthAddition;
                rndr.enabled = false;
                boxCollider.enabled = false;
                StartCoroutine(BottleActivator());
            }
            else if(this.tag == "Lemonade")
            {
                Debug.Log("laymohnayde");
                other.gameObject.GetComponent<Player>().infiniteAmmo = true;
                rndr.enabled = false;
                boxCollider.enabled = false;
                StartCoroutine(BottleActivator());
            }
            else if(this.tag == "Fusion")
            {
                //player goes faster for 15 sec
                Debug.Log("fast as fuck");
                StartCoroutine(SpeedBooster(other));
                rndr.enabled = false;
                boxCollider.enabled = false;
                StartCoroutine(BottleActivator());
            }
            else if(this.tag == "SpeedCola")
            {
                //make reload faster
                Debug.Log("reload fast now");
                other.gameObject.GetComponent<Player>().speedLoader = true;
                rndr.enabled = false;
                boxCollider.enabled = false;
                StartCoroutine(BottleActivator());
            }
            else if (this.tag == "Brisk")
            {
                //cant take damage for 10sec
                Debug.Log("local man is fucking invincible");
                rndr.enabled = false;
                boxCollider.enabled = false;
                other.gameObject.GetComponent<Player>().invincible = true;
                StartCoroutine(BottleActivator());
            }
        }
        else
        {
            return;
        }
    }

    public IEnumerator SpeedBooster(Collision other)
    {
        Debug.Log("speed booster call");
        other.gameObject.GetComponent<FirstPersonController>().m_WalkSpeed += 10;
        other.gameObject.GetComponent<FirstPersonController>().m_RunSpeed += 10;
        yield return new WaitForSeconds(15);
        Debug.Log("slow the fuck down");
        other.gameObject.GetComponent<FirstPersonController>().m_WalkSpeed -= 10;
        other.gameObject.GetComponent<FirstPersonController>().m_RunSpeed -= 10;
    }
    public IEnumerator BottleActivator()
    {
        yield return new WaitForSeconds(20);
        rndr.enabled = true;
        boxCollider.enabled = true;
    }
    
}