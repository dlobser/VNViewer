using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace Viewer
{

    public class VNController : MonoBehaviour
    {
        // Serialized fields
        public TextMesh displayText; // The text UI element that displays the dialogue text
        public Sprite backgroundImage; // The image UI element that displays the background image
        // public AudioSource audioSource; // The audio source used to play sound effects and music

        // Private fields
        private int currentSceneIndex = 0; // The index of the current scene in the scenes list

        Page[] pages;
        private Dictionary<string, Page> pageDictionary;

        public TextAsset text;

        public UIElement uiElement;

        public Page currentPage;

        public string startingPage;

        GameObject menu;

        // public AudioSource backgroundMusic;
        public VNMusicController vNMusicController;
        public TextureLoader textureLoader;

        void Start()
        {
            menu = new GameObject("Menu");
            ParsePages(text.text);
            currentPage = Utils.GetPage(pageDictionary, startingPage);
            currentPage.pageView.Activate();
        }

        public Dictionary<string, Page> ParsePages(string data)
        {
            List<Page> pages = new List<Page>();
            pageDictionary = new Dictionary<string, Page>();
            string[] pageEntries = data.Split('^');
            foreach (string pageEntry in pageEntries)
            {
                string[] pageProperties = pageEntry.Trim().Split('\n');
                Page page = new Page();
                foreach (string property in pageProperties)
                {
                    string[] keyValue = property.Split(new char[] { ':' }, 2);// property.Split(':');
                    string key = keyValue[0].Trim();
                    string value = keyValue[1].Trim();
                    switch (key)
                    {
                        case "OptionType":
                            page.OptionType = (PageOptionType)Enum.Parse(typeof(PageOptionType), value);
                            break;
                        case "Name":
                            page.Name = value;
                            break;
                        case "Label":
                            page.Label = value;
                            break;
                        case "PreviousPage":
                            page.PreviousPage = value;
                            break;
                        case "NextPage":
                            page.NextPage = value;
                            break;
                        case "Pages":
                            string[] splitPages = value.Split('|');
                            for (int i = 0; i < splitPages.Length; i++)
                            {
                                splitPages[i].Trim();
                            }
                            page.Pages = splitPages;
                            break;
                        case "IntroText":
                            page.IntroText = value;
                            break;
                        case "BGMusic":
                            page.BGMusic = value;
                            break;
                        case "MusicOneShot":
                            page.MusicOneShot = value;
                            break;
                        case "ImagesDirectory":
                            //check if it's a real directory or relative
                            page.ImagesDirectory = value.Contains(":") ? "" + value.Replace("\\", "/") : (Application.dataPath + "/") + value;
                            break;
                        default:
                            Debug.LogWarning($"Unknown property key '{key}' in page data.");
                            break;
                    }
                }
                string k = page.Name.Replace(" ", "").ToLower();
                pageDictionary[k] = page;
            }
            foreach (string key in pageDictionary.Keys)
            {
                GameObject p = new GameObject(pageDictionary[key].Name);
                p.transform.parent = menu.transform;
                PageView pv = p.AddComponent<PageView>();
                pv.page = pageDictionary[key];
                pv.SetupUI(Instantiate(uiElement), pageDictionary);
                if (!string.IsNullOrEmpty(pv.page.ImagesDirectory))
                    pv.SetupImages(pv.page.ImagesDirectory);
                pageDictionary[key].pageView = pv;
            }
            return pageDictionary;
        }

        private Page FindPageByName(List<Page> pages, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            foreach (Page page in pages)
            {
                if (page.Name == name)
                {
                    return page;
                }
            }
            return null;
        }

        public void ToggleMenu()
        {
            if (!menu.gameObject.activeInHierarchy)
                menu.SetActive(true);
            else
                menu.SetActive(false);
        }

        public void StartVisualNovel()
        {
            // TODO: Display the first scene in the visual novel
        }

        public void DisplayRandomImageOnPage()
        {
            print("Page Images Directory: " + currentPage.ImagesDirectory);
            string imageLocation = currentPage.pageView.GetRandomImage();
            if (!string.IsNullOrEmpty(imageLocation))
                textureLoader.LoadImage(imageLocation);
        }

        // Private methods
        public void DisplayPage(Page page)
        {
            print("Display Page: " + page.Name);
            page.pageView.Activate();
            currentPage = page;
            if (!string.IsNullOrEmpty(page.BGMusic))
            {
                AudioClip clip = GetAudioClip(page.BGMusic);
                vNMusicController.PlayBGMusic(clip);
            }
            DisplayRandomImageOnPage();
            // TODO: Set the displayText, backgroundImage, and audioSource fields based on the data in the scene
        }

        private void AdvanceToNextScene()
        {
            // TODO: Determine the next scene based on the current scene's options and display it
        }

        private AudioClip GetAudioClip(string path)
        {
            WWW www = new WWW(path);
            while (!www.isDone) { }
            return www.GetAudioClip(false, false, AudioType.MPEG);
        }
    }
}