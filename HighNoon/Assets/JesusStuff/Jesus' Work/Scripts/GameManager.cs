using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private float matchTime;
    [SerializeField] private float minsLeft;
    [SerializeField] private float secLeft;
    public Text matchTimer;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        matchTime -= Time.deltaTime;
        matchTime = Mathf.Clamp(matchTime, 0, matchTime);
        matchTimer.text = Mathf.Round(matchTime).ToString();
    }
}