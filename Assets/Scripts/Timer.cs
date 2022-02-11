using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    System.Random r = new System.Random();
    [SerializeField] AudioClip cnt;
    [SerializeField] AudioClip bgm;
    [SerializeField] AudioSource cntSource;
    [SerializeField] AudioSource bgmSource;
    public float countTime = 0; // 時間をカウントするための汎用変数
    int countDownNum = 0; // カウントダウン(3,2,1,0)が入る
    [SerializeField] GameObject countDownPanel; // ゲーム開始カウントダウンのパネル
    [SerializeField] Text countDownText; // ゲーム開始カウントダウンのテキスト
    [SerializeField] Image countDownGauge; // ゲーム開始カウントダウンのゲージ
    int timeLimit = 180;
    public float left = 0; // 残り時間を扱うための変数
    [SerializeField] Text leftTime; // 残り時間のテキスト
    [SerializeField] Image timeGauge; // 残り時間のゲージ
    float gaugeR, gaugeG, gaugeB;

    // Start is called before the first frame update
    void Start()
    {
        timeGauge.color = new Color(0.5f,1f,0.5f);
        countDownNum = 3;
        countDownText.text = "3";
        countDownPanel.SetActive(false);
        leftTime.text = timeLimit.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown (KeyCode.Space)) 
        {
            if(countDownNum == 3 && GameManager.instance.isBeforeStart == false)
            {
                GameManager.instance.isCount = true;
                GameManager.instance.isBeforeStart = true;
                countDownPanel.SetActive(true);
                cntSource.PlayOneShot(cnt);
            }
        }

        if(GameManager.instance.isCount)
        {
            countTime += Time.deltaTime;
        }

        if(GameManager.instance.isBeforeStart)
        {
            if(countTime < 1)
            {
                countDownGauge.fillAmount = 1 - countTime;
            }
            else
            {
                if(countDownNum == 1)
                {
                    countTime = 0;
                    countDownNum--;
                    GameManager.instance.isBeforeStart = false;
                    GameManager.instance.isGameStart = true;
                    GameManager.instance.Question.GetComponent<Image>().sprite = GameManager.instance.level[r.Next(3)];
                    countDownPanel.SetActive(false);
                    bgmSource.PlayOneShot(bgm);
                }
                else
                {
                    countTime = 0;
                    countDownGauge.fillAmount = 1;
                    countDownNum--;
                    countDownText.text = countDownNum.ToString();
                }
            }
        }

        if(GameManager.instance.isGameStart)
        {
            left = timeLimit - countTime;
            if(left >= 60)
            {
                leftTime.text = Mathf.Floor(left).ToString("F0");
                timeGauge.fillAmount = left / timeLimit;
            }
            else if(left >= 0)
            {
                leftTime.text = left.ToString("F2");
                timeGauge.fillAmount = left / timeLimit;
                gaugeR = 1 - left/60 * 0.5f;
                gaugeG = left/60;
                gaugeB = gaugeG/2;
                timeGauge.color = new Color(gaugeR, gaugeG, gaugeB);
            }
            else
            {
                leftTime.text = "00.00";
                timeGauge.fillAmount = 0;
                GameManager.instance.isCount = false;
                GameManager.instance.isGameStart = false;
            }
        }
    }
}
