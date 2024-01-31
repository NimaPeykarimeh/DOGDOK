using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [HideInInspector] public bool gameIsPaused;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject optionsPanel;

    [SerializeField] Slider volumeSlider;
    [SerializeField] TextMeshProUGUI volumeText;

    [SerializeField] Slider sensitivitySlider;
    [SerializeField] TextMeshProUGUI sensitivityText;
    public float sensitivityValue;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Dropdown resDropdown;

    private Resolution[] resolutions;

    private void Awake()
    {
        resolutions = Screen.resolutions;

        resDropdown.ClearOptions();

        List<string> options = new();

        int currentResIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }
        resDropdown.AddOptions(options);
        resDropdown.value = currentResIndex;
        resDropdown.RefreshShownValue();
        Resume();
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        gameIsPaused = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }
    public void Resume()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        gameIsPaused = false;
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OpenSettings()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        pausePanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void SetResolution(int resIndex)
    {
        Resolution res = resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetVolume()
    {
        volumeText.text = (volumeSlider.value*5).ToString();
        float _volumeValue = Mathf.Clamp(volumeSlider.value/20,0.0001f,20f);
        audioMixer.SetFloat("volume", Mathf.Log10(_volumeValue) * 20);
    }

    public void SetSensitivity()
    {
        sensitivityText.text = (sensitivitySlider.value*5).ToString();
        sensitivityValue = sensitivitySlider.value;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        print("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
