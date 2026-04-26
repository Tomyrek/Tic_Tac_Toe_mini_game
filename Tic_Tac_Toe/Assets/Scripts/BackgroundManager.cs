using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    private void Start()
    {
        UpdateBackground();
    }

    public void UpdateBackground()
    {
        gameObject.SetActive(SettingsManager.Instance.IsBackgroundOn());
    }
}
