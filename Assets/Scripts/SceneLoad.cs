using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{

    [SerializeField] private GameObject _exitMenuUi;

    void Update ()
    {
        
    }
    public void Resume()
    {

    }

    public void ChangeScenes(int numScenes)
    {
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
