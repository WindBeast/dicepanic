using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
public class Staff : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text connectionCheck;
    [SerializeField] private Text viewCheck;
    [SerializeField] private Text floorNumText;

    private PhotonView hostView;

    private bool isConnected = false;

    private int floorNum = 0;

    private void Start() {
        Connect();
        viewCheck.text = "取得待機";
        floorNumText.text = "0";
        // プレイヤー自身の名前を"Player"に設定する
        PhotonNetwork.NickName = "Staff";
        Connect();
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster() {
        isConnected = true;
        // "Game"という名前のルームに参加する
        PhotonNetwork.JoinRoom("Game");
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom() {
        connectionCheck.text = "接続成功";
        isConnected = true;
    }

    // ゲームサーバーへの接続が失敗した時に呼ばれるコールバック
    public override void OnJoinRoomFailed(short returnCode, string message) {
        connectionCheck.text = "接続失敗";
    }

    public void Connect() {
        connectionCheck.text = "接続中";
        if (!isConnected) {
            // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
            PhotonNetwork.ConnectUsingSettings();
        } else {
            PhotonNetwork.JoinRoom("Game");
        }
    }

    public void GetHostView() {
        if(GameObject.Find("OnlineManager(Clone)")!=null) {
            hostView = GameObject.Find("OnlineManager(Clone)").GetComponent<PhotonView>();
            viewCheck.text = "取得成功";
        } else {
            viewCheck.text = "取得失敗";
        }
    }

    public void InputNum(int num) {
        if(floorNum == 0) {
            floorNum = num;
        } else {
            floorNum = floorNum * 10 + num;
        }
        floorNumText.text  = floorNum.ToString();
    }

    public void DeleteNum() {
        floorNum = floorNum / 10;
        floorNumText.text  = floorNum.ToString();
    }

    public void SendNum() {

    }

    public void Judge(string judgeText) {
        hostView.RPC("Judge", hostView.Owner, judgeText);
    }

    public void Pause() {
        hostView.RPC("Pause", hostView.Owner);
    }

    public void Back() {
        hostView.RPC("Back", hostView.Owner);
    }

    public void GameStart() {
        hostView.RPC("GameStart", hostView.Owner);
    }
}