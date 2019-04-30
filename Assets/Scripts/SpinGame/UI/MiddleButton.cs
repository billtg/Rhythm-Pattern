using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiddleButton : MonoBehaviour
{
    public bool clickable = false;
    public static MiddleButton instance;
    public Animator animator;

    private void Awake()
    {
        instance = this;
    }

    public void SequenceComplete()
    {
        //play the animation
        animator.SetTrigger("SongComplete");
        clickable = true;
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

        //Take it back to the main menu
        SceneManager.LoadScene(0);
    }
}
