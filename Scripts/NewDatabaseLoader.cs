using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

// Access a website and use UnityWebRequest.Get to download a page.
public class NewDatabaseLoader : MonoBehaviour
{
    public string[] datas;//khai bao mang ki tu datas
    public string LinkPHP;
    public bool ActivateAtStart = false;
    public bool TestMode = false;
    public string InputPasswordDB;
    public string PHPMyAdminUsername;
    public string InputDBname;
    public string DBTableName;
    public string[] datafields;
    public string DatabaseLoaderStatus;
    public int Columns;
    public int Rows;
    public string[,] DatabaseTable = new string[200, 20];//khai bao mang 2 chieu 
    private int vitri = 0;
    void Start()
    {
        if (ActivateAtStart)
        {
            DatabaseLoaderFunction();
        }
    }
    public void testing()
    {
        Debug.Log(DatabaseTable[10,3]);
    }
    public string GetLoadDatabaseStatus()
    {
        return DatabaseLoaderStatus;
    }
    public string GetDataOfDatabaseTable(int row, int column)
    {
        return DatabaseTable[row, column];
    }
    public void DatabaseLoaderFunction()
    {
        StartCoroutine(GetRequest(LinkPHP, PHPMyAdminUsername, InputPasswordDB, InputDBname, DBTableName, Columns));
        // Thực hiện hàm request đến server, load data và xử lí chuỗi dữ liệu load về để lưu vào bảng charactertable
    }
    IEnumerator GetRequest(string uri, string Username, string passwordDB, string DBname, string TableName, int Colums)
    {
        WWWForm Form = new WWWForm();
        Form.AddField("Username", Username);
        Form.AddField("DBname", DBname);
        Form.AddField("passwordDB", passwordDB);
        Form.AddField("TableName", TableName);
        //Ket noi toi trang http de lay thong tin
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, Form)) // thủ tục gửi request từ Unity lên server
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError || webRequest.isNetworkError)
            {
                Debug.Log(webRequest.error);
                if (!TestMode)
                {
                    SceneManager.LoadScene("ErrorReport");
                }
            }
            else
            {
                // Khai bao chuoi characterdatastring la cac thong tin lay duoc tu trang http
                string datainput = webRequest.downloadHandler.text;
                //in ra man hinh chuoi do
                // truyen vao mang datas tung gia tri la chuoi gia tri cua tung thong tin
                // su dung ham split de tach tung thong tin
                datas = datainput.Split('|'); // tách chuỗi thành các chuỗi kí tự phân cách nhau bởi dấu ; và lưu các chuỗi vào từng phần tử của mảng data
            }
        }
        Rows = (datas.Length-1) / Columns;
        for (int i = 0;i< Rows; i++)
        {
            for( int j = 0; j < Columns; j++)
            {
                DatabaseTable[i, j] = datas[vitri];
                vitri++;
            }
        }
        DatabaseLoaderStatus = "FinishLoadData";
    }
}
