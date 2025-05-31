using System.Collections;
using UnityEngine;

public enum State
{
    Idle,
    Sleep,
    WakeUp,
}

public class MonsterAnimationController : MonoBehaviour
{
    private Animator animator;
    private State currentState = State.Idle;

    void Start()
    {
        animator = GetComponent<Animator>();
        Util.AssertObject(animator, "Animator component is not assigned in the inspector.");
    }

    public void Idle()
    {
        switch (currentState)
        {
            case State.Sleep:
                WakeUp();
                Idle();
                break;
            case State.WakeUp:
                StartCoroutine(StartIdleAnimation());
                break;
        }
    }

    IEnumerator StartIdleAnimation()
    {
        yield return new WaitForEndOfFrame(); // wait 1 frame
        animator.SetTrigger("idle");
        currentState = State.Idle;
    }

    public void GoToSleep()
    {
        switch (currentState)
        {
            case State.Idle:
                StartCoroutine(StartSleepAnimation());
                break;
            case State.WakeUp:
                Idle();
                GoToSleep();
                break;
        }
    }

    IEnumerator StartSleepAnimation()
    {
        yield return new WaitForEndOfFrame(); // wait 1 frame
        animator.SetTrigger("gotoSleep");
        currentState = State.Sleep;
    }

    public void WakeUp()
    {
        switch (currentState)
        {
            case State.Idle:
                GoToSleep();
                WakeUp();
                break;
            case State.Sleep:
                StartCoroutine(WakeupAnimation());
                break;
        }
    }

    IEnumerator WakeupAnimation()
    {
        yield return new WaitForEndOfFrame(); // wait 1 frame
        animator.SetTrigger("wakeUp");
        currentState = State.WakeUp;
    }
}
