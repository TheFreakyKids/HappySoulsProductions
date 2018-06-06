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
    public Text elims; 
    public Transform[] playerSpawns;
    public Transform OGCapTrans;

    public SkinnedMeshRenderer[] rends;

    public int playerNum;

    [SerializeField]
    protected AudioClip[] grunts;
    public AudioClip grunt;

    private void Awake()
    {
        currentHealth = maxHealth;
        playerSpawns = GameObject.Find("PlayerSpawns").GetComponentsInChildren<Transform>();
        rb = this.gameObject.GetComponent<Rigidbody>();
        string numberOnly = Regex.Replace(gameObject.name, "[^0-9]", "");
        playerNum = int.Parse(numberOnly);
        rends = transform.Find("FirstPersonCharacter").GetComponentsInChildren<SkinnedMeshRenderer>();
        FloatingTextController.Initialize();
        OGCapTrans = transform.Find("Capsule").transform;
    }

    private void Update ()
    {
        health.value = currentHealth / 100;

        if (infiniteAmmo == true || invincible == true || speedLoader == true)
        {
            StartCoroutine("PowerUpTimer");
        }

        if (Input.GetKeyDown(KeyCode.K) && isRespawning == false) 
            Suicide();
        if (Input.GetKeyDown(KeyCode.H) && isRespawning == false) 
            TakeDamage(25, playerNum);
        if (currentHealth == 0f && isRespawning == false)
        {
            died = true;
            Die();
        }
    }

    public void TakeDamage(float dam, int playerNumWhoShot)
    {
        if (!died)
        {
            if (invincible == false)
            {
                currentHealth = Mathf.Clamp(currentHealth - dam, 0, maxHealth);
                FloatingTextController.CreateFloatingText(dam.ToString(), transform, playerNum, playerNumWhoShot);
                int index = Random.Range(0, grunts.Length);
                grunt = grunts[index];
                SoundManager.instance.Play(grunt, "vfx");
            }
        }                                                                                                                                                                                                                                                   
    }

    private void Die()
    {
        fpsCon.enabled = false;
        camCon.enabled = false;
        transform.Find("Capsule").GetComponent<Rigidbody>().isKinematic = false;
        transform.Find("FirstPersonCharacter").GetComponentInChildren<CameraController>().enabled = false;
        transform.Find("FirstPersonCharacter").GetComponentInChildren<WeaponSwapping>().enabled = false;
        transform.Find("FirstPersonCharacter").GetComponentInChildren<ArmControllerScript>().enabled = false;
        foreach (SkinnedMeshRenderer rend in rends)
        {
            rend.enabled = false;
        }
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
            transform.Find("Capsule").GetComponent<Rigidbody>().isKinematic = true;
            transform.Find("FirstPersonCharacter").GetComponentInChildren<ArmControllerScript>().enabled = true;
            transform.Find("FirstPersonCharacter").GetComponentInChildren<CameraController>().enabled = true;
            transform.Find("FirstPersonCharacter").GetComponentInChildren<WeaponSwapping>().enabled = true;
            foreach (SkinnedMeshRenderer rend in rends)
            {
                rend.enabled = true;
            }
            transform.Find("Capsule").transform.localPosition = Vector3.zero;
            transform.Find("Capsule").transform.rotation = Quaternion.identity;
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