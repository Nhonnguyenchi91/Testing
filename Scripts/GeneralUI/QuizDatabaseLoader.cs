using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class QuizDatabaseLoader : MonoBehaviour
{
    public string[] datas;//khai bao mang ki tu datas
    public string[] InputField;
    public string LinkPHP;
    public bool ActivateAtStart = false;
    public bool TestMode = false;
    public string InputPasswordDB;
    public string InputDBname;
    public string TableName;
    public string[,] DatabaseTable = new string[100, 100];//khai bao mang 2 chieu 
    public int LengthCells;

    void Start()
    {
        if (ActivateAtStart)
        {
            DatabaseLoaderFunction();
        }
    }
    public void DatabaseLoaderFunction()
    {
        for (int a = 0; a <= (InputField.Length - 1); a++)
        {
            DatabaseTable[0, a] = InputField[a];
        }
        Debug.Log(DatabaseTable[0, 0]);
        // Đưa các thành phần cần truy vấn data vào hàng đầu tiên của mảng.
        StartCoroutine(GetRequest(LinkPHP, InputPasswordDB, InputDBname, TableName));
        // Thực hiện hàm request đến server, load data và xử lí chuỗi dữ liệu load về để lưu vào bảng charactertable
    }
    IEnumerator GetRequest(string uri, string passwordDB, string DBname , string TableName)
    {
        WWWForm Form = new WWWForm();
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
                string characterdatastring = webRequest.downloadHandler.text;
                //in ra man hinh chuoi do
                // truyen vao mang datas tung gia tri la chuoi gia tri cua tung thong tin
                // su dung ham split de tach tung thong tin
                datas = characterdatastring.Split(';'); // tách chuỗi thành các chuỗi kí tự phân cách nhau bởi dấu ; và lưu các chuỗi vào từng phần tử của mảng data
                LengthCells = datas.Length - 1;
                for (int i = 0; i < (datas.Length - 1); i++) // quét các hàng của database được lưu trong datas
                {
                    for (int j = 0; j < InputField.Length; j++) // quét các cột của database được lưu trong datas
                    {
                        DatabaseTable[i + 1, j] = GetDataValue(datas[i], (DatabaseTable[0, j] + ":"));
                        // tách dữ liệu từ các hàng của datas và lưu vào bên trong bảng charactertable tương ứng.
                    }
                }
                if (!TestMode)
                {
                    //GameObject.Find("GameManager").GetComponent<GameManagement>().StatusAction = "LoadDataSuccess";
                }
            }

            Debug.Log(DatabaseTable[1, 1]);
        }
    }
    string GetDataValue(string data, string index) //Hàm lọc chuỗi kí tự cần lấy
    {
        string value = data.Substring(data.IndexOf(index) + index.Length); //loại bỏ tất cả kí tự trước thông tin cần lấy
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|")); //loại bỏ tất cả kí tự nằm phía sau thông tin cần lấy
        return value;
    }
}
