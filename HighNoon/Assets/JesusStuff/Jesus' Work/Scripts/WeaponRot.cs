using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRot : MonoBehaviour
{
    public Transform weaponRot;
    public GameObject[] players;
    public float[] dist;
    public float speed;

	void Awake ()
    {
        weaponRot = transform;
        players = GameObject.FindGameObjectsWithTag("Player");
        dist = new float[players.Length];
    }
	
	void Update ()
    {
        for (int i = 0; i < players.Length; i++)
        {
            dist[i] = Vector3.Distance(players[i].transform.position, transform.position);
        }

        float smallestDist = Mathf.Min(dist);
        float distMod = smallestDist / smallestDist / smallestDist;

        weaponRot.Rotate(Vector3.up * Time.deltaTime * speed * distMod);
	}
}