﻿using System.Collections;
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
    [SerializeField] GameObject BangCauHoi; // Panel bảng câu hỏi
    public bool ModeCauHoiNhieuLan = true; // mode xác nhận bài kiểm tra là một câu hỏi đơn lẽ hay nhiều câu hỏi
    [SerializeField] string AnswerResult; // Kết quả của câu trả lời người chơi vừa chọn
    public string Quizmode; // Các chế dộ của bài Quiz
    private int TTcauhoi; // Số thứ tự các câu hỏi
    private int Socaudung; // Số câu đúng
    private string linkQuizData; // link server để load bài Quiz
    private string DapAn; // Đáp án
    public int TTcauhoiCotTruyen; // thứ tự của câu hỏi cốt truyện
    public string[] CacKQNguoiChoiChon; // Các kết quả người chơi chọn
    [SerializeField] string[] DangCauHoi; // Lưu trữ
    [SerializeField] string[] CacDangCauHoi; // Lưu trữ các dạng câu hỏi
    private float timermonitor; // giám sát timer
    private bool checktimer = true; // kiểm tra trạng thái của timer
    private bool FirstStart = false; // kiểm tra hiện người chơi có phải lần đầu vào Quiz không
    public bool QuizTutorial = false; // kiểm tra người chơi hiện có đang ỏ mode QuizTutorial không
    public string QuizStatus; // trạng thái của Quiz
    [SerializeField] int TongCauHoi; // Tổng số câu hỏi
    [SerializeField] int TongSoCauDung; // Tổng Số Câu Đúng;
    [SerializeField] int TongSoCauSai;  // Tổng Số Câu Sai;
    [SerializeField] int TongSoCauKTL; // Tổng Số Câu Không Trả Lời;
    [SerializeField] int[] CacCauSaiOCacDang; //Các Câu Sai Ở Các Dạng;
    [SerializeField] int[] CacCauDungOCacDang; //Các Câu Đúng Ở Các Dạng;
    [SerializeField] int[] CacCauKTLOCacDang; //Các Câu KTL Ở Các Dạng;
    [SerializeField] string check_choice; // kiểm tra lựa chọn
    [SerializeField] string LinkLoiGiai; // link server để load lời giải
    [SerializeField] string LinkCauHoi; // link server để load câu hỏi
    // Start is called before the first frame update
    void Start()
    {
        Socaudung = 0; // Lúc bắt đầu điều chỉnh số câu đúng về 0
        TTcauhoi = 1; // Lúc bắt đầu điều chỉnh thứ tự câu hỏi về 1
        TongCauHoi = 0; // Lúc bắt đầu reset biến tổng câu hỏi về 0
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
        //Tạo vòng lặp để quét chi tiết tất cả các kết quả người chơi chọn
        for (int j = 1; j <= TongCauHoi; j++)
            {
            // xét các trường hợp cần quét
                switch(mode)
                {
                   // nếu trường mode cần xử lí là các kết quả để trống, không trả lời của người chơi
                    case "KTL":
                        {
                        // Nếu các kết quả của người chơi chọn là null có nghĩa đây là câu mà người chơi không trả lời
                            if (CacKQNguoiChoiChon[j] == null)
                            {
                            // ta lưu câu mà người chơi không trả lời này vào mảng Các Câu không trả lời ở các dạng
                                CacCauKTLOCacDang[i] = j;
                             // tăng i lên 1 đơn vị để lần tiếp theo lưu vào phần tử kế tiếp của mảng Các Câu không trả lời ở các dạng
                            i++;
                            }
                            break;
                        }
                         // nếu trường mode cần xử lí là các kết quả "Đúng"
                    case "Dung":
                        {
                        // Nếu các kết quả của người chơi chọn là "T" có nghĩa đây là câu mà người chơi đã trả lời đúng
                            if (CacKQNguoiChoiChon[j] == "T")
                            {
                            // ta lưu câu mà người chơi trả lời đúng này vào mảng Các Câu trả lời đúng ở các dạng
                                CacCauDungOCacDang[i] = j;
                            // tăng i lên 1 đơn vị để lần tiếp theo lưu vào phần tử kế tiếp của mảng Các Câu trả lời đúng ở các dạng
                            i++;
                        }
                            break;
                        }
                        // nếu trường mode cần xử lí là các kết quả "Sai"
                    case "Sai":
                        {
                         // Nếu các kết quả của người chơi chọn là "F" có nghĩa đây là câu mà người chơi đã trả lời sai
                            if (CacKQNguoiChoiChon[j] == "F")
                            {
                            // ta lưu câu mà người chơi trả lời đúng này vào mảng Các Câu trả lời sai ở các dạng
                                CacCauSaiOCacDang[i] = j;
                            // tăng i lên 1 đơn vị để lần tiếp theo lưu vào phần tử kế tiếp của mảng Các Câu trả lời sai ở các dạng
                            i++;
                        }
                            break;
                        }

                }    
            }
    }
    // IE_HoanThanhQuiz() là hàm tính tổng số câu đúng, sai, không trả lời của bài kiểm tra sau khi người chơi hoàn thành Quiz
    public IEnumerator IE_HoanThanhQuiz()
    {
        // Tạo vòng lặp để quét tất cả các phần tử của mảng Các kết quả người chơi chọn
        for (int i = 1; i <= TongCauHoi; i++)
        {
            //Nếu phần tử của mảng có giá trị null, nghĩa là câu này người chơi không trả lời
            if (CacKQNguoiChoiChon[i] == null)
            {
                //sau khi phát hiện ra câu mà người chơi không trả lời thì ta tăng biến đếm TongSoCauKTL lên 1 đơn vị
                TongSoCauKTL++;
            }
            //Nếu phần tử của mảng có giá trị "T", nghĩa là câu này người chơi trả lời đúng
            else if (CacKQNguoiChoiChon[i] == "T")
            {
                //sau khi phát hiện ra câu mà người chơi trả lời đúng thì ta tăng biến đếm TongSoCauDung lên 1 đơn vị
                TongSoCauDung++;
            }
            //Nếu phần tử của mảng có giá trị "F", nghĩa là câu này người chơi trả lời sai
            else if (CacKQNguoiChoiChon[i] == "F")
            {
                 //sau khi phát hiện ra câu mà người chơi trả lời sai thì ta tăng biến đếm TongSoCauSai lên 1 đơn vị
                TongSoCauSai++;
            }
        }
        CacCauSaiOCacDang = new int[TongSoCauSai + 1]; //Tạo mảng CacCauSaiOCacDang ( +1 để vị trí phần tử lớn nhất = TongSoCauSai)
        CacCauDungOCacDang = new int[TongSoCauDung + 1]; //Tạo mảng CacCauDungOCacDang ( +1 để vị trí phần tử lớn nhất = TongSoCauDung)
        CacCauKTLOCacDang = new int[TongSoCauKTL + 1]; //CacCauKTLOCacDang ( +1 để vị trí phần tử lớn nhất = TongSoCauKTL)
        // xác định các câu đúng, sai, không trả lời của bài kiểm tra
        xulicaccauDSKTL(TongCauHoi, "KTL"); // chạy hàm xulicaccauDSKTL (hàm xử lí các câu đúng, sai, không trả lời) cho trường hợp các câu không trả lời
        xulicaccauDSKTL(TongCauHoi, "Dung"); // chạy hàm xulicaccauDSKTL cho trường hợp các câu trả lời đúng
        xulicaccauDSKTL(TongCauHoi, "Sai"); // chạy hàm xulicaccauDSKTL cho trường hợp các câu trả lời sai
        QuizStatus = "FinishQuiz"; // Sau khi xử lí xong IE_HoanThanhQuiz() thì ta cập nhật QuizStatus = "FinishQuiz"
        yield return new WaitForSeconds(0.1f); // delay 0.1 giây
    }
    // Hàm Hoàn Thành Quiz để chạy StartCoroutine cho IE_HoanThanhQuiz()
        public void HoanThanhQuiz()
    {
        StartCoroutine(IE_HoanThanhQuiz());
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
    // Hàm ẩn hiện bảng quản lí câu hỏi
    public void AnHienBangCauHoi()
    {
        // Check xem vị trí của BangCauHoi có trùng với vị trí của QuízSystem không
        if (GameObject.Find("BangCauHoi").transform.position == GameObject.Find("QuízSystem").transform.position)
        {
        // Nếu trùng thì có nghĩa là BangCauHoi đang hiện, ta thực hiện ẩn bằng cách cho BangCauHoi di chuyển đến vị trí của GiauBangCauHoi
            GameObject.Find("BangCauHoi").transform.position = GameObject.Find("GiauBangCauHoi").transform.position;
        }
        // Check xem vị trí của BangCauHoi có trùng với vị trí của GiauBangCauHoi không
        else if(GameObject.Find("BangCauHoi").transform.position == GameObject.Find("GiauBangCauHoi").transform.position)
        {
        // Nếu trùng thì có nghĩa là BangCauHoi đang ẩn, ta cho hiện lên bằng cách cho BangCauHoi di chuyển đến vị trí của QuízSystem
            GameObject.Find("BangCauHoi").transform.position = GameObject.Find("QuízSystem").transform.position;
        }
    }
     // đưa đến câu hỏi được người dùng nhấn trên bảng quản lí câu hỏi
    public void DenCauHoiChiDinh()      
    {
        
        string cauhoichidinh = EventSystem.current.currentSelectedGameObject.name; // lưu tên của câu hỏi mà người chơi vừa chọn vào biến cauhoichidinh
        TTcauhoi = int.Parse(cauhoichidinh); //chuyển cauhoichidinh sang int và lưu giá trị vào biến TTcauhoi
        linkQuizData = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseTable[TTcauhoi-1, 3]; // load link QuizData từ bảng dữ liệu trong QuizDatabaseLoader
        DapAn = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseTable[TTcauhoi-1, 2]; // load đáp án từ bảng dữ liệu trong QuizDatabaseLoader
        StartCoroutine(GetImageFromServer(linkQuizData)); // thực hiện hàm load hình ảnh từ server
        // check xem các kết quả người chơi chọn có bị null không
        if (CacKQNguoiChoiChon[TTcauhoi] != null)
        {
        nếu không null, có nghĩa là câu hỏi nãy đã được trả lời, do đó ta ẩn các lựa chọn đi
            A.SetActive(false);
            B.SetActive(false);
            C.SetActive(false);
            D.SetActive(false);
        }
        nếu null, có nghĩa là câu hỏi nãy chưa được trả lời, ta hiện các lựa chọn lên
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
