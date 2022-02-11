using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public GameObject Question;
    public int levelNum = 1;
    public string source = "Images/Questions/Level1/";
    public Sprite[] level;
    public int correctNum = 0;
    public bool isCount = false; // countTimeを進めていいかどうかのフラグ
    public bool isBeforeStart = false; // ゲーム開始カウントダウン中かどうかのフラグ
    public bool isGameStart = false; // ゲーム中かどうかのフラグ

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // source = "Images/Questions/Level"+ num.ToString() + "/";
        level = Resources.LoadAll<Sprite>(source);
        System.Random r = new System.Random();
        // Question.GetComponent<Image>().sprite = level[r.Next(3)];
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
