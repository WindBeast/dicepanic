using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance = null;

    [SerializeField] GameObject setting;
    [SerializeField] GameObject questionScene;

    [SerializeField] private Text connectionCheck;
    [SerializeField] private Text clientCheck;
    [SerializeField] private Judge judge;
    [SerializeField] private Timer timer;

    public int levelNum = 1;
    public string source;
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

        // プレイヤー自身の名前を設定する
        PhotonNetwork.NickName = "Host";

        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            setting.SetActive(true);
            questionScene.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            setting.SetActive(false);
            questionScene.SetActive(true);
        }
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster() {
    // "Game"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
        PhotonNetwork.JoinOrCreateRoom("Game", new RoomOptions(), TypedLobby.Default);
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
        if(isGameStart && !timer.isPause) judge.FadeInOutJudge(judgeText);
    }

    public void Pause() {
        if(isGameStart) {
            if(timer.isPause) timer.UnPause();
            else timer.Pause();
            }
    }

    public void Back() {
        isBeforeStart = false;
        isCount = false;
        isGameStart = false;
        SceneManager.LoadScene("TitleScene"); // ここ変える
    }

    public void GameStart() {
        if(timer.countDownNum == 3 && !isBeforeStart)
            {
                setting.SetActive(false);
                questionScene.SetActive(true);
                isCount = true;
                isBeforeStart = true;
                timer.countDownPanel.SetActive(true);
                timer.cntSource.PlayOneShot(timer.cnt);
                timer.countDownGauge.enabled = true;
            }
    }
}
