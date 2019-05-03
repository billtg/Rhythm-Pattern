using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public enum PauseMenuButtons { Mainmenu, Options, Quit }
    public PauseMenuButtons buttonID;
    public GameObject optionsCanvas;
    public GameObject pauseCanvas;
    
    public GameObject ring;

    private void OnMouseDown()
    {
        switch (buttonID)
        {
            case PauseMenuButtons.Mainmenu:
                MainMenu();
                break;
            case PauseMenuButtons.Options:
                Options();
                break;
            case PauseMenuButtons.Quit:
                QuitGame();
                break;
        }
    }


    private void OnMouseOver()
    {
        ring.transform.localScale = new Vector3(1.2f, 1.2f, 1);
    }

    private void OnMouseExit()
    {
        ring.transform.localScale = Vector3.one;
    }

    public void Resume()
    {
        Conductor.instance.Resume();
    }

    public void Options()
    {
        //Debug.Log("Sorry, no options yet");
        optionsCanvas.SetActive(true);
        pauseCanvas.SetActive(false);

    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
