using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public GameObject playText;

    private void OnMouseDown()
    {
        PlayerPrefs.SetInt("FirstTime", 1);
        SongLoader.instance.LoadScene(1, SongType.Dance);
        SceneManager.LoadScene(1);
    }

    private void OnMouseEnter()
    {
        this.transform.localScale = new Vector3(1.2f, 1.2f, 1);
        playText.SetActive(true);
    }

    private void OnMouseExit()
    {
        this.transform.localScale = Vector3.one;
        playText.SetActive(false);
    }
}
