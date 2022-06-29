using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(-9)]

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{

    [SerializeField] AudioClip BGM_Title;
    [SerializeField] AudioClip BGM_Main;
    [SerializeField] AudioClip SE_Damage;

    //�g�p����AudioSource
    private AudioSource source;
    private AudioSource seSource;

    void Start()
    {
        //�������V�[���؂�ւ������j�����Ȃ��悤�ɂ���
//        DontDestroyOnLoad(gameObject);

        //�g�p����AudioSource�擾
        source = GetComponent<AudioSource>();
        seSource = gameObject.AddComponent<AudioSource>();
        source.clip = BGM_Title;    //�ŏ��̓^�C�g��
        source.Play();
    }


    public void PlayTitleBGM()
    {
        source.Stop();
        source.clip = BGM_Title;    //�����N���b�v��؂�ւ���
        source.Play();
    }

    public void PlayMainBGM()
    {
        source.Stop();
        source.clip = BGM_Main;    //�����N���b�v��؂�ւ���
        source.Play();
    }

    public void StopBGM()
    {
        source.Stop();
    }

    public void PlayDamageSE()
    {
        seSource.PlayOneShot(SE_Damage);
    }


}
