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
    [SerializeField] private GameObject _newRecordsMessage;
    [SerializeField] private TextMeshProUGUI _textScoreGameOver;

    public static float timeLeft;
    public static bool gameIsPause = false;

    private bool gameOver = false;
    private bool saveResult = false;
    private float resultTime;
    private int minPosition = 0;
    private const string nameScenesGame = "Game";

    private List<string[]> allNote = new List<string[]>();

    void Start()
    {
        timeLeft = _time;
    }

    void Update ()
    {
        if (SceneManager.GetActiveScene().name == nameScenesGame)
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

        if (gameOver && !saveResult) 
        {
            Debug.Log(1111);
            SaveResultGame();
            gameOver = false;
        }
    }

    private void SaveResultGame()
    {

        int lastNote = 0;
        int maxIndex = 0;
        int max;

        for (int i = 0; i < 10; i++)
        {
            if (PlayerPrefs.HasKey($"N{i}"))
            {
                string[] records = TextParse(PlayerPrefs.GetString($"N{i}"));
                allNote.Add(records);
            }
        }


        if (PlayerPrefs.HasKey("lastNote") && allNote.Count != 0)
        {
            lastNote = PlayerPrefs.GetInt("lastNote");
            max = SearchMax();
            if (BoardLoad.score > max)
            {
                _newRecordsMessage.SetActive(true);
                if (lastNote == 9)
                {
                    maxIndex = SearchMin();
                    var today = DateTime.Now;
                    string record = today + "&" + BoardLoad.score;
                    string numRow = $"N{maxIndex}";
                    PlayerPrefs.SetString(numRow, record);
                    PlayerPrefs.Save();
                }
                else
                {
                    lastNote++;
                    var today = DateTime.Now;
                    string record = today + "&" + BoardLoad.score;
                    string numRow = $"N{lastNote}";

                    PlayerPrefs.SetString(numRow, record);
                    PlayerPrefs.SetInt("lastNote", lastNote);
                    PlayerPrefs.Save();
                    if (BoardLoad.score > max)
                    {
                        _newRecordsMessage.SetActive(true);
                    }
                }
            }
        }
        else
        {
            var today = DateTime.Now;
            string record = today + "&" + BoardLoad.score;
            string numRow = $"N{lastNote}";

            PlayerPrefs.SetString(numRow, record);
            PlayerPrefs.SetInt("lastNote", lastNote);
            PlayerPrefs.Save();

            _newRecordsMessage.SetActive(true);
        }

        saveResult = true;
    }

    private int SearchMax() 
    {
        int max = Convert.ToInt32(allNote[0][1]);
         
        for (int i = 1; i < allNote.Count; i++)
        {
            if (max < Convert.ToInt32(allNote[i][1]))
            {
                max = Convert.ToInt32(allNote[i][1]);
            }
        }
        return max;
    }

    private int SearchMin()
    {
        int min = Convert.ToInt32(allNote[0][1]);

        for (int i = 1; i < allNote.Count; i++)
        {
            if (min > Convert.ToInt32(allNote[i][1]))
            {
                min = Convert.ToInt32(allNote[i][1]);
                minPosition = i;
            }
        }
        return minPosition;
    }

    private string[] TextParse(string record)
    {
        return record.Split(new char[] { '&' });
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
        /*if (gameOver) 
        {
            SaveResultGame();
        }*/
        gameIsPause = false;
        gameOver = false;
        saveResult = false;
        Time.timeScale = 1f;
        timeLeft = _time;
        BoardLoad.score = 0;
        SceneManager.LoadScene(numScenes);
    }

    public void Exit()
    {
        _exitMenuUi.SetActive(true);
    }

    public void CloseNewRecordsMessage()
    {
        _newRecordsMessage.SetActive(false);
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
