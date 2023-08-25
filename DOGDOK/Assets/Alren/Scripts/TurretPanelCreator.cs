using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretPanelCreator : MonoBehaviour
{
    public List<Build1> builds;
    [SerializeField] GameObject buildingCard;
    [SerializeField] GameObject resourceBar;
    [SerializeField] InventoryManager InventoryManager;
    private List<TextMeshProUGUI> requiredTexts;
    private void Awake()
    {
        requiredTexts = new List<TextMeshProUGUI>();
        SetupPanel();
    }

    void SetupPanel()
    {
        for (int i = 0; i < builds.Count; i++)
        {
            Build1 _currentBuild = builds[i];
            GameObject _card = Instantiate(buildingCard, transform.position, transform.rotation);
            _card.transform.parent = transform;
            _card.GetComponent<RectTransform>().localScale = Vector3.one;

            _card.transform.Find("TurretImage").GetComponent<Image>().sprite = _currentBuild.buildingImage;
            _card.transform.Find("TurretName").GetComponent<TextMeshProUGUI>().text = _currentBuild.buildingName;
            _card.GetComponent<CraftBuilding>().id = i;

            for (int j = 0; j < _currentBuild.requiredResource.Count; j++)
            {
                GameObject _resourceBar = Instantiate(resourceBar, transform.position, transform.rotation);
                _resourceBar.transform.parent = _card.transform.Find("ResourceHolder").transform;
                _resourceBar.GetComponent<RectTransform>().localScale = Vector3.one;

                _resourceBar.transform.Find("ResourceImage").GetComponent<Image>().sprite = _currentBuild.requiredResource[j].resourceImage;
                requiredTexts.Add(_resourceBar.transform.Find("Required").GetComponent<TextMeshProUGUI>());
                requiredTexts[j].text =
                    "0"
                    + "/"
                    + _currentBuild.requiredAmount[j].ToString();
            }
        }
    }

    public void UpdateRequiredResource()
    {
        int z = 0;
        for (int i = 0; i < builds.Count; i++)
        {
            Build1 _currentBuild = builds[i];
            for (int j = 0; j < _currentBuild.requiredResource.Count; j++)
            {
                StringBuilder str = new();
                str.Append(InventoryManager.resourceIndices[_currentBuild.requiredResource[j]].ToString());
                str.Append("/");
                str.Append(_currentBuild.requiredAmount[j].ToString());
                requiredTexts[z++].text = str.ToString();
                //requiredTexts[z++].SetText(str.ToString());
            }
        }
    }
}
