using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    public MenuScript[] menuScripts;
    public Animator animator;
    public static MenuController instance;
    public TextMeshPro levelsBeaten;

    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
        menuScripts = GetComponentsInChildren<MenuScript>(true);
    }
    public void Start()
    {
        animator.SetTrigger("StartMenu");
    }

    public void ActivateMenu(MenuNames menuName)
    {
        //Debug.Log("Deactivating Menus " + menuName);
        //Deactivate any active menus
        for (int i = 0; i < menuScripts.Length; i++)
        {
            if (menuScripts[i].menuActive)
                menuScripts[i].DeactivateMenu();
        }

        //Activate the menuName menu
        //Debug.Log("Activating Menu " + menuName);
        switch (menuName)
        {
            case MenuNames.main:
                menuScripts[0].gameObject.SetActive(true);
                menuScripts[0].ActivateMenu();
                break;
            case MenuNames.options:
                menuScripts[1].gameObject.SetActive(true);
                menuScripts[1].ActivateMenu();
                break;
            case MenuNames.levelSelect:
                menuScripts[2].gameObject.SetActive(true);
                menuScripts[2].ActivateMenu();
                break;
            case MenuNames.quit:
                Debug.Log("Quitting");
                Application.Quit();
                break;
        }

    }
}
public enum MenuNames { main, options, levelSelect, quit };
