using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    private bool musicOn = true;
    private bool backgroundOn = true;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSettings();
    }

    public void ToggleMusic()
{
    musicOn = !musicOn;
    SaveSettings();

    if (MusicManager.Instance != null)
    {
        MusicManager.Instance.UpdateMusic();
    }
}

    public void ToggleBackground()
    {
        backgroundOn = !backgroundOn;
        SaveSettings();
        
    }

    public bool IsMusicOn() => musicOn;
    public bool IsBackgroundOn() => backgroundOn;

    private void SaveSettings()
    {
        PlayerPrefs.SetInt("music", musicOn ? 1 : 0);
        PlayerPrefs.SetInt("background", backgroundOn ? 1 : 0);
    }

    private void LoadSettings()
    {
        musicOn = PlayerPrefs.GetInt("music", 1) == 1;
        backgroundOn = PlayerPrefs.GetInt("background", 1) == 1;
    }
}
