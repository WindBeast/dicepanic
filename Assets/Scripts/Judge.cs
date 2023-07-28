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
    [SerializeField] AudioClip clearSound;
    [SerializeField] AudioClip passSound;
    [SerializeField] AudioClip fanfare;
    [SerializeField] AudioSource soundSource;

    public System.Random r = new System.Random();

    [SerializeField] Image judgeImage;
    [SerializeField] Sprite correct;
    [SerializeField] Sprite wrong;
    [SerializeField] Sprite pass;
    // [SerializeField] Sprite starImage;
    // [SerializeField] Sprite starNull;
    // [SerializeField] Image[] stars;
    // [SerializeField] Text levelText;

    public int floorNum = 1;
    public int qNum = 0; // 問題の種類数を管理するやつ。
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
        GameManager.instance.source = "Images/Questions/"+ floorNum.ToString() + "/";
        GameManager.instance.level = Resources.LoadAll<Sprite>(GameManager.instance.source);
        qNum += r.Next(GameManager.instance.level.Length);
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
            if (Input.GetKeyDown(KeyCode.C))
            {
                FadeInOutJudge("pass");
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                FadeInOutJudge("skip");
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                BackQ();
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
                if(floorNum == 50) {
                    judgeImage.color = new Color(1,0,0,0);
                    judgeImage.sprite = correct;
                    soundSource.PlayOneShot(clearSound);
                    timer.bgmSource.Stop();
                    GameManager.instance.isCount = false;
                    GameManager.instance.isGameStart = false;
                    DOVirtual.DelayedCall(3f, () => soundSource.PlayOneShot(fanfare)).Play();
                    DOVirtual.DelayedCall(9f, () => {
                        GameManager.instance.congratulations.SetActive(true);
                        GameManager.instance.resultNum.text = "全50";
                        GameManager.instance.ChangeScene("result");
                    }).Play();
                } else {
                    judgeImage.color = new Color(1,0,0,0);
                    judgeImage.sprite = correct;
                    soundSource.PlayOneShot(correctSound);
                    UpFloor();
                    if(floorNum==4 || floorNum==8 || floorNum==13 || floorNum==18) timer.Pause();
                    NextQ();
                }
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
                // timer.countTime += timer.passTime;
                NextQ();
                break;
            case "skip":
                timer.countTime += 10;
                break;
        }
        fadeSequence.Restart();
    }

    public void NextQ()
    {
        // Debug.Log(GameManager.instance.level);
        qNum = (qNum + 1) % GameManager.instance.level.Length;
        timer.question.sprite = GameManager.instance.level[qNum];
    }

    public void BackQ()
    {
        if(floorNum!=1)
        {
            floorNum--;
            floorText.text = floorNum.ToString();
            GameManager.instance.source = "Images/Questions/"+ floorNum.ToString() + "/";
            GameManager.instance.level = Resources.LoadAll<Sprite>(GameManager.instance.source);
            qNum += r.Next(GameManager.instance.level.Length-1);
            qNum = (qNum + 1) % GameManager.instance.level.Length;
            timer.question.sprite = GameManager.instance.level[qNum];
        }
    }

    public void UpFloor()
    {
        floorNum++;
        floorText.text = floorNum.ToString();
        GameManager.instance.source = "Images/Questions/"+ floorNum.ToString() + "/";
        GameManager.instance.level = Resources.LoadAll<Sprite>(GameManager.instance.source);
        qNum += r.Next(GameManager.instance.level.Length-1);
    }
}
