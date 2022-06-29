using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(-9)]

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{

    [SerializeField] AudioClip BGM_Title;
    [SerializeField] AudioClip BGM_Main;
    [SerializeField] AudioClip SE_Damage;

    //使用するAudioSource
    private AudioSource source;
    private AudioSource seSource;

    void Start()
    {
        //自分をシーン切り替え時も破棄しないようにする
//        DontDestroyOnLoad(gameObject);

        //使用するAudioSource取得
        source = GetComponent<AudioSource>();
        seSource = gameObject.AddComponent<AudioSource>();
        source.clip = BGM_Title;    //最初はタイトル
        source.Play();
    }


    public void PlayTitleBGM()
    {
        source.Stop();
        source.clip = BGM_Title;    //流すクリップを切り替える
        source.Play();
    }

    public void PlayMainBGM()
    {
        source.Stop();
        source.clip = BGM_Main;    //流すクリップを切り替える
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
