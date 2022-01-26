using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LopHocControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }
    public void Select_Student()
    {
        string Student = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(Student);
        Button ChanDung = GameObject.Find("ChanDung").GetComponent<Button>();
        var ChanDung_sprite = Resources.Load<Sprite>("ChanDung/" + Student.ToString());
        ChanDung.image.sprite = ChanDung_sprite; 
        Debug.Log("Thành Công"); // Nên remove
    }
    public void CheckDiem()
    {
        string CheckDiem = EventSystem.current.currentSelectedGameObject.name;
    }
    public void ThoiKhoaBieu()
    {

    }
    public void KhoiDau_ThoiKhoaBieu()
    {
        Button Mon1 = GameObject.Find("Mon1").GetComponent<Button>();
        string TenMon1 = Mon1.GetComponent<Image>().sprite.name;
        Debug.Log(TenMon1);
        Button Mon2 = GameObject.Find("Mon2").GetComponent<Button>();
        string TenMon2 = Mon2.GetComponent<Image>().sprite.name;
        Debug.Log(TenMon2);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
