using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayText : MonoBehaviour
{
    public static DisplayText Instance;
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] bool isDisplaying = false;

    [SerializeField] float displayTimer;
    [SerializeField] bool isCounting;

    [SerializeField] Color regularTextColor;
    [SerializeField] Color warningTextColor;

    Color currentColor;

    [SerializeField] float fadeTimer;
    [SerializeField] float alphaVal;
    [SerializeField] bool doesFadeIn;

    bool willFade;
    public enum TextType
    {
        Regular,
        Warning
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        mainText = GetComponentInChildren<TextMeshProUGUI>();
        mainText.gameObject.SetActive(false);
        Debug.Log(mainText.name);
    }


    public void ShowText(bool _display,string _text = "", float _duration = 0f, TextType _type = TextType.Regular)
    {
        willFade = _duration > 0;
        if (willFade)
        {
            if (!isDisplaying && _display)
            {
                isDisplaying = true;
                doesFadeIn = true;
                fadeTimer = 0;
                mainText.text = _text;
                currentColor = (_type == TextType.Regular) ? regularTextColor : warningTextColor;
                currentColor.a = 0;
                mainText.color = currentColor;
                mainText.gameObject.SetActive(true);
                if (_duration > 0f)
                {
                    //isCounting = true;
                    displayTimer = _duration;
                }

            }
            else if (!_display && isDisplaying)
            {
                isDisplaying = false;
                mainText.text = "";
                mainText.gameObject.SetActive(false);
            }
        }
        else
        {
            if (!isDisplaying && _display)
            {
                isDisplaying = true;
                isCounting = true;
                mainText.text = _text;
                currentColor = (_type == TextType.Regular) ? regularTextColor : warningTextColor;

                mainText.color = currentColor;
                mainText.gameObject.SetActive(true);
            }
            else if (!_display && isDisplaying)
            {
                isDisplaying = false;
                isCounting = false;
                mainText.text = "";
                mainText.gameObject.SetActive(false);
            }
        }
        
    }

    private void Update()
    {
        if (willFade)
        {
            if (doesFadeIn)
            {
                fadeTimer += Time.deltaTime;
                alphaVal = Mathf.Lerp(0, 1, Mathf.Clamp01(fadeTimer / 0.3f));
                currentColor.a = alphaVal;
                mainText.color = currentColor;
                if (alphaVal == 1)
                {
                    doesFadeIn = false;
                    isCounting = true;
                }
            }
            if (isCounting)
            {

                displayTimer -= Time.deltaTime;
                if (displayTimer < 0)
                {
                    isCounting = false;
                }
            }
            if (!isCounting && !doesFadeIn && isDisplaying)
            {
                fadeTimer -= Time.deltaTime;
                alphaVal = Mathf.Lerp(0, 1, Mathf.Clamp01(fadeTimer / 0.3f));
                currentColor.a = alphaVal;
                mainText.color = currentColor;
                if (alphaVal == 0)
                {
                    ShowText(false);
                }
            }
        }
    }
        

}
