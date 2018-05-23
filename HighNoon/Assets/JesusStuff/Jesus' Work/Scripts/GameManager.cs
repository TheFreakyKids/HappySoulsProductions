using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        DeathCheck();
        VictoryCheck();
    }

    void DeathCheck()
    {
        if (player2.gameObject.GetComponent<Player>().died == true)
        {
            //player 2 died
            player1Points += 1;
            player2.gameObject.GetComponent<Player>().died = false;
            player1ScoreDisplay.text = player1Points.ToString();
        }
        if (player1.gameObject.GetComponent<Player>().died == true)
        {
            //player 1 died
            player2Points += 1;
            player1.gameObject.GetComponent<Player>().died = false;
            player2ScoreDisplay.text = player2Points.ToString();
        }
    }

    void VictoryCheck()
    {
        if (player2Points == 10)
        {
            player2Win = true;
            SceneManager.LoadScene("Player2Win");
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("p2 wins");
        }
        if (player1Points == 2)
        {
            player1Win = true;
            SceneManager.LoadScene("Player1Win");
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("p1 wins");
        }
        if (matchTime == 0)
        {
            if(player1Points > player2Points)
            {
                player1Win = true;
            }
            else if(player2Points > player1Points)
            {
                player2Win = true;
            }
            else if(player1Points == player2Points)
            {
                //tie
            }
        }
    }
}