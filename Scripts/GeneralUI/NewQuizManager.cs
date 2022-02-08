using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using TMPro;


public class NewQuizManager : MonoBehaviour
{
    public GameObject QuizQuestionPanel; // Game Object chứa giao diện của Quiz question
    public GameObject QuizDatabaseLoader; // Game Object chứa script điều khiển việc load Quiz Database từ server và xử lí chúng.
    public GameObject QuizDatabase; // Game Object chứa script điều khiển Quiz Database
    public GameObject Timer; // hiển thị thời gian làm bài
    public GameObject Next; // Nút nhấn tiếp tục
    public GameObject Back; // Nút nhấn quay lại bài kiểm tra trước
    public GameObject Complete; // Nút nhấn hoàn thành bài kiểm tra
    [SerializeField] GameObject A; // Nút nhấn đáp án A
    [SerializeField] GameObject B; // Nút nhấn đáp án B
    [SerializeField] GameObject C; // Nút nhấn đáp án C
    [SerializeField] GameObject D; // Nút nhấn đáp án D
    public TextMeshProUGUI ThongBaoQuiz; // Text hiển thị các thông báo của Quiz
    public Image Pic; // Game Object hiển thị câu hỏi của Quiz
    [SerializeField] GameObject BangCauHoi;
    public bool ModeCauHoiNhieuLan = true; // mode xác nhận bài kiểm tra là một câu hỏi đơn lẽ hay nhiều câu hỏi
    [SerializeField] string AnswerResult; // Kết quả của câu trả lời người chơi vừa chọn
    public string Quizmode; // Các chế dộ của bài Quiz
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
    // Hàm để các script khác lấy link lời giải
    public string LayLinkLoiGiai()
    {
        return LinkLoiGiai;
    }
    // Hàm để các script khác lấy link câu hỏi
    public string LayLinkCauHoi()
    {
        return LinkCauHoi;
    }
    // Hàm để các script khác thông tin Quiz Status
    public string ThongTinQuizStatus()
    {
        return QuizStatus;
    }  
    // Xử lí phân loại các câu mà người chơi đã chọn ( đúng, sai, không trả lời)
    void xulicaccauDSKTL(int TongCauHoi,string mode)
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
    // tính tổng số câu đúng, sai, không trả lời của bài kiểm tra
    public IEnumerator IE_HoanThanhQuiz()
    {
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
    // Hàm để các script khác lấy thông tin tổng số các dạng trong bài kiểm tra
    public int TongSoCauHoi()
    {
        return TongCauHoi;
    }
    // Hàm để các script khác lấy thông tin tổng số câu sai trong bài kiểm tra
    public int LayTTTongSoCauSai()
    {
        return TongSoCauSai;
    }
    // Hàm để các script khác lấy thông tin các kết quả người chơi chọn
    public string LayTTCacKQNguoiChoiChon(int stt)
    {
        return CacKQNguoiChoiChon[stt];
    }
    // Hàm để các script khác lấy thông tin tổng số câu đúng
    public int LayTTTongSoCauDung()
    {
        return TongSoCauDung;
    }
     // Hàm để các script khác lấy thông tin tổng số câu không trả lời
    public int LayTTTongSoCauKTL()
    {
        return TongSoCauKTL;
    }
    // Hàm để các script khác lấy thông tin các câu sai ở các dạng
    public int xemcaccausai(int stt)
    {
        return CacCauSaiOCacDang[stt];
    }
    // Hàm để các script khác lấy thông tin các câu đúng ở các dạng
    public int xemcaccaudung(int stt)
    {
        return CacCauDungOCacDang[stt];
    }
    // Hàm để các script khác lấy thông tin các câu không trả lời ở các dạng
    public int xemcaccauktl(int stt)
    {
        return CacCauKTLOCacDang[stt];
    }
    // Hàm để các script khác lấy thông tin tổng số các dạng trong bài kiểm tra
    public int TongCacDang()
    {
        return CacDangCauHoi.Length - 1;
    }
    // Hàm để các script khác lấy thông tin các dạng câu hỏi
    public string TenDangCauHoi(int stt)
    {
        return CacDangCauHoi[stt];
    }
    // Hàm để các script khác lấy thông tin dạng câu hỏi
    public string LayTTDangCuaCH(int stt)
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
        // Hàm ẩn hiện bảng quản lí câu hỏi
    public void AnHienBangCauHoi()
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
     // đưa đến câu hỏi được người dùng nhấn trên bảng quản lí câu hỏi
    public void DenCauHoiChiDinh()      
    {
        string cauhoichidinh = EventSystem.current.currentSelectedGameObject.name;
        TTcauhoi = int.Parse(cauhoichidinh);
        linkQuizData = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseTable[TTcauhoi-1, 3];
        DapAn = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseTable[TTcauhoi-1, 2];
        StartCoroutine(GetImageFromServer(linkQuizData));
        if (CacKQNguoiChoiChon[TTcauhoi] != null)
        {
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
    //Xử lí đáp án vừa được người dùng chọn
    public void CheckDapAn()
    {
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
