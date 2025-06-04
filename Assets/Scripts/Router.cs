using UnityEngine;
using UnityEngine.SceneManagement;

public class Router : MonoBehaviour
{
    private string additiveSceneName = string.Empty;

    private void LoadSceneByName(string sceneName, LoadSceneMode mode)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);    // force single scene load for now
        //     Debug.Log($"Loading scene: {sceneName} with mode: {mode}");
        //     if (mode == LoadSceneMode.Single)
        //     {
        //         if (additiveSceneName.Length > 0)
        //         {
        //             // Unload the previous additive scene if it exists
        //             Debug.Log($"Unloading previous additive scene: {additiveSceneName}");
        //             SceneManager.UnloadSceneAsync(additiveSceneName);
        //             additiveSceneName = string.Empty;
        //         }
        //         else
        //         {
        //             SceneManager.LoadScene(sceneName, mode);
        //         }
        //     }
        //     else
        //     {
        //         SceneManager.LoadScene(sceneName, mode);
        //         additiveSceneName = sceneName; // Update the current additive scene name
        //     }
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

    public void OnStartGameFromSplashPressed() => LoadSceneByName("1-main", LoadSceneMode.Single);

    public void OnStorePressed() => LoadSceneByName("4-store", LoadSceneMode.Additive);

    public void OnSettingsPressed() => LoadSceneByName("2-settings", LoadSceneMode.Additive);

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

    public void OnDeleteUserButtonClicked() => GameManager.Instance.ResetGame();

    public void OnCreateNewFlowersPressed() => GameManager.Instance.AddNewRandomFlower();

    public void OnDebug_IncreaseSleepSec_Pressed() =>
        GameManager.Instance.debug_IncreaseSleepTime(10);

    public void OnDebug_IncreaseSleepMin_Pressed() =>
        GameManager.Instance.debug_IncreaseSleepTime(60);
}
