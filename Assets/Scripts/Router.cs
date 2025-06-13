using UnityEngine;
using UnityEngine.SceneManagement;

public class Router : MonoBehaviour
{
    private string additiveSceneName = string.Empty;

    private void LoadSceneByName(string sceneName)
    {
        Debug.Log($"Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
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

    ToolbarManager loadToolbarManager()
    {
        ToolbarManager toolbarManager = FindFirstObjectByType<ToolbarManager>();
        Util.AssertObject(toolbarManager, "ToolbarManager not found in the scene.");
        return toolbarManager;
    }

    public void OnGotoSleepPressed()
    {
        ToolbarManager toolbarManager = loadToolbarManager();
        if (toolbarManager != null)
            toolbarManager.SetGameState(GameState.Sleep);
    }

    public void OnStartGameFromSplashPressed() => LoadSceneByName("1-main");

    public void OnStorePressed()
    {
        ToolbarManager toolbarManager = loadToolbarManager();
        if (toolbarManager != null)
            toolbarManager.SetGameState(GameState.Store);
    }

    public void OnSettingsPressed()
    {
        ToolbarManager toolbarManager = loadToolbarManager();
        if (toolbarManager != null)
            toolbarManager.SetGameState(GameState.Settings);
    }

    public void OnExitPressed() => QuitGame();

    public void OnWakeUpPressed()
    {
        ToolbarManager toolbarManager = loadToolbarManager();
        if (toolbarManager != null)
            toolbarManager.SetGameState(GameState.Wake);
    }

    public void OnMainScreenPressed()
    {
        ToolbarManager toolbarManager = loadToolbarManager();
        if (toolbarManager != null)
            toolbarManager.SetGameState(GameState.Main);
    }

    public void OnDebugModePressed() => GameManager.Instance.ToggleDebugMode();

    public void OnDeleteUserButtonClicked() => GameManager.Instance.ResetGame();

    public void OnCreateNewFlowersPressed() => GameManager.Instance.AddNewRandomFlower();

    public void OnDebug_GrowGarden_Pressed() => GameManager.Instance.IncreaseAllFlowerStages();

    public void OnDebug_IncreaseSleepTime_Pressed()
    {
        int time = Random.Range(10, 600);
        GameManager.Instance.debug_IncreaseSleepTime(time);
    }
}
