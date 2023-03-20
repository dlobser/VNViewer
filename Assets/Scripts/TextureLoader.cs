using System.IO;
using UnityEngine;

public class TextureLoader : MonoBehaviour
{
    public string texturePath; // The path to the texture file
    public Material[] materials; // The material to apply the texture to
    public string channel = "_MainTex";
    public Vector2 res;
    public GameObject stereo;
    public GameObject mono;

    public void LoadImage(string path)
    {
        print("Load: " + path);
        byte[] byteArray = System.IO.File.ReadAllBytes(path);
        Texture2D texTmp = new Texture2D((int)res.x, (int)res.y, TextureFormat.DXT1, false);
        texTmp.LoadImage(byteArray);
        texTmp.Apply();
        foreach (Material m in materials)
        {
            m.SetTexture(channel, texTmp);
        }
        if (texTmp.width > texTmp.height)
        {
            stereo.SetActive(true);
            mono.SetActive(false);
        }
        else
        {
            stereo.SetActive(false);
            mono.SetActive(true);
        }


    }
}