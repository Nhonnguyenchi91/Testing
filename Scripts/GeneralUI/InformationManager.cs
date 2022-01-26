using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InformationManager : MonoBehaviour
{
    public GameObject HeroIconClickPanel;
    public GameObject GeneralHeroTeamPanel;
    public GameObject LordEquipInfor;
    public GameObject LordHeroes;
    public GameObject HeroesDabase;
    public GameObject HeroInfo;
    public GameObject HeroesPanel;
    public GameObject ObjectNeedRefer;
    public string[] MainTeamHerosList = new string[5];
    public string[] PlayerHerolist = new string[200];
    public GameObject[] MainTeamHero = new GameObject[5];
    // Start is called before the first frame update
    void Start()
    {
        LordEquipInfor.SetActive(false);
        LordHeroes.SetActive(false);
        HeroInfo.SetActive(false);
        HeroesPanel.SetActive(false);
        HeroIconClickPanel.SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            MainTeamHerosList[i] = null;
        }
    }
    public void HeroIconClick()
    {
        HeroIconClickPanel.SetActive(true);
        ObjectNeedRefer = EventSystem.current.currentSelectedGameObject;
        HeroIconClickPanel.transform.position = EventSystem.current.currentSelectedGameObject.transform.position + new Vector3(100,0,0);
    }
    public void LenDoiHinhChinh()
    {
        HeroIconClickPanel.SetActive(false);
        GeneralHeroTeamPanel.SetActive(true);
        for (int i=0;i<5;i++)
        {
           if(MainTeamHerosList[i]==null)
            {
                MainTeamHero[i].GetComponent<Image>().sprite = ObjectNeedRefer.GetComponent<Image>().sprite;
                break;
            }    
        }    
    }
    public void XemChiTietPhanThuong(string LoaiPhanThuong, string IDPhanThuong)
    {
        switch (LoaiPhanThuong)
        {
            case "Hero":
                {
                    HeroInfo.SetActive(true);
                    HeroesDabase.GetComponent<HeroesDataManager>().UploadHeroInformation(IDPhanThuong);
                    break;
                }
        }
    }
    public void XemChiTiet()
    {
        HeroInfo.SetActive(true);
    }
    public void GiaoDich()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
