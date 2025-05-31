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

    public void SetStartStateIdle()
    {
        animator.Play("Idle_Wake_Sad");
        currentState = State.Idle;
    }

    public void SetStartStateSleep()
    {
        animator.Play("Idle_Sleep");
        currentState = State.Sleep;
    }

    public void SetStartStateWakeUp()
    {
        animator.Play("Idle_Wake_Happy");
        currentState = State.WakeUp;
    }

    public void ChangeStateToIdle()
    {
        switch (currentState)
        {
            case State.Sleep:
                ChangeStateToWakeUp();
                ChangeStateToIdle();
                break;
            case State.WakeUp:
                animator.SetTrigger("idle");
                currentState = State.Idle;
                break;
        }
    }

    public void ChangeStateToSleep()
    {
        switch (currentState)
        {
            case State.Idle:
                animator.SetTrigger("gotoSleep");
                currentState = State.Sleep;
                break;
            case State.WakeUp:
                ChangeStateToIdle();
                ChangeStateToSleep();
                break;
        }
    }

    public void ChangeStateToWakeUp()
    {
        switch (currentState)
        {
            case State.Idle:
                ChangeStateToSleep();
                ChangeStateToWakeUp();
                break;
            case State.Sleep:
                animator.SetTrigger("wakeUp");
                currentState = State.WakeUp;
                break;
        }
    }
}
