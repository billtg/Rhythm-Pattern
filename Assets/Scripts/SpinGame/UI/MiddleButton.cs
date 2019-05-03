using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiddleButton : MonoBehaviour
{
    public bool clickable = false;
    public static MiddleButton instance;
    public Animator animator;
    public bool sequenceComplete;

    public GameObject optionsCanvas;
    public GameObject pauseCanvas;

    private void Awake()
    {
        instance = this;
    }

    public void SequenceComplete()
    {
        //play the animation
        animator.SetTrigger("SongComplete");
        clickable = true;
        sequenceComplete = true;
    }

    public void Pause ()
    {
        clickable = true;
    }

    public void UnPause()
    {
        if (!sequenceComplete)
            clickable = false;
    }

    private void OnMouseEnter()
    {
        if (!clickable) return;
        this.transform.localScale = new Vector3(1.2f, 1.2f, 1);
    }

    private void OnMouseExit()
    {
        if (!clickable) return;
        this.transform.localScale = Vector3.one;
    }

    private void OnMouseDown()
    {
        if (!clickable) return;

        //Will only be clickable in pause menu or when song is finished
        if (!Conductor.paused)
        {
            //Take it back to the main menu
            Debug.Log("Maain Menu");
            SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log("Conductor not paused");
            if (pauseCanvas.activeInHierarchy)
            {
                //resume playing
                Conductor.instance.Resume();
                UnPause();
            }
            else
            {
                //only occurs when on options screen
                pauseCanvas.SetActive(true);
                optionsCanvas.SetActive(false);
            }
        }
    }
}
