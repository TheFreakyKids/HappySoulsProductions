using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Rifle : MonoBehaviour
{
    public float damage = 65f;
    public float range = 100f;
    public int ammoCount = 1;
    public float shotVelocity = 1f;
    public bool triggerPulled = false;

    public AudioClip rifleShot;
    public AudioClip rifleLoad;
    public AudioClip rifleDryFire;

    public Camera fpsCam;
    public ParticleSystem gunFX;

    private void Update()
    {
        if(CrossPlatformInputManager.GetButtonDown("Fire2") && ammoCount == 0)//xbutton
        {
            Reload();
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetAxis("Fire1") == -1 && triggerPulled == false)
        {
            triggerPulled = true;
            Shoot();
        }
        if(Input.GetAxis("Fire1") == 0)
        {
            triggerPulled = false;
        }
    }

    private void Reload()
    {
        SoundManager.instance.Play(rifleLoad, "sfx");
        //reload animation
        ammoCount = 1;
    }

    private void Shoot()
    {
        if (ammoCount > 0)
        {
            gunFX.Play();
            SoundManager.instance.Play(rifleShot, "sfx");
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                print(hit.transform.name);
            }
            ammoCount -= 1;
        }
        if (ammoCount == 0)
        {
            SoundManager.instance.Play(rifleDryFire, "sfx");
        }
    }
}