using System.Collections;
using TMPro;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    public GameObject comicsBalloon;
    public GameObject storePanel;
    public TMP_Text comicsText;
    public TMP_Text timerText;
    public TMP_Text summaryText;

    public GameObject[] debugObjects;

    public float sleepComicDuration = 2f;
    public float wakeupComicDuration = 2f;

    private MonsterAnimationController monsterController;

    private void Start()
    {
        Util.AssertObject(comicsBalloon, "Comics Balloon is not assigned in the inspector.");
        Util.AssertObject(comicsText, "Comics Text is not assigned in the inspector.");
        Util.AssertObject(timerText, "Timer Text is not assigned in the inspector.");
        Util.AssertObject(summaryText, "Summary Text is not assigned in the inspector.");
        Util.AssertObject(storePanel, "Store Panel is not assigned in the inspector.");

        monsterController = FindFirstObjectByType<MonsterAnimationController>();
        Util.AssertObject(monsterController, "MonsterAnimationController not found in the scene.");

        StartCoroutine(InitializeIdle());
        comicsBalloon.SetActive(false);
        timerText.gameObject.SetActive(false);
        summaryText.gameObject.SetActive(false);
        storePanel.SetActive(false);
        OnToggleDebugMode();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsSleeping())
            return;

        timerText.text = Util.GetFormattedTime(GameManager.Instance.gameData.CurrentSleepTime);
    }

    public void OnMainScreenPressed() => StartCoroutine(InitializeIdle());

    public void OnGotoSleepPressed() => StartCoroutine(GotoSleepRoutine());

    public void OnWakeupPressed() => StartCoroutine(WakeupRoutine());

    public void OnStorePressed()
    {
        comicsBalloon.SetActive(false);
        timerText.gameObject.SetActive(false);
        summaryText.gameObject.SetActive(false);
        storePanel.SetActive(true);
        monsterController.gameObject.SetActive(false);
    }

    public void OnDeleteUserPressed() => GameManager.Instance.ResetGame();

    public void OnShowGarden() => monsterController.gameObject.SetActive(false);

    public void OnToggleDebugMode()
    {
        bool debugMode = GameManager.Instance.IsDebugMode();
        Debug.Log($"Debug mode toggled: {debugMode}");
        foreach (GameObject obj in debugObjects)
        {
            Debug.Log($"Setting debug object {obj.name} active state to {debugMode}");
            obj.SetActive(debugMode);
        }
    }

    private IEnumerator InitializeIdle()
    {
        yield return new WaitForSeconds(0f);
        GameManager.Instance.LoadProgress();
        monsterController.gameObject.SetActive(true);
        monsterController.SetStartStateIdle();

        comicsBalloon.SetActive(false);
        timerText.gameObject.SetActive(false);
        summaryText.gameObject.SetActive(false);
        storePanel.SetActive(false);
    }

    private IEnumerator GotoSleepRoutine()
    {
        comicsText.text = Util.GetRandomSleepMessage();
        monsterController.ChangeStateToSleep();

        comicsBalloon.SetActive(true);
        timerText.gameObject.SetActive(false);
        summaryText.gameObject.SetActive(false);
        storePanel.SetActive(false);
        GameManager.Instance.StartSleepSession();

        yield return new WaitForSeconds(sleepComicDuration);

        comicsBalloon.SetActive(false);
        timerText.gameObject.SetActive(true);
        summaryText.gameObject.SetActive(false);
        storePanel.SetActive(false);

        monsterController.SetStartStateSleep();
    }

    private IEnumerator WakeupRoutine()
    {
        GameManager.Instance.StopSleepSession();
        monsterController.ChangeStateToWakeUp();
        comicsText.text = Util.GetRandomWakeMessage();
        comicsBalloon.SetActive(true);
        timerText.gameObject.SetActive(false);
        summaryText.gameObject.SetActive(false);
        storePanel.SetActive(false);

        yield return new WaitForSeconds(wakeupComicDuration);
        GameManager.Instance.EndSleepSession();

        buildSummaryText();
        comicsBalloon.SetActive(false);
        timerText.gameObject.SetActive(false);
        summaryText.gameObject.SetActive(true);
        storePanel.SetActive(false);

        monsterController.SetStartStateWakeUp();
    }

    private void buildSummaryText()
    {
        string currentTime = Util.GetFormattedTime(
            GameManager.Instance.gameData.CurrentSleepTime,
            true
        );
        string totalTime = Util.GetFormattedTime(
            GameManager.Instance.gameData.AccumulatedSleepTime,
            true
        );

        summaryText.text =
            $"הייתה שינה טובה!\n" + $"זמן שינה: {currentTime}\n" + $"סה״כ זמן שינה: {totalTime}\n";
    }
}
