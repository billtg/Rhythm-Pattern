using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public MenuNames menuButton;
    public GameObject ring;
    public Animator dotAnimator;

    public void ResetButton()
    {
        Debug.Log("Ding!");
        dotAnimator.SetTrigger("Deactivate");
    }

    private void OnMouseDown()
    {
        Debug.Log("Click");
        dotAnimator.SetTrigger("Activate");
        MenuController.instance.ActivateMenu(menuButton);
    }

    private void OnMouseOver()
    {
        ring.transform.localScale = new Vector3(1.2f,1.2f,1);
    }

    private void OnMouseExit()
    {
        ring.transform.localScale = Vector3.one;
    }
}
