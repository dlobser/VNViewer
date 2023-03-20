using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Flipper : MonoBehaviour
{
    public string subdirectoryPath; // The subdirectory path to search for files

    private List<string[]> fileNamesList; // The list of string arrays of file names
    public List<List<int>> fileNamesIndeces;
    private int selectedDirIndex = 0; // The index of the currently selected directory
    private int selectedFileIndex = 0; // The index of the currently selected file in the selected directory
    public TextureLoader loader;
    public bool randomizeIndeces;
    string[] subdirectoryNames;
    public TextMesh text;

    void Start()
    {
        Setup();
        SelectFile(0);
    }

    public void Setup()
    {
        // Initialize list of string arrays
        fileNamesList = new List<string[]>();
        fileNamesIndeces = new List<List<int>>();

        // Get all subdirectory names in the specified path
        print("Dir: " + Application.dataPath + "/" + subdirectoryPath);
        subdirectoryNames = Directory.GetDirectories(Application.dataPath + "/" + subdirectoryPath);

        foreach (string name in subdirectoryNames)
        {
            // Get all file names in the current subdirectory
            string[] fileNames = Directory.GetFiles(name, "*.png");
            fileNamesList.Add(fileNames); // Add array of file names to list
            List<int> indeces = new List<int>();
            fileNamesIndeces.Add(indeces);
            for (int i = 0; i < fileNames.Length; i++)
            {
                indeces.Add(i);
            }
        }

        // Set the first file to be selected by default
        // SelectFile(0);
    }

    public void SelectNextDirectory()
    {
        SetSelectedDirectory(selectedDirIndex + 1);
        text.text = new DirectoryInfo(subdirectoryNames[selectedDirIndex]).Name;
        text.color = Color.white;
        StopAllCoroutines();
        StartCoroutine(FadeText());
    }

    public void SelectPreviousDirectory()
    {
        SetSelectedDirectory(selectedDirIndex - 1);
        text.text = new DirectoryInfo(subdirectoryNames[selectedDirIndex]).Name;
        text.color = Color.white;
        StopAllCoroutines();
        StartCoroutine(FadeText());
    }

    public void SelectNextFile()
    {
        SelectFile(selectedFileIndex + 1);
    }

    public void SelectPreviousFile()
    {
        SelectFile(selectedFileIndex - 1);
    }

    IEnumerator FadeText()
    {
        float count = 0;
        while (count < 1)
        {
            count += Time.deltaTime / 3f;
            text.color = Color.white * (1 - count);
            yield return null;
        }
    }

    public void KickFile()
    {
        if (!Directory.Exists(subdirectoryNames[selectedDirIndex] + "/kicked"))
        {
            Directory.CreateDirectory(subdirectoryNames[selectedDirIndex] + "/kicked");
        }
        string path = Path.GetDirectoryName(fileNamesList[selectedDirIndex][selectedFileIndex]);
        string file = Path.GetFileName(fileNamesList[selectedDirIndex][selectedFileIndex]);
        print(path + "/kicked/" + file);
        File.Move(fileNamesList[selectedDirIndex][selectedFileIndex], path + "/kicked/" + file);
        Setup();
    }

    void SetSelectedDirectory(int index)
    {
        // Wrap around to the beginning or end of the list if necessary
        if (index < 0)
        {
            index = fileNamesList.Count - 1;
        }
        else if (index >= fileNamesList.Count)
        {
            index = 0;
        }

        // Select the new directory and update the selected directory index
        selectedDirIndex = index;
        selectedFileIndex = 0;

        // Debug.Log("Selected directory: " + subdirectoryPath + "/" + Path.GetFileName(fileNamesList[selectedDirIndex][0]));
        Debug.Log("Selected file: " + fileNamesList[selectedDirIndex][selectedFileIndex]);
        LoadTexture();
        FindObjectOfType<CorrectCameraParentAim>().ResetView();
    }

    void SelectFile(int index)
    {
        // Wrap around to the beginning or end of the list if necessary
        if (index < 0)
        {
            index = fileNamesList[selectedDirIndex].Length - 1;
        }
        else if (index >= fileNamesList[selectedDirIndex].Length)
        {
            index = 0;
        }

        int thisIndex = index;

        if (randomizeIndeces)
        {
            int r = (int)Random.Range(0, fileNamesIndeces[selectedDirIndex].Count - 1);
            thisIndex = fileNamesIndeces[selectedDirIndex][r];
            fileNamesIndeces[selectedDirIndex].RemoveAt(r);
            if (fileNamesIndeces[selectedDirIndex].Count == 0)
            {
                for (int i = 0; i < fileNamesList[selectedDirIndex].Length; i++)
                {
                    fileNamesIndeces[selectedDirIndex].Add(i);
                }
            }
        }

        // Select the new file and update the selected file index
        selectedFileIndex = thisIndex;

        Debug.Log("Selected file: " + fileNamesList[selectedDirIndex][selectedFileIndex]);
        LoadTexture();
        FindObjectOfType<CorrectCameraParentAim>().ResetView();
    }

    void LoadTexture()
    {
        loader.LoadImage(fileNamesList[selectedDirIndex][selectedFileIndex]);
    }
}


// using System.IO;
// using System.Collections.Generic;
// using UnityEngine;

// public class Flipper : MonoBehaviour
// {
//     public string subdirectoryPath; // The subdirectory path to search for files
//     public GameObject spherePrefab; // The prefab to use for creating spheres
//     public float sphereSpacing = 1.0f; // The amount of space to leave between spheres

//     private List<string[]> fileNamesList; // The list of string arrays of file names
//     private int selectedDirIndex = 0; // The index of the currently selected directory
//     private int selectedFileIndex = 0; // The index of the currently selected file in the selected directory

//     public void

//     void Start()
//     {
//         // Initialize list of string arrays
//         fileNamesList = new List<string[]>();

//         // Get all subdirectory names in the specified path
//         string[] subdirectoryNames = Directory.GetDirectories(subdirectoryPath);

//         int offset = 0;
//         foreach (string name in subdirectoryNames)
//         {
//             // Get all file names in the current subdirectory
//             string[] fileNames = Directory.GetFiles(name);
//             fileNamesList.Add(fileNames); // Add array of file names to list

//             // Create a new sphere GameObject and set its name to the name of the subdirectory
//             GameObject sphere = Instantiate(spherePrefab, this.transform);
//             sphere.name = Path.GetFileName(name);

//             // Position the sphere to the right of the previous sphere, with spacing between them
//             Vector3 pos = sphere.transform.position;
//             pos.x += sphereSpacing * offset;
//             offset++;
//             sphere.transform.position = pos;
//         }

//         // Set the first sphere to be selected by default
//         SetSelectedSphere(0);
//     }

//     void Update()
//     {
//         // Check for input to select the next or previous sphere
//         if (Input.GetKeyDown(KeyCode.RightArrow))
//         {
//             SetSelectedSphere(selectedDirIndex + 1);
//         }
//         else if (Input.GetKeyDown(KeyCode.LeftArrow))
//         {
//             SetSelectedSphere(selectedDirIndex - 1);
//         }

//         // Check for input to select the next or previous file in the currently selected directory
//         if (Input.GetKeyDown(KeyCode.DownArrow))
//         {
//             selectedFileIndex--;
//             if (selectedFileIndex < 0)
//                 selectedFileIndex = fileNamesList[selectedDirIndex].Length - 1;
//             Debug.Log("Selected file: " + fileNamesList[selectedDirIndex][selectedFileIndex]);
//         }
//         else if (Input.GetKeyDown(KeyCode.UpArrow))
//         {
//             selectedFileIndex++;
//             selectedFileIndex = selectedFileIndex % (fileNamesList[selectedDirIndex].Length - 1);
//             Debug.Log("Selected file: " + fileNamesList[selectedDirIndex][selectedFileIndex]);
//         }
//     }

//     void SetSelectedSphere(int index)
//     {
//         // Wrap around to the beginning or end of the list if necessary
//         if (index < 0)
//         {
//             index = fileNamesList.Count - 1;
//         }
//         else if (index >= fileNamesList.Count)
//         {
//             index = 0;
//         }

//         // Deselect the previously selected sphere
//         Transform previousSphere = transform.GetChild(selectedDirIndex);
//         previousSphere.GetComponent<Renderer>().material.color = Color.blue;

//         // Select the new sphere and update the selected directory index
//         Transform newSphere = transform.GetChild(index);
//         newSphere.GetComponent<Renderer>().material.color = Color.red;
//         selectedDirIndex = index;
//         selectedFileIndex = 0;

//         Debug.Log("Selected directory: " + newSphere.name);
//         Debug.Log("Selected file: " + fileNamesList[selectedDirIndex][selectedFileIndex]);
//     }
// }


// // using System.IO;
// // using System.Collections.Generic;
// // using UnityEngine;

// // public class Flipper : MonoBehaviour
// // {
// //     public string subdirectoryPath; // The subdirectory path to search for files
// //     public GameObject spherePrefab; // The prefab to use for creating spheres
// //     public float sphereSpacing = 1.0f; // The amount of space to leave between spheres

// //     void Start()
// //     {
// //         List<string[]> Dirs = new List<string[]>(); // Initialize list of string arrays

// //         string[] subdirectoryNames = Directory.GetDirectories(subdirectoryPath);
// //         int offset = 0;
// //         foreach (string name in subdirectoryNames)
// //         {
// //             string[] fileNames = Directory.GetFiles(name);
// //             Dirs.Add(fileNames); // Add array of file names to list

// //             // Create a new sphere GameObject and set its name to the name of the subdirectory
// //             GameObject sphere = Instantiate(spherePrefab);
// //             sphere.name = Path.GetFileName(name);

// //             // Position the sphere to the right of the previous sphere, with spacing between them
// //             Vector3 pos = sphere.transform.position;
// //             pos.x += sphereSpacing * offset;
// //             offset++;
// //             sphere.transform.position = pos;
// //         }
// //     }
// // }
