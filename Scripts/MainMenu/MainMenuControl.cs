using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class MainMenuControl : MonoBehaviour
{
   [SerializeField] GameObject MainMenu;
   [SerializeField] GameObject SignUpPanel;
   [SerializeField] GameObject SignInPanel;
   [SerializeField] GameObject RegisSuccess;
   [SerializeField] GameObject ChangeViewEffect;
   [SerializeField] GameObject ChoiceGender;
   [SerializeField] GameObject MaDangKyPanel;
   [SerializeField] Animator MessageAnim;
   [SerializeField] GameObject Message;
    private string MaDangKyString;
    private int GioiHanSoLanNhapMa;
    public bool FirstGame;

    // Start is called before the first frame update
    void Start()
    {
        MainMenu.SetActive(true);
        SignUpPanel.SetActive(false);
        SignInPanel.SetActive(false);
        MainMenu.transform.position = new Vector3(0, 0, 0);
        ChangeViewEffect.SetActive(false);
        GioiHanSoLanNhapMa = 0;
        MaDangKyPanel.SetActive(false);
    }
    public void XuLyMaDangKy()
    {
        bool NhapMaThanhCong = false;
        bool MaDaDK = false;
        if (GioiHanSoLanNhapMa > 3)
        {
            GameObject.Find("ThongBaoMaDangKy").GetComponent<TextMeshProUGUI>().fontSize = 30;
            GameObject.Find("ThongBaoMaDangKy").GetComponent<TextMeshProUGUI>().text = "Đã quá giới hạn nhập mã, trò chơi sẽ bị thoát!";
            Application.Quit();
        }
        else
        {
            for (int i = 0; i < 100; i++)
            {
                if (MaDangKyString == GameObject.Find("MaDangKy").GetComponent<NewDatabaseLoader>().DatabaseTable[i, 1])
                {
                    if (GameObject.Find("MaDangKy").GetComponent<NewDatabaseLoader>().DatabaseTable[i, 2] == "ChuaDK")
                    {
                        GameObject.Find("MaDangKy").GetComponent<NewDatabaseLoader>().DatabaseTable[i, 2] = "DaDK";
                        Debug.Log(GameObject.Find("MaDangKy").GetComponent<NewDatabaseLoader>().DatabaseTable[i, 2]);
                        MaDangKyPanel.SetActive(false);
                        SignUpPanelActive();
                        NhapMaThanhCong = true;
                        GameObject.Find("MaDangKy").GetComponent<UpdateDatabase>().DataToSearch = MaDangKyString;
                        GameObject.Find("MaDangKy").GetComponent<UpdateDatabase>().DataToUpdate = "DaDK";
                        GameObject.Find("MaDangKy").GetComponent<UpdateDatabase>().DatabaseUpdateFunction();
                        break;
                    }
                    else
                    {
                        MaDaDK = true;
                        break;
                    }
                }
            }
            if (!NhapMaThanhCong)
            {
                if (MaDaDK)
                {
                    GameObject.Find("ThongBaoMaDangKy").GetComponent<TextMeshProUGUI>().fontSize = 30;
                    GameObject.Find("ThongBaoMaDangKy").GetComponent<TextMeshProUGUI>().text = "Mã đã có người đăng ký!";
                    GioiHanSoLanNhapMa++;
                }
                else
                {
                    GameObject.Find("ThongBaoMaDangKy").GetComponent<TextMeshProUGUI>().fontSize = 30;
                    GameObject.Find("ThongBaoMaDangKy").GetComponent<TextMeshProUGUI>().text = "Mã đăng ký không chính xác!";
                    GioiHanSoLanNhapMa++;
                }

            }
        }
            
    }
    public void InputMaDangKy(string InputMaDangKy)
    {
        MaDangKyString = InputMaDangKy;
        Debug.Log(MaDangKyString);
    }    
    public void SignUpProcessing()
    {
        MaDangKyPanel.SetActive(true);
        GameObject.Find("ThongBaoMaDangKy").GetComponent<TextMeshProUGUI>().text = "Hãy Nhập Mã Đăng Ký";
    }    
    public void SignUpPanelActive()
    {
        MainMenu.SetActive(false);
        SignUpPanel.SetActive(true);
        SignInPanel.SetActive(false);
        SignUpPanel.transform.position = new Vector3(0, 0, 0);
    }
    public void SignInPanelActive()
    {
        MainMenu.SetActive(false);
        SignUpPanel.SetActive(false);
        SignInPanel.SetActive(true);
        SignInPanel.transform.position = new Vector3(0, 0, 0);
    }
    public void BackToMainMenu()
    {
        MainMenu.SetActive(true);
        SignUpPanel.SetActive(false);
        SignInPanel.SetActive(false);
    }
    public void ChoiceGenderResult()
    {
        string CheckChoiceGender = EventSystem.current.currentSelectedGameObject.name;
        if(CheckChoiceGender=="Male")
        {
            SceneManager.LoadScene("Tutorials");
            Debug.Log(CheckChoiceGender);
        }
        else
        {
            Debug.Log(CheckChoiceGender);
        }
    }
    public void ChoiceGendePanel()
    {
        MainMenu.SetActive(false);
        SignUpPanel.SetActive(false);
        SignInPanel.SetActive(false);
        ChoiceGender.SetActive(true);
        ChoiceGender.transform.position = new Vector3(0, 0, 0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {
        if(SignInPanel.activeSelf)
        {
            if (GameObject.Find("SignInControl").GetComponent<SIGN_IN_CONTROL>().LoginSuccessful)
            {
                GameObject.Find("SignInControl").GetComponent<SIGN_IN_CONTROL>().LoginSuccessful = false;
                if (FirstGame)
                {
                    SceneManager.LoadScene("KiemTraDauVao");
                }
            }
        }    
        
    }
}
