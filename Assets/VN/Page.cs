using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace Viewer
{

    public static class Utils
    {
        public static Page GetPage(Dictionary<string, Page> dictionary, string inputKey)
        {
            foreach (string key in dictionary.Keys)
            {
                if (key.Replace(" ", "").ToLower() == inputKey.Replace(" ", "").ToLower())
                {
                    return dictionary[inputKey.Replace(" ", "").ToLower()];
                }
            }
            return null;
        }
    }

    [System.Serializable]
    public enum PageOptionType { MENU, SCENE }
    [System.Serializable]
    public class Page
    {
        public PageOptionType OptionType;
        public string Name;
        public string Label;
        public string PreviousPage;
        public string NextPage;
        public string[] Pages;
        public string IntroText;
        public string BGMusic;
        public string MusicOneShot;
        public string ImagesDirectory;
        public PageView pageView { get; set; }
    }

    public class PageView : MonoBehaviour
    {

        public Page page;
        public Canvas canvas;
        public bool active;
        GameObject thisPage;
        public string[] imagesURLs;


        public List<Button> buttons;
        VNController vnController;

        public int currentIndex = 0;

        void SetupButtons()
        {
            buttons = new List<Button>();
            vnController = FindObjectOfType<VNController>();
        }

        public void Activate()
        {
            // Activate the game object
            this.active = true;
            thisPage.SetActive(true);
        }

        public void Deactivate()
        {
            this.active = false;
            thisPage.SetActive(false);
        }

        public void GoToPage(Page page)
        {

        }

        public void SetupUI(UIElement ui, Dictionary<string, Page> dict)
        {
            thisPage = new GameObject(page.Name);
            ui.text.text = page.IntroText;
            thisPage.transform.parent = this.transform;
            ui.transform.parent = thisPage.transform;

            SetupButtons();

            if (page.NextPage != null)
            {
                if (page.NextPage.Length > 0)
                {
                    GameObject b = Instantiate(ui.button, ui.buttonContainer.transform);
                    b.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Next";
                    b.GetComponent<UnityEngine.UI.Button>().interactable = true;
                    buttons.Add(b.GetComponent<UnityEngine.UI.Button>());
                    b.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                        vnController.DisplayPage(Utils.GetPage(dict, page.NextPage)));
                    b.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => Deactivate());
                }
            }
            if (page.PreviousPage != null)
            {
                if (page.PreviousPage.Length > 0)
                {
                    GameObject b = Instantiate(ui.button, ui.buttonContainer.transform);
                    b.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Previous";
                    buttons.Add(b.GetComponent<UnityEngine.UI.Button>());
                    b.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                        vnController.DisplayPage(Utils.GetPage(dict, page.PreviousPage)));
                    b.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => Deactivate());
                }
            }
            if (page.Pages != null)
            {
                foreach (string p in page.Pages)
                {
                    GameObject b = Instantiate(ui.button, ui.buttonContainer.transform);
                    b.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = Utils.GetPage(dict, p).Label;
                    buttons.Add(b.GetComponent<UnityEngine.UI.Button>());
                    b.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
                        vnController.DisplayPage(Utils.GetPage(dict, p)));
                    b.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => Deactivate());
                }
            }
            Deactivate();
        }

        public void SetupImages(string directory)
        {
            print(directory);
            imagesURLs = Directory.GetFiles(directory, "*.png");
            print(imagesURLs.Length);
        }

        public string GetRandomImage()
        {
            if (imagesURLs != null)
                return imagesURLs[Random.Range(0, imagesURLs.Length)];
            else
                return "";
        }
    }
}