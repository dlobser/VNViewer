using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class CorrectCameraParentAim : MonoBehaviour
{

    public float speed;
    public bool fixPosition;
    public float positionSpeed;

    bool present;
    bool prevPresent;

    public float minAngle = 45;

    Quaternion prevRotation;
    bool shouldBeMoving = false;

    public float moveTimer = 3;
    float timer;

    float rotateSpeed = 0;

    public Vector3 resetAxes;

    public float fadeSpeed = .2f;
    public GameObject fader;

    public Vector3 initPos;

    public GameObject target;


    void Awake()
    {

        Init();

    }


    private void Init()
    {
        if (target == null)
        {
            GameObject g = new GameObject();
            g.transform.parent = Camera.main.transform.parent;
            g.transform.localScale = Vector3.one;
            g.transform.localPosition = Vector3.zero;
            g.transform.localEulerAngles = Vector3.zero;
            target = g;
        }

        target.transform.localEulerAngles = new Vector3(
           Camera.main.transform.localEulerAngles.x,
           Camera.main.transform.localEulerAngles.y,
           0);
        target.transform.position = Camera.main.transform.position;


        this.transform.localRotation = Quaternion.Inverse(target.transform.localRotation);
        if (fixPosition)
            this.transform.GetChild(0).localPosition = target.transform.localPosition * -1;
        initPos = target.transform.position;
    }

    private void OnEnable()
    {
        Init();
    }

    void FixAxes()
    {
        this.transform.localEulerAngles = new Vector3(
            resetAxes.x > 0 ? 0 : this.transform.localEulerAngles.x,
            resetAxes.y > 0 ? 0 : this.transform.localEulerAngles.y,
            resetAxes.z > 0 ? 0 : this.transform.localEulerAngles.z);
    }

    public void Fade(float a)
    {
        if (fader != null)
        {
            if (a >= 0)
            {
                fader.SetActive(true);
                fader.GetComponent<MeshRenderer>().enabled = true;
            }
            a = Mathf.Clamp(a, 0, 1);
            fader.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, a);
            if (a <= 0)
            {
                fader.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    IEnumerator F(bool down)
    {
        float counter = 0;
        while (counter < 1)
        {
            counter += Time.deltaTime / fadeSpeed;
            Fade(down ? counter : 1 - counter);

            yield return null;
        }
        if (down)
        {
            shouldBeMoving = true;
            StartCoroutine(F(false));
        }
    }

    public void ResetView()
    {
        shouldBeMoving = true;
    }

    void Update()
    {

        FixAxes();

        target.transform.localEulerAngles = new Vector3(
            Camera.main.transform.localEulerAngles.x,
            Camera.main.transform.localEulerAngles.y,
            0);
        target.transform.position = Camera.main.transform.position;

        // present = XRDevice.isPresent;

        if (present != prevPresent)
        {
            Init();
        }

        //if (Input.anyKeyDown)
        //Init();

        //if (Quaternion.Angle(Camera.main.transform.localRotation, prevRotation) > 20)
        //    Init();

        //float angle = Quaternion.Angle(Quaternion.Inverse(this.transform.localRotation), Camera.main.transform.localRotation);
        //float dist = Vector3.Distance(Camera.main.transform.position,initPos);

        if (Quaternion.Angle(target.transform.localRotation, prevRotation) > 20)
            Init();

        float angle = Quaternion.Angle(Quaternion.Inverse(this.transform.localRotation), target.transform.localRotation);
        float dist = Vector3.Distance(target.transform.position, initPos);


        if (!shouldBeMoving)
        {
            if (angle > minAngle || (fixPosition && dist > .2f))
            {
                timer += Time.deltaTime;
                if (timer > moveTimer)
                {
                    StartCoroutine(F(true));
                    timer = 0;
                }

            }
            else
            {
                timer = 0;
            }
        }
        //if (shouldBeMoving)
        //{
        //    if (rotateSpeed < 1)
        //    {
        //        rotateSpeed += Time.deltaTime;
        //    }
        //    if (angle < 1)
        //    {
        //        rotateSpeed = 0;
        //        shouldBeMoving = false;
        //    }
        //}
        if (shouldBeMoving)
        {
            this.transform.localRotation = Quaternion.Inverse(target.transform.localRotation);// Quaternion.Lerp(
                                                                                              //this.transform.localRotation, Quaternion.Inverse(target.transform.localRotation), 1); //rotateSpeed*(Time.deltaTime*speed*200f)/Mathf.Pow(angle,.5f));
        }
        if (shouldBeMoving && fixPosition)
            this.transform.GetChild(0).localPosition = Vector3.Lerp(this.transform.GetChild(0).localPosition, target.transform.localPosition * -1, 1);// positionSpeed);
        prevPresent = present;
        prevRotation = target.transform.localRotation;
        shouldBeMoving = false;
    }
}
