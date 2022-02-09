using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text leftTime;
    public Image timeGauge; 
    float countTime = 0;
    float left = 0;
    bool loop = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // countTimeに、ゲームが開始してからの秒数を格納
        if(loop)
        {
            countTime += Time.deltaTime;
            left = 180 - countTime;
        }

        // 小数2桁にして表示
        if(left >= 60)
        {
            leftTime.text = left.ToString("F0");
            timeGauge.fillAmount = left / 180;
        }
        else if(left >= 0)
        {
            leftTime.text = left.ToString("F2");
            timeGauge.fillAmount = left / 180;
        }
        else
        {
            leftTime.text = "00.00";
            timeGauge.fillAmount = 0;
            loop = false;
        }
    }
}
