using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class syncedAnimation : MonoBehaviour {

    public float timeSig;
    public Animator animator;
    public AnimatorStateInfo animSI;
    public int currentState;
    public int holdState = Animator.StringToHash("holdState");

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        animSI = animator.GetCurrentAnimatorStateInfo(0);
        currentState = animSI.fullPathHash;
    }
	
	// Update is called once per frame
	void Update () {
        if (!Conductor.instance.musicStarted)
            return;

        

        animator.Play(currentState, -1, (Conductor.instance.loopPosInAnalog));
        animator.speed = 0;
	}

    private void OnMouseDown()
    {
        Debug.Log("clicked");
        currentState = holdState;
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        Debug.Log("Bip!");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Bop!");
    }
}
