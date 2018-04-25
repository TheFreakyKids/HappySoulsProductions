using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float health = 100;
    public float stamina = 5f;
    public float spawnTime = 4f;

    public MonoBehaviour fpsCon;

	public void Start ()
    {
		
	}
	
	private void Update ()
    {
        Mathf.Clamp(health, 0, 100);

        if (Input.GetKeyDown(KeyCode.K))
        {
            Suicide();
        }

        if (health == 0f)
            Die();
	}

    public void TakeDamage(float dam)
    {
        health -= dam;
    }

    private void Die()
    {
        fpsCon.enabled = false;
        StartCoroutine(Respawn());
    }

    public IEnumerator Respawn ()
    {
        yield return new WaitForSeconds(spawnTime);

        //move to spawn point

        if (health != 100f)
        {
            print("respawned");
            health = 100f;
            fpsCon.enabled = true;
        }
    }

    private void Suicide ()
    {
        print("kys");
        health = 0f;
    }
}