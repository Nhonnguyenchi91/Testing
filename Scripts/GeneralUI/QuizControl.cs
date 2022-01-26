using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class QuizControl : MonoBehaviour
{
    [SerializeField] bool QuizModeSingle;
    [SerializeField] Image Pic;
    public void check_choice()
    {
        string check_choice = EventSystem.current.currentSelectedGameObject.name;
    }
    public void LoadImage()
    {
        StartCoroutine(GetImageFromServer("fyd.ddns.net/BT295161529/C1.png"));
    }
    public IEnumerator GetImageFromServer(string Url)
    {
        UnityWebRequest reg = UnityWebRequestTexture.GetTexture(Url);
        yield return reg.SendWebRequest();
        if (reg.isNetworkError || reg.isNetworkError)
        {
            Debug.Log(reg.error);
        }
        else
        {
            Texture2D img = ((DownloadHandlerTexture)reg.downloadHandler).texture;
            Pic.sprite = Sprite.Create(img, new Rect(0, 0, 500, 500), Vector2.zero);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
