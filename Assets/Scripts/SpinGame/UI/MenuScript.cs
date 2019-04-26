using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public bool menuActive = false;
    private Animator animator;
    private MenuButton[] menuButtons;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        menuButtons = GetComponentsInChildren<MenuButton>();
    }

    public void ActivateMenu()
    {
        animator.SetTrigger("Activate");
        menuActive = true;
        foreach (MenuButton menuButton in menuButtons)
            menuButton.ResetButton();
    }

    public void DeactivateMenu()
    {
        animator.SetTrigger("Deactivate");
        menuActive = false;
    }

    public void SetInactive()
    {
        this.gameObject.SetActive(false);
    }
}
