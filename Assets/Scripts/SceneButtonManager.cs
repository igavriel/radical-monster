using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonManager : MonoBehaviour
{
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
