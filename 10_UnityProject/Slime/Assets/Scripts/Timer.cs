using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Text timerText;    //�@�^�C�}�[�\���p�e�L�X�g
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
        
        //�@�l���ς�����������e�L�X�gUI���X�V
        if (oldTimerText != gameManager.CountTime)
        {
            timerText.text = gameManager.CountTime;
        }
        oldTimerText = timerText.text;

    }


}
