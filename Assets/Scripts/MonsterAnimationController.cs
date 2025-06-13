using UnityEngine;

public enum MonsterState
{
    Idle,
    Sleep,
    WakeUp,
}

public class MonsterAnimationController : MonoBehaviour
{
    private Animator animator;
    private MonsterState monsterState = MonsterState.Idle;

    void Start()
    {
        animator = GetComponent<Animator>();
        Util.AssertObject(animator, "Animator component is not assigned in the inspector.");
    }

    public void SetStartStateIdle()
    {
        animator.Play("Idle_Wake_Sad");
        monsterState = MonsterState.Idle;
    }

    public void SetStartStateSleep()
    {
        animator.Play("Idle_Sleep");
        monsterState = MonsterState.Sleep;
    }

    public void SetStartStateWakeUp()
    {
        animator.Play("Idle_Wake_Happy");
        monsterState = MonsterState.WakeUp;
    }

    public void ChangeStateToIdle()
    {
        switch (monsterState)
        {
            case MonsterState.Sleep:
                ChangeStateToWakeUp();
                ChangeStateToIdle();
                break;
            case MonsterState.WakeUp:
                animator.SetTrigger("idle");
                monsterState = MonsterState.Idle;
                break;
        }
    }

    public void ChangeStateToSleep()
    {
        switch (monsterState)
        {
            case MonsterState.Idle:
                animator.SetTrigger("gotoSleep");
                monsterState = MonsterState.Sleep;
                break;
            case MonsterState.WakeUp:
                ChangeStateToIdle();
                ChangeStateToSleep();
                break;
        }
    }

    public void ChangeStateToWakeUp()
    {
        switch (monsterState)
        {
            case MonsterState.Idle:
                ChangeStateToSleep();
                ChangeStateToWakeUp();
                break;
            case MonsterState.Sleep:
                animator.SetTrigger("wakeUp");
                monsterState = MonsterState.WakeUp;
                break;
        }
    }
}
