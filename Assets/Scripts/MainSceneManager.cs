using System.Collections;
using UnityEngine;
using TMPro;

public class MainSceneManager : MonoBehaviour
{
    public GameObject comicsBalloon;
    public TMP_Text comicsText;

    private MonsterAnimationController monsterController;

    string[] sleepMessages =
    {
        "הגיע הזמן לישון...",
        "נא לא לגעת בכלום!",
        ". . . z Z z",
        "חלומות פז!",
        "אל תשכחו לכבות את האור!",
    };

    void Start()
    {
        comicsBalloon.SetActive(false);
        monsterController = FindFirstObjectByType<MonsterAnimationController>();
        if (monsterController == null)
        {
            Debug.LogError("MonsterAnimationController not found in the scene.");
        }
        else
        {
            monsterController.Idle();
        }
    }

    public void OnGotoSleepButtonClicked()
    {
        if (monsterController == null)
        {
            Debug.LogError("MonsterAnimationController not found in the scene.");
            return;
        }
        StartCoroutine(GotoSleepRoutine());
    }

    private IEnumerator GotoSleepRoutine()
    {
        comicsText.text = GetRandomSleepMessage();
        comicsBalloon.SetActive(true);

        yield return new WaitForSeconds(2f);
        comicsBalloon.SetActive(false);

        GameManager.Instance.StartSleepSession();
        monsterController.GoToSleep();
    }

    string GetRandomSleepMessage() => sleepMessages[Random.Range(0, sleepMessages.Length)];
}
