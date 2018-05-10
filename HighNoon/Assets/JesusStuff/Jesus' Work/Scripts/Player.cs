using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public float stamina = 5f;
    public float spawnTime = 4f;
    public bool isRespawning = false;

    public int revolverAmmoPool = 12;
    public int rifleAmmoPool = 4;
    public int shotgunAmmoPool = 4;

    public bool infiniteAmmo = false;
    public bool invincible = false;
    public bool speedLoader = false;

    public MonoBehaviour fpsCon;
    public Camera fpsCam;
    public Slider health;
    public Text elims; //For elim count when we have that functionality
    [SerializeField] private GameObject[] ragdollParts;

    private void Awake()
    {
        currentHealth = maxHealth;

        GameObject[] temp = GameObject.FindGameObjectsWithTag("RagdollPart");

        ragdollParts = new GameObject[temp.Length];

        for (int i = 0; i < temp.Length; i++)
        {
            ragdollParts[i] = temp[i];
        }
    }

    private void Update ()
    {
        health.value = currentHealth / 100; //Divide by 100 because Slider goes from 0-1 so w/o this Slider doesn't slide properly
        Mathf.Clamp(currentHealth, 0, maxHealth); //Keeps health in appropriate range
        if (infiniteAmmo == true || invincible == true || speedLoader == true)
        {
            StartCoroutine("PowerUpTimer");
        }

        if (Input.GetKeyDown(KeyCode.K) && isRespawning == false) //For debugging purposes
            Suicide();
        if (Input.GetKeyDown(KeyCode.H) && isRespawning == false) //For debugging purposes
            TakeDamage(25);
        if (currentHealth == 0f && isRespawning == false) //If you have no health & you're not already respawning, then die
            Die();
	}

    public void FixedUpdate()
    {
       
    }

    public void TakeDamage(float dam)
    {
        if(invincible == false)
        {
            currentHealth -= dam;
        }        
    }

    private void Die()
    {
        fpsCon.enabled = false;

        for (int i = 0; i < ragdollParts.Length; i++)
        {
            ragdollParts[i].GetComponent<Ragdoll>().TurnOnRagdoll();
        }

        StartCoroutine(Respawn());
    }

    public IEnumerator Respawn ()
    {
        isRespawning = true;

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

            for (int i = 0; i < ragdollParts.Length; i++)
            {
                ragdollParts[i].GetComponent<Ragdoll>().TurnOffRagdoll();
            }

            currentHealth = 100f;
            fpsCon.enabled = true;
        }
        isRespawning = false;
    }

    private void Suicide ()
    {
        currentHealth = 0f;
    }
    private IEnumerator PowerUpTimer()
    {
        yield return new WaitForSeconds(10);
        infiniteAmmo = false;
        invincible = false;
        speedLoader = false;
    }
}