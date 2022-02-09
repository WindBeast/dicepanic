using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject Question;
    int num = 1;
    string source = "Images/Questions/Level1/";
    Sprite[] level;
    public bool isStart = false;

    // Start is called before the first frame update
    void Start()
    {
        // source = "Images/Questions/Level"+ num.ToString() + "/";
        level = Resources.LoadAll<Sprite>(source);
        System.Random r = new System.Random();
        Question.GetComponent<Image>().sprite = level[r.Next(3)];
    }
    // Update is called once per frame
    void Update()
    {
        System.Random r = new System.Random();
        if (Input.GetKeyDown (KeyCode.RightArrow)) 
        {
            Question.GetComponent<Image>().sprite = level[r.Next(3)];
        }
        if (Input.GetKeyDown (KeyCode.UpArrow))
        {
            if(num<3)
            {
                num ++;
                source = "Images/Questions/Level"+ num.ToString() + "/";
                level = Resources.LoadAll<Sprite>(source);
                Question.GetComponent<Image>().sprite = level[r.Next(3)];
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isStart = true;
        }

    }
}
