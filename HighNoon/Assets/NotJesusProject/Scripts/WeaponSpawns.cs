using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawns : MonoBehaviour
{
    public bool weaponIsRespawning = false;
    float weaponSpawnTime = 10f;
    public MeshRenderer meshRend;
    public BoxCollider boxCol;
    [SerializeField]
    public List<MeshRenderer> kids;
    public void Start()
    {
        meshRend = this.gameObject.GetComponent<MeshRenderer>();
        boxCol = this.gameObject.GetComponent<BoxCollider>();
    }
    public void OnTriggerEnter(Collider other)
    {
        List<MeshRenderer> kids = new List<MeshRenderer>();
        if (other.gameObject.tag == "Player" && weaponIsRespawning == false)
        {
            Debug.Log("fuck you");
            if (this.tag == "SixShooter")
            {
                kids.AddRange(GetComponentsInChildren<MeshRenderer>());
                print("Nabbed Sixshooter");

                other.GetComponent<Player>().revolverAmmoPoolP1 += 12;
                meshRend.enabled = false;
                boxCol.enabled = false;
                foreach (MeshRenderer mRend in kids)
                {
                    mRend.enabled = false;
                }
                StartCoroutine(WeaponRespawn(this.gameObject, kids));
            }
            if (this.tag == "Shotgun")
            {
                kids.AddRange(GetComponentsInChildren<MeshRenderer>());
                print("Nabbed Shotgun");

                other.GetComponent<Player>().shotgunAmmoPoolP1 += 4;
               // this.gameObject.SetActive(false);
                meshRend.enabled = false;
                boxCol.enabled = false;
                foreach (MeshRenderer mRend in kids)
                {
                    mRend.enabled = false;
                }
                StartCoroutine(WeaponRespawn(this.gameObject, kids));
            }
            if (this.tag == "Rifle")
            {
                kids.AddRange(GetComponentsInChildren<MeshRenderer>());
                print("Nabbed Rifle");

                other.GetComponent<Player>().rifleAmmoPoolP1 += 2;
               // this.gameObject.SetActive(false);
                meshRend.enabled = false;
                boxCol.enabled = false;
                foreach (MeshRenderer mRend in kids)
                {
                    mRend.enabled = false;
                }
                StartCoroutine(WeaponRespawn(this.gameObject, kids));
            }
        }
    }

    public IEnumerator WeaponRespawn(GameObject spawn, List<MeshRenderer> rend)
    {
        weaponIsRespawning = true;
        yield return new WaitForSeconds(weaponSpawnTime);

        Debug.Log("WEAPON RESPAWNED");
        meshRend.enabled = true;
        boxCol.enabled = true;
        foreach (MeshRenderer mRend in rend)
        {
            mRend.enabled = true;
        }
        weaponIsRespawning = false;
        
    }
}
