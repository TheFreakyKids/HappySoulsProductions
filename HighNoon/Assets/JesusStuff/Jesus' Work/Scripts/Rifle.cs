using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Rifle : MonoBehaviour
{
    public float damage = 65f;
    public float range = 100f;
    public int ammoCount = 1;
    public float shotVelocity = 1f;

    public Camera fpsCam;
    public ParticleSystem gunFX;

    private void Update()
    {
        if(CrossPlatformInputManager.GetButtonDown("Fire2"))//xbutton
        {
            Reload();
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetAxis("Fire1") == -1)
        {
            Shoot();
        }
    }

    private void Reload()
    {
        //SoundManager.instance.Play(rifle load, "sfx");
        ammoCount = 1;
    }

    private void Shoot()
    {
        if (ammoCount > 0)
        {
            gunFX.Play();
            //SoundManager.instance.Play(rifle fire, "sfx");
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                print(hit.transform.name);
            }
            ammoCount -= 1;
        }
    }
}