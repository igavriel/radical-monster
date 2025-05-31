using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    SplashScreen,
    MainMenu,
    Settings,
    StartGame,
    Sleep,
    WakeUp,
    Store,
}

public class SceneButtonManager : MonoBehaviour
{
    private IDictionary<SceneName, string> sceneNames = new Dictionary<SceneName, string>
    {
        { SceneName.SplashScreen, "0-splash" },
        { SceneName.MainMenu, "1-main" },
        { SceneName.Settings, "2-settings" },
        { SceneName.StartGame, "3-1-start" },
        { SceneName.Sleep, "3-2-sleep" },
        { SceneName.WakeUp, "3-3-wake" },
        { SceneName.Store, "4-store" },
    };

    public void LoadScene(SceneName sceneName)
    {
        LoadSceneByName(sceneNames[sceneName]);
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowMessage(string message)
    {
        Debug.Log($"Popup Message: {message}");
        // Alternatively activate a popup UI here
    }
}
