using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField] GameObject buttonText;
    [SerializeField] GameObject loadPanel;
    [SerializeField] Slider slider;
    [SerializeField] int sceneId;

    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject CreditsPanel;
    [SerializeField] private GameObject OptionsPanel;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Dropdown resDropdown;

    private Resolution[] resolutions;

    private void Awake()
    {
        resolutions = Screen.resolutions;

        resDropdown.ClearOptions();

        List<string> options = new();

        int currentResIndex = 0;

        for(int i = 0; i< resolutions.Length; i++)
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

    }

    public void LoadMainMenu()
    {
        AudioManager.Instance.PlayAudio(AudioManager.Instance.menuClickButton);
        MainMenuPanel.SetActive(true);
        CreditsPanel.SetActive(false);
        OptionsPanel.SetActive(false);
    }


    public void LoadScene(int _sceneId) //New Game Button
    {
        AudioManager.Instance.audioSouce.Stop();
        StartCoroutine(LoadSceneAsync(_sceneId));
        buttonText.SetActive(false);
    }

    public void LoadCredits() //Credits Button
    {
        AudioManager.Instance.PlayAudio(AudioManager.Instance.menuClickButton);
        MainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(true);
    }

    public void LoadOptions() //Options Button
    {
        AudioManager.Instance.PlayAudio(AudioManager.Instance.menuClickButton);
        MainMenuPanel.SetActive(false);
        OptionsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        print("quit");
    }

    public void SetResolution(int resIndex)
    {
        Resolution res = resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetVolume(float value)
    {
        audioMixer.SetFloat("volume", value);
    }
    
    IEnumerator LoadSceneAsync(int _sceneId)
    {
        AsyncOperation _operation = SceneManager.LoadSceneAsync(_sceneId);

        loadPanel.SetActive(true);

        while (!_operation.isDone)
        {
            float _progress = Mathf.Clamp01(_operation.progress);
            slider.value = _progress;
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) 
            || 
            (Input.GetKeyDown(KeyCode.Escape) 
            && 
            (CreditsPanel.activeInHierarchy || OptionsPanel.activeInHierarchy)))
        {
            LoadMainMenu();
            buttonText.SetActive(false);
        }
    }
}
