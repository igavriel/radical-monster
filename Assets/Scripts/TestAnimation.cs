using UnityEngine;

public class TestAnimation : MonoBehaviour
{
    public GameObject monster1;
    public GameObject monster2;
    public GameObject monster3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        Util.AssertObject(monster1, "Monster 1 is not assigned in the inspector.");
        Util.AssertObject(monster2, "Monster 2 is not assigned in the inspector.");
        Util.AssertObject(monster3, "Monster 3 is not assigned in the inspector.");

        // Example of setting an animation trigger
        Animator animator1 = monster1.GetComponent<Animator>();
        Util.AssertObject(animator1, "Animator component on Monster 1 is not found.");
        animator1.Play("Idle_Wake_Happy");

        Animator animator2 = monster2.GetComponent<Animator>();
        Util.AssertObject(animator2, "Animator component on Monster 2 is not found.");
        animator2.Play("Idle_Sleep");

        Animator animator3 = monster3.GetComponent<Animator>();
        Util.AssertObject(animator3, "Animator component on Monster 3 is not found.");
        animator3.Play("Idle_Wake_Sad");
    }

}
