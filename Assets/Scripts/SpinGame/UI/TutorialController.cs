using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public SequenceController sequenceController;
    public GameObject arrowKey;
    public GameObject shiftKey;
    private bool arrowDone = false;
    private bool shiftDone = false;

    private void Update()
    {
        if (sequenceController.activeRing == 1 && !arrowDone)
        {
            arrowKey.SetActive(false);
            shiftKey.SetActive(true);
            arrowDone = true;
        }
        if (sequenceController.activeRing == 2 && !shiftDone)
        {
            shiftKey.SetActive(false);
            shiftDone = true;
        }
    }
}
