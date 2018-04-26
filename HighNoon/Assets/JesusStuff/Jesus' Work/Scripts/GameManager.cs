using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float spawnTime = 4f;

    private void Update()
    {
        if ()
        {

        }
        StartCoroutine(Respawn());
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSecondsRealtime(spawnTime);

        if (//something)
        {
            #region Spawning
            int point = Random.Range(0, 7);

            switch (point)
            {
                case 0:
                    transform.position = GameObject.Find("SpawnPoint0").transform.position;
                    break;
                case 1:
                    transform.position = GameObject.Find("SpawnPoint1").transform.position;
                    break;
                case 2:
                    transform.position = GameObject.Find("SpawnPoint2").transform.position;
                    break;
                case 3:
                    transform.position = GameObject.Find("SpawnPoint3").transform.position;
                    break;
                case 4:
                    transform.position = GameObject.Find("SpawnPoint4").transform.position;
                    break;
                case 5:
                    transform.position = GameObject.Find("SpawnPoint5").transform.position;
                    break;
                case 6:
                    transform.position = GameObject.Find("SpawnPoint6").transform.position;
                    break;
                case 7:
                    transform.position = GameObject.Find("SpawnPoint7").transform.position;
                    break;
            }
            #endregion
            print("respawned");
        }
    }
}