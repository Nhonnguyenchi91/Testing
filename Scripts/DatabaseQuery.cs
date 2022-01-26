using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DatabaseQuery : MonoBehaviour
{
    [SerializeField] string PHPMyAdminUsername;
    [SerializeField] string InputPasswordDB;
    [SerializeField] string InputDBname;
    [SerializeField] string DBTablename;
    [SerializeField] string FieldToQuery;
    [SerializeField] string FieldToSearch;
    public bool ActivateAtStart = false;
    public string DataToQuery;
    public string QueryResult;
    public bool TestMode;
    [SerializeField] string LinkPHP;
    // Start is called before the first frame update
    void Start()
    {
        if(ActivateAtStart)
        {
            DatabaseQueryFunction();
        }    
    }
    public void DatabaseQueryFunction()
    {
        StartCoroutine(GetRequest(LinkPHP, PHPMyAdminUsername, InputPasswordDB, InputDBname, DBTablename, FieldToQuery, FieldToSearch, DataToQuery));
    }

        IEnumerator GetRequest(string LinkPHP, string PHPMyAdminUsername, string passwordDB, string DBname,string DBTablename, string FieldToQuery, string FieldToSearch, string DataToQuery)
        {
            WWWForm Form = new WWWForm();
            Form.AddField("PHPMyAdminUsername", PHPMyAdminUsername);
            Form.AddField("DBname", DBname);
            Form.AddField("passwordDB", passwordDB);
            Form.AddField("DBTablename", DBTablename);
            Form.AddField("FieldToQuery", FieldToQuery);
            Form.AddField("FieldToSearch", FieldToSearch);
            Form.AddField("DataToQuery", DataToQuery);
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
                QueryResult = webRequest.downloadHandler.text;
                Debug.Log(QueryResult);
                if(!TestMode)
                {
                    //GameObject.Find("GameManager").GetComponent<GameManagement>().StatusAction = "QueryDataSuccess";
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
