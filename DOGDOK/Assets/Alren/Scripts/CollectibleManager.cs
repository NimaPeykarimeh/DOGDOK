using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour //Collect iþlemi ve kontrolünün yapýldýðý script. Collect yapýlýnca miktara ekleme yapýlýyor.
{
    private bool isCollectibleFound; // Belli bir collectible'ýn seçildiðinin kontrolü
    private List<int> resourceCountList; // Toplanan resource sayýsý
    private List<Resource1> resourceTypeList; // Toplanan resource türü
    private int objectID; // Þu anda toplanan kaynaðýn ID'sini gösterir. Collectible'ýn deðiþtirilip deðiþtirilmediði kontrolü
    private int currentlyDissolvedID; // Anlatmasý zor ben bile unuttum. Spaghetti code.
    
    private ResourceCreation ResourceCreation; // Toplanan kaynaðýn içindeki bazý bilgileri almak için (resource ve count gibi)

    private List<ResourceCreation> resCreationList = new(); // Cismin içindeki koddur. Method çaðýrýmý için kullanýlýr. Method ile birden fazla cismi generate/dissolve edebiliriz.
    private List<float> animationValueList = new(); // Çözünmekte olan cismi topla/býrak yaptýðýmýz zaman daha akýcý gözükmesi için ve birden fazla cismi generate/dissolve edebilmek için
    private List<int> objectIDList = new(); // Her cismin ID'si tutulur ve böylece daha önce kaydedilen bir cisim varsa ayný id ile baþvurulduðunda reddedilecektir. Listelerin hepsine tekrar ayný bir cisim eklenmez. Cisimler dissolve/generate olunca listeden kalkarlar.

    private float currentAnimationValue = -0.1f; // Collectible'ýn bozulma/generate edilme durumunda hangi sayý deðerinde olduðu
    private const float unsolvedValue = -0.1f; // Collectible'ýn generate olmasý için gerekli olan deðer
    private const float dissolvedValue = 1f; // Collectible'ýn bozulmasý için gerekli olan deðer


    private WeaponController WeaponController;
    [SerializeField] private InventoryManager InventoryManager;

    [Header("Attributes")]
    [SerializeField] private float collectingDistance = 5f; // Kaynak Toplama Raycast Uzunluðu
    [SerializeField] private float generatingDuration = 1f; // Kaynak Toplamada bozulan cismin geri generatelenme uzunluðu
    [SerializeField] private float dissolvingDuration = 1f; // Kaynak Toplama Süresi ve Cismin Bozulma Uzunluðu

    private Ray ray;

    private void Start()
    {
        resourceCountList = new();
        resourceTypeList = new();
        WeaponController = GetComponent<WeaponController>();
        objectID = -1; //objectID için default deðeri -1,
        currentlyDissolvedID = -2; // DissolvedID için ise -2'yi kullandým.
    }

    private void OnEnable()
    {
        isCollectibleFound = false;
        currentAnimationValue = unsolvedValue;
        objectID = -1;
        currentlyDissolvedID = -2;
    }
    private void Update()
    {
        if (WeaponController.canShoot && Input.GetMouseButton(0)) //Toplama iþlemi yapma
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!isCollectibleFound) // Ýlk defa cisimle etkileþim ve cismin bilgilerini alma
            {
                FirstCollectibleHit();
            }
            else //Toplamaya devam
            {
                Gathering();
            }
        }
        else if (isCollectibleFound) //gatheringdeyken ama toplamayý býrakýnca dissolve etmek için
        {
            AddForGenerating();
        }
        else currentlyDissolvedID = -2;


        if (resCreationList.Count > 0)  
        {
            print(resCreationList.Count);
            for (int i = 0; i < resCreationList.Count; i++) // Generation'ý veya dissolve'u bitmiþ cisimleri silme
            {
                if (animationValueList[i] == unsolvedValue || animationValueList[i] == dissolvedValue) 
                {
                    resCreationList.RemoveAt(i);
                    animationValueList.RemoveAt(i);
                    objectIDList.RemoveAt(i);
                }
            }
            for (int i = 0; i < resCreationList.Count; i++) // Generate'i bitmemiþ cisimleri generate etme
            {// NOT: Dissolve iþlemi Gathering()'tedir.
                if (currentlyDissolvedID != objectIDList[i])
                {
                    animationValueList[i] = resCreationList[i].GenerateCollectible(animationValueList[i], unsolvedValue, generatingDuration);
                }

            }
        }
    }

    private void AddForGenerating()
    {
        if (objectIDList.Count == 0) // Hiç bir cisim þuan generate edilmiyorsa
        {
            AddToList(); //Generate edilmesi için Listeye ekle
        }
        else if (objectIDList.Count > 0 && !objectIDList.Exists(x => x == objectID)) // Bazý cisimler generate ediliyorsa VE þu an toplamayý býraktýðýmýz cisim listede yoksa
        {
            AddToList(); //Generate edilmesi için Listeye ekle
        }
        else if (objectIDList.Count > 0 && objectIDList.Exists(x => x == objectID)) // Bazý cisimler generate ediliyorsa VE þu an toplamayý býraktýðýmýz cisim listede varsa
        {
            UpdateTheList(); // O cismi güncelle
        }


        void AddToList() // Listeye generate edilmesi için ekle
        {
            print("ekledim");
            resCreationList.Add(ResourceCreation);
            animationValueList.Add(currentAnimationValue);
            objectIDList.Add(objectID);
            objectID = -1;
            currentlyDissolvedID = -2;
            isCollectibleFound = false;
        }
        void UpdateTheList() // Listedeki cismin bilgilerini güncelle
        {
            int index = objectIDList.FindIndex(x => x == objectID);
            resCreationList[index] = ResourceCreation;
            animationValueList[index] = currentAnimationValue;
            objectIDList[index] = objectID;
            objectID = -1;
            isCollectibleFound = false;
        }
    }

    private void FirstCollectibleHit() // Ýlk defa cisimle etkileþim ve cismin bilgilerini alma
    {
        if (Physics.Raycast(ray, out RaycastHit hit, collectingDistance))
        {
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.TryGetComponent<ResourceCreation>(out ResourceCreation)) //Cisim resource ise gir
            {
                //  Cisimden bilgileri al
                isCollectibleFound = true;

                resourceTypeList = ResourceCreation.resourceTypeList;
                resourceCountList = ResourceCreation.resourceCountList;

                objectID = hitObject.GetInstanceID();
                currentlyDissolvedID = objectID;

                // Cisim daha önceden listeye alýnmýþ mý kontrolleri
                if (objectIDList.Count > 0) 
                {
                    int index = objectIDList.FindIndex(x => x == objectID);
                    if (index != -1)
                    {
                        currentAnimationValue = animationValueList[index]; //Cisim daha önceden listeye alýnmýþsa animasyonda generate edilmenin ilerlemesini düzgün þekilde durdurup, dissolve edilmesi için cismin _Dissolve deðerini tutan deðiþken bilgisi alýnýr.
                        currentlyDissolvedID = -2;
                    }
                    else //Bu ID'de bir cisim yoksa unsolvedValue(-0.1f) durumundadýr.
                    {
                        currentAnimationValue = unsolvedValue;
                    }

                }
                else // Liste boþsa zaten hepsi unsolvedValue(-0.1f) durumundadýr.
                {
                    currentAnimationValue = unsolvedValue;
                }


            }
        }
    }

    private void Gathering()
    {
        if (Physics.Raycast(ray, out RaycastHit hit, collectingDistance))
        {
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.GetInstanceID() == objectID)
            {
                currentlyDissolvedID = objectID;
                currentAnimationValue = ResourceCreation.DissolveCollectible(currentAnimationValue, dissolvedValue, dissolvingDuration);
                if (currentAnimationValue == dissolvedValue)
                {
                    // Cisim tamamen saydamlaþýnca cismi yok et ve envanteri güncelle
                    Destroy(hitObject);
                    Dictionary<Resource1, int> addingResources = new();
                    for(int i = 0; i < resourceCountList.Count && i < resourceTypeList.Count; i++)
                    {
                        addingResources.Add(resourceTypeList[i], resourceCountList[i]);
                    }
                    InventoryManager.AddResources(addingResources);
                }
            }
            else
            {
                print("farklý cisim");
                AddForGenerating(); // Vurulan obje deðiþtiyse, eski vurulan objeyi kaydedip geri generate et.
            }
        }
    }
}
