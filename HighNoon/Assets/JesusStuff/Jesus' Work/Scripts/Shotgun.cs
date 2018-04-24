using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Shotgun : MonoBehaviour
{
    public int ammoCount = 2;
    public int pelletCount;
    public float spreadAngle;
    public float pelletVelocity = 1f;
    public float shotTimer = .5f;
    public float shotWaitPeriod = .5f;

    public GameObject pellet;
    public Transform barrelExit;
    List<Quaternion> pellets;
    public ParticleSystem gunFX;

    private void Awake ()
    {
        pellets = new List<Quaternion>(pelletCount);
        for (int i = 0; i < pelletCount; i++)
        {
            pellets.Add(Quaternion.Euler(Vector3.zero));
        }
	}
	
	private void FixedUpdate ()
    {
		if(Input.GetAxis("Fire1") == -1)
            Shoot();
	}

    private void Update()
    {
        shotTimer += Time.deltaTime;
        shotTimer = Mathf.Clamp(shotTimer, 0f, .5f);
        if (CrossPlatformInputManager.GetButtonDown("Fire2"))//xbutton
        {
            Reload();
        }
    }

    private void Shoot()
    {
        if (shotTimer == shotWaitPeriod && ammoCount > 0)
        {
            gunFX.Play();
            int i = 0;
            foreach (Quaternion quat in pellets)
            {
                pellets[i] = Random.rotation;
                GameObject pell = Instantiate(pellet, barrelExit.position, barrelExit.rotation);
                pell.transform.rotation = Quaternion.RotateTowards(pellet.transform.rotation, pellets[i], spreadAngle);
                pell.GetComponent<Rigidbody>().AddForce(barrelExit.transform.forward * pelletVelocity);
                Destroy(pell, .3f);
                i++;
            }
            ammoCount -= 1;
            shotTimer = 0f;
        }
    }

    private void Reload()
    {
        ammoCount = 2;
    }
}