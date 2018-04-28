using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float health = 100;
    public float stamina = 5f;
    public float spawnTime = 4f;
    public bool isRespawning = false;

    public MonoBehaviour fpsCon;

	private void Update ()
    {
        StopCoroutine(Respawn());
        Mathf.Clamp(health, 0, 100); //Keeps health in appropriate range

        if (Input.GetKeyDown(KeyCode.K) && isRespawning == false) //kys xd
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
        isRespawning = true;
        print("Respawning");

        yield return new WaitForSecondsRealtime(spawnTime);

        if (health != 100f)
        {
            #region Spawning
            int point = Random.Range(0, 7);

            switch (point)
            {
                case 0:
                    transform.position = GameObject.Find("SpawnPoint0").transform.position;
                    break;
                case 1:
                    transform.position = GameObject.Find("SpawnPoint1").transform.position;
                    break;
                case 2:
                    transform.position = GameObject.Find("SpawnPoint2").transform.position;
                    break;
                case 3:
                    transform.position = GameObject.Find("SpawnPoint3").transform.position;
                    break;
                case 4:
                    transform.position = GameObject.Find("SpawnPoint4").transform.position;
                    break;
                case 5:
                    transform.position = GameObject.Find("SpawnPoint5").transform.position;
                    break;
                case 6:
                    transform.position = GameObject.Find("SpawnPoint6").transform.position;
                    break;
                case 7:
                    transform.position = GameObject.Find("SpawnPoint7").transform.position;
                    break;
            }
            #endregion

            health = 100f;
            fpsCon.enabled = true;
        }
        isRespawning = false;
        print("Respawned");
    }

    private void Suicide ()
    {
        health = 0f;
        print("KYS");
    }
}