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

    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject controller;
    [SerializeField] private GameObject roomPanel;

    private PhotonView hostView;

    private int roomNumStaff;
    private int floorNum = 0;

    private void Start() {
        connectionCheck.text = "接続待機";
        viewCheck.text = "取得待機";
        floorNumText.text = "0";
        // プレイヤー自身の名前を"Player"に設定する
        PhotonNetwork.NickName = "Staff";
        floor.SetActive(true);
        controller.SetActive(false);
        roomPanel.SetActive(true);
    }

    public void ChoiceRoomStaff(int num) {
        roomNumStaff = num;
        roomPanel.SetActive(false);
        Connect();
    }
    public void Connect() {
        if (!PhotonNetwork.IsConnected) {
            connectionCheck.text = "接続中";
            // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
            PhotonNetwork.ConnectUsingSettings();
        } else if(!PhotonNetwork.InRoom) {
            connectionCheck.text = "接続中";
            PhotonNetwork.JoinRoom(("Room" + roomNumStaff.ToString()));
        }
    }


    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster() {
        // "Game"という名前のルームに参加する
        PhotonNetwork.JoinRoom(("Room" + roomNumStaff.ToString()));
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom() {
        connectionCheck.text = "接続成功";
    }

    // ゲームサーバーへの接続が失敗した時に呼ばれるコールバック
    public override void OnJoinRoomFailed(short returnCode, string message) {
        connectionCheck.text = "接続失敗";
    }


    public void GetHostView() {
        if(GameObject.Find("OnlineManager(Clone)")!=null) {
            hostView = GameObject.Find("OnlineManager(Clone)").GetComponent<PhotonView>();
            viewCheck.text = "取得成功";
        } else {
            viewCheck.text = "取得失敗";
        }
    }

    public void ToInputFloor() {
        controller.SetActive(false);
        floor.SetActive(true);
    }

    public void ToController() {
        floor.SetActive(false);
        controller.SetActive(true);
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
        hostView.RPC("InputFloorNum", hostView.Owner, floorNum);
        floorNum = 0;
        floorNumText.text  = floorNum.ToString();
    }

    public void Judge(string judgeText) {
        hostView.RPC("Judge", hostView.Owner, judgeText);
    }

    public void Pause() {
        hostView.RPC("Pause", hostView.Owner);
    }

    public void NextScene() {
        hostView.RPC("NextScene", hostView.Owner);
    }

    public void BackScene() {
        hostView.RPC("BackScene", hostView.Owner);
    }

    public void GameStart() {
        hostView.RPC("GameStart", hostView.Owner);
    }
}