﻿using UnityEngine;

public class SixShooter : MonoBehaviour
{
    public float damage = 35f;
    public float range = 50f;
    public int ammoCount = 6;
    public float shotVelocity = 1f;
    public float shotTimer = .5f;
    public float shotWaitPeriod = .5f;

    //public GameObject revolverBullet;
    //public Transform barrelExit;
    public Camera fpsCam;
    public ParticleSystem gunFX;
	
	private void Update ()
    {
        shotTimer += Time.deltaTime;
        shotTimer = Mathf.Clamp(shotTimer, 0f, .5f);
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
	}

    private void FixedUpdate()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Reload()
    {
        ammoCount = 6;
    }

    private void Shoot()
    {
        if (shotTimer == shotWaitPeriod && ammoCount > 0)
        {
            //GameObject bullet = Instantiate(revolverBullet, barrelExit.position, barrelExit.rotation);
            //bullet.GetComponent<Rigidbody>().AddForce(barrelExit.transform.forward * shotVelocity);

            gunFX.Play();

            RaycastHit hit;
            if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                print(hit.transform.name);
            }

            ammoCount -= 1;
            shotTimer = 0f;
        }
    }
}