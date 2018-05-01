using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float weaponSpawnTime = 1f;
    [SerializeField] private bool weaponIsRespawning = false;
    [SerializeField] private float matchTime;
    [SerializeField] private float minsLeft;
    [SerializeField] private float secLeft;

    [SerializeField] private GameObject player;
    [SerializeField] private List<GameObject> Wspawns;
    public Text matchTimer;

    private void Awake()
    {
        Wspawns = new List<GameObject>();
        foreach (Transform spawn in GameObject.Find("WeaponSpawns").transform)
            Wspawns.Add(spawn.gameObject);

        player = GameObject.Find("Player");

        matchTime = 180f;
    }

    private void Update()
    {
        matchTime -= Time.deltaTime;
        matchTime = Mathf.Clamp(matchTime, 0, matchTime);
        matchTimer.text = Mathf.Round(matchTime).ToString();

        foreach (GameObject spawn in Wspawns) //For weapon spawns
        {
            if (Vector3.Distance(player.transform.position, spawn.gameObject.transform.position) <= 1f &&
                Input.GetAxisRaw("X") == 1 && spawn.gameObject.activeSelf == true && weaponIsRespawning == false &&
                player.GetComponent<Player>().isLookingAtWeaponSpawn == true)
            {
                #region Weapon Differentiation
                if (spawn.CompareTag("SixShooter") == true)
                {
                    print("Nabbed Sixshooter");

                    player.GetComponentInChildren<SixShooter>().AddAmmo(9);
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