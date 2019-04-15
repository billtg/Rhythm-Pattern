using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableObject : MonoBehaviour {

    public Animator animator;
    private int indexHash = Animator.StringToHash("index");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ActivateTrigger(int index)
    {
        Debug.Log("Triggered " + this.gameObject.name + " with index " + index.ToString());
        animator.SetInteger(indexHash, index);
    }

    [System.Serializable]
    public class Trigger
    {
        public TriggerableObject targetObject;
        public int triggerIndex;
    }
}
