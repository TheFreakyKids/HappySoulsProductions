using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        ammoReserves = magSize * 2;
        currentAmmoInMag = magSize;
    }

    private void Update()
    {
        shotTimer += Time.deltaTime; //Gun can't be laser
        shotTimer = Mathf.Clamp(shotTimer, 0f, .5f); //Keeps timer in appropriate range

        if (Input.GetAxis("Right Trigger") == 1 && triggerPulled == false && shotTimer == shotWaitPeriod) //If you pressed RT, the trigger is not already pulled
        {                                                                                                 //and the timer is set, shoot
            Shoot();
        }
        if (Input.GetAxis("Right Trigger") == 0) //If RT is not pushed, trigger is not pulled
            triggerPulled = false;

        if (Input.GetButtonDown("Right Bumper") == true && currentAmmoInMag < magSize && ammoReserves > 0 && isReloading == false) //If RB is pushed, you actually
            Reload();                                                                                                              //need to reload, you have ammo
    }                                                                                                                              //to reload with & you're not
                                                                                                                                   //already reloading, reload
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
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) //Fire a bullet
        {
            print(hit.transform.name);
        }

        currentAmmoInMag -= 1; //Remove a bullet from mag
        shotTimer = 0f; //Reset shot timer
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
            ammoReserves--; //Take a bullet from the reserves
        }

        isReloading = false; //Done reloading
        print("Reloaded Shotgun");
    }
}