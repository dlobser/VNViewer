using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{

    bool A = false;
    bool B = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.X) || A && B)
        {
            Application.Quit();
        }

    }

    public void PressA()
    {
        A = true;
    }
    public void ReleaseA()
    {
        A = false;
    }

    public void PressB()
    {
        B = true;
    }
    public void ReleaseB()
    {
        B = false;
    }
}
