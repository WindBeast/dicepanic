using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    [SerializeField] Sprite starImage;
    [SerializeField] Sprite starNull;
    [SerializeField] Image[] stars;
    [SerializeField] Text levelText;

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
            stars[i].sprite = starNull;
        }
        levelText.text = "Level.1";
        judgeImage.color = new Color(0,0,0,0);
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
        if(GameManager.instance.isGameStart)
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
    }
    
    public void FadeInOutJudge(string judgeText)
    {
        switch (judgeText)
        {
            case "correct":
                judgeImage.color = new Color(1,0,0,0);
                judgeImage.sprite = correct;
                soundSource.PlayOneShot(correctSound);
                if(GameManager.instance.correctNum < 5)
                {
                    GameManager.instance.correctNum++;
                    stars[GameManager.instance.correctNum-1].sprite = starImage;
                }
                if(GameManager.instance.correctNum == 5)
                {
                    if(GameManager.instance.levelNum<3)
                    {
                        GameManager.instance.correctNum = 0;
                        GameManager.instance.levelNum ++;
                        GameManager.instance.source = "Images/Questions/Level"+ GameManager.instance.levelNum.ToString() + "/";
                        GameManager.instance.level = Resources.LoadAll<Sprite>(GameManager.instance.source);
                        for(int i=0; i<5; i++)
                        {
                            stars[i].sprite = starNull;
                        }
                        levelText.text = "Level." + GameManager.instance.levelNum.ToString();
                    }
                }
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
                timer.countTime += 5;
                NextQ();
                break;
        }
        fadeSequence.Restart();
    }
    
    public void NextQ()
    {
        GameManager.instance.Question.GetComponent<Image>().sprite = GameManager.instance.level[r.Next(3)];
    }
}
