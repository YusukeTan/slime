using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;


[DefaultExecutionOrder(-4)]
public class TitleManager : MonoBehaviour
{

    [SerializeField] Text inputName = default;
    [SerializeField] Text errorText = default;      //名前未入力時のエラーテキスト
    [SerializeField] GameObject titlePanel;
    [SerializeField] GameObject rankingPanel;
    [SerializeField] GameObject howtoPlayPanel;
    [SerializeField] GameObject rankingItemBase;    //ランキングが表示されるスクロールビューコンテンツ
    [SerializeField] GameObject rankingItem;
    [SerializeField] Text _displayField;            //「ランキング取得中」と表示するテキスト

    private List<MemberData> _memberList;
    private SoundManager soundManager;
    private MoveSceneManager moveSceneManager;

    private void Awake()
    {
        soundManager = SoundManager.Instance;
        moveSceneManager = MoveSceneManager.Instance;
    }


    //スタートボタンの処理
    void StartGame()
    {
        if ("" == inputName.text)
        {
            errorText.text = "名前を入れてからスタートしてね";
        }
        else
        {
            //入力された名前を保存
            PlayerPrefs.SetString("NAME", inputName.text);
            PlayerPrefs.Save();
            moveSceneManager.GoToMain();
            soundManager.PlayMainBGM();
        }
    }

    public void OnClickRanking()
    {
        titlePanel.SetActive(false);
        rankingPanel.SetActive(true);

        _displayField.text = "データ取得中...";
        GetJsonFromWebRequest();
    }

    public void OnClickHowtoPlay()
    {
        titlePanel.SetActive(false);
        howtoPlayPanel.SetActive(true);

    }


    void ShowMemberList()
    {
        string sStrOutput = "";

        if (null == _memberList)
        {
            sStrOutput = "no list !";
        }
        else
        {
            //リストの内容を表示
            int positionCount = 0;
            int num = 1;
            _displayField.text = "";

            foreach (MemberData memberOne in _memberList)
            {
                GameObject rankingObject = Instantiate(rankingItem, rankingItemBase.transform.position, Quaternion.identity, rankingItemBase.transform);
                rankingObject.transform.SetParent(rankingItemBase.transform);   //スクロールビューのコンテンツを親にする

                rankingObject.transform.Find("Rank").GetComponent<Text>().text = num.ToString();    //順位
                rankingObject.transform.Find("Score").GetComponent<Text>().text = memberOne.Time;   //クリア時間                
                rankingObject.transform.Find("Name").GetComponent<Text>().text = memberOne.Name;    //名前

                //2位以降は下に30ずつずらして表示するように
                Vector3 itemOffset = new Vector3(0, -30f * positionCount, 0);
                rankingObject.transform.GetComponent<RectTransform>().position += itemOffset;

                positionCount++;
                num += 1;
            }
        }

        _displayField.text = sStrOutput;
    }

    private void GetJsonFromWebRequest()
    {
        //  json データ取得をリクエストする
        StartCoroutine(
            DownloadJson(
                CallbackWebRequestSuccess, // APIコールが成功した際に呼ばれる関数
                CallbackWebRequestFailed // APIコールが失敗した際に呼ばれる関数
            )
        );
    }

    private void CallbackWebRequestSuccess(string response)
    {
        //Json の内容を MemberData型のリストとしてデコードする。
        _memberList = MemberDataModel.DeserializeFromJson(response);
        ShowMemberList();

    }

    public void OnClickReturn()
    {
        moveSceneManager.GoToTitle();
    }

    private void CallbackWebRequestFailed()
    {
        _displayField.text = "ランキング取得失敗しました";
    }

    private IEnumerator DownloadJson(Action<string> cbkSuccess = null, Action cbkFailed = null)
    {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost/rankingapi/ranking/getRanking");
        yield return www.SendWebRequest();
        if (www.error != null)
        {
            //レスポンスエラーの場合
            Debug.LogError(www.error);
            if (null != cbkFailed)
            {
                cbkFailed();
            }
        }
        else if (www.isDone)
        {
            // リクエスト成功の場合
//            Debug.Log($"Success:{www.downloadHandler.text}");
            if (null != cbkSuccess)
            {
                cbkSuccess(www.downloadHandler.text);
            }
        }
    }

}
