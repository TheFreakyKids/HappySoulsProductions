using System.Collections;
using UnityEngine;

public class WeaponSpawns : MonoBehaviour
{
    public bool weaponIsRespawning = false;
    public float weaponSpawnTime = 10f;
    public MeshRenderer[] meshRend;
    public BoxCollider boxCol;
    public ParticleSystem[] effects;
    public void Start()
    {
        meshRend = this.gameObject.GetComponentsInChildren<MeshRenderer>();
        boxCol = this.gameObject.GetComponent<BoxCollider>();

        effects = GetComponentsInChildren<ParticleSystem>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && weaponIsRespawning == false)
        {
            Debug.Log("fuck you");
            if (this.tag == "SixShooter")
            {
                print("Nabbed Sixshooter");

                other.GetComponent<Player>().revolverAmmoPoolP1 += 12;
                for (int i = 0; i < meshRend.Length; i++)
                {
                    meshRend[i].enabled = false;
                }
                boxCol.enabled = false;
                for (int i = 0; i < effects.Length; i++)
                {
                    effects[i].enableEmission = false;
                }
                StartCoroutine(WeaponRespawn(this.gameObject, meshRend));
            }
            if (this.tag == "Shotgun")
            {
                print("Nabbed Shotgun");

                other.GetComponent<Player>().shotgunAmmoPoolP1 += 4;
                for (int i = 0; i < meshRend.Length; i++)
                {
                    meshRend[i].enabled = false;
                }
                boxCol.enabled = false;
                StartCoroutine(WeaponRespawn(this.gameObject, meshRend));
            }
            if (this.tag == "Rifle")
            {
                print("Nabbed Rifle");

                other.GetComponent<Player>().rifleAmmoPoolP1 += 2;
                for (int i = 0; i < meshRend.Length; i++)
                {
                    meshRend[i].enabled = false;
                }
                boxCol.enabled = false;
                StartCoroutine(WeaponRespawn(this.gameObject, meshRend));
            }
        }
    }

    public IEnumerator WeaponRespawn(GameObject spawn, MeshRenderer[] meshs)
    {
        weaponIsRespawning = true;
        yield return new WaitForSeconds(weaponSpawnTime);

        Debug.Log("WEAPON RESPAWNED");
        for (int i = 0; i < meshs.Length; i++)
        {
            meshs[i].enabled = true;
        }
        boxCol.enabled = true;
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].enableEmission = true;
        }
        weaponIsRespawning = false;
       
    }
}