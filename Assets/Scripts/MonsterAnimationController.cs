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
                animator.SetTrigger("idle");
                currentState = State.Idle;
                break;
        }
    }

    public void GoToSleep()
    {
        switch (currentState)
        {
            case State.Idle:
                animator.SetTrigger("gotoSleep");
                currentState = State.Sleep;
                break;
            case State.WakeUp:
                Idle();
                GoToSleep();
                break;
        }
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
                animator.SetTrigger("wakeUp");
                currentState = State.WakeUp;
                break;
        }
    }
}
