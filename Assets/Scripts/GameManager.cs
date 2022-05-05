using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public int maxQuestionNum = 20; // 1レベルあたりのMAX問題数(ここの仕様変わるのであまり気にしなくてよい)

    public static GameManager instance = null;

    [SerializeField] GameObject roomPanel;
    [SerializeField] GameObject setting;
    [SerializeField] GameObject questionScene;
    [SerializeField] GameObject inputFloorScene;
    [SerializeField] GameObject resultScene;
    [SerializeField] GameObject titleScene;

    [SerializeField] public AudioClip titleBgm;
    [SerializeField] public AudioClip inputFloorBgm;
    [SerializeField] public AudioClip resultBgm;
    [SerializeField] public AudioSource allBgmSource; // (allはTimerのsourceとの区別)
    [SerializeField] public AudioClip choiceSE1; // タイトル画面から次の画面への遷移音
    [SerializeField] public AudioClip choiceSE2; // 最高到達問題数入力音
    [SerializeField] public AudioClip choiceSE3; // 問題数入力画面から出題画面への遷移音
    [SerializeField] public AudioClip choiceSE4; // 出題画面からスコア画面への遷移音
    [SerializeField] public AudioClip scoreSE1; // スコア画面で使うSE
    [SerializeField] public AudioSource allSoundSource; // (allはTimerのsourceとの区別)

    [SerializeField] private Text connectionCheck;
    [SerializeField] private Text clientCheck;

    [SerializeField] private Image logo;
    private RectTransform logoRect;
    [SerializeField] private Image logoShadow;
    private RectTransform logoShadowRect;

    [SerializeField] private Text floorNumForInput;
    [SerializeField] private Text resultNum;
    [SerializeField] private Text cardText;
    // [SerializeField] private Text testText;

    [SerializeField] private Judge judge;
    [SerializeField] private Timer timer;

    public int roomNum = 0;

    public int levelNum = 1; // 内部レベルのための変数。
    public string source; // 画像が置かれている場所のURL
    public Sprite[] level; // 画像がたくさん入る
    public int correctNum = 0; // 正解数を記録する？要らない？

    public bool isCount = false; // countTime(カウントダウンタイマー)を進めていいかどうかのフラグ。
    // ポーズ中は止まる。ゲーム開始前(要確認)にfalse。
    public bool isBeforeStart = false; // ゲーム開始カウントダウン中かどうかのフラグ
    public bool isGameStart = false; // ゲーム中かどうかのフラグ

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        clientCheck.text = "スタッフ端末待機中…";
        connectionCheck.text = "サーバー接続中…";

        judge.floorNum = 1;
        floorNumForInput.text = "0";

        ChangeScene("title");
        roomPanel.SetActive(true);

        // プレイヤー自身の名前を設定する
        PhotonNetwork.NickName = "Host";

        logoRect = logo.GetComponent<RectTransform>();
        logoRect.DOMoveY(0.2f, 1.5f).SetRelative(true).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        logoShadowRect = logoShadow.GetComponent<RectTransform>();
        logoShadowRect.DOMoveY(-0.1f, 1.5f).SetRelative(true).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

        cardText.DOFade(0f, 2f).SetEase(Ease.InQuad).SetLoops(-1, LoopType.Yoyo);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if(setting.activeSelf) setting.SetActive(false);
            else setting.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reconnect();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextScene();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            BackScene();
        }
    }


    // ルームナンバーを選んでサーバーに接続
    public void ChoiceRoom(int num) {
        roomNum = num;
        roomPanel.SetActive(false);
        Cursor.visible = false;
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }
    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster() {
    // "Game"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
        PhotonNetwork.JoinOrCreateRoom(("Room" + roomNum.ToString()), new RoomOptions(), TypedLobby.Default);
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom() {
        connectionCheck.text = "サーバー接続に成功";
        PhotonNetwork.Instantiate("OnlineManager", Vector3.zero, Quaternion.identity);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        clientCheck.text = "スタッフ端末の接続に成功";
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        clientCheck.text = "スタッフ端末の接続が切断";
    }

    public void Judge(string judgeText) {
        // ゲームスタート中であり、ポーズ中でなければジャッジをしてくれる。
        if(isGameStart && !timer.isPause) judge.FadeInOutJudge(judgeText);
    }

    public void Pause() {
        // ゲームスタート中ならばポーズを切り替える
        // ダイス追加の説明をするタイミングでもポーズになる
        if(isGameStart) {
            if(timer.isPause) timer.UnPause();
            else timer.Pause();
            }
    }

    public void Back() {
        // 色々な設定をリセットして階数入力画面に戻る(仕様について要検討)
        isBeforeStart = false;
        isCount = false;
        isGameStart = false;
        SceneManager.LoadScene("TitleScene"); // ここ変える
    }

    public void GameStart() {
        // ゲームスタート前で、カウントダウン数字が3であればカウントダウンを始める
        if(timer.countDownNum == 3 && !isBeforeStart && questionScene.activeSelf)
            {
                isCount = true; // カウントダウンタイマーをスタート
                isBeforeStart = true; // カウントダウン中フラグを立てる
                timer.countDownPanel.SetActive(true); // カウントダウンパネルを表示
                timer.cntSource.PlayOneShot(timer.cnt); // カウントダウンSEを鳴らす
                timer.countDownGauge.enabled = true; // カウントダウンゲージを表示させる
                source = "Images/Questions/"+ judge.floorNum.ToString() + "/";
                level = Resources.LoadAll<Sprite>(source);
                judge.qNum += judge.r.Next(level.Length);
                judge.NextQ();
            }
    }

    public void InputFloorNum(int num) {
        allSoundSource.PlayOneShot(choiceSE2);
        floorNumForInput.text = num.ToString();
        judge.floorNum = num+1;
    }

    public void ChangeScene(string name) {
        titleScene.SetActive(false);
        inputFloorScene.SetActive(false);
        questionScene.SetActive(false);
        resultScene.SetActive(false);
        switch(name) {
            case "title":
                allBgmSource.volume = 0.4f;
                allBgmSource.clip = titleBgm;
                allBgmSource.Play();
                titleScene.SetActive(true);
                break;
            case "input":
                allBgmSource.volume = 0.3f;
                allSoundSource.PlayOneShot(choiceSE1);
                allBgmSource.clip = inputFloorBgm;
                allBgmSource.Play();
                inputFloorScene.SetActive(true);
                break;
            case "question":
                allSoundSource.PlayOneShot(choiceSE3);
                allBgmSource.Stop();
                questionScene.SetActive(true);
                break;
            case "result":
                allBgmSource.clip = resultBgm;
                allBgmSource.Play();
                resultScene.SetActive(true);
                break;
        }
    }

    public void NextScene() {
        if(titleScene.activeSelf) ChangeScene("input");
        else if(inputFloorScene.activeSelf) {
            ChangeScene("question");
            judge.floorText.text = judge.floorNum.ToString();
        }
        else if(resultScene.activeSelf) ChangeScene("title");
    }

    public void BackScene() {
        if(inputFloorScene.activeSelf) ChangeScene("title");
        else if(!isBeforeStart && !isGameStart && questionScene.activeSelf) ChangeScene("input");
    }

    public void GoToResult() {
        resultNum.text = (judge.floorNum-1).ToString();
        ChangeScene("result");
        timer.ResetTimer();
        judge.floorNum = 1;
        floorNumForInput.text = "0";
    }

    public void Reconnect() {
        connectionCheck.text = "サーバー接続中…";
        PhotonNetwork.ConnectUsingSettings();
    }
}
