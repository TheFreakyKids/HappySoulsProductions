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
    public ParticleSystem[] effects;

    private void Awake()
    {
        effects = GetComponentsInChildren<ParticleSystem>();
        rndr = this.gameObject.GetComponent<MeshRenderer>();
        boxCollider = this.gameObject.GetComponent<BoxCollider>();
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        if(other.gameObject.transform.root.tag == "Player")
        {
            if (this.tag == "RootBeer")
            {
                
                healthAddition = Mathf.Clamp(healthAddition, 0, other.gameObject.transform.root.GetComponent<Player>().maxHealth);
                other.gameObject.transform.root.GetComponent<Player>().currentHealth += healthAddition;
                rndr.enabled = false;
                boxCollider.enabled = false;
                for (int i = 0; i < effects.Length; i++)
                {
                    var foo = effects[i].emission;
                    foo.enabled = false;
                }
                StartCoroutine(BottleActivator());
            }
            else if(this.tag == "Lemonade")
            {
                Debug.Log("laymohnayde");
                other.gameObject.transform.root.GetComponent<Player>().infiniteAmmo = true;
                rndr.enabled = false;
                boxCollider.enabled = false;
                for (int i = 0; i < effects.Length; i++)
                {
                    var foo = effects[i].emission;
                    foo.enabled = false;
                }
                StartCoroutine(BottleActivator());
            }
            else if(this.tag == "Fusion")
            {
                //player goes faster for 15 sec
                Debug.Log("fast as fuck");
                StartCoroutine(SpeedBooster(other));
                rndr.enabled = false;
                boxCollider.enabled = false;
                for (int i = 0; i < effects.Length; i++)
                {
                    var foo = effects[i].emission;
                    foo.enabled = false;
                }
                StartCoroutine(BottleActivator());
            }
            else if (this.tag == "Brisk")
            {
                //cant take damage for 10sec
                Debug.Log("local man is fucking invincible");
                rndr.enabled = false;
                boxCollider.enabled = false;
                for (int i = 0; i < effects.Length; i++)
                {
                    var foo = effects[i].emission;
                    foo.enabled = false;
                }
                other.gameObject.transform.root.GetComponent<Player>().invincible = true;
                StartCoroutine(BottleActivator());
            }
        }
        else
        {
            return;
        }
    }

    public IEnumerator SpeedBooster(Collider other)
    {
        Debug.Log("speed booster call");
        other.gameObject.transform.root.GetComponent<FirstPersonController>().m_WalkSpeed += 10;
        other.gameObject.transform.root.GetComponent<FirstPersonController>().m_RunSpeed += 10;
        yield return new WaitForSeconds(15);
        Debug.Log("slow the fuck down");
        other.gameObject.transform.root.GetComponent<FirstPersonController>().m_WalkSpeed -= 10;
        other.gameObject.transform.root.GetComponent<FirstPersonController>().m_RunSpeed -= 10;
    }
    public IEnumerator BottleActivator()
    {
        yield return new WaitForSeconds(20);
        rndr.enabled = true;
        boxCollider.enabled = true; for (int i = 0; i < effects.Length; i++)
        {
            var foo = effects[i].emission;
            foo.enabled = true;
        }
    }
    
}