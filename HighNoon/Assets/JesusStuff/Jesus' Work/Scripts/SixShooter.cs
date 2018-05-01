using UnityEngine;
using UnityEngine.UI;

public class SixShooter : MonoBehaviour
{
    public int ammoReserves;
    public int magSize = 6;
    public int currentAmmoInMag;
    public float damage = 35f;
    public float range = 50f;
    public float shotTimer;
    public float shotWaitPeriod;
    public bool triggerPulled = false;
    public bool isReloading = false;

    public AudioClip revolverShot;
    public AudioClip revolverLoad;
    public AudioClip revolverDryFire;
    public Camera fpsCam;
    public ParticleSystem gunFX;
    public Text ammoInMag;
    public Text ammoRes;
    public Texture2D crosshairTexture;
    [SerializeField] private float crosshairScale = 0.015f;

    private void Awake()
    {
        ammoReserves = magSize * 2;
        currentAmmoInMag = magSize;
    }
        shotTimer += Time.deltaTime;
        shotTimer = Mathf.Clamp(shotTimer, 0f, shotWaitPeriod);
        if (CrossPlatformInputManager.GetButtonDown("Fire2") && ammoCount < 6)//xbutton
        {
            Reload();
        }
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

    private void Update ()
    {
        SoundManager.instance.Play(revolverLoad, "sfx");
        //reload animation
        ammoCount = 6;
        ammoInMag.text = currentAmmoInMag.ToString(); //For ammo count UI
        ammoRes.text = ammoReserves.ToString();

        shotTimer += Time.deltaTime; //Doesn't allow the gun to become a laser
        shotTimer = Mathf.Clamp(shotTimer, 0f, .5f); //Keeps the timer in the appropriate range

        if (Input.GetAxis("Right Trigger") == 0) //If no RT Input, triggered is not pulled
            triggerPulled = false;

        if (Input.GetAxis("Right Trigger") == 1 && triggerPulled == false && shotTimer == shotWaitPeriod) //If there is RT input, the trigger is not pulled &                                                                                                
            Shoot();                                                                                      //the shotTimer is set, then you can shoot

        #region Rolling
        //if (Input.GetButtonDown("Left Bumper") == true)  //put in controller for rolling later
        //{
        //    print("rolling");
        //}
        #endregion

        if (Input.GetButtonDown("Right Bumper") == true && triggerPulled == false && currentAmmoInMag < magSize && ammoReserves > 0 && isReloading == false) 
            Reload(); //If RB is pushed & the trigger is not pulled & you actually need to reload and you have ammo to reload with & you're not already reloading,
                      //then reload
    }

    private void Shoot()
    {
        triggerPulled = true; //You just pulled the trigger

        if (currentAmmoInMag == 0) //If you have no ammo, you hear a dry gun sound and return
        {
            SoundManager.instance.Play(revolverDryFire, "sfx");
            Debug.Log("Revolver is empty.");
            return;
        }

        gunFX.Play(); //Otherwise, see the gun smoke, hear a gunshot, fire a bullet
        SoundManager.instance.Play(revolverShot, "sfx");

        RaycastHit hit;

        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            print(hit.transform.name);

            if (hit.transform.CompareTag("Player") == false)
            {
                hit.transform.GetComponent<Rigidbody>().AddForce(fpsCam.transform.forward * 500f);
            }
        }

            currentAmmoInMag -= 1; //Minus 1 from ammo in mag
            shotTimer = 0f; //Reset shot timer

        print("Fired Sixshooter");
    }

    private void Reload()
    {
        print("Reloading SixShooter");
        isReloading = true; //You are reloading

        SoundManager.instance.Play(revolverLoad, "sfx"); //Hear reloading noise

        while (currentAmmoInMag < magSize) //WHile the ammo in the mag is lower then it's size, reload
        {
            SoundManager.instance.Play(revolverDryFire, "sfx");
            if (ammoReserves == 0) //If you have no ammo to reload with, break
                break;

            currentAmmoInMag++; //Otherwise, put a bullet in
            ammoReserves--; //Take a bullet from the reserves
        }

        isReloading = false; //You're done reloading
        print("Reloaded SixShooter");
    }

    public void AddAmmo(int outsideAmmo)
    {
        ammoReserves += outsideAmmo; //When you get ammo from a weapon spawn, put in the reserves
    }
}