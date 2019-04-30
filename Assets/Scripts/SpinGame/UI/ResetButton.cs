using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("Resetting Data");
        PlayerPrefs.SetInt("Dance", 0);
        PlayerPrefs.SetInt("Hiphop", 0);
        SceneManager.LoadScene(0);
    }
}
