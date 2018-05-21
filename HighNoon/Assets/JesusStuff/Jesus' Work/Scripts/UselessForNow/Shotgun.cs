using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shotgun : MonoBehaviour
{
    public int ammoReserves;
    public int magSize = 2;
    public int currentAmmoInMag;
    public float shotTimer = .5f;
    public float shotWaitPeriod = .5f;
    public float damage = 85f;
    public float range = 25f;
    public bool triggerPulled = false;
    public bool isReloading = false;
    
    public AudioClip shotgunShot;
    public AudioClip shotgunLoad;
    public AudioClip shotgunDryFire;
    public Camera fpsCam;
    public ParticleSystem gunFX;
    public Text ammoInMag;
    public Text ammoRes;
    public Texture2D crosshairTexture;
    [SerializeField] private float crosshairScale  = .5f;

    private void Awake()
    {
        ammoReserves = this.gameObject.GetComponentInParent<Player>().shotgunAmmoPoolP1;
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
        
        ammoInMag.text = currentAmmoInMag.ToString(); //For ammo count UI
        ammoRes.text = this.gameObject.GetComponentInParent<Player>().shotgunAmmoPoolP1.ToString();
        ammoReserves = this.gameObject.GetComponentInParent<Player>().shotgunAmmoPoolP1;
        shotTimer += Time.deltaTime; //Gun can't be laser
        shotTimer = Mathf.Clamp(shotTimer, 0f, .5f); //Keeps timer in appropriate range
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
        if (Input.GetAxis("Right Trigger") == 1 && triggerPulled == false && shotTimer == shotWaitPeriod) //If you pressed RT, the trigger is not already pulled                                                                                                
            Shoot();                                                                                      //and the timer is set, shoot
        if (Input.GetAxis("Right Trigger") == 0) //If RT is not pushed, trigger is not pulled
            triggerPulled = false;
        if (Input.GetButtonDown("Right Bumper") == true && currentAmmoInMag < magSize && ammoReserves > 0 && isReloading == false) //If RB is pushed, you actually
            Reload();
    }
    void Shooter2()
    {
        if (Input.GetAxis("Fire1") == 1 && triggerPulled == false && shotTimer == shotWaitPeriod) //If you pressed RT, the trigger is not already pulled                                                                                                
            Shoot();                                                                                      //and the timer is set, shoot
        if (Input.GetAxis("Fire1") == 0) //If RT is not pushed, trigger is not pulled
            triggerPulled = false;
        if (Input.GetButtonDown("p2 rb") == true && currentAmmoInMag < magSize && ammoReserves > 0 && isReloading == false) //If RB is pushed, you actually
            Reload();
    }
    private void Shoot()
    {
        triggerPulled = true; //The trigger is pulled

        if (currentAmmoInMag == 0) //If you have no ammo, you fire nothing
        {
            SoundManager.instance.Play(shotgunDryFire, "sfx");
            return;
        }

        gunFX.Play(); //Otherwise, you fire
        SoundManager.instance.Play(shotgunShot, "sfx");
        if (this.GetComponentInParent<Player>().infiniteAmmo == false)
        {
            currentAmmoInMag -= 1;
        }
        shotTimer = 0f; //Reset shot timer
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) //Fire a bullet
        {
            //print(hit.transform.name);

            if (hit.transform.CompareTag("Player") == false)
            {
                hit.transform.GetComponent<Rigidbody>().AddForce(fpsCam.transform.forward * 1000f);
            }
            if (hit.transform.CompareTag("Player") == true)
            {
                hit.transform.GetComponent<Player>().TakeDamage(damage);
            }
        }

        
    }

    private void Reload()
    {
        print("Reloading Shotgun");
        isReloading = true; //You're reloading

        SoundManager.instance.Play(shotgunLoad, "sfx"); //Hear reload noise

        while (currentAmmoInMag < magSize) //While the mag is not full, reload
        {
            if (ammoReserves == 0) //If you can't reload, don't
                break;

            currentAmmoInMag++; //Put a bullet in
            this.gameObject.GetComponentInParent<Player>().shotgunAmmoPoolP1--; //Take a bullet from the reserves
        }

        isReloading = false; //Done reloading
        print("Reloaded Shotgun");
    }

    public void AddAmmo(int outsideAmmo)
    {
        //SoundManager.instance.Play(shotgunLoad, "sfx");
        //reload animation
        currentAmmoInMag = 2;
        ammoReserves += outsideAmmo;
    }
}