using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;


public class QuizManager : MonoBehaviour
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
    public Image Pic;
    public bool ModeCauHoiNhieuLan = true;
    public string AnswerResult;
    public string Quizmode;
    private int TTcauhoi;
    private int Socaudung;
    private string linkQuizData;
    private string DapAn;
    public int TTcauhoiCotTruyen;
    private string[] Datraloi = new string[100];
    private float timermonitor;
    private bool checktimer = true;
    private bool FirstStart = true;
    public bool QuizTutorial = false;
    public string QuizStatus;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=1; i<= 100; i++)
        {
            Datraloi[i-1] = null;
        }
        Socaudung = 0;
        TTcauhoi = 1;
        QuizQuestionPanel.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void CheckDapAn()
    {
        string check_choice = EventSystem.current.currentSelectedGameObject.name;
        if(check_choice== DapAn)
        {
            Debug.Log("correct");
            AnswerResult = "Dung";
            Socaudung++;
            Datraloi[TTcauhoi] = "T";
            A.SetActive(false);
            B.SetActive(false);
            C.SetActive(false);
            D.SetActive(false);
        }
        else
        {
            Debug.Log("incorrect");
            AnswerResult = "Sai";
            Datraloi[TTcauhoi] = "F";
            A.SetActive(false);
            B.SetActive(false);
            C.SetActive(false);
            D.SetActive(false);
        }
        if(Quizmode == "1cauhoi")
        {
            QuizQuestionPanel.SetActive(false);
        }
        else if (Quizmode == "cottruyen")
        {
            TTcauhoiCotTruyen++;
            QuizStatus = "FinishCHCotTruyen";
            QuizQuestionPanel.SetActive(false);
        }
    }
        public void LoadQuiz(bool ActivateFromButton)
    {
        string check_choice = EventSystem.current.currentSelectedGameObject.name;
        if(!ActivateFromButton)
        {
            check_choice = "Start";
        }
        switch(check_choice)
        {
            case "Start":
                { 
                    if(ModeCauHoiNhieuLan)
                    {
                        if (FirstStart)
                        {
                            QuizDatabase.GetComponent<QuizDatabase>().DongBoHoaData1CauHoi();
                            FirstStart = false;
                        }
                        if (Quizmode == "kiemtra15p")
                        {
                            Timer.GetComponent<Timer>().timevalue = 900;
                            checktimer = true;
                            Next.SetActive(true);
                            Back.SetActive(true);
                            QuizQuestionPanel.SetActive(true);
                            linkQuizData = QuizDatabaseLoader.GetComponent<DatabaseLoader>().DatabaseTable[1, 2];
                            DapAn = QuizDatabaseLoader.GetComponent<DatabaseLoader>().DatabaseTable[1, 1];
                            StartCoroutine(GetImageFromServer(linkQuizData));
                        }
                        else if (Quizmode == "kiemtra45p")
                        {
                            Timer.GetComponent<Timer>().timevalue = 2700;
                            checktimer = true;
                            Next.SetActive(true);
                            Back.SetActive(true);
                            QuizQuestionPanel.SetActive(true);
                            linkQuizData = QuizDatabaseLoader.GetComponent<DatabaseLoader>().DatabaseTable[1, 2];
                            DapAn = QuizDatabaseLoader.GetComponent<DatabaseLoader>().DatabaseTable[1, 1];
                            StartCoroutine(GetImageFromServer(linkQuizData));
                        }
                        else if (Quizmode == "cottruyen")
                        {
                            Timer.GetComponent<Timer>().timevalue = 30;
                            checktimer = true;
                            Next.SetActive(false);
                            Back.SetActive(false);
                            QuizQuestionPanel.SetActive(true);
                            linkQuizData = QuizDatabaseLoader.GetComponent<DatabaseLoader>().DatabaseTable[1, 2];
                            DapAn = QuizDatabaseLoader.GetComponent<DatabaseLoader>().DatabaseTable[1, 1];
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
                            QuizDatabase.GetComponent<QuizDatabase>().Randomcauhoi();
                            int cauhoi = QuizDatabase.GetComponent<QuizDatabase>().SoRamdomCauHoi;
                            linkQuizData = QuizDatabase.GetComponent<QuizDatabase>().QuizDatabase1CauHoiTable[cauhoi, 2];
                            DapAn = QuizDatabase.GetComponent<QuizDatabase>().QuizDatabase1CauHoiTable[cauhoi, 1];
                            StartCoroutine(GetImageFromServer(linkQuizData));
                            QuizDatabase.GetComponent<QuizDatabase>().SapxeplaiTable();
                        }
                    }
                    break;
                }
            case "Back":
                {
                    if(TTcauhoi>1)
                    {
                        TTcauhoi--;
                        linkQuizData = QuizDatabaseLoader.GetComponent<DatabaseLoader>().DatabaseTable[TTcauhoi, 2];
                        DapAn = QuizDatabaseLoader.GetComponent<DatabaseLoader>().DatabaseTable[TTcauhoi, 1];
                        StartCoroutine(GetImageFromServer(linkQuizData));
                        if(Datraloi[TTcauhoi]!=null)
                        {
                            Debug.Log("datraloi");
                            A.SetActive(false);
                            B.SetActive(false);
                            C.SetActive(false);
                            D.SetActive(false);
                        }
                        else if (Datraloi[TTcauhoi] == null)
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
                    TTcauhoi = TTcauhoi+1;
                    Debug.Log(TTcauhoi);    
                    linkQuizData = QuizDatabaseLoader.GetComponent<DatabaseLoader>().DatabaseTable[TTcauhoi, 2];
                    DapAn = QuizDatabaseLoader.GetComponent<DatabaseLoader>().DatabaseTable[TTcauhoi, 1];
                    if(linkQuizData==null)
                    {
                        TTcauhoi--;
                        linkQuizData = QuizDatabaseLoader.GetComponent<DatabaseLoader>().DatabaseTable[TTcauhoi, 2];
                        DapAn = QuizDatabaseLoader.GetComponent<DatabaseLoader>().DatabaseTable[TTcauhoi, 1];
                    }
                    else if(linkQuizData != null)
                    {
                        StartCoroutine(GetImageFromServer(linkQuizData));
                    }
                    if (Datraloi[TTcauhoi] != null)
                    {
                        Debug.Log("datraloi");
                        A.SetActive(false);
                        B.SetActive(false);
                        C.SetActive(false);
                        D.SetActive(false);
                    }
                    else if (Datraloi[TTcauhoi] == null)
                    {
                        A.SetActive(true);
                        B.SetActive(true);
                        C.SetActive(true);
                        D.SetActive(true);
                    }
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
        if(reg.isNetworkError||reg.isNetworkError)
        {
            Debug.Log(reg.error);
        }
        else
        {
            Texture2D img = ((DownloadHandlerTexture)reg.downloadHandler).texture;
            Pic.sprite = Sprite.Create(img, new Rect(0, 0, 1200, 400), Vector2.zero);
        }
    }
    public void RamdomCauHoi()
    {
        int a = Random.Range(1,10);
        Debug.Log(a);
    }
    // Update is called once per frame
    void Update()
    {
            timermonitor = Timer.GetComponent<Timer>().timevalue;
            if (timermonitor == 0 && checktimer)
            {
                Debug.Log("hetgio");
                checktimer = false;
            } 
            
    }
}
