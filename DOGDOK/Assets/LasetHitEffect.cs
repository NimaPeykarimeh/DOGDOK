using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LasetHitEffect : MonoBehaviour
{
    [SerializeField] float effectFadeOutDuration = 1f;
    [SerializeField] float effectFadeInDuration = 0.2f;
    [SerializeField] float currentFadeValue = 0;
    [SerializeField] bool isFadingIn = true;
    DecalProjector decalProjector;

    private void Start()
    {
        decalProjector = GetComponent<DecalProjector>();
        isFadingIn = true;
    }
    void Update()
    {
        if (isFadingIn)
        {
            currentFadeValue = Mathf.MoveTowards(currentFadeValue, 1, (1 / effectFadeInDuration) * Time.deltaTime);
            decalProjector.fadeFactor = currentFadeValue;
            if (currentFadeValue == 1)
            {
                isFadingIn = false;
            }
        }
        else
        {
            currentFadeValue = Mathf.MoveTowards(currentFadeValue, 0, (1 / effectFadeOutDuration) * Time.deltaTime);
            decalProjector.fadeFactor = currentFadeValue;
        }
        if (currentFadeValue == 0)
        {
            Destroy(gameObject);
        }
    }
}
