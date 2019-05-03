using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MenuScript : MonoBehaviour
{
    public bool menuActive = false;
    private Animator animator;
    private MenuButton[] menuButtons;
    public AudioMixer audioMixer;

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

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
