using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField]
    private float weaponSpawnTime = 5f;
    [SerializeField]
    private bool weaponIsRespawning = false;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private List<GameObject> Wspawns;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        Wspawns = new List<GameObject>();
        foreach (Transform spawn in GameObject.Find("WeaponSpawns").transform)
            Wspawns.Add(spawn.gameObject);

        player = GameObject.Find("Player");
    }
    
	void Update ()
    {
        foreach (GameObject spawn in Wspawns) //For weapon spawns
        {
            if (Vector3.Distance(player.transform.position, spawn.gameObject.transform.position) <= 2f &&
                Input.GetButtonDown("X") && spawn.gameObject.activeSelf == true /*&& weaponIsRespawning == false*/ &&
                player.GetComponent<Player>().isLookingAtWeaponSpawn == true)
            {
                #region Weapon Differentiation
                if (spawn.CompareTag("SixShooter") == true)
                {
                    print("Nabbed Sixshooter");

                    player.GetComponentInChildren<SixShooter>().AddAmmo(12);
                    spawn.gameObject.SetActive(false);
                    StartCoroutine(WeaponRespawn(spawn));
                }
                if (spawn.CompareTag("Shotgun") == true)
                {
                    print("Nabbed Shotgun");

                    player.GetComponentInChildren<Shotgun>().AddAmmo(4);
                    spawn.gameObject.SetActive(false);
                    StartCoroutine(WeaponRespawn(spawn));
                }
                if (spawn.CompareTag("Rifle") == true)
                {
                    print("Nabbed Rifle");

                    player.GetComponentInChildren<Rifle>().AddAmmo(2);
                    spawn.gameObject.SetActive(false);
                    StartCoroutine(WeaponRespawn(spawn));
                }
                #endregion
            }
        }
    }

    public IEnumerator WeaponRespawn(GameObject spawn)
    {
        weaponIsRespawning = true;
        yield return new WaitForSeconds(weaponSpawnTime);

        Debug.Log("WEAPON RESPAWNED");
        spawn.gameObject.SetActive(true);
        weaponIsRespawning = false;
    }
}
