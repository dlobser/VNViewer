using UnityEngine;

public class FlipperInput : MonoBehaviour
{
    public Flipper flipper;

    void Update()
    {
        // Check for input to select the next or previous sphere
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            flipper.SelectNextDirectory();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            flipper.SelectPreviousDirectory();
        }

        // Check for input to select the next or previous file in the currently selected directory
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            flipper.SelectPreviousFile();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            flipper.SelectNextFile();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            flipper.KickFile();
        }
    }
}
