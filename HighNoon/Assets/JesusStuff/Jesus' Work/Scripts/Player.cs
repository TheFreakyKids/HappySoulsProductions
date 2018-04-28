using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public float stamina = 5f;
    public float spawnTime = 4f;
    public bool isRespawning = false;
    public bool isLookingAtWeaponSpawn = false;

    public MonoBehaviour fpsCon;
    public Camera fpsCam;
    public Slider health;
    public Text elims; //For elim count when we have that functionality

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update ()
    {
        health.value = currentHealth / 100; //Divide by 100 because Slider goes from 0-1 so w/o this Slider doesn't slide properly
        Mathf.Clamp(currentHealth, 0, maxHealth); //Keeps health in appropriate range

        if (Input.GetKeyDown(KeyCode.K) && isRespawning == false) //For debugging purposes
            Suicide();
        if (Input.GetKeyDown(KeyCode.H) && isRespawning == false) //For debugging purposes
            TakeDamage(25);
        if (currentHealth == 0f && isRespawning == false) //If you have no health & you're not already respawning, then die
            Die();
	}

    public void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            if (hit.transform.CompareTag("WeaponSpawn") == true)
                isLookingAtWeaponSpawn = true;
            else
                isLookingAtWeaponSpawn = false;
        }
    }

    public void TakeDamage(float dam)
    {
        currentHealth -= dam;
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

        if (currentHealth != 100f)
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

            currentHealth = 100f;
            fpsCon.enabled = true;
        }
        isRespawning = false;
        print("Respawned");
    }

    private void Suicide ()
    {
        currentHealth = 0f;
        print("Ya KYS");
    }
}