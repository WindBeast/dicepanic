using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Timer : MonoBehaviour
{
    public int timeLimit = 180; // 制限時間
    public int passTime = 0; // パスをしたときに減る時間

    System.Random r = new System.Random(); // ランダム変数を用意
    [SerializeField] public Image question; // 問題表示用イメージオブジェクト
    [SerializeField] public AudioClip cnt; // カウントダウンSE
    [SerializeField] public AudioClip bgm; // BGM
    [SerializeField] public AudioClip pause; // ポーズSE
    [SerializeField] public AudioClip end; // 制限時間終了SE
    [SerializeField] public AudioSource cntSource; // SE用ソース ソース=オーディオプレイヤー
    [SerializeField] public AudioSource bgmSource; // BGM用ソース

    public float countTime = 0; // 時間をカウントするための汎用変数
    public int countDownNum = 0; // カウントダウン(3,2,1,0)が入る
    [SerializeField] public GameObject countDownPanel; // ゲーム開始カウントダウンのパネル
    [SerializeField] public Text countDownText; // ゲーム開始カウントダウンのテキスト
    [SerializeField] public Image countDownGauge; // ゲーム開始カウントダウンのゲージ
    public float left = 0; // 残り時間を扱うための変数
    [SerializeField] public Text leftTime; // 残り時間のテキスト
    [SerializeField] public Image timeGauge; // 残り時間のゲージ
    public float gaugeR, gaugeG, gaugeB; //ゲージのRGBを扱うための変数

    public bool isPause = false; // ポーズ中か否かを判定する変数
    [SerializeField] public Image pauseImage; // ポーズ中の画像
    [SerializeField] public Image timeUpImage; // time upの画像

    // Start is called before the first frame update
    void Start()
    {
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown (KeyCode.Space))
        {
            if(countDownNum == 3 && !GameManager.instance.isBeforeStart)
            {
                GameManager.instance.isCount = true;
                GameManager.instance.isBeforeStart = true;
                countDownPanel.SetActive(true);
                cntSource.volume = 0.7f;
                cntSource.PlayOneShot(cnt);
                countDownGauge.enabled = true;
            }
        }

        if (Input.GetKeyDown (KeyCode.P))
        {
            if(GameManager.instance.isGameStart)
            {
                if(isPause)
                {
                    UnPause();
                }
                else
                {
                    Pause();
                }
            }
        }

        if(GameManager.instance.isCount)
        {
            countTime += Time.deltaTime;
        }

        if(GameManager.instance.isBeforeStart) // こっちは開始前カウントダウン中に動く
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
                    GameManager.instance.isBeforeStart = false; // カウントダウン中ではなくなる
                    GameManager.instance.isGameStart = true; // ゲーム中になる
                    // Debug.Log("test 1");
                    // question.sprite = GameManager.instance.level[r.Next(GameManager.instance.level.Length)];
                    // Debug.Log("test 2");
                    countDownGauge.enabled = false;
                    countDownText.enabled = false;
                    countDownPanel.SetActive(false);
                    bgmSource.Play();
                    question.enabled = true;
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
            else if(left > 0)
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
                bgmSource.Stop();
                cntSource.volume = 0.3f;
                cntSource.PlayOneShot(end);
                timeUpImage.enabled = true;
                countDownPanel.SetActive(true);
                DOVirtual.DelayedCall(4f, () => GameManager.instance.GoToResult()).Play();
            }
        }

        if (Input.GetKeyDown (KeyCode.D))
        {
            bgmSource.Stop();
            leftTime.text = "00.00";
            timeGauge.fillAmount = 0;
            countDownNum = 3;
            GameManager.instance.isCount = false;
            GameManager.instance.isGameStart = false;
        }
    }

    public void Pause()
    {
        cntSource.PlayOneShot(pause);
        pauseImage.enabled = true;
        question.enabled = false;
        bgmSource.Pause();
        GameManager.instance.isCount = false;
        countDownPanel.SetActive(true);
        isPause = !isPause;
    }

    public void UnPause()
    {
        cntSource.PlayOneShot(pause);
        bgmSource.UnPause();
        GameManager.instance.isCount = true;
        countDownPanel.SetActive(false);
        pauseImage.enabled = false;
        question.enabled = true;
        isPause = !isPause;
    }

    public void ResetTimer() {
        timeGauge.color = new Color(0.5f,1f,0.5f);
        timeGauge.fillAmount = 1;
        countTime = 0;
        leftTime.text = timeLimit.ToString();

        countDownNum = 3;
        countDownText.text = "3";
        countDownPanel.SetActive(false);

        pauseImage.enabled = false;
        timeUpImage.enabled = false;
        isPause = false;

        question.enabled = false;
        countDownText.enabled = true;
    }
}
