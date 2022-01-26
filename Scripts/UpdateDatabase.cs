using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class UpdateDatabase : MonoBehaviour
{
    public string PHPMyAdminUsername;
    public string InputPasswordDB;
    public string InputDBname;
    public string DBTablename;
    public string FieldToUpdate;
    public string FieldToSearch;
    public bool ActivateAtStart = false;
    public bool TestMode;
    public string DataToUpdate;
    public string DataToSearch;
    public string UpdateResult;
    public string LinkPHP;
    // Start is called before the first frame update
    void Start()
    {
        if (ActivateAtStart)
        {
            DatabaseUpdateFunction();
        }
    }
    public void DatabaseUpdateFunction()
    {
        StartCoroutine(GetRequest(LinkPHP, PHPMyAdminUsername, InputPasswordDB, InputDBname, DBTablename, FieldToUpdate, FieldToSearch, DataToUpdate, DataToSearch));
    }

    IEnumerator GetRequest(string LinkPHP, string PHPMyAdminUsername, string passwordDB, string DBname, string DBTablename, string FieldToUpdate, string FieldToSearch, string DataToUpdate, string DataToSearch)
    {
        WWWForm Form = new WWWForm();
        Form.AddField("PHPMyAdminUsername", PHPMyAdminUsername);
        Form.AddField("DBname", DBname);
        Form.AddField("passwordDB", passwordDB);
        Form.AddField("DBTablename", DBTablename);
        Form.AddField("FieldToUpdate", FieldToUpdate);
        Form.AddField("FieldToSearch", FieldToSearch);
        Form.AddField("DataToUpdate", DataToUpdate);
        Form.AddField("DataToSearch", DataToSearch);
        //Ket noi toi trang http de lay thong tin
        using (UnityWebRequest webRequest = UnityWebRequest.Post(LinkPHP, Form)) // thủ tục gửi request từ Unity lên server
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError || webRequest.isNetworkError)
            {
                Debug.Log(webRequest.error);
                SceneManager.LoadScene("ErrorReport");
            }
            else
            {
                UpdateResult = webRequest.downloadHandler.text;
                Debug.Log(UpdateResult);
                if(!TestMode)
                {
                    //GameObject.Find("GameManager").GetComponent<GameManagement>().StatusAction = UpdateResult;
                }    
            }
            // Ket qua cac thong tin lay duoc tu trang http

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
