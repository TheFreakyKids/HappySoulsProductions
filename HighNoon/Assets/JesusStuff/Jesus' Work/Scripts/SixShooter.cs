using UnityEngine;

public class SixShooter : MonoBehaviour
{
    public int ammoReserves;
    public int magSize = 6;
    public int currentAmmoInMag;
    public float damage = 35f;
    public float range = 50f;
    public float shotTimer = .5f;
    public float shotWaitPeriod = .5f;
    public bool triggerPulled = false;
    public bool isReloading = false;

    public AudioClip revolverShot;
    public AudioClip revolverLoad;
    public AudioClip revolverDryFire;
    public Camera fpsCam;
    public ParticleSystem gunFX;

    private void Awake()
    {
        ammoReserves = magSize * 2;
        currentAmmoInMag = magSize;
    }
	
	private void Update ()
    {
        shotTimer += Time.deltaTime; //Doesn't allow the gun to become a laser
        shotTimer = Mathf.Clamp(shotTimer, 0f, .5f); //Keeps the timer in the appropriate range

        if (Input.GetAxis("Right Trigger") == 0) //If no RT Input, triggered is not pulled
            triggerPulled = false;

        if (Input.GetAxis("Right Trigger") == 1 && triggerPulled == false && shotTimer == shotWaitPeriod) //If there is RT input, the trigger is not pulled &
        {                                                                                                 //the shotTimer is set, then you can shoot
            Shoot();
        }

        #region Rolling
        //if (Input.GetButtonDown("Left Bumper") == true)  //put in controller for rolling later
        //{
        //    print("rolling");
        //}
        #endregion

        if (Input.GetButtonDown("Right Bumper") == true && triggerPulled == false && currentAmmoInMag < magSize && ammoReserves > 0 && isReloading == false)
        {//If RB is pushed & the trigger is not pulled & you actually need to reload and you have ammo to reload with & you're not already reloading, then reload
            Reload();
        }
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
            print(hit.transform.name);

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