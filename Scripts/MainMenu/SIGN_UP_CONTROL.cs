using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;


public class SIGN_UP_CONTROL : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject SignUpPanel;
    [SerializeField] GameObject Message;
    [SerializeField] Animator MessageAnim;
    private string SignupUsername; // User Name người chơi đặt
    private string SignupPassword; // password người chơi đặt
    private string SignupPasswordRepeat;
    private string CharacterName;
    string check_signup_result;
    public bool TestMode;
    [SerializeField] string passwordDB;
    [SerializeField] string DBname;
    [SerializeField] string link;
    // Start is called before the first frame update
    void Start()
    {
        Message.SetActive(false);
    }
    public void username_input(string username)
    {
        SignupUsername = username;     
    }
    public void password_input(string password)
    {
        SignupPassword = password;
    }
    public void password_repeat_input(string password_repeat)
    {
        SignupPasswordRepeat = password_repeat;
    }
    public void character_name_input(string charactername)
    {
        CharacterName = charactername;
    }
    public void signup_check()
    {
        Message.SetActive(false);
        if (SignupPassword != SignupPasswordRepeat)
        {
            Debug.Log("password repeat not match");
            Message.SetActive(true);
            Message.GetComponent<TextMeshProUGUI>().text = "mật khẩu xác nhận không trùng khớp";
            MessageAnim.Play("MessageAnim");
            GameObject.Find("setpassword").GetComponent<InputField>().text = null;
            GameObject.Find("repeatpassword").GetComponent<InputField>().text = null;
        }
        else
        {
            StartCoroutine(Check_username(SignupUsername, SignupPassword, CharacterName, passwordDB, DBname));
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Check_username(string usernamesignup, string passwordsignup, string charactername, string passwordDB, string DBname)
    {
        WWWForm Form = new WWWForm();
        Form.AddField("DBname", DBname);
        Form.AddField("passwordDB", passwordDB);
        Form.AddField("usernamesignup", usernamesignup);
        Form.AddField("passwordsignup", passwordsignup);
        Form.AddField("charactername", charactername);
        using (UnityWebRequest CheckID = UnityWebRequest.Post(link, Form))
        {
            yield return CheckID.SendWebRequest();
            if (CheckID.isNetworkError || CheckID.isHttpError)
            {
                Debug.Log(CheckID.error);
            }
            else
            {
                Debug.Log("upload complete");
                Debug.Log(CheckID.downloadHandler.text);
                check_signup_result = CheckID.downloadHandler.text;
                if(check_signup_result == "Registration successfully")
                {
                    MainMenu.SetActive(true);
                    SignUpPanel.SetActive(false);
                    Message.SetActive(true);
                    MessageAnim.Play("MessageAnim");
                    if(! TestMode)
                    {
                        //GameObject.Find("GameManager").GetComponent<GameManagement>().FirstTimeinGame = true;
                    }        
                }
                else if (check_signup_result == "username exsit")
                {
                    Message.SetActive(true);
                    Message.GetComponent<TextMeshProUGUI>().text = "tên đăng nhập đã tồn tại";
                    MessageAnim.Play("MessageAnim");
                }
                else if (check_signup_result == "character_name exsit")
                {
                    Message.SetActive(true);
                    Message.GetComponent<TextMeshProUGUI>().text = "tên nhân vật đã tồn tại";
                    MessageAnim.Play("MessageAnim");
                }
            }
        }
    }
}
