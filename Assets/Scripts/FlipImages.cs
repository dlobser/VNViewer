using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class FlipImages : MonoBehaviour
{
    List<UnityEngine.XR.InputDevice> devices;
    bool prev;
    // private Texture2D[] textures;
    // private Texture2D[] texturesSafe;
    string[] paths;
    string[] pathssfw;
    public string location = "Images";
    // public string locationSafe = "ImagesSFW";
    public Material mat;
    public Material mat2;
    // public Material mat3;

    public TextMesh text;

    List<int> indeces;

    Texture2D LoadImage(int which, bool safe = true)
    {
        string pathTemp = safe ? paths[which] : pathssfw[which];
        byte[] byteArray = System.IO.File.ReadAllBytes(pathTemp);
        Texture2D texTmp = new Texture2D(2048, 1024, TextureFormat.DXT1, false);
        texTmp.LoadImage(byteArray);
        texTmp.Apply();
        return texTmp;
    }

    void Start()
    {
        indeces = new List<int>();

        string path = location;
        string pathPreFix = Application.dataPath;
        paths = System.IO.Directory.GetFiles(pathPreFix + "/" + path, "*.png");

        // path = locationSafe;
        // pathssfw = System.IO.Directory.GetFiles(pathPreFix + "/" + path, "*.png");

        if (paths == null || paths.Length == 0)
            text.text = "Place png images here: \n" + pathPreFix + "/" + path;

        devices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, devices);

    }
    void Update()
    {
        bool triggerValue = false;
        if (Input.GetKeyDown(KeyCode.X))
        {
            Application.Quit();
        }
        if (devices.Count < 1)
        {
            UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, devices);
        }
        else
        {
            if (devices[0].TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && triggerValue)
            {
                if (triggerValue != prev)
                    Flip();
            }
        }
        prev = triggerValue;
    }
    public void Flip()
    {
        if (indeces.Count == 0)
        {
            for (int i = 0; i < paths.Length; i++)
            {
                indeces.Add(i);
            }
        }
        int j = Random.Range(0, indeces.Count);
        int index = indeces[j];
        indeces.RemoveAt(j);
        // print(paths[index]);
        // int indexSafe = Random.Range(0, pathssfw.Length);
        mat.SetTexture("_MainTex", LoadImage(index));
        mat2.SetTexture("_MainTex", LoadImage(index));
        // mat3.SetTexture("_MainTex", LoadImage(indexSafe, false));
        FindObjectOfType<CorrectCameraParentAim>().ResetView();
    }
}
