using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SixShooter : MonoBehaviour
{
    public int totalAmmo;
    public int magSize = 6;
    public int currentAmmoInMag;
    public float damage = 35f;
    public float range = 50f;
    public float shotTimer = .5f;
    public float shotWaitPeriod = .5f;
    public bool triggerPulled = false;

    public AudioClip revolverShot;
    public AudioClip revolverLoad;
    public AudioClip revolverDryFire;
    public Camera fpsCam;
    public ParticleSystem gunFX;

    private void Awake()
    {
        totalAmmo = magSize * 2;
        currentAmmoInMag = magSize;
    }
	
	private void Update ()
    {
        shotTimer += Time.deltaTime;
        shotTimer = Mathf.Clamp(shotTimer, 0f, .5f);

        if (CrossPlatformInputManager.GetAxis("Fire1") == -1 && triggerPulled == false) //Right Trigger
        {
            triggerPulled = true;
            Shoot();
        }
        if (CrossPlatformInputManager.GetAxis("Fire1") == 0)
            triggerPulled = false;

        if (CrossPlatformInputManager.GetButtonDown("Fire2") && currentAmmoInMag < magSize && totalAmmo > 0) //X Button
            Reload();
    }

    private void Shoot()
    {
        if (shotTimer == shotWaitPeriod && currentAmmoInMag > 0)
        {
            gunFX.Play();
            SoundManager.instance.Play(revolverShot, "sfx");
            RaycastHit hit;
            if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
                print(hit.transform.name);

            currentAmmoInMag -= 1;
            totalAmmo -= 1;

            shotTimer = 0f;
        }
        else if (shotTimer == shotWaitPeriod && currentAmmoInMag == 0)
        {
            SoundManager.instance.Play(revolverDryFire, "sfx");
            Debug.Log("Revolver is empty.");
        }
    }

    private void Reload()
    {
        SoundManager.instance.Play(revolverLoad, "sfx");

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

    public void AddAmmo(int outsideAmmo)
    {
        totalAmmo += outsideAmmo;
    }
}