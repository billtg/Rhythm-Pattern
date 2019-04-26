using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class syncedSpin : MonoBehaviour {

    public bool activateSpin = true;
    private void Start()
    {
        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(360, 0, -Conductor.instance.loopPosInAnalog));
    }
    // Update is called once per frame
    void Update () {
        if (!activateSpin)
            return;

        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, 360, Conductor.instance.loopPosInAnalog));
	}
}
