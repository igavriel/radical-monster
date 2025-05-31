using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class SleepSceneManager : MonoBehaviour
{
    public GameObject comicsBalloon;
    public TMP_Text comicsText;

    public TMP_Text timerText;

    private MonsterAnimationController monsterController;

    private void Start()
    {
        Util.AssertObject(comicsBalloon, "Comics Balloon is not assigned in the inspector.");
        Util.AssertObject(comicsText, "Comics Text is not assigned in the inspector.");
        Util.AssertObject(timerText, "Timer Text is not assigned in the inspector.");

        monsterController = FindFirstObjectByType<MonsterAnimationController>();
        Util.AssertObject(monsterController, "MonsterAnimationController not found in the scene.");

        monsterController.GoToSleep();
        comicsBalloon.SetActive(false);
    }

    void Update()
    {
        if (!GameManager.Instance.IsSleeping())
            return;

        timerText.text = Util.GetFormattedTime(GameManager.Instance.currentSleepTime);
    }

    public void OnDebug_IncreaseSleepSec_ButtonClicked() =>
        GameManager.Instance.debug_IncreaseSleepTime(10);

    public void OnDebug_IncreaseSleepMin_ButtonClicked() =>
        GameManager.Instance.debug_IncreaseSleepTime(60);

    public void OnWakeupButtonClicked() => StartCoroutine(WakeupRoutine());

    private IEnumerator WakeupRoutine()
    {
        GameManager.Instance.StopSleepSession();
        comicsText.text = Util.GetRandomWakeMessage();
        comicsBalloon.SetActive(true);
        timerText.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);
        comicsBalloon.SetActive(false);
        GameManager.Instance.EndSleepSession();
    }
}
