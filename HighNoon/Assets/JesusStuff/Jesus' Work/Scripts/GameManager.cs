using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject p1win;
    public GameObject p2win;
    public GameObject levelUI;
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
    void OnSceneLoaded()
    {
        Debug.Log("hi");
        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        levelUI = GameObject.Find("GameUI");
        matchTimer = GameObject.FindGameObjectWithTag("ExplosiveBarrel").GetComponent<Text>();
        Debug.Log(player1Points + " update function");
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        matchTime -= Time.deltaTime;
        matchTime = Mathf.Clamp(matchTime, 0, matchTime);
        matchTimer.text = Mathf.Round(matchTime).ToString();
        DeathCheck();
        VictoryCheck();
        if (p1win.activeSelf || p2win.activeSelf)
        {
            if(Input.GetButton("A1") || Input.GetButton("A2"))
            {
                p1win.SetActive(false);
                p2win.SetActive(false);
                SceneManager.LoadScene("Menu");
                levelUI.gameObject.GetComponent<Canvas>().enabled = true;
            }
        }
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
        if (player2Points == 5)
        {
            player2Win = true;
            StartCoroutine(SlowMo());
            Debug.Log("p2 wins");
            levelUI.gameObject.GetComponent<Canvas>().enabled = false;
            p2win.SetActive(true);
            player1Points = 0;
            player2Points = 0;
            player2Win = false;
        }
        if (player1Points == 5)
        {
            player1Win = true;
            StartCoroutine(SlowMo());
            Debug.Log("p1 wins");
            levelUI.gameObject.GetComponent<Canvas>().enabled = false;
            p1win.SetActive(true);
            player1Points = 0;
            player2Points = 0;
            player1Win = false;
            
            Debug.Log(player1Points + " win checker function" + player1Win);
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
    IEnumerator SlowMo()
    {
        Time.timeScale = 0.4f;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        Cursor.lockState = CursorLockMode.None;
        yield return new WaitForSeconds(.932f);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
}