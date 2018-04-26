using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Shotgun : MonoBehaviour
{
    public int totalAmmo;
    public int magSize = 2;
    public int currentAmmoInMag;
    public float shotTimer = .5f;
    public float shotWaitPeriod = .5f;
    public float damage = 85f;
    public float range = 25f;
    public bool triggerPulled = false;

    public AudioClip shotgunShot;
    public AudioClip shotgunLoad;
    public AudioClip shotgunDryFire;
    public Camera fpsCam;
    public ParticleSystem gunFX;

    private void Awake()
    {
        totalAmmo = magSize * 2;
        currentAmmoInMag = magSize;
    }

    private void Update()
    {
        shotTimer += Time.deltaTime;
        shotTimer = Mathf.Clamp(shotTimer, 0f, .5f);

        if (CrossPlatformInputManager.GetAxis("Fire1") == -1 && triggerPulled == false)
        {
            triggerPulled = true;
            Shoot();
        }
        if (CrossPlatformInputManager.GetAxis("Fire1") == 0)
            triggerPulled = false;

        if (CrossPlatformInputManager.GetButtonDown("Fire2") && currentAmmoInMag < magSize && totalAmmo > 0) //X button
            Reload();
    }

    private void Shoot()
    {
        if (shotTimer == shotWaitPeriod && currentAmmoInMag > 0)
        {
            gunFX.Play();
            SoundManager.instance.Play(shotgunShot, "sfx");

            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                print(hit.transform.name);
            }
            currentAmmoInMag -= 1;
            totalAmmo -= 1;

            shotTimer = 0f;
        }
        if(shotTimer == shotWaitPeriod && currentAmmoInMag == 0)
            SoundManager.instance.Play(shotgunDryFire, "sfx");
    }

    private void Reload()
    {
        SoundManager.instance.Play(shotgunLoad, "sfx");

        if (totalAmmo > magSize)
        {
            currentAmmoInMag = magSize;
            totalAmmo -= magSize;
        }
        else
        {
            currentAmmoInMag = totalAmmo;
            totalAmmo -= totalAmmo;
        }
    }
}