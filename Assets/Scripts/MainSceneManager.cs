using System.Collections;
using TMPro;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    public GameObject comicsBalloon;
    public TMP_Text comicsText;

    void Start()
    {
        Util.AssertObject(comicsBalloon, "Comics Balloon is not assigned in the inspector.");
        Util.AssertObject(comicsText, "Comics Text is not assigned in the inspector.");

        MonsterAnimationController monsterController =
            FindFirstObjectByType<MonsterAnimationController>();
        Util.AssertObject(monsterController, "MonsterAnimationController not found in the scene.");

        monsterController.Idle();
        comicsBalloon.SetActive(false);
    }

    public void OnGotoSleepButtonClicked() => StartCoroutine(GotoSleepRoutine());

    private IEnumerator GotoSleepRoutine()
    {
        comicsText.text = Util.GetRandomSleepMessage();
        comicsBalloon.SetActive(true);

        yield return new WaitForSeconds(2f);
        comicsBalloon.SetActive(false);
        GameManager.Instance.StartSleepSession();
    }
}
