using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] AudioClip cnt;
    [SerializeField] AudioClip bgm;
    [SerializeField] AudioSource cntSource;
    [SerializeField] AudioSource bgmSource;
    float countTime = 0; // 時間をカウントするための汎用変数
    int countDownNum = 0; // カウントダウン(3,2,1,0)が入る
    public GameObject countDownPanel; // ゲーム開始カウントダウンのパネル
    public Text countDownText; // ゲーム開始カウントダウンのテキスト
    public Image countDownGauge; // ゲーム開始カウントダウンのゲージ
    int timeLimit = 180;
    float left = 0; // 残り時間を扱うための変数
    public Text leftTime; // 残り時間のテキスト
    public Image timeGauge; // 残り時間のゲージ
    public bool isCount = false; // countTimeを進めていいかどうかのフラグ
    public bool isBeforeStart = false; // ゲーム開始カウントダウン中かどうかのフラグ
    public bool isGameStart = false; // ゲーム中かどうかのフラグ
    // Start is called before the first frame update
    void Start()
    {
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
            if(countDownNum == 3)
            {
                isCount = true;
                isBeforeStart = true;
                countDownPanel.SetActive(true);
                cntSource.PlayOneShot(cnt);
            }
        }

        if(isCount)
        {
            countTime += Time.deltaTime;
        }

        if(isBeforeStart)
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
                    isBeforeStart = false;
                    isGameStart = true;
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

        if(isGameStart)
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
            }
            else
            {
                leftTime.text = "00.00";
                timeGauge.fillAmount = 0;
                isCount = false;
                isGameStart = false;
            }
        }
    }
}
