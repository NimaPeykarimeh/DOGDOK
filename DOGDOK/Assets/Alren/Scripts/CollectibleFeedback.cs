using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleFeedback : MonoBehaviour
{
    [SerializeField] private GameObject CollectiblePanel;
    [SerializeField] private GameObject FeedbackText;

    public void GiveFeedback(Dictionary<Resource1, int> addedResources)
    {
        foreach (var obj in addedResources)
        {
            GameObject text = Instantiate(FeedbackText, CollectiblePanel.transform);
            StringBuilder str = new();
            str.Append("x");
            str.Append(obj.Value);
            str.Append(' ');
            str.Append(obj.Key.resourceName);
            str.Append(" Added");
            text.GetComponent<TextMeshProUGUI>().SetText(str);
            Destroy(text, 3);
        }
    }
}
