using UnityEngine;
using UnityEngine.UI;

public class Rifle : MonoBehaviour
{
    public int ammoReserves;
    public int currentAmmoInMag;
    public int magSize = 1;
    public float damage = 65f;
    public float range = 100f;
    public bool triggerPulled = false;

    public GameObject player;

    public AudioClip rifleShot;
    public AudioClip rifleLoad;
    public AudioClip rifleDryFire;
    public Camera fpsCam;
    public ParticleSystem gunFX;
    public Text ammoInMag;
    public Text ammoRes;
    public Texture2D crosshairTexture;
    [SerializeField] private float crosshairScale = .2f;

    private void Awake()
    {
        ammoReserves = this.gameObject.GetComponentInParent<Player>().rifleAmmoPoolP1;
        currentAmmoInMag = magSize;
    }

    private void OnGUI()
    {
        if (Time.timeScale != 0)
        {
            if (crosshairTexture != null)
                GUI.DrawTexture(new Rect((Screen.width - crosshairTexture.width * crosshairScale) / 2, (Screen.height - crosshairTexture.height * crosshairScale)
                    / 2, crosshairTexture.width * crosshairScale, crosshairTexture.height * crosshairScale), crosshairTexture);
            else
                Debug.Log("No crosshair texture set in the Inspector");
        }
    }

    private void Update()
    {
        ammoReserves = this.gameObject.GetComponentInParent<Player>().rifleAmmoPoolP1;
        //reload animation

        ammoInMag.text = currentAmmoInMag.ToString(); //For ammo count UI
        ammoRes.text = this.gameObject.GetComponentInParent<Player>().rifleAmmoPoolP1.ToString();
        if (this.transform.parent.transform.parent.name == "Player1")
        {
            Debug.Log("p1");
            Shooter1();
        }
        if (this.transform.parent.transform.parent.name == "Player2")
        {
            Shooter2();
        }
        
    }
    void Shooter1()
    {
        if (Input.GetAxis("Right Trigger") == 1 && triggerPulled == false)
            Shoot();
        if (Input.GetAxis("Right Trigger") == 0)
            triggerPulled = false;
        if (Input.GetButtonDown("Right Bumper") && currentAmmoInMag < magSize && ammoReserves > 0)
            Reload();
    }
    void Shooter2()
    {
        if (Input.GetAxis("Fire1") == 1 && triggerPulled == false)
            Shoot();
        if (Input.GetAxis("Fire1") == 0)
            triggerPulled = false;
        if (Input.GetButtonDown("p2 rb") && currentAmmoInMag < magSize && ammoReserves > 0)
            Reload();
    }
    private void Shoot()
    {
        triggerPulled = true;
        if (currentAmmoInMag == 0)
        {
            SoundManager.instance.Play(rifleDryFire, "sfx");
            return;
        }
            gunFX.Play();
            SoundManager.instance.Play(rifleShot, "sfx");
        if(this.GetComponentInParent<Player>().infiniteAmmo == false)
        {
            currentAmmoInMag -= 1;
        }
            RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            //print(hit.transform.name);

            if (hit.transform.CompareTag("Player") == false)
            {
                hit.transform.GetComponent<Rigidbody>().AddForce(fpsCam.transform.forward * 750f);
            }
            if (hit.transform.CompareTag("Player") == true)
            {
                hit.transform.GetComponent<Player>().TakeDamage(damage, 0);
            }
        }
    }

    private void Reload()
    {
        SoundManager.instance.Play(rifleLoad, "sfx");

        while (currentAmmoInMag < magSize)
        {
            if (ammoReserves == 0)
                break;

            currentAmmoInMag++;
            this.gameObject.GetComponentInParent<Player>().rifleAmmoPoolP1--;
        }
    }

    public void AddAmmo(int outsideAmmo)
    {
        ammoReserves += outsideAmmo;
    }
}