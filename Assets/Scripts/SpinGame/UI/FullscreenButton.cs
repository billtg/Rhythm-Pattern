using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenButton : MonoBehaviour
{
    public Animator dotAnimator;
    public bool isFullScreen;
    public GameObject ring;
    public GameObject dot;

    private void Awake()
    {
        if (Screen.fullScreen)
        {
            isFullScreen = true;
            dotAnimator.SetTrigger("Expand");
        }
        else
            isFullScreen = false;
    }

    private void OnMouseDown()
    {
        if (isFullScreen)
        {
            Screen.fullScreen = false;
            isFullScreen = false;
            dotAnimator.SetTrigger("Deflate");
        }
        else
        {
            Screen.fullScreen = true;
            isFullScreen = true;
            dotAnimator.SetTrigger("Expand");
        }
    }

    private void OnMouseOver()
    {
        ring.transform.localScale = new Vector3(1.2f, 1.2f, 1);
        dot.transform.localScale = new Vector3(1.2f, 1.2f, 1);
    }

    private void OnMouseExit()
    {
        ring.transform.localScale = Vector3.one;
        dot.transform.localScale = Vector3.one;
    }
}
