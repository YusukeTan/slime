using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Text timerText;    //　タイマー表示用テキスト
    private GameManager gameManager;
    private string oldTimerText;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    void Start()
    {
        timerText = GetComponent<Text>();
        timerText.text = "00:00";
    }


    void Update()
    {
        
        //　値が変わった時だけテキストUIを更新
        if (oldTimerText != gameManager.CountTime)
        {
            timerText.text = gameManager.CountTime;
        }
        oldTimerText = timerText.text;

    }


}
