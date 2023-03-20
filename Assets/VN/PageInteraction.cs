using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Viewer
{
    public class PageInteraction : MonoBehaviour
    {
        VNController vnController;
        public Page currentPage;
        public int currentIndex = -1;

        void Start()
        {
            vnController = FindObjectOfType<VNController>();
        }

        void Update()
        {
            if (currentPage != vnController.currentPage)
            {
                currentPage = vnController.currentPage;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentIndex--;
                if (currentIndex < 0)
                {
                    currentIndex = currentPage.pageView.buttons.Count - 1;
                }
                UpdateButtonSelection();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentIndex++;
                if (currentIndex >= currentPage.pageView.buttons.Count)
                {
                    currentIndex = 0;
                }
                UpdateButtonSelection();
            }
            else if (Input.GetKeyDown(KeyCode.R) && currentIndex > -1)
            {
                if (currentIndex >= 0 && currentIndex < currentPage.pageView.buttons.Count)
                    currentPage.pageView.buttons[currentIndex].onClick.Invoke();
                currentIndex = -1;
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                vnController.DisplayRandomImageOnPage();
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                vnController.ToggleMenu();
            }
        }

        void UpdateButtonSelection()
        {
            print(currentIndex);
            currentPage.pageView.buttons[currentIndex].Select();
        }
    }
}