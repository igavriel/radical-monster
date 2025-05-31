using UnityEngine;
using TMPro;
using System;

public class SleepSceneManager : MonoBehaviour
{
    public TMP_Text timerText;
    public GameObject comicsBalloon;

    private MonsterAnimationController monsterController;

    private void Start()
    {
        Util.AssertObject(timerText, "Timer Text is not assigned in the inspector.");
        Util.AssertObject(comicsBalloon, "Comics Balloon is not assigned in the inspector.");

        monsterController = FindFirstObjectByType<MonsterAnimationController>();
        Util.AssertObject(monsterController, "MonsterAnimationController not found in the scene.");

        monsterController.GoToSleep();
    }

    void Update()
    {
        timerText.text = TimeSpan.FromSeconds(
            GameManager.Instance.currentSleepTime).ToString(@"hh\:mm\:ss");
    }
}
