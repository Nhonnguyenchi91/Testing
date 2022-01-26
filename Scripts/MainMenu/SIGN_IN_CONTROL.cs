using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SIGN_IN_CONTROL : MonoBehaviour
{
    private string signin_username;
    private string signin_password;
    public string SignInResult;
    public MainMenuControl MainmenuCS;
    public bool TestMode;
    public bool LoginSuccessful;
    [SerializeField] string passwordDB;
    [SerializeField] string DBname;
    [SerializeField] string link;
    public void username_input(string username)
    {
        signin_username = username;
        if(!TestMode)
        {
            //GameObject.Find("GameManager").GetComponent<GameManagement>().username = signin_username;
        }
    }
    public void password_input(string password)
    {
        signin_password = password;
    }
    public void signin_control()
    {
        StartCoroutine(Sign_in_control(signin_username, signin_password, passwordDB, DBname));
    }
    void Start()
    {
        LoginSuccessful = false;
    }
    IEnumerator Sign_in_control(string usernamesignin, string passwordsignin, string passwordDB, string DBname)
    {
        WWWForm Form = new WWWForm();
        Form.AddField("DBname", DBname);
        Form.AddField("passwordDB", passwordDB);
        Form.AddField("usernamesignin", usernamesignin);
        Form.AddField("passwordsignin", passwordsignin);
        using (UnityWebRequest Sign_in = UnityWebRequest.Post(link, Form))
        {
            yield return Sign_in.SendWebRequest();
            if (Sign_in.isNetworkError || Sign_in.isHttpError)
            {
                Debug.Log(Sign_in.error);
            }
            else
            {
                Debug.Log("upload complete");
                SignInResult = Sign_in.downloadHandler.text;
                Debug.Log(SignInResult);
                string checklogin = "LoginSuccessful";
                if (SignInResult=="LoginSuccessfulLoginSuccessfulLoginSuccessfulLoginSuccessful")
                {
                    Debug.Log("LoginSuccessful");
                    LoginSuccessful = true;
                    if (!TestMode)
                    {
                        //GameObject.Find("GameManager").GetComponent<GameManagement>().Status = "SignInSuccess";
                    }
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}





























































































































































































































































































































































































































































































































































