using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OnlineManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [PunRPC]
    private void InputNum(int num) {
        GameManager.instance.InputNum(num);
    }

    [PunRPC]
    private void Judge(string judgeText) {
        GameManager.instance.Judge(judgeText);
    }

    [PunRPC]
    private void BackQ(string judgeText) {
        GameManager.instance.BackQ_j();
    }

    [PunRPC]
    private void Pause() {
        GameManager.instance.Pause();
    }

    [PunRPC]
    private void NextScene() {
        GameManager.instance.NextScene();
    }

    [PunRPC]
    private void BackScene() {
        GameManager.instance.BackScene();
    }

    [PunRPC]
    private void GameStart() {
        GameManager.instance.GameStart();
    }

    [PunRPC]
    private void ResetGame(string resetText) {
        GameManager.instance.ResetGame(resetText);
    }
}
