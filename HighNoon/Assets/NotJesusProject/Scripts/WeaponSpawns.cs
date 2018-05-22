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
            if (this.tag == "SixShooter")
            {
                for (int i = 0; i < meshRend.Length; i++)
                {
                    meshRend[i].enabled = false;
                }
                other.GetComponent<Player>().revolverAmmoPool += 12;
                boxCol.enabled = false;
                for (int i = 0; i < effects.Length; i++)
                {
                    var foo = effects[i].emission;
                    foo.enabled = false;
                }
                StartCoroutine(WeaponRespawn(this.gameObject, meshRend));
            }
            if (this.tag == "Shotgun")
            {
                other.GetComponent<Player>().shotgunAmmoPool += 4;
                for (int i = 0; i < meshRend.Length; i++)
                {
                    meshRend[i].enabled = false;
                }
                boxCol.enabled = false;
                StartCoroutine(WeaponRespawn(this.gameObject, meshRend));
                for (int i = 0; i < effects.Length; i++)
                {
                    var foo = effects[i].emission;
                    foo.enabled = false;
                }
            }
            if (this.tag == "Rifle")
            {
                other.GetComponent<Player>().rifleAmmoPool += 6;
                for (int i = 0; i < meshRend.Length; i++)                
                {
                    meshRend[i].enabled = false;
                }
                boxCol.enabled = false;
                StartCoroutine(WeaponRespawn(this.gameObject, meshRend));
                for (int i = 0; i < effects.Length; i++)
                {
                    var foo = effects[i].emission;
                    foo.enabled = false;
                }
            }
        }
    }

    public IEnumerator WeaponRespawn(GameObject spawn, MeshRenderer[] meshs)
    {
        weaponIsRespawning = true;
        yield return new WaitForSeconds(weaponSpawnTime);

        for (int i = 0; i < meshs.Length; i++)
        {
            meshs[i].enabled = true;
        }
        boxCol.enabled = true;
        for (int i = 0; i < effects.Length; i++)
        {
            var foo = effects[i].emission;
            foo.enabled = true;
        }
        weaponIsRespawning = false;
      
    }
}