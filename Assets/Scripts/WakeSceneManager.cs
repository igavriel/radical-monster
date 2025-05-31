using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class WakeSceneManager : MonoBehaviour
{
    public TMP_Text summaryText;

    private MonsterAnimationController monsterController;

    private void Start()
    {
        Util.AssertObject(summaryText, "Summary Text is not assigned in the inspector.");

        monsterController = FindFirstObjectByType<MonsterAnimationController>();
        Util.AssertObject(monsterController, "MonsterAnimationController not found in the scene.");

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
