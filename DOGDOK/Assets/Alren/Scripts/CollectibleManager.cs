using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour //Collect i�lemi ve kontrol�n�n yap�ld��� script. Collect yap�l�nca miktara ekleme yap�l�yor.
{
    private bool isCollectibleFound; // Belli bir collectible'�n se�ildi�inin kontrol�
    private List<int> resourceCountList; // Toplanan resource say�s�
    private List<Resource1> resourceTypeList; // Toplanan resource t�r�
    private int objectID; // �u anda toplanan kayna��n ID'sini g�sterir. Collectible'�n de�i�tirilip de�i�tirilmedi�i kontrol�
    private int currentlyDissolvedID; // Anlatmas� zor ben bile unuttum. Spaghetti code.
    
    private ResourceCreation ResourceCreation; // Toplanan kayna��n i�indeki baz� bilgileri almak i�in (resource ve count gibi)

    private List<ResourceCreation> resCreationList = new(); // Cismin i�indeki koddur. Method �a��r�m� i�in kullan�l�r. Method ile birden fazla cismi generate/dissolve edebiliriz.
    private List<float> animationValueList = new(); // ��z�nmekte olan cismi topla/b�rak yapt���m�z zaman daha ak�c� g�z�kmesi i�in ve birden fazla cismi generate/dissolve edebilmek i�in
    private List<int> objectIDList = new(); // Her cismin ID'si tutulur ve b�ylece daha �nce kaydedilen bir cisim varsa ayn� id ile ba�vuruldu�unda reddedilecektir. Listelerin hepsine tekrar ayn� bir cisim eklenmez. Cisimler dissolve/generate olunca listeden kalkarlar.

    private float currentAnimationValue = -0.1f; // Collectible'�n bozulma/generate edilme durumunda hangi say� de�erinde oldu�u
    private const float unsolvedValue = -0.1f; // Collectible'�n generate olmas� i�in gerekli olan de�er
    private const float dissolvedValue = 1f; // Collectible'�n bozulmas� i�in gerekli olan de�er


    private WeaponController WeaponController;
    [SerializeField] private InventoryManager InventoryManager;

    [Header("Attributes")]
    [SerializeField] private float collectingDistance = 5f; // Kaynak Toplama Raycast Uzunlu�u
    [SerializeField] private float generatingDuration = 1f; // Kaynak Toplamada bozulan cismin geri generatelenme uzunlu�u
    [SerializeField] private float dissolvingDuration = 1f; // Kaynak Toplama S�resi ve Cismin Bozulma Uzunlu�u

    private Ray ray;

    private void Start()
    {
        resourceCountList = new();
        resourceTypeList = new();
        WeaponController = GetComponent<WeaponController>();
        objectID = -1; //objectID i�in default de�eri -1,
        currentlyDissolvedID = -2; // DissolvedID i�in ise -2'yi kulland�m.
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
        if (WeaponController.canShoot && Input.GetMouseButton(0)) //Toplama i�lemi yapma
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!isCollectibleFound) // �lk defa cisimle etkile�im ve cismin bilgilerini alma
            {
                FirstCollectibleHit();
            }
            else //Toplamaya devam
            {
                Gathering();
            }
        }
        else if (isCollectibleFound) //gatheringdeyken ama toplamay� b�rak�nca dissolve etmek i�in
        {
            AddForGenerating();
        }
        else currentlyDissolvedID = -2;


        if (resCreationList.Count > 0)  
        {
            print(resCreationList.Count);
            for (int i = 0; i < resCreationList.Count; i++) // Generation'� veya dissolve'u bitmi� cisimleri silme
            {
                if (animationValueList[i] == unsolvedValue || animationValueList[i] == dissolvedValue) 
                {
                    resCreationList.RemoveAt(i);
                    animationValueList.RemoveAt(i);
                    objectIDList.RemoveAt(i);
                }
            }
            for (int i = 0; i < resCreationList.Count; i++) // Generate'i bitmemi� cisimleri generate etme
            {// NOT: Dissolve i�lemi Gathering()'tedir.
                if (currentlyDissolvedID != objectIDList[i])
                {
                    animationValueList[i] = resCreationList[i].GenerateCollectible(animationValueList[i], unsolvedValue, generatingDuration);
                }

            }
        }
    }

    private void AddForGenerating()
    {
        if (objectIDList.Count == 0) // Hi� bir cisim �uan generate edilmiyorsa
        {
            AddToList(); //Generate edilmesi i�in Listeye ekle
        }
        else if (objectIDList.Count > 0 && !objectIDList.Exists(x => x == objectID)) // Baz� cisimler generate ediliyorsa VE �u an toplamay� b�rakt���m�z cisim listede yoksa
        {
            AddToList(); //Generate edilmesi i�in Listeye ekle
        }
        else if (objectIDList.Count > 0 && objectIDList.Exists(x => x == objectID)) // Baz� cisimler generate ediliyorsa VE �u an toplamay� b�rakt���m�z cisim listede varsa
        {
            UpdateTheList(); // O cismi g�ncelle
        }


        void AddToList() // Listeye generate edilmesi i�in ekle
        {
            print("ekledim");
            resCreationList.Add(ResourceCreation);
            animationValueList.Add(currentAnimationValue);
            objectIDList.Add(objectID);
            objectID = -1;
            currentlyDissolvedID = -2;
            isCollectibleFound = false;
        }
        void UpdateTheList() // Listedeki cismin bilgilerini g�ncelle
        {
            int index = objectIDList.FindIndex(x => x == objectID);
            resCreationList[index] = ResourceCreation;
            animationValueList[index] = currentAnimationValue;
            objectIDList[index] = objectID;
            objectID = -1;
            isCollectibleFound = false;
        }
    }

    private void FirstCollectibleHit() // �lk defa cisimle etkile�im ve cismin bilgilerini alma
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

                // Cisim daha �nceden listeye al�nm�� m� kontrolleri
                if (objectIDList.Count > 0) 
                {
                    int index = objectIDList.FindIndex(x => x == objectID);
                    if (index != -1)
                    {
                        currentAnimationValue = animationValueList[index]; //Cisim daha �nceden listeye al�nm��sa animasyonda generate edilmenin ilerlemesini d�zg�n �ekilde durdurup, dissolve edilmesi i�in cismin _Dissolve de�erini tutan de�i�ken bilgisi al�n�r.
                        currentlyDissolvedID = -2;
                    }
                    else //Bu ID'de bir cisim yoksa unsolvedValue(-0.1f) durumundad�r.
                    {
                        currentAnimationValue = unsolvedValue;
                    }

                }
                else // Liste bo�sa zaten hepsi unsolvedValue(-0.1f) durumundad�r.
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
                    // Cisim tamamen saydamla��nca cismi yok et ve envanteri g�ncelle
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
                print("farkl� cisim");
                AddForGenerating(); // Vurulan obje de�i�tiyse, eski vurulan objeyi kaydedip geri generate et.
            }
        }
    }
}
