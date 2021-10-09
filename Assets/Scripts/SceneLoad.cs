using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoad : MonoBehaviour
{

    [SerializeField] private GameObject _exitMenuUi;
    [SerializeField] private float _time;
    [SerializeField] private TextMeshProUGUI _textTime;
    [SerializeField] private GameObject _gameOverMenu;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private TextMeshProUGUI _textScoreGameOver;
    public static float timeLeft;
    public static bool gameIsPause = false;
    bool gameOver = false;
    float resultTime;

    void Start()
    {
        timeLeft = _time;
    }

    void Update ()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                resultTime = (float)System.Math.Round(timeLeft, 1);
                _textTime.text = resultTime.ToString();
            }
            else
            {
                Time.timeScale = 0f;
                gameIsPause = true;
                gameOver = true;
                _textScoreGameOver.text = BoardLoad.score.ToString();
                _gameOverMenu.SetActive(true);
            }
        }
    }

    void SaveResultGame()
    {
        int lastNote = 0;

        if (PlayerPrefs.HasKey("lastNote"))
        {
            lastNote = PlayerPrefs.GetInt("lastNote");
            if (lastNote == 9)
            {
                lastNote = 0;
            }
            else
            {
                lastNote++;
            }
        }

        var today = DateTime.Now;
        string record = today + "&" + BoardLoad.score;
        string numRow = $"N{lastNote}";

        PlayerPrefs.SetString(numRow, record);
        PlayerPrefs.SetInt("lastNote", lastNote); ;
        PlayerPrefs.Save();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        gameIsPause = true;
        _pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        gameIsPause = false;
        _pauseMenu.SetActive(false);
    }

    public void ChangeScenes(int numScenes)
    {
        if (gameOver) 
        {
            SaveResultGame();
        }
        gameIsPause = false;
        gameOver = false;
        Time.timeScale = 1f;
        timeLeft = _time;
        BoardLoad.score = 0;
        SceneManager.LoadScene(numScenes);
    }

    public void Exit()
    {
        _exitMenuUi.SetActive(true);
    }

    public void CancelExit()
    {
        _exitMenuUi.SetActive(false);
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
