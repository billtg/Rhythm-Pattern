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
    public GameObject backText;

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
        backText.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (!clickable) return;
        this.transform.localScale = Vector3.one;
        backText.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (!clickable) return;

        //Will only be clickable in pause menu or when song is finished
        if (!Conductor.paused)
        {
            //Load the next song
            Debug.Log("Check for end of songs");
            if (SongLoader.instance.NextSong())
            {
                Debug.Log("Not at end of songs. Reloading Main with new songIndex");
                SceneManager.LoadScene(1);
            }
            else
            {
                Debug.Log("End of songs. Load Main Menu");
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            Debug.Log("Conductor not paused");
            if (pauseCanvas.activeInHierarchy)
            {
                //resume playing
                Conductor.instance.Resume();
                UnPause();
                //Also eliminate side effects from being highlighted
                this.transform.localScale = Vector3.one;
                backText.SetActive(false);
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
