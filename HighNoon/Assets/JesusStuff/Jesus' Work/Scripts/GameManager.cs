using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float weaponSpawnTime = 1f;
    [SerializeField] private GameObject player;

    [SerializeField] private List<GameObject> Wspawns;

    private void Awake()
    {
        Wspawns = new List<GameObject>();
        foreach (Transform spawn in GameObject.Find("WeaponSpawns").transform)
            Wspawns.Add(spawn.gameObject);

        player = GameObject.Find("Player");
    }

    private void Update()
    {
        foreach (GameObject spawn in Wspawns)
        {
            if (Vector3.Distance(player.transform.position, spawn.gameObject.transform.position) < 1f && Input.GetAxisRaw("X") == 1 && spawn.gameObject.active == true)
            {
                player.GetComponentInChildren<SixShooter>().AddAmmo(6);
                spawn.gameObject.active = false;
                Debug.Log("WEAPON GONE");

                Debug.Log("BEGINNING SPAWNING");
                StartCoroutine(WeaponRespawn(spawn));
            }
        }

        //Checks for empty spawns
        foreach (GameObject spawn in Wspawns)
        {
            if (spawn.activeInHierarchy == false)
            {

            }
        }
    }

    public IEnumerator WeaponRespawn(GameObject spawn)
    {
        yield return new WaitForSeconds(weaponSpawnTime);

        Debug.Log("SPAWNED");
        spawn.gameObject.active = true;
    }
}