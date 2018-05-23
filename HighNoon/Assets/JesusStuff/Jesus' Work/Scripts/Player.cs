using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Player : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public float spawnTime = 4f;
    public bool isRespawning = false;
    public bool died = false;
    public int revolverAmmoPool = 12;
    public int rifleAmmoPool = 4;
    public int shotgunAmmoPool = 4;
    #region BecauseCompilers
    public int revolverAmmoPoolP1 = 12;
    public int rifleAmmoPoolP1 = 4;
    public int shotgunAmmoPoolP1 = 4;
    public int revolverAmmoPoolP2 = 12;
    public int rifleAmmoPoolP2 = 4;
    public int shotgunAmmoPoolP2 = 4;
    #endregion
    public bool infiniteAmmo = false;
    public bool invincible = false;
    public bool speedLoader = false;

    public Rigidbody rb;
    public MonoBehaviour fpsCon;
    public MonoBehaviour camCon;
    public Slider health;
    public Text elims; //For elim count when we have that functionality
    public Transform[] playerSpawns;

    public int playerNum;
    public Text damText;
    public Animator textAnim;
    public static GameObject canvas;

    private void Awake()
    {
        currentHealth = maxHealth;
        playerSpawns = GameObject.Find("PlayerSpawns").GetComponentsInChildren<Transform>();
        rb = this.gameObject.GetComponent<Rigidbody>();
        string numberOnly = Regex.Replace(gameObject.name, "[^0-9]", "");
        playerNum = int.Parse(numberOnly);

        canvas = GameObject.Find("Canvas");
        damText = textAnim.GetComponent<Text>();
    }

    private void Update ()
    {
        health.value = currentHealth / 100; //Divide by 100 because Slider goes from 0-1 so w/o this Slider doesn't slide properly
         //Keeps health in appropriate range
        if (infiniteAmmo == true || invincible == true || speedLoader == true)
        {
            StartCoroutine("PowerUpTimer");
        }

        if (Input.GetKeyDown(KeyCode.K) && isRespawning == false) //For debugging purposes
            Suicide();
        if (Input.GetKeyDown(KeyCode.H) && isRespawning == false) //For debugging purposes
            TakeDamage(25);
        if (currentHealth == 0f && isRespawning == false)
        {
            died = true;//If you have no health & you're not already respawning, then die
            Die();
        }
	}

    public void TakeDamage(float dam)
    {
        if (!died)
        {
            if (invincible == false)
            {
                currentHealth = Mathf.Clamp(currentHealth - dam, 0, maxHealth);
                damText.text = dam.ToString();

                Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

                var foo = Instantiate(damText, screenPosition, Quaternion.identity, canvas.transform);
                AnimatorClipInfo[] clipInfo = textAnim.GetCurrentAnimatorClipInfo(0);
                Destroy(foo, clipInfo[0].clip.length);
            }
        }                                                                                                                                                                                                                                                   
    }

    private void Die()
    {
        fpsCon.enabled = false;
        camCon.enabled = false;
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        StartCoroutine(Respawn());
    }

    public IEnumerator Respawn ()
    {
        isRespawning = true;

        yield return new WaitForSecondsRealtime(spawnTime);

        if (currentHealth != 100f)
        {
            int point = Random.Range(0, 7);

            transform.position = playerSpawns[point].position;
            
            currentHealth = 100f;
            fpsCon.enabled = true;
            camCon.enabled = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
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