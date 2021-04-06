using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public bool gameOver = false;
    public bool gameStart = false;

    public GameObject retryButton;
    public GameObject startButton;

    public Text text;
    public GameObject playerDiedText;

    public string whoWon;

    float currentTime = 0f;
    float startingTime = 3f;

    void Start()
    {
        currentTime = startingTime;
    }


    void Update()
    {

        currentTime -= Time.deltaTime;

        if(gameOver)
        {
            retryButton.SetActive(true);
        }
        Debug.Log(whoWon);
    }

    public void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void startGame()
    {
        gameStart = true;
        startButton.SetActive(false);
    }

    public void PlayerDied()
    {
        gameOver = true;
        playerDiedText.SetActive(true);
    }

    public void gameFinished()
    {
        gameOver = true;
        retryButton.SetActive(true);
        text.text = "Game Finished";
    }
}
