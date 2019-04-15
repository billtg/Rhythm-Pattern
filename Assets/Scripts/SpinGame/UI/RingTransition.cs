using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingTransition : MonoBehaviour {

	public void MidTransition()
    {
        Debug.Log("Ding!");
        MenuScript.instance.LoadScreenButtons();
    }
}
