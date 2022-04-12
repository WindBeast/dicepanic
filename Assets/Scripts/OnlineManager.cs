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
    private void Judge(string judgeText) {
        GameManager.instance.Judge(judgeText);
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
    private void InputFloorNum(int num) {
        GameManager.instance.InputFloorNum(num);
    }
}
