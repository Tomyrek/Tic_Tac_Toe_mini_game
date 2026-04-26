using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    public void ToggleMusic()
    {
        SettingsManager.Instance.ToggleMusic();
    }

    public void ToggleBackground()
    {
        SettingsManager.Instance.ToggleBackground();
    }
}
