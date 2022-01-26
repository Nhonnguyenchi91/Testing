
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class BuildWebLoader : MonoBehaviour
{
    public string bundleUrl;
    public string assetName = "Circle";
    void Start()
    {
        StartCoroutine(GetAssetBundle());
    }

    IEnumerator GetAssetBundle()
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            var bundle = DownloadHandlerAssetBundle.GetContent(www);
            Debug.Log(bundle);
            var video = bundle.LoadAsset<GameObject>("Violin1");
            Debug.Log(video);
            Instantiate(video);
        }
    }
}

