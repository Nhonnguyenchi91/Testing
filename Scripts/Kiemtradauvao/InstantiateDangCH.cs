using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateDangCH : MonoBehaviour
{
    public GameObject DangCauHoiPrefab;
    // Start is called before the first frame update
    void Start()
    {
        GameObject test = Instantiate(DangCauHoiPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        test.name = "abc";
        test.transform.parent = GameObject.Find("PhanTichKetQua").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
