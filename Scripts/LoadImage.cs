using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class LoadImage : MonoBehaviour
{
    [SerializeField] string LoadImageStatus;
    [SerializeField] GameObject ScollContent;
    public static LoadImage Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public string GetLoadImageStatus()
    {
        return LoadImageStatus;
    }
    public void LoadImageFunction(string Url, string ObjectImageTarget)
    {
        StartCoroutine(GetImageFromServer(Url, ObjectImageTarget));
    }    
    public IEnumerator GetImageFromServer(string Url,string ObjectImageTarget)
    {
        UnityWebRequest reg = UnityWebRequestTexture.GetTexture(Url);
        yield return reg.SendWebRequest();
        if (reg.isNetworkError || reg.isNetworkError)
        {
            Debug.Log(reg.error);
        }
        else
        {
            GameObject ObjectImage = GameObject.Find(ObjectImageTarget);
            string Content = ObjectImage.transform.parent.name;
            ScollContent = GameObject.Find(Content);
            Texture2D img = ((DownloadHandlerTexture)reg.downloadHandler).texture;
            float widht = ObjectImage.GetComponent<RectTransform>().rect.width;
            float height = ObjectImage.GetComponent<RectTransform>().rect.height;
            Debug.Log(widht);
            Debug.Log(height);
            float NewHeight = (float)img.height / (float)img.width * widht;
            ScollContent.GetComponent<RectTransform>().sizeDelta = new Vector2(widht, NewHeight);
            Debug.Log(NewHeight);
            ObjectImage.GetComponent<RectTransform>().sizeDelta = new Vector2(widht, NewHeight);
            ObjectImage.GetComponent<Image>().sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), Vector2.zero);
            LoadImageStatus = "finish";
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
