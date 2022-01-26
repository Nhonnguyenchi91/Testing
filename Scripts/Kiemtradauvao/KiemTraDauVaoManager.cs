using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class KiemTraDauVaoManager : MonoBehaviour
{
    [SerializeField] string[] Dialog;
    [SerializeField] int NBDialog;
    [SerializeField] string KiemTraDauVaoStatus;
    [SerializeField] int TongSoCauHoi;
    [SerializeField] GameObject ThamGiaKTDV;
    [SerializeField] GameObject Character;
    [SerializeField] GameObject Dialogue;
    [SerializeField] GameObject PhanTichKetQua;
    [SerializeField] GameObject QuizSystem;
    [SerializeField] GameObject ChiTietKetQua;
    [SerializeField] string[] chitietcacdang;
    [SerializeField] int TongCacDang;
    [SerializeField] GameObject[] CDH;
    [SerializeField] int[] TongSoCauCacDang;
    [SerializeField] int[] TongSoCauDungCacDang;
    [SerializeField] GameObject[] Dang;
    [SerializeField] GameObject LoiGiaiPanel;
    [SerializeField] string DangCanXemHienTai;
    [SerializeField] string DangCanXemTruocDo;
    [SerializeField] Config Config;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(setup());
    }
    IEnumerator setup()
    {
        while (GameObject.Find("QuizManager").GetComponent<NewQuizManager>().ThongTinQuizStatus() != "ReadyForQuery")
        {
            yield return new WaitForSeconds(0.1f);
        }
        CDH = new GameObject[8];
        TongSoCauHoi = GameObject.Find("QuizManager").GetComponent<NewQuizManager>().TongSoCauHoi();
        ChiTietKetQua = PhanTichKetQua.transform.GetChild(1).gameObject; // Gán Object ChiTietKetQua từ scence vào object ChiTietKetQua trong script
        LoiGiaiPanel = PhanTichKetQua.transform.GetChild(2).gameObject;
        // đưa các Object hiển thị các câu (CHD) gán vào mảng Object CHD[] để tiện kiểm soát
        for (int i = 0; i < 8; i++)
        {
            CDH[i] = ChiTietKetQua.transform.GetChild(i).gameObject;
        }
        //
        GameObject.Find("Dialogue").GetComponent<DialogManager>().LoadDialogData(Dialog);
        TongCacDang = GameObject.Find("QuizManager").GetComponent<NewQuizManager>().TongCacDang();
        TongSoCauCacDang = new int[TongCacDang];
        TongSoCauDungCacDang = new int[TongCacDang];
        KiemTraDauVaoStatus = "Start";
        ChiTietKetQua.SetActive(false);
        Dialogue.SetActive(true);
        yield return new WaitForSeconds(1);
    }
    public void ShowLoiGiai()
        {
        StartCoroutine(IE_ShowLoiGiai());
        }
    private IEnumerator IE_ShowLoiGiai()
    {
        GameObject CurrentSelected = EventSystem.current.currentSelectedGameObject;
        string LinkCauHoi = GameObject.Find("QuizManager").GetComponent<NewQuizManager>().LayLinkCauHoi();
        string LinkLoiGiai = GameObject.Find("QuizManager").GetComponent<NewQuizManager>().LayLinkLoiGiai();
        string STTCauHoi = CurrentSelected.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        //Debug.Log(STTCauHoi);
        LoiGiaiPanel = PhanTichKetQua.transform.GetChild(2).gameObject;
        LoiGiaiPanel.SetActive(true);
        Debug.Log(LinkCauHoi + STTCauHoi + ".png");
        LoadImage.Instance.LoadImageFunction(LinkCauHoi + STTCauHoi+".png", "DislayCHPTKQ");
        while (LoadImage.Instance.GetLoadImageStatus()!="finish")
        {
            yield return new WaitForSeconds(0.1f);
        }
        LoadImage.Instance.LoadImageFunction(LinkLoiGiai + STTCauHoi+".png", "DislayLGPTKQ");
        while (LoadImage.Instance.GetLoadImageStatus() != "finish")
        {
            yield return new WaitForSeconds(0.1f);
        }
        PhanTichKetQua.transform.position = GameObject.Find("Canvas").transform.position;
        //Debug.Log(STTCauHoi);
    }    
    public void StartTutorial0()
    {
        StartCoroutine(Tutorial0());
    }       
    //
    // Thủ tục xử lý tổng quá cho bảng phân tích hiện thị kết quả chi tiết từng dạng.
    public void BangKetQuaChiTietTungDang()
    {
        DangCanXemHienTai = EventSystem.current.currentSelectedGameObject.name;
            if(DangCanXemTruocDo == DangCanXemHienTai)
            {
                if(ChiTietKetQua.activeSelf)
                {
                    ChiTietKetQua.SetActive(false);
                }
            else
                {
                ChiTietKetQua.SetActive(true);
                }
            }
            else
            {
                ChiTietKetQua.SetActive(true);
                GameObject DangCanXem = GameObject.Find(EventSystem.current.currentSelectedGameObject.name);
                int j = 0;
                int TongSoCauSai = GameObject.Find("QuizManager").GetComponent<NewQuizManager>().LayTTTongSoCauSai();
                int TongSoCauDung = GameObject.Find("QuizManager").GetComponent<NewQuizManager>().LayTTTongSoCauDung();
                int TongSoCauKTL = GameObject.Find("QuizManager").GetComponent<NewQuizManager>().LayTTTongSoCauKTL();
                string TenDangCanXem = DangCanXem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
                Debug.Log(TenDangCanXem);
                for (int i = 1; i <= TongSoCauHoi; i++)
                {
                    if (GameObject.Find("QuizManager").GetComponent<NewQuizManager>().LayTTDangCuaCH(i) == TenDangCanXem)
                    {
                        CDH[j].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = i.ToString();
                        // Hiển thị các câu sai trong dạng bài này
                        for (int a = 1; a <= TongSoCauSai; a++)
                        {
                            if (GameObject.Find("QuizManager").GetComponent<NewQuizManager>().xemcaccausai(a) == i)
                            {
                                CDH[j].GetComponent<Image>().color = new Color(255, 0, 0, 255); // Màu Đỏ
                            }
                        }
                        //
                        // Hiển thị các câu đúng trong dạng bài này
                        for (int a = 1; a <= TongSoCauDung; a++)
                        {
                            if (GameObject.Find("QuizManager").GetComponent<NewQuizManager>().xemcaccaudung(a) == i)
                            {
                                CDH[j].GetComponent<Image>().color = new Color(0, 255, 0, 255); //Mau Xanh La Cay
                            }
                        }
                        //
                        // Hiển thị các câu không trả lời trong dạng bài này
                        for (int a = 1; a <= TongSoCauKTL; a++)
                        {
                            if (GameObject.Find("QuizManager").GetComponent<NewQuizManager>().xemcaccauktl(a) == i)
                            {
                                CDH[j].GetComponent<Image>().color = new Color(255, 255, 255, 255); // Màu trắng
                            }
                        }
                        //
                        j++;
                    }
                }   
        }
        DangCanXemTruocDo = DangCanXemHienTai;
    }    
    //
    // Hướng dẫn chính cho người chơi về scene KiemTraDauVao
    public IEnumerator Tutorial0()
    {
        yield return new WaitForSeconds(1);
        Character.SetActive(true);
        yield return new WaitForSeconds(1);
        DialogManager.Instance.NewShowDialog();
        yield return new WaitForSeconds(3);
        DialogManager.Instance.NewShowDialog();
        yield return new WaitForSeconds(4);
        DialogManager.Instance.NewShowDialog();
        ThamGiaKTDV.SetActive(true);
        while(!QuizSystem.transform.GetChild(0).gameObject.activeSelf)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Character.SetActive(false);
        Dialogue.transform.GetChild(0).gameObject.SetActive(false);
        Dialogue.transform.GetChild(1).gameObject.SetActive(false);
        Dialogue.transform.GetChild(2).gameObject.SetActive(false);
        KiemTraDauVaoStatus = "Tutorial0Finish";
        while (GameObject.Find("QuizManager").GetComponent<NewQuizManager>().ThongTinQuizStatus()!= "FinishQuiz")
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1.5f);
        PhanTichKetQua.transform.position = QuizSystem.transform.position;
    }  
    //
    // Thủ tục xử lí chính cho chức năng phân tích kết quả
    public void PhanTichKetQuaFunction()
    {
        chitietcacdang = new string[TongCacDang];
        for(int i=1;i<=TongCacDang;i++)
        {
            chitietcacdang[i-1] = GameObject.Find("QuizManager").GetComponent<NewQuizManager>().TenDangCauHoi(i);
        }    
        GameObject PhanTichKetQua = GameObject.Find("PhanTichKetQua");
        GameObject PTKQPanel = PhanTichKetQua.transform.GetChild(0).gameObject;
        Dang = new GameObject[18];
        //Đưa các GameObject thanh hiển thị các dạng của Panel vào mảng Dang[i] để dễ điều khiển
        for (int i = 0;i<5;i++)
        {
            Dang[i] = PTKQPanel.transform.GetChild(i).gameObject;
        }
        //Đưa tên các dạng vào các thanh hiển thị trên panel phân tích
        for (int i = 0; i < TongCacDang; i++)
        {
            Dang[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = chitietcacdang[i];
        }
        //
        for (int SttCacDang = 0; SttCacDang < TongCacDang; SttCacDang++)
        {
            for (int i = 1; i <= TongSoCauHoi; i++)
            {
                if (GameObject.Find("QuizManager").GetComponent<NewQuizManager>().LayTTDangCuaCH(i) == chitietcacdang[SttCacDang])
                {
                    TongSoCauCacDang[SttCacDang]++;
                    if (GameObject.Find("QuizManager").GetComponent<NewQuizManager>().LayTTCacKQNguoiChoiChon(i) == "T")
                    {
                        TongSoCauDungCacDang[SttCacDang]++;
                    }
                }
            }
        }
        //
        // float TiLeSoCauDungMoiDang = (float)TongSoCauDungCacDang[i] / (float)TongSoCauCacDang[i];
        for (int i = 0; i < TongSoCauCacDang.Length; i++)
        {
            float TiLeSoCauDungMoiDang = (float)TongSoCauDungCacDang[i] / (float)TongSoCauCacDang[i];
            if (TiLeSoCauDungMoiDang < ((float)1 / (float)2))
            {
                Dang[i].GetComponent<Image>().color = new Color(255, 0, 0, 255);
            }
            else if (TiLeSoCauDungMoiDang >= ((float)1 / (float)2))
            {
                if (TiLeSoCauDungMoiDang < ((float)8 / (float)10))
                {
                    Dang[i].GetComponent<Image>().color = new Color(255, 255, 98f/255f, 255);
                }
                else if (TiLeSoCauDungMoiDang >= ((float)8 / (float)10))
                {
                    Dang[i].GetComponent<Image>().color = new Color(0, 255, 0, 255);
                }
            }
        }
    }    
    // Update is called once per frame
    void Update()
    {
        if(KiemTraDauVaoStatus == "Start")
        {
            
        }   
        switch(KiemTraDauVaoStatus)
        {
            case "Start":
            {
                    if (GameObject.Find("Dialogue").GetComponent<DialogManager>().DialogStatus == "FinishLoadData")
                    {
                        StartCoroutine(Tutorial0());
                        KiemTraDauVaoStatus = "Tutorial0Processing";
                    }
                    break;
             }
            case "Tutorial0Finish":
                {
                    break;
                }
        }
    }
}
