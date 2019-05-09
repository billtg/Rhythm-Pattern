using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour {

    public KeyCode up;
    public KeyCode right;
    public KeyCode down;
    public KeyCode left;
    public KeyCode pause;
    public KeyCode assist;
    public KeyCode assistAlt;

    private void Update()
    {
        if (!Conductor.paused)
        {
            if (Input.GetKeyDown(up))
            {
                //Debug.Log("Up Pushed on loop beat:" + Conductor.instance.loopPosInBeats.ToString());
                EventManager.TriggerEvent(up.ToString());
            }

            if (Input.GetKeyDown(right))
            {
                //Debug.Log("Right Pushed on loop beat:" + Conductor.instance.loopPosInBeats.ToString());
                EventManager.TriggerEvent(right.ToString());
            }

            if (Input.GetKeyDown(down))
            {
                //Debug.Log("Down Pushed on loop beat:" + Conductor.instance.loopPosInBeats.ToString());
                EventManager.TriggerEvent(down.ToString());
            }

            if (Input.GetKeyDown(left))
            {
                //Debug.Log("Left Pushed on loop beat:" + Conductor.instance.loopPosInBeats.ToString());
                EventManager.TriggerEvent(left.ToString());
            }

            if (Input.GetKeyDown(assist))
            {
                //Debug.Log("Assist started");
                EventManager.TriggerEvent("assistStart");
            }

            if (Input.GetKeyUp(assist))
            {
                //Debug.Log("Assist stopped");
                EventManager.TriggerEvent("assistStop");
            }

            if (Input.GetKeyDown(assistAlt))
            {
                //Debug.Log("Assist started");
                EventManager.TriggerEvent("assistStart");
            }

            if (Input.GetKeyUp(assistAlt))
            {
                //Debug.Log("Assist stopped");
                EventManager.TriggerEvent("assistStop");
            }
        }

        if (Input.GetKeyDown(pause))
        {
            if (Conductor.instance.isTutorial)
            {
                PlayerPrefs.SetInt("FirstTime", 1);
                Debug.Log("Loading Main Scene");
                SongLoader.instance.LoadScene(1, SongType.Dance);
                SceneManager.LoadScene(1);
            }
            else
            {
                if (!Conductor.paused)
                {
                    Conductor.paused = true;
                    MiddleButton.instance.Pause();
                }
                else
                {
                    Conductor.instance.Resume();
                    MiddleButton.instance.UnPause();
                }
            }
        }
    }
}
