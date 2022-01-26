using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HeroesDataManager : MonoBehaviour
{
    public GameObject HeroesDatabase;
    public GameObject HeroInfo;
    public GameObject[] LV = new GameObject[11];
    public Image HeroImage;
    public TextMeshProUGUI HeroTitleTMP;
    public TextMeshProUGUI ATKVALUETMP;
    public TextMeshProUGUI DEFVALUETMP;
    public GameObject HeroAttribute;
    public GameObject ArmyType;
    public int RowStoredData;
    public string HeroLevel;
    public string HeroIconImageString;
    // Start is called before the first frame update
    public void UploadHeroInformation(string FindHeroID)
    {
        int Rows = HeroesDatabase.GetComponent<NewDatabaseLoader>().Rows;
        int Columns = HeroesDatabase.GetComponent<NewDatabaseLoader>().Columns;
        for(int i=0;i< Rows;i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if(FindHeroID == HeroesDatabase.GetComponent<NewDatabaseLoader>().DatabaseTable[i,j])
                {
                    RowStoredData = i;
                    var Sprite = Resources.Load<Sprite>(HeroesDatabase.GetComponent<NewDatabaseLoader>().DatabaseTable[i, 5]);
                    HeroImage.sprite = Sprite;
                    Sprite = Resources.Load<Sprite>(HeroesDatabase.GetComponent<NewDatabaseLoader>().DatabaseTable[i, 4]);
                    HeroAttribute.GetComponent<Image>().sprite = Sprite;
                    Sprite = Resources.Load<Sprite>(HeroesDatabase.GetComponent<NewDatabaseLoader>().DatabaseTable[i, 6]);
                    ArmyType.GetComponent<Image>().sprite = Sprite;
                    HeroTitleTMP.text = HeroesDatabase.GetComponent<NewDatabaseLoader>().DatabaseTable[i, 3];
                    ATKVALUETMP.text = HeroesDatabase.GetComponent<NewDatabaseLoader>().DatabaseTable[i, 8] + " - " + HeroesDatabase.GetComponent<NewDatabaseLoader>().DatabaseTable[i, 9];
                    DEFVALUETMP.text = HeroesDatabase.GetComponent<NewDatabaseLoader>().DatabaseTable[i, 10] + " - " + HeroesDatabase.GetComponent<NewDatabaseLoader>().DatabaseTable[i, 11];
                    HeroLevel = HeroesDatabase.GetComponent<NewDatabaseLoader>().DatabaseTable[i, 7];
                    HeroIconImageString = HeroesDatabase.GetComponent<NewDatabaseLoader>().DatabaseTable[i, 13];
                    for (int a = 1; a <= int.Parse(HeroLevel); a++)
                    {
                        LV[a].SetActive(true);
                    }
                        for(int b = int.Parse(HeroLevel)+1; b<=10;b++ )
                    {
                        LV[b].SetActive(false);
                    }
                }
                else
                {

                }
            }
        }       
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
