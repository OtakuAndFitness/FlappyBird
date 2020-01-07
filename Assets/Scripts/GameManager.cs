using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;
    public static GameManager Instance;
    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;
    public Text scoreText;
    enum PageState{
        None,
        Start,
        GameOver,
        Countdown
    }
    int score = 0;
    bool gameOver = true;

    public bool GameOver{
        get{
            return gameOver;
        }
    }

    public int Score{
        get{
            return score;
        }
    }

    private void Awake() {
        Instance = this;
    }

    private void OnEnable() {
        CountdownText.OnCountdownFinished += OnCountdownFinished;
        TapController.OnPlayerScored += OnPlayerScored;
        TapController.OnPlayerDied += OnPlayerDied;
    }

    private void OnDisable() {
        CountdownText.OnCountdownFinished -= OnCountdownFinished;
        TapController.OnPlayerScored -= OnPlayerScored;
        TapController.OnPlayerDied -= OnPlayerDied;
    }

    void OnCountdownFinished(){
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
        gameOver = false;
    }

    void OnPlayerDied(){
        gameOver = true;
        SetPageState(PageState.GameOver);
        int savedScore = PlayerPrefs.GetInt("HighScore");
        if (savedScore < score){
            PlayerPrefs.SetInt("HighScore",score);
        }
    }

    void OnPlayerScored(){
        score++;
        scoreText.text = score.ToString();
    }

    void SetPageState(PageState state){
        switch (state)
        {
            case PageState.None:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;
            case PageState.Start:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;
            case PageState.GameOver:
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                countdownPage.SetActive(false);
                break;
            case PageState.Countdown:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(true);
                break;
            
        }
    }
    
    public void ConfirmGameOver(){
        OnGameOverConfirmed();
        SetPageState(PageState.Start);
        scoreText.text = "0";
    }

    public void StartGame(){
        SetPageState(PageState.Countdown);

    }
}
