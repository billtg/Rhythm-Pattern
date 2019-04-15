using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    public enum MenuScreen {main, levelSelect, options};

    public MenuScreen currentMenuScreen;

    public Text leftText;
    public Text rightText;
    public Text middleText;

    public Animator midTextAnimator;
    public Animator leftTextAnimator;
    public Animator rightTextAnimator;

    public Animator playAnimator;
    public Animator optionsAnimator;
    public Animator exitAnimator;
    public Animator ringAnimator;

    public static MenuScript instance;

    private void Awake()
    {
        instance = this;
        currentMenuScreen = MenuScreen.main;
    }

    public void MiddleButton()
    {
        switch (currentMenuScreen)
        {
            case MenuScreen.main:
                Debug.Log("Play");
                currentMenuScreen = MenuScreen.levelSelect;
                //Should probably disable the other buttons here
                playAnimator.SetTrigger("expand");
                ringAnimator.SetTrigger("transition");
                midTextAnimator.SetTrigger("transition");
                leftTextAnimator.SetTrigger("transition");
                rightTextAnimator.SetTrigger("transition");
                break;
            case MenuScreen.options:
                Debug.Log("Back to main menu");
                currentMenuScreen = MenuScreen.main;
                //Something something disable buttons
                playAnimator.SetTrigger("expand");
                ringAnimator.SetTrigger("transition");
                midTextAnimator.SetTrigger("transition");
                leftTextAnimator.SetTrigger("enable");
                rightTextAnimator.SetTrigger("enable");
                break;
            case MenuScreen.levelSelect:
                Debug.Log("Back to main menu");
                currentMenuScreen = MenuScreen.main;
                //Something something disable buttons
                playAnimator.SetTrigger("expand");
                ringAnimator.SetTrigger("transition");
                midTextAnimator.SetTrigger("transition");
                leftTextAnimator.SetTrigger("enable");
                rightTextAnimator.SetTrigger("enable");
                break;
        }
    }

    public void LeftButton()
    {
        switch (currentMenuScreen)
        {
            case MenuScreen.main:
                Debug.Log("Options");
                currentMenuScreen = MenuScreen.options;
                optionsAnimator.SetTrigger("expand");
                ringAnimator.SetTrigger("transition");
                midTextAnimator.SetTrigger("transition");
                leftTextAnimator.SetTrigger("disable");
                rightTextAnimator.SetTrigger("disable");
                break;
            case MenuScreen.options:
                break;
            case MenuScreen.levelSelect:
                Debug.Log("Load level 1");
                SceneManager.LoadScene(1);
                break;
        }
    }

    public void Back()
    {
        Debug.Log("Back");
        currentMenuScreen = MenuScreen.main;
        ringAnimator.SetTrigger("transition");
    }

    public void RightButton()
    {
        switch (currentMenuScreen)
        {
            case MenuScreen.main:
                exitAnimator.SetTrigger("expand");
                Debug.Log("Quitting Game");
                Application.Quit();
                break;
            case MenuScreen.options:
                break;
            case MenuScreen.levelSelect:
                SceneManager.LoadScene(2);
                break;
        }
    }

    public void LoadScreenButtons()
    {
        //This gets called after the buttons have been collapsed into the centre.
        //Load the correct buttons based on whatever screen you're on
        //the level select one will eventually need to have an idea of how many levels you've beaten
        switch (currentMenuScreen)
        {
            case MenuScreen.main:
                middleText.text = "Play";
                leftText.text = "Options";
                rightText.text = "Quit";
                playAnimator.SetTrigger("collapse");
                break;
            case MenuScreen.levelSelect:
                middleText.text = "Back";
                leftText.text = "1";
                rightText.text = "2";
                playAnimator.SetTrigger("collapse");
                break;
            case MenuScreen.options:
                Debug.Log("No options yet. Sorry!");
                middleText.text = "Back";
                optionsAnimator.SetTrigger("collapse");
                break;
        }
    }
}
