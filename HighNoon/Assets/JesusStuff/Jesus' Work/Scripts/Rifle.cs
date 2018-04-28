using UnityEngine;

public class Rifle : MonoBehaviour
{
    public int ammoReserves;
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
        ammoReserves = magSize * 4;
        currentAmmoInMag = magSize;
    }

    private void Update()
    {
        if (Input.GetAxis("Right Trigger") == 1 && triggerPulled == false)
        {
            Shoot();
        }

        if (Input.GetAxis("Right Trigger") == 0)
            triggerPulled = false;

        if (Input.GetButtonDown("Right Bumper") && currentAmmoInMag < magSize && ammoReserves > 0) //X button
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

            RaycastHit hit;

            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                print(hit.transform.name);
            }

            currentAmmoInMag -= 1;
    }

    private void Reload()
    {
        SoundManager.instance.Play(rifleLoad, "sfx");

        while (currentAmmoInMag < magSize)
        {
            if (ammoReserves == 0)
                break;

            currentAmmoInMag++;
            ammoReserves--;
        }
    }
}