using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PhanThuongManager : MonoBehaviour
{
    public GameObject[] phanthuong =new GameObject[7];
    public bool TestMode;
    public string Mode;
    public string PhanThuongManagerStatus;
    public int SoPhanThuong;
    public GameObject PhanThuongPanel;
    public string[] IDPhanThuongList = new string[6];
    public TextMeshProUGUI ThongBaoPanel;
    public TextMeshProUGUI XacNhanPhanThuong;
    // Start is called before the first frame update
    void Start()
    {
        PhanThuongPanel.SetActive(false);
        PhanThuongManagerStatus = "KetThucStart";
    }
    public void UpdateThongTinPhanThuong(int vitri,string LoaiPhanThuong, string IDPhanThuong, string IconImagePhanThuong)
    {
        phanthuong[vitri].GetComponent<ThongTinPhanThuong>().LoaiPhanThuong = LoaiPhanThuong;
        phanthuong[vitri].GetComponent<ThongTinPhanThuong>().IDPhanThuong = IDPhanThuong;
        phanthuong[vitri].GetComponent<ThongTinPhanThuong>().IconImagePhanThuong = IconImagePhanThuong;
        PhanThuongManagerStatus = "KetThucUpdateThongTinPhanThuong";
    }
    public void XuLiThongBao(string KetQua)
    {
        if(KetQua=="Dung")
        {
            ThongBaoPanel.text = "Xin chúc mừng! Bạn đã trả lời đúng!";
            ThongBaoPanel.color = new Color32(255, 237, 100, 255);
            XacNhanPhanThuong.text = "Nhận Thưởng";
            PhanThuongManagerStatus = "KetThucXuLiThongBao";
        }  
        else if(KetQua == "Sai")
        {
            ThongBaoPanel.text = "Thật không may! Bạn đã trả lời sai!";
            ThongBaoPanel.color = new Color32(255, 7, 0, 255);
            XacNhanPhanThuong.text = "Kết Thúc";
            PhanThuongManagerStatus = "KetThucXuLiThongBao";
        }    
    }    
    public void PhanThuongUI()
    {
        PhanThuongPanel.SetActive(true);
        phanthuong[0].SetActive(true);
        for (int i = 1; i < 7; i++)
        {
            phanthuong[i].SetActive(false);
        }
    }
    public void HoanThanhNhanThuong()
    {
        GameObject.Find("GeneralUI").GetComponent<GeneralUI>().checkclickChiTietPhanThuong = 1;
        GameObject.Find("GeneralUI").GetComponent<GeneralUI>().ChiTietPhanThuong();
        PhanThuongPanel.SetActive(false);
        PhanThuongManagerStatus = "HoanTatNhanThuong";
    }
    public void XuLiPhanThuong(int SoPhanThuong,string Mode)
    {
        StartCoroutine(XuLiPhanThuongIEnumerator(SoPhanThuong,Mode));
    }
    IEnumerator XuLiPhanThuongIEnumerator(int SoPhanThuong, string Mode)
    {
        if(Mode == "nhieuphanthuong")
        {
            for (int i = 1; i <= SoPhanThuong; i++)
            {
                yield return new WaitForSeconds(1.5f);
                phanthuong[i].SetActive(true);
                var Sprite = Resources.Load<Sprite>(phanthuong[i].GetComponent<ThongTinPhanThuong>().IconImagePhanThuong);
                phanthuong[i].GetComponent<Image>().sprite = Sprite;
                yield return new WaitForSeconds(0.5f);
                phanthuong[i].GetComponent<Animator>().SetBool("phanthuongidle", true);
                yield return new WaitForSeconds(0.5f);
                PhanThuongManagerStatus = "ketthucnhieuphanthuong";
            }
        }
        else if(Mode =="motphanthuong")
        {
            yield return new WaitForSeconds(1.5f);
            phanthuong[2].SetActive(true);
            var Sprite = Resources.Load<Sprite>(phanthuong[2].GetComponent<ThongTinPhanThuong>().IconImagePhanThuong);
            phanthuong[2].GetComponent<Image>().sprite = Sprite;
            yield return new WaitForSeconds(0.5f);
            phanthuong[2].GetComponent<Animator>().SetBool("phanthuongidle", true);
            yield return new WaitForSeconds(0.5f);
            PhanThuongManagerStatus = "ketthucmotphanthuong";
        }
                  
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
