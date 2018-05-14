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
    private int player1Points;
    private int player2Points;
    public Text player1ScoreDisplay;
    public Text player2ScoreDisplay;
    public bool player1Win = false;
    public bool player2Win = false;
    public GameObject player1;
    public GameObject player2;

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
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        matchTime -= Time.deltaTime;
        matchTime = Mathf.Clamp(matchTime, 0, matchTime);
        matchTimer.text = Mathf.Round(matchTime).ToString();
        if(Input.GetKey(KeyCode.A))
        {
            player2.gameObject.GetComponent<Player>().died = true;
        }
        DeathCheck();
        VictoryCheck();
    }

    void DeathCheck()
    {
        if (player2.gameObject.GetComponent<Player>().died == true)
        {
            //player 2 died
            player1Points++;
            player1ScoreDisplay.text = player1Points.ToString();
        }
        if (player1.gameObject.GetComponent<Player>().died == true)
        {
            //player 1 died
            player2Points++;
            player2ScoreDisplay.text = player2Points.ToString();
        }
    }

    void VictoryCheck()
    {
        if (player2Points == 10)
        {
            player2Win = true;
            Debug.Log("p2 wins");
        }
        if (player1Points == 10)
        {
            player1Win = true;
            Debug.Log("p1 wins");
        }
    }
}