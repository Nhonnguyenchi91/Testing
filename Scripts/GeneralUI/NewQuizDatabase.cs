using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewQuizDatabase : MonoBehaviour
{
    public GameObject QuizDatabaseLoader;
    public int QuizDatabase1CauHoiTableLenghtCell;
    public int SoRamdomCauHoi;
    public string[,] QuizDatabase1CauHoiTable = new string[100, 4];
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void DongBoHoaData1CauHoi()
    {
        QuizDatabase1CauHoiTableLenghtCell = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().Rows;
        for (int i = 0; i <= QuizDatabase1CauHoiTableLenghtCell; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                QuizDatabase1CauHoiTable[i, j] = QuizDatabaseLoader.GetComponent<NewDatabaseLoader>().DatabaseTable[i, j];
            }
        }
        Debug.Log(QuizDatabase1CauHoiTable[2, 2]);
    }
    public void Randomcauhoi()
    {
        SoRamdomCauHoi = Random.Range(1, QuizDatabase1CauHoiTableLenghtCell);

    }
    public void SapxeplaiTable()
    {
        for (int i = SoRamdomCauHoi; i <= QuizDatabase1CauHoiTableLenghtCell - 1; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                QuizDatabase1CauHoiTable[i, j] = QuizDatabase1CauHoiTable[i + 1, j];
            }
        }
        QuizDatabase1CauHoiTableLenghtCell--;
        Debug.Log(QuizDatabase1CauHoiTableLenghtCell);
    }
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
