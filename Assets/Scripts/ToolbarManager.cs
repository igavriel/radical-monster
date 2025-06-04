using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum GameState
{
    Main,
    Sleep,
    Wake
}

public class ToolbarManager : MonoBehaviour
{
    [System.Serializable]
    public class ToolbarData
    {
        public GameState state;
        public RectTransform toolbar;
        public Vector2 hiddenPosition; // relative to top-left
    }

    public Vector2 visiblePosition = Vector2.zero; // top-left
    public float moveDuration = 0.3f;

    public ToolbarData[] toolbars;

    private RectTransform currentToolbar;

    void Start()
    {
        SetGameState(GameState.Main);
    }

    public void SetGameState(GameState newState)
    {
        StartCoroutine(SwitchToolbar(newState));
    }

    private IEnumerator SwitchToolbar(GameState newState)
    {
        RectTransform nextToolbar = null;
        Vector2 nextHiddenPos = Vector2.zero;

        foreach (var tb in toolbars)
        {
            if (tb.state == newState)
            {
                nextToolbar = tb.toolbar;
                nextHiddenPos = tb.hiddenPosition;
                break;
            }
        }

        if (nextToolbar == null)
        {
            Debug.LogWarning("Toolbar not found for state: " + newState);
            yield break;
        }

        // Slide current toolbar out
        if (currentToolbar != null)
        {
            Vector2 returnPosition = GetHiddenPositionForToolbar(currentToolbar);
            yield return StartCoroutine(MoveToolbar(currentToolbar, returnPosition));
        }

        // Slide next toolbar in
        yield return StartCoroutine(MoveToolbar(nextToolbar, visiblePosition));
        currentToolbar = nextToolbar;
    }

    private Vector2 GetHiddenPositionForToolbar(RectTransform toolbar)
    {
        foreach (var tb in toolbars)
        {
            if (tb.toolbar == toolbar)
                return tb.hiddenPosition;
        }
        return new Vector2(-1000, 0); // fallback
    }

    private IEnumerator MoveToolbar(RectTransform toolbar, Vector2 targetPosition)
    {
        Vector2 start = toolbar.anchoredPosition;
        float t = 0;
        while (t < moveDuration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / moveDuration);
            toolbar.anchoredPosition = Vector2.Lerp(start, targetPosition, progress);
            yield return null;
        }
        toolbar.anchoredPosition = targetPosition;
    }

    private void QuitGame()
    {
        Debug.Log("Quitting game...");
        // If running in the editor, stop playing
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If running as a standalone build, quit the application
        Application.Quit();
#endif
    }

    private void LoadSceneByName(string sceneName)
    {
        Debug.Log($"Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    public void ShowMessage(string message)
    {
        Debug.Log($"Popup Message: {message}");
        // Alternatively activate a popup UI here
    }

    public void OnGotoSleepPressed() => SetGameState(GameState.Sleep);

    public void OnStorePressed() => LoadSceneByName("4-store");

    public void OnSettingsPressed() => LoadSceneByName("2-settings");

    public void OnExitPressed() => QuitGame();

    public void OnWakeUpPressed() => SetGameState(GameState.Wake);

    public void OnMainScreenPressed() => SetGameState(GameState.Main);

    public void OnDebug_IncreaseSleepSec_Pressed() =>
        GameManager.Instance.debug_IncreaseSleepTime(10);

    public void OnDebug_IncreaseSleepMin_Pressed() =>
        GameManager.Instance.debug_IncreaseSleepTime(60);
}
