using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

public class CircleScript : MonoBehaviour {

    public bool hittable = true;
    public bool didHit = false;
    public bool active = true;
    public bool hitThisLoop = false;
    public Animator animator;
    public Animator childAnimator;
    public AudioSource audioSource;
    public RingParent ringParent;
    public float beat;
    public KeyCode keyCode;
    public int measure;
    public GameObject arrow;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        ringParent = GetComponentInParent<RingParent>();
        animator = GetComponent<Animator>();
    }
    
    public void Initialize(KeyCode keyCode, float beat, AudioClip audioClip, AudioMixerGroup audioMixerGroup)
    {
        this.keyCode = keyCode;
        this.beat = beat;
        audioSource.clip = audioClip;
        audioSource.outputAudioMixerGroup = audioMixerGroup;

        //Set the arrow rotation
        switch(keyCode)
        {
            case KeyCode.UpArrow:
                break;
            case KeyCode.RightArrow:
                this.gameObject.transform.Rotate(new Vector3(0, 0, 270));
                break;
            case KeyCode.DownArrow:
                this.gameObject.transform.Rotate(new Vector3(0, 0, 180));
                break;
            case KeyCode.LeftArrow:
                this.gameObject.transform.Rotate(new Vector3(0, 0, 90));
                break;
            default:
                Debug.LogError("Incorrect arrow key assignment!");
                break;
        }


    }

    public void LocateCircle(float ringDistance)
    {
        //Give it a fraction based on it's
        float loopFraction = (beat - 1) / Conductor.instance.timeSig;
        float angleFromTop = loopFraction * 360*Mathf.Deg2Rad;
        this.gameObject.transform.Rotate(new Vector3(0, 0, -angleFromTop * Mathf.Rad2Deg));
        //Debug.Log("Angle: " + angleFromTop);
        float circleX = Mathf.Sin(angleFromTop) * ringDistance;
        float circleY = Mathf.Cos(angleFromTop) * ringDistance;
        //Debug.Log("Setting circle to (" + circleX + ", " + circleY + ")");
        this.gameObject.transform.position = new Vector3(circleX, circleY, 0);
    }

    public void SetClip(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
    }
    
    private void Update()
    {
        if (!active) return;
        CheckForMissedNote();
        if (hittable && !hitThisLoop)
        {
            hitThisLoop = CanHitThisLoop();
        }
    }

    private void CheckForMissedNote()
    {
        //Only check for notes that are hittable
        if (!hittable || didHit) return;
        if (beat >= Conductor.instance.timeSig + 1 - Conductor.instance.beatThreshold)
        {
            //Debug.Log("Note is just below loop point");
            //Note, this will give you grief if the first note is just after the loop point, followed by this note just before. Try to never do that.
            if (Conductor.instance.loopPosInBeats < beat + Conductor.instance.beatThreshold - Conductor.instance.timeSig)
            {
                //Debug.Log("beat (" + Conductor.instance.loopPosInBeats.ToString() + ") is past the current arrow: " + beat.ToString());
                ringParent.MissedNote();
            }
        }else
        {
            //Beat is far from loop point or just after loop point
            //Debug.Log("Beat  is far from loop point or just after");
            if (Conductor.instance.loopPosInBeats > beat + Conductor.instance.beatThreshold && hitThisLoop)
            {
                //Debug.Log("beat (" + Conductor.instance.loopPosInBeats.ToString() + ") is past the current arrow: " + beat.ToString());
                ringParent.MissedNote();
            }
        }
    }

    public bool CanHitThisLoop()
    {
        if (Conductor.instance.loopPosInBeats < (beat + Conductor.instance.beatThreshold))
            return true;
        else
            return false;
    }

    private void HitCircle()
    {
        //Only process for hittable notes
        //Debug.Log("Trying to hit");
        if (!hittable) return;
        //Debug.Log("Circle Hit Activated with " + keyCode.ToString());
        if (Conductor.instance.InThreshold(beat))
        {
            //Debug.Log("Hit on beat " + Conductor.instance.loopPosInBeats.ToString());
            EventManager.StopListening(keyCode.ToString(), HitCircle);
            didHit = true;
            hittable = false;
            //play the ssound
            audioSource.Play();
            //activaate the inner circle
            ExpandDot();
            //Check with parent for completion
            ringParent.CheckForCompleteRing();
        } else
        {
            //Debug.Log("Hit on off beat. Punish");
            ringParent.MissedNote();
        }
    }

    public void ActivateCircle()
    {
        //Debug.Log("Activating Small Ring");
        active = true;
        if (!arrow.activeInHierarchy) arrow.SetActive(true);
        ExpandCircle();
    }

    public void EnableCircle()
    {
        //Debug.Log("Enabling circle");
        hittable = true;
        hitThisLoop = CanHitThisLoop();
        //Debug.Log(hitThisLoop);
        EventManager.StartListening(keyCode.ToString(), HitCircle);
    }

    public void DeactivateCircle()
    {
        //Debug.Log("Deactivating Small Ring");
        if (didHit) CollapseDot();
        if (active) CollapseCircle();
        didHit = false;
        hittable = false;
        active = false;
        hitThisLoop = false;
        EventManager.StopListening(keyCode.ToString(), HitCircle);
    }

    public void ResetCircle()
    {
        //Debug.Log("Resetting Small Ring");
        if (didHit) CollapseDot();
        didHit = false;
        hittable = false;
        hitThisLoop = false;
        EventManager.StopListening(keyCode.ToString(), HitCircle);
    }

    public void AssistActivate()
    {
        if (!active) ActivateCircle();
        if (hittable) hittable = false;
        if (didHit)
        {
            CollapseDot();
            didHit = false;
        }
        arrow.SetActive(false);
    }

    //
    //Animation Triggers
    //

    public void ExpandCircle()
    {
        //Debug.Log("Expanding Small Ring");
        animator.ResetTrigger("collapse");
        animator.SetTrigger("expand");
    }

    public void CollapseCircle()
    {
        //Debug.Log("Collapsing Small Ring");
        animator.ResetTrigger("expand");
        animator.SetTrigger("collapse");
    }

    public void ExpandDot()
    {
        //Debug.Log("Expanding Circle");
        childAnimator.ResetTrigger("collapse");
        childAnimator.SetTrigger("expand");
    }

    public void CollapseDot()
    {
        //Debug.Log("Collapsing Circle");
        childAnimator.ResetTrigger("expand");
        childAnimator.SetTrigger("collapse");
    }
}
