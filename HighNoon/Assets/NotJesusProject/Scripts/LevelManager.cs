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
    }
    
	void Update ()
    {
    }

    
}
