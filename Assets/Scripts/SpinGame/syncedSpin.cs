using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class syncedSpin : MonoBehaviour {

    public bool activateSpin = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!Conductor.instance.musicStarted || !activateSpin)
            return;

        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, 360, Conductor.instance.loopPosInAnalog));
	}
}
