using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnimate : MonoBehaviour
{
    public Animator animator;
    public AnimatorStateInfo animSI;
    public int currentState;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animSI = animator.GetCurrentAnimatorStateInfo(0);
        currentState = animSI.fullPathHash;
    }

    private void Update()
    {
        animator.Play(currentState, -1, (Conductor.instance.loopPosInAnalog));
        animator.speed = 0;
    }
}
