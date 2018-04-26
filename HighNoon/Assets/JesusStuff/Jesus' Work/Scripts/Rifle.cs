using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Rifle : MonoBehaviour
{
    public int totalAmmo;
    public int currentAmmoInMag;
    public int magSize = 1;
    public float damage = 65f;
    public float range = 100f;
    public bool triggerPulled = false;

    public AudioClip rifleShot;
    public AudioClip rifleLoad;
    public AudioClip rifleDryFire;
    public Camera fpsCam;
    public ParticleSystem gunFX;

    private void Awake()
    {
        totalAmmo = magSize * 4;
        currentAmmoInMag = magSize;
    }

    private void Update()
    {
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
        if (currentAmmoInMag > 0)
        {
            gunFX.Play();
            SoundManager.instance.Play(rifleShot, "sfx");

            RaycastHit hit;

            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                print(hit.transform.name);
            }

            currentAmmoInMag -= 1;
            totalAmmo -= 1;
        }

        if (currentAmmoInMag == 0)
        {
            SoundManager.instance.Play(rifleDryFire, "sfx");
        }
    }

    private void Reload()
    {
        SoundManager.instance.Play(rifleLoad, "sfx");

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