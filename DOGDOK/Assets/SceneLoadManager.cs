using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField] GameObject buttonText;
    [SerializeField] GameObject loadPanel;
    [SerializeField] Slider slider;
    [SerializeField] int sceneId;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadScene(int _sceneId)
    {
        StartCoroutine(LoaadSceneAsync(_sceneId));
    }

    IEnumerator LoaadSceneAsync(int _sceneId)
    {
        AsyncOperation _operation = SceneManager.LoadSceneAsync(_sceneId);

        loadPanel.SetActive(true);
        buttonText.SetActive(false);

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadScene(sceneId);
        }
    }
}
