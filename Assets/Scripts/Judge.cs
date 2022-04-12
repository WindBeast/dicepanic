using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Judge : MonoBehaviour
{
    [SerializeField] Timer timer;
    [SerializeField] AudioClip correctSound;
    [SerializeField] AudioClip wrongSound;
    [SerializeField] AudioClip passSound;
    [SerializeField] AudioSource soundSource;

    System.Random r = new System.Random();

    [SerializeField] Image judgeImage;
    [SerializeField] Sprite correct;
    [SerializeField] Sprite wrong;
    [SerializeField] Sprite pass;
    // [SerializeField] Sprite starImage;
    // [SerializeField] Sprite starNull;
    // [SerializeField] Image[] stars;
    // [SerializeField] Text levelText;

    public int floorNum = 1;
    [SerializeField] public Text floorText;

    Sequence fadeSequence;

    float fadeTime = 0.3f;

    void Awake()
    {
        DOTween.Init();
        DOTween.defaultAutoPlay = AutoPlay.AutoPlayTweeners;
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<5; i++)
        {
            // stars[i].sprite = starNull;
        }
        // levelText.text = "Level." + GameManager.instance.levelNum.ToString();
        // floorNum = 1;
        floorText.text = floorNum.ToString();
        judgeImage.color = new Color(0,0,0,0);
        NextQ();
        fadeSequence  = DOTween.Sequence()
                                .Append(DOTween.ToAlpha(
                                    () => judgeImage.color,
                                    color => judgeImage.color = color,
                                    1f,
                                    fadeTime))
                                .Append(DOTween.ToAlpha(
                                    () => judgeImage.color,
                                    color => judgeImage.color = color,
                                    0f,
                                    fadeTime))
                                .SetAutoKill(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.isGameStart && !timer.isPause)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                FadeInOutJudge("correct");
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                FadeInOutJudge("wrong");
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                FadeInOutJudge("pass");
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameManager.instance.isBeforeStart = false;
            GameManager.instance.isCount = false;
            GameManager.instance.isGameStart = false;
            SceneManager.LoadScene("TitleScene");
            GameManager.instance.correctNum = 0;
            GameManager.instance.levelNum = 1;
        }
    }

    public void FadeInOutJudge(string judgeText)
    {
        switch (judgeText)
        {
            case "correct":
                judgeImage.color = new Color(1,0,0,0);
                judgeImage.sprite = correct;
                soundSource.PlayOneShot(correctSound);
                UpFloor();
                if(floorNum==7 || floorNum==13 || floorNum==19 || floorNum==27) timer.Pause();
                NextQ();
                break;
            case "wrong":
                judgeImage.color = new Color(0,0,1,0);
                judgeImage.sprite = wrong;
                soundSource.PlayOneShot(wrongSound);
                break;
            case "pass":
                judgeImage.color = new Color(0,0.7f,0,0);
                judgeImage.sprite = pass;
                soundSource.PlayOneShot(passSound);
                timer.countTime += timer.passTime;
                NextQ();
                break;
        }
        fadeSequence.Restart();
    }

    public void NextQ()
    {
        GameManager.instance.source = "Images/Questions/"+ floorNum.ToString() + "/";
        GameManager.instance.level = Resources.LoadAll<Sprite>(GameManager.instance.source);
        Debug.Log(GameManager.instance.level);
        timer.question.sprite = GameManager.instance.level[r.Next(GameManager.instance.level.Length)];
    }

    public void UpFloor()
    {
        floorNum++;
        floorText.text = floorNum.ToString();
    }
}
