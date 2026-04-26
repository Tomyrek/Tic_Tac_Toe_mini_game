using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public string mainMenuScene;
    public void Back()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
