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

    private void Update()
    {
        if (!Conductor.paused)
        {
            if (Input.GetKeyDown(up))
            {
                Debug.Log("Up Pushed on loop beat:" + Conductor.instance.loopPosInBeats.ToString());
                EventManager.TriggerEvent(up.ToString());
            }

            if (Input.GetKeyDown(right))
            {
                Debug.Log("Right Pushed on loop beat:" + Conductor.instance.loopPosInBeats.ToString());
                EventManager.TriggerEvent(right.ToString());
            }

            if (Input.GetKeyDown(down))
            {
                Debug.Log("Down Pushed on loop beat:" + Conductor.instance.loopPosInBeats.ToString());
                EventManager.TriggerEvent(down.ToString());
            }

            if (Input.GetKeyDown(left))
            {
                Debug.Log("Left Pushed on loop beat:" + Conductor.instance.loopPosInBeats.ToString());
                EventManager.TriggerEvent(left.ToString());
            }
        }

        if (Input.GetKeyDown(pause))
        {
            if (!Conductor.paused)
            {
                Conductor.paused = true;
            }
            else
            {
                Conductor.instance.Resume();
            }
        }
    }
}
