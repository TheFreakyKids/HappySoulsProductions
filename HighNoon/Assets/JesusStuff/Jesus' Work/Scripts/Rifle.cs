﻿using UnityEngine;

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
        ammoCount = 1;
    }

    private void Shoot()
    {
        if (ammoCount > 0)
        {
            gunFX.Play();

            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                print(hit.transform.name);
            }
            ammoCount -= 1;
        }
    }
}