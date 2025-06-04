using UnityEngine;

public class SettingsSceneManager : MonoBehaviour
{
    public void OnDeleteUserButtonClicked() => GameManager.Instance.ResetGame();
}
