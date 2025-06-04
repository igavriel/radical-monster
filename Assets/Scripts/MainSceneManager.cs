using System.Collections;
using TMPro;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    public GameObject comicsBalloon;
    public TMP_Text comicsText;

    public TMP_Text timerText;
    public TMP_Text summaryText;
    public float sleepComicDuration = 2f;
    public float wakeComicDuration = 2f;

    private MonsterAnimationController monsterController;

    private void Start()
    {
        Util.AssertObject(comicsBalloon, "Comics Balloon is not assigned in the inspector.");
        Util.AssertObject(comicsText, "Comics Text is not assigned in the inspector.");
        Util.AssertObject(timerText, "Timer Text is not assigned in the inspector.");
        Util.AssertObject(summaryText, "Summary Text is not assigned in the inspector.");

        monsterController = FindFirstObjectByType<MonsterAnimationController>();
        Util.AssertObject(monsterController, "MonsterAnimationController not found in the scene.");

        StartCoroutine(InitializeIdle());
        comicsBalloon.SetActive(false);
        timerText.gameObject.SetActive(false);
        summaryText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!GameManager.Instance.IsSleeping())
            return;

        timerText.text = Util.GetFormattedTime(GameManager.Instance.currentSleepTime);
    }

    private IEnumerator InitializeIdle()
    {
        yield return new WaitForSeconds(0f);
        monsterController.SetStartStateIdle();
    }

    public void OnGotoSleepPressed() => StartCoroutine(GotoSleepRoutine());

    public void OnWakeupButtonClicked() => StartCoroutine(WakeupRoutine());

    private IEnumerator GotoSleepRoutine()
    {
        comicsText.text = Util.GetRandomSleepMessage();
        comicsBalloon.SetActive(true);
        monsterController.ChangeStateToSleep();

        yield return new WaitForSeconds(2f);
        comicsBalloon.SetActive(false);
        GameManager.Instance.StartSleepSession();
        monsterController.SetStartStateSleep();
    }

    private IEnumerator WakeupRoutine()
    {
        GameManager.Instance.StopSleepSession();
        monsterController.ChangeStateToWakeUp();
        comicsText.text = Util.GetRandomWakeMessage();
        comicsBalloon.SetActive(true);
        timerText.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);
        comicsBalloon.SetActive(false);
        GameManager.Instance.EndSleepSession();
        summaryText.gameObject.SetActive(true);
        monsterController.SetStartStateWakeUp();
        buildSummaryText();
    }

    private void buildSummaryText()
    {
        string currentTime = Util.GetFormattedTime(GameManager.Instance.currentSleepTime, true);
        string totalTime = Util.GetFormattedTime(GameManager.Instance.accumulatedSleepTime, true);

        summaryText.text =
            $"הייתה שינה טובה!\n" +
            $"זמן שינה: {currentTime}\n" +
            $"סה״כ זמן שינה: {totalTime}\n";
    }
}
