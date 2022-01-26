using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using TMPro;


public class NewQuizManager : MonoBehaviour
{
    public GameObject QuizQuestionPanel;
    public GameObject QuizDatabaseLoader;
    public GameObject QuizDatabase;
    public GameObject Timer;
    public GameObject Next;
    public GameObject Back;
    public GameObject Complete;
    public GameObject A;
    public GameObject B;
    public GameObject C;
    public GameObject D;
    public TextMeshProUGUI ThongBaoQuiz;
    public Image Pic;
    [SerializeField] GameObject BangCauHoi;
    public bool ModeCauHoiNhieuLan = true;
    public string AnswerResult;
    public string Quizmode;
    private int TTcauhoi;
    private int Socaudung;
    private string linkQuizData;
    private string DapAn;
    public int TTcauhoiCotTruyen;
    public string[] CacKQNguoiChoiChon;
    [SerializeField] string[] DangCauHoi;
    [SerializeField] string[] CacDangCauHoi;
    private float timermonitor;
    private bool checktimer = true;
    private bool FirstStart = false;
    public bool QuizTutorial = false;
    public string QuizStatus;
    [SerializeField] int TongCauHoi;
    [SerializeField] int TongSoCauDung; // Tổng Số Câu Đúng;
    [SerializeField] int TongSoCauSai;  // Tổng Số Câu Sai;
    [SerializeField] int TongSoCauKTL; // Tổng Số Câu Không Trả Lời;
    [SerializeField] int[] CacCauSaiOCacDang; //Các Câu Sai Ở Các Dạng;
    [SerializeField] int[] CacCauDungOCacDang; //Các Câu Đúng Ở Các Dạng;
    [SerializeField] int[] CacCauKTLOCacDang; //Các Câu KTL Ở Các Dạng;
    [SerializeField] string check_choice;
    [SerializeField] string LinkLoiGiai;
    [SerializeField] string LinkCauHoi;
    // Start is called before the first frame update
    void Start()
    {
        Socaudung = 0;
        TTcauhoi = 1;
        TongCauHoi = 0;
        TongSoCauSai = 0;
        TTcauhoiCotTruyen = 1;
        TongSoCauDung = 0;
        TongSoCauKTL = 0;
        QuizQuestionPanel.SetActive(false);
        ThongBaoQuiz.text = "";
        StartCoroutine(LoadDangCauHoi());
    }
    public string LayLinkLoiGiai()
       // Hàm để các script khác lấy link lời giải
    {
        return LinkLoiGiai;
    }
    public string LayLinkCauHoi()
    // Hàm để các script khác lấy link lời giải
    {
        return LinkCauHoi;
    }
    public string ThongTinQuizStatus()
    {
        return QuizStatus;
    }    
    void xulicaccauDSKTL(int TongCauHoi,string mode)
        // Xử lí các câu hỏi mà người chơi không trả lời
        {
        int i = 1;
        for (int j = 1; j <= TongCauHoi; j++)
            {
                switch(mode)
                {
                    case "KTL":
                        {
                            if (CacKQNguoiChoiChon[j] == null)
                            {
                                CacCauKTLOCacDang[i] = j;
                            i++;
                            }
                            break;
                        }
                    case "Dung":
                        {
                            if (CacKQNguoiChoiChon[j] == "T")
                            {
                                CacCauDungOCacDang[i] = j;
                            i++;
                        }
                            break;
                        }
                    case "Sai":
                        {
                            if (CacKQNguoiChoiChon[j] == "F")
                            {
                                CacCauSaiOCacDang[i] = j;
                            i++;
                        }
                            break;
                        }

                }    
            }
    }
    public IEnumerator IE_HoanThanhQuiz()
    {
        // tính tổng số câu đúng, sai, không trả lời của bài kiểm tra
        for (int i = 1; i <= TongCauHoi; i++)
        {
            if (CacKQNguoiChoiChon[i] == null)
            {
                TongSoCauKTL++;
            }
            else if (CacKQNguoiChoiChon[i] == "T")
            {
                TongSoCauDung++;
            }
            else if (CacKQNguoiChoiChon[i] == "F")
            {
                TongSoCauSai++;
            }
        }
        CacCauSaiOCacDang = new int[TongSoCauSai + 1];
        CacCauDungOCacDang = new int[TongSoCauDung + 1];
        CacCauKTLOCacDang = new int[TongSoCauKTL + 1];
        // xác định các câu đúng, sai, không trả lời của bài kiểm tra
        xulicaccauDSKTL(TongCauHoi, "KTL");
        xulicaccauDSKTL(TongCauHoi, "Dung");
        xulicaccauDSKTL(TongCauHoi, "Sai");
        QuizStatus = "FinishQuiz";
        yield return new WaitForSeconds(0.1f);
    }
    public int TongSoCauHoi()
    // Tổng số các dạng trong bài kiểm tra
    {
        return TongCauHoi;
    }
    public int LayTTTongSoCauSai()
    // Tổng số câu sai trong bài kiểm tra
    {
        return TongSoCauSai;
    }
    public string LayTTCacKQNguoiChoiChon(int stt)
    // Tổng số câu sai trong bài kiểm tra
    {
        return CacKQNguoiChoiChon[stt];
    }
    public int LayTTTongSoCauDung()
    // Tổng số câu sai trong bài kiểm tra
    {
        return TongSoCauDung;
    }
    public int LayTTTongSoCauKTL()
    // Tổng số câu sai trong bài kiểm tra
    {
        return TongSoCauKTL;
    }
    public int xemcaccausai(int stt)
    {
        return CacCauSaiOCacDang[stt];
    }
    public int xemcaccaudung(int stt)
    {
        return CacCauDungOCacDang[stt];
    }
    public int xemcaccauktl(int stt)
    {
        return CacCauKTLOCacDang[stt];
    }
    public int TongCacDang()
    // Tổng số các dạng trong bài kiểm tra
    {
        return CacDangCauHoi.Length - 1;
    }
    public string TenDangCauHoi(int stt)
    // Lấy thông tin tên các dạng câu hỏi
    {
        return CacDangCauHoi[stt];
    }
    public string LayTTDangCuaCH(int stt)
    //Lấy thông tin dạng của câu hỏi
    {
        return DangCauHoi[stt];
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void HoanThanhQuiz()
    {
        StartCoroutine(IE_HoanThanhQuiz());
    }    
    public void AnHienBangCauHoi()
    // Ẩn hiện bảng quản lí câu hỏi
    {
        if (GameObject.Find("BangCauHoi").transform.position == GameObject.Find("QuízSystem").transform.position)
        {
            GameObject.Find("BangCauHoi").transform.position = GameObject.Find("GiauBangCauHoi").transform.position;
        }
        else if(GameObject.Find("BangCauHoi").transform.position == GameObject.Find("GiauBangCauHoi").transform.position)
        {
            GameObject.Find("BangCauHoi").transform.position = GameObject.Find("QuízSystem").transform.position;
        }
    }
    public void DenCauHoiChiDinh()
        // đưa đến câu hỏi được người dùng nhấn trên bảng quản lí câu hỏi
    {
        string cauhoichidinh = EventSystem.current.currentSelectedGameObject.name;
        TTcauhoi = int.Parse(cauhoichidinh);
        linkQuizData = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseTable[TTcauhoi-1, 3];
        DapAn = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseTable[TTcauhoi-1, 2];
        StartCoroutine(GetImageFromServer(linkQuizData));
        if (CacKQNguoiChoiChon[TTcauhoi] != null)
        {
            Debug.Log("datraloi");
            A.SetActive(false);
            B.SetActive(false);
            C.SetActive(false);
            D.SetActive(false);
        }
        else if (CacKQNguoiChoiChon[TTcauhoi] == null)
        {
            A.SetActive(true);
            B.SetActive(true);
            C.SetActive(true);
            D.SetActive(true);
        }
    }
    public void CheckDapAn()
    {
        //Xử lí đáp án vừa được người dùng chọn
        check_choice = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(check_choice);
        GameObject.Find(TTcauhoi.ToString()).GetComponent<Image>().color = new Color(0, 255, 0, 255);
        if (check_choice == DapAn)
        {
            Debug.Log("correct");
            AnswerResult = "Dung";
            Socaudung++;
            CacKQNguoiChoiChon[TTcauhoi] = "T";
            A.SetActive(false);
            B.SetActive(false);
            C.SetActive(false);
            D.SetActive(false);
        }
        else
        {
            Debug.Log("incorrect");
            AnswerResult = "Sai";
            CacKQNguoiChoiChon[TTcauhoi] = "F";
            A.SetActive(false);
            B.SetActive(false);
            C.SetActive(false);
            D.SetActive(false);
        }
        if (Quizmode == "1cauhoi")
        {
            QuizQuestionPanel.SetActive(false);
        }
        else if (Quizmode == "cottruyen")
        {
            if(AnswerResult == "Sai")
            {
                if(TTcauhoiCotTruyen==1)
                {
                    ThongBaoQuiz.text = "Trả lời sai rồi! Lần đầu nên có cơ hội trả lời lại! Chỉ duy nhất lần này thôi đấy!";
                    A.SetActive(true);
                    B.SetActive(true);
                    C.SetActive(true);
                    D.SetActive(true);
                }    
                
            }
            else
            {
                TTcauhoiCotTruyen++;
                ThongBaoQuiz.text = "";
                QuizStatus = "FinishCHCotTruyen";
                QuizQuestionPanel.SetActive(false);
            }         
        }
        else
        {
            check_choice = "Next";
            LoadQuiz(false);
        }    
    }
    public void LoadQuiz(bool ActivateFromButton)
    {
        if (!ActivateFromButton)
        {
            if(check_choice != "Next")
            {
                check_choice = "Start";
            }    
        }
        else
        {
        check_choice = EventSystem.current.currentSelectedGameObject.name;
        }
        switch (check_choice)
        {
            case "Start":
                {
                    if (ModeCauHoiNhieuLan)
                    {
                        if (FirstStart)
                        {
                            QuizDatabase.GetComponent<NewQuizDatabase>().DongBoHoaData1CauHoi();
                            FirstStart = false;
                        }
                        if (Quizmode == "kiemtra15p")
                        {
                            Timer.GetComponent<Timer>().timevalue = 900;
                            checktimer = true;
                            Next.SetActive(true);
                            Back.SetActive(true);
                            QuizQuestionPanel.SetActive(true);
                            linkQuizData = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseTable[0, 3];
                            DapAn = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseTable[0, 2];
                            StartCoroutine(GetImageFromServer(linkQuizData));
                        }
                        else if (Quizmode == "kiemtra45p")
                        {
                            Timer.GetComponent<Timer>().timevalue = 2700;
                            checktimer = true;
                            Next.SetActive(true);
                            Back.SetActive(true);
                            QuizQuestionPanel.SetActive(true);
                            linkQuizData = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseTable[0, 3];
                            DapAn = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseTable[0, 2];
                            StartCoroutine(GetImageFromServer(linkQuizData));
                        }
                        else if (Quizmode == "cottruyen")
                        {
                            Timer.GetComponent<Timer>().timevalue = 30;
                            checktimer = true;
                            Next.SetActive(false);
                            Back.SetActive(false);
                            QuizQuestionPanel.SetActive(true);
                            linkQuizData = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseTable[0, 3];
                            DapAn = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseTable[0, 2];
                            Debug.Log(DapAn);
                            StartCoroutine(GetImageFromServer(linkQuizData));
                        }
                        else if (Quizmode == "1cauhoi")
                        {
                            Timer.GetComponent<Timer>().timevalue = 90;
                            checktimer = true;
                            Next.SetActive(false);
                            Back.SetActive(false);
                            Complete.SetActive(false);
                            A.SetActive(true);
                            B.SetActive(true);
                            C.SetActive(true);
                            D.SetActive(true);
                            QuizQuestionPanel.SetActive(true);
                            QuizDatabase.GetComponent<NewQuizDatabase>().Randomcauhoi();
                            int cauhoi = QuizDatabase.GetComponent<NewQuizDatabase>().SoRamdomCauHoi;
                            linkQuizData = QuizDatabase.GetComponent<NewQuizDatabase>().QuizDatabase1CauHoiTable[cauhoi, 3];
                            DapAn = QuizDatabase.GetComponent<NewQuizDatabase>().QuizDatabase1CauHoiTable[cauhoi, 2];
                            StartCoroutine(GetImageFromServer(linkQuizData));
                            QuizDatabase.GetComponent<NewQuizDatabase>().SapxeplaiTable();
                        }
                    }
                    break;
                }
            case "Back":
                {
                    if (TTcauhoi > 1)
                    {
                        TTcauhoi--;
                        linkQuizData = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseTable[TTcauhoi-1, 3];
                        DapAn = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseTable[TTcauhoi-1, 2];
                        StartCoroutine(GetImageFromServer(linkQuizData));
                        if (CacKQNguoiChoiChon[TTcauhoi] != null)
                        {
                            Debug.Log("datraloi");
                            A.SetActive(false);
                            B.SetActive(false);
                            C.SetActive(false);
                            D.SetActive(false);
                        }
                        else if (CacKQNguoiChoiChon[TTcauhoi] == null)
                        {
                            A.SetActive(true);
                            B.SetActive(true);
                            C.SetActive(true);
                            D.SetActive(true);
                        }
                    }

                    break;
                }
            case "Next":
                {
                    TTcauhoi = TTcauhoi + 1;
                    Debug.Log(TTcauhoi);
                    linkQuizData = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseTable[TTcauhoi-1, 3];
                    DapAn = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseTable[TTcauhoi-1, 2];
                    if (linkQuizData == null)
                    {
                        TTcauhoi--;
                        linkQuizData = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseTable[TTcauhoi-1, 3];
                        DapAn = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseTable[TTcauhoi-1, 2];
                    }
                    else if (linkQuizData != null)
                    {
                        StartCoroutine(GetImageFromServer(linkQuizData));
                    }
                    if (CacKQNguoiChoiChon[TTcauhoi] != null)
                    {
                        Debug.Log("datraloi");
                        A.SetActive(false);
                        B.SetActive(false);
                        C.SetActive(false);
                        D.SetActive(false);
                    }
                    else if (CacKQNguoiChoiChon[TTcauhoi] == null)
                    {
                        A.SetActive(true);
                        B.SetActive(true);
                        C.SetActive(true);
                        D.SetActive(true);
                    }
                    break;
                }
            case "Complete":
                {
                    QuizQuestionPanel.SetActive(false);
                    break;
                }
        }
    }
    public void QuizModeCotTruyen()
    {

    }
    public IEnumerator GetImageFromServer(string Url)
    {
        UnityWebRequest reg = UnityWebRequestTexture.GetTexture(Url);
        yield return reg.SendWebRequest();
        if (reg.isNetworkError || reg.isNetworkError)
        {
            Debug.Log(reg.error);
        }
        else
        {
            Texture2D img = ((DownloadHandlerTexture)reg.downloadHandler).texture;
            Pic.sprite = Sprite.Create(img, new Rect(0, 0, 1200, 400), Vector2.zero);
        }
    }
    public IEnumerator LoadDangCauHoi()
    {
        int sodangcauhoi = 0;
        while(QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseLoaderStatus != "FinishLoadData")
        {
            yield return new WaitForSeconds(0.1f);
        }
        int Rows = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().Rows;
        TongCauHoi = Rows;
        DangCauHoi = new string[Rows+1];
        string[] LuuSoDangTamThoi = new string[Rows + 1];
        CacKQNguoiChoiChon = new string[Rows + 1];
        for (int i = 1; i<= Rows;i++)
        {
            DangCauHoi[i] = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseTable[i - 1, 4];
            LuuSoDangTamThoi[i] = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseTable[i - 1, 5];
            CacKQNguoiChoiChon[i]= null;
            if (LuuSoDangTamThoi[i] != "")
            {
                sodangcauhoi++;
            }    
        }
        CacDangCauHoi = new string[sodangcauhoi+1];
        for(int i = 1; i < CacDangCauHoi.Length; i++)
        {
            CacDangCauHoi[i] = LuuSoDangTamThoi[i];
        }
        QuizStatus = "ReadyForQuery";
    }
    public void RamdomCauHoi()
    {
        int a = Random.Range(0, 10);
        Debug.Log(a);
    }
    // Update is called once per frame
    void Update()
    {
        if(QuizQuestionPanel.activeSelf)
        {
            timermonitor = Timer.GetComponent<Timer>().timevalue;
            if (timermonitor == 0 && checktimer)
            {
                Debug.Log("hetgio");
                checktimer = false;
            }
        }
    }
}
