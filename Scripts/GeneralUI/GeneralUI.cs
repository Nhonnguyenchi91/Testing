using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GeneralUI : MonoBehaviour
{
    public GameObject PhanThuongManager;
    public GameObject Information;
    public GameObject HeroInfo;
    public GameObject ObjectNeedRefer;
    public int checkclickChiTietPhanThuong = 0;
    public GameObject Event;
    // Start is called before the first frame update
    void Start()
    {
        PhanThuongManager.SetActive(true);
        Information.SetActive(true);
    }
    public void ChiTietPhanThuong()
    {
        if(checkclickChiTietPhanThuong == 0)
        {
            ObjectNeedRefer = EventSystem.current.currentSelectedGameObject;
            string LoaiPhanThuong = ObjectNeedRefer.GetComponent<ThongTinPhanThuong>().LoaiPhanThuong;
            string IDPhanThuong = ObjectNeedRefer.GetComponent<ThongTinPhanThuong>().IDPhanThuong;
            Information.GetComponent<InformationManager>().XemChiTietPhanThuong(LoaiPhanThuong, IDPhanThuong);
            checkclickChiTietPhanThuong = 1;
        }
        else if (checkclickChiTietPhanThuong == 1)
        {
            HeroInfo.SetActive(false);
            checkclickChiTietPhanThuong = 0;
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
