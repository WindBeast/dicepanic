using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    AsyncOperation mainScene;
    [SerializeField] Text progressText;
    [SerializeField] Text mainText;
    public bool isLoaded = false;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.levelNum = 1;
        GameManager.instance.correctNum = 0;
        progressText.text = "";
        mainScene = SceneManager.LoadSceneAsync("MainScene");
        mainScene.allowSceneActivation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mainScene.allowSceneActivation = true;
        }
    }
}
