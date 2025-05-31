using System.Collections;
using TMPro;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    public GameObject comicsBalloon;
    public TMP_Text comicsText;

    private MonsterAnimationController monsterController;

    private void Start()
    {
        Util.AssertObject(comicsBalloon, "Comics Balloon is not assigned in the inspector.");
        Util.AssertObject(comicsText, "Comics Text is not assigned in the inspector.");

        monsterController = FindFirstObjectByType<MonsterAnimationController>();
        Util.AssertObject(monsterController, "MonsterAnimationController not found in the scene.");

        StartCoroutine(InitializeIdle());
        comicsBalloon.SetActive(false);
    }

    private IEnumerator InitializeIdle()
    {
        yield return new WaitForSeconds(0f);
        monsterController.SetStartStateIdle();
    }

    public void OnGotoSleepButtonClicked() => StartCoroutine(GotoSleepRoutine());

    private IEnumerator GotoSleepRoutine()
    {
        comicsText.text = Util.GetRandomSleepMessage();
        comicsBalloon.SetActive(true);
        monsterController.ChangeStateToSleep();

        yield return new WaitForSeconds(2f);
        comicsBalloon.SetActive(false);
        GameManager.Instance.StartSleepSession();
    }
}
