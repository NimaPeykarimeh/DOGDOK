using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretPanelCreator : MonoBehaviour
{
    public List<Build1> builds;
    [SerializeField] GameObject buildingCard;
    [SerializeField] GameObject resourceBar;
    [SerializeField] CollectibleManager CollectibleManager;
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

            for (int j = 0; j < _currentBuild.requiredResource.Count; j++)
            {
                GameObject _resourceBar = Instantiate(resourceBar, transform.position, transform.rotation);
                Debug.Log(_resourceBar.name);
                _resourceBar.transform.parent = _card.transform.Find("ResourceHolder").transform;
                _resourceBar.GetComponent<RectTransform>().localScale = Vector3.one;

                _resourceBar.transform.Find("ResourceImage").GetComponent<Image>().sprite = _currentBuild.requiredResource[j].resourceImage;
                requiredTexts.Add(_resourceBar.transform.Find("Required").GetComponent<TextMeshProUGUI>());
                requiredTexts[j].text =
                    //InventoryManager.InventorySlots[_currentBuild.requiredResource[j].id].ToString()
                    "0"
                    + "/"
                    + _currentBuild.requiredAmount[j].ToString();
                //_resourceBar.transform.Find("Required").GetComponent<TextMeshProUGUI>().text =
                //InventoryManager.InventorySlots[_currentBuild.requiredResource[j].id].ToString()
                //    + "/"
                //    + _currentBuild.requiredAmount[j].ToString();
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
                //requiredTexts[z++].text =
                    //CollectibleManager.resourceIndices[_currentBuild.requiredResource[j].id].ToString()
                    //+ "/"
                    //+ _currentBuild.requiredAmount[j].ToString();
            }

        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
