using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    public GameObject ring;

    private void OnMouseDown()
    {
        Debug.Log("Resetting Data");
        PlayerPrefs.SetInt("Dance", 0);
        PlayerPrefs.SetInt("Hiphop", 0);
        PlayerPrefs.SetInt("FirstTime", 0);
        SceneManager.LoadScene(0);
    }
    private void OnMouseOver()
    {
        ring.transform.localScale = new Vector3(1.2f, 1.2f, 1);
    }

    private void OnMouseExit()
    {
        ring.transform.localScale = Vector3.one;
    }
}
