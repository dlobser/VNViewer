
using System.IO;
using UnityEngine;

public class ListSubFolders : MonoBehaviour
{
    public string directoryPath; // The directory path to search for subdirectories

    void Start()
    {
        string[] subdirectoryNames = Directory.GetDirectories(directoryPath);
        foreach (string name in subdirectoryNames)
        {
            Debug.Log(name);
        }
    }
}