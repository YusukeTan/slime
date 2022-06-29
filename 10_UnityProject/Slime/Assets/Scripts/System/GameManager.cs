using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  
using UnityEngine.Networking;
using System; 

[RequireComponent(typeof(MoveSceneManager))]
[DefaultExecutionOrder(-5)]
public class GameManager : SingletonMonoBehaviour<GameManager>
{

    [HideInInspector] public string CountTime { get; private set; } //タイマー用の時間

    [SerializeField] GameObject pref = null;                     //ランキングに登録成功か失敗かの表示する文字
    [SerializeField] GameObject bossPrefab = null;
    [SerializeField] Vector3 bossSpawrnPoint = default;          //ボスのスポーン位置
    [SerializeField] Vector3 itemSpawrnPoint = default;          //クリアアイテムのスポーン位置
    [SerializeField] GameObject item;                            //クリアアイテム
    [SerializeField] GameObject clearCanvasPrefab = null;        //クリア時に表示するキャンバス
    [SerializeField] GameObject gameOverCanvasPrefab = null;     //ゲームオーバー時に表示するキャンバス

    Transform CanvasTr;             // ヒエラルキー上にある Canvas の Transform
    GameObject display;             //ランキングに登録成功か失敗かの表示
    string retryButtonName = "RetryButton";
    string titleButtonName = "TitleButton";
    Text displayText;
    MoveSceneManager moveSceneManager;
    SoundManager soundManager;
    PlayerController playerController;
    bool isClear = false;
    bool isGameOver = false;
    bool isTimerOn = false;
    int minute = 0;
    float seconds = 0f;
    float oldSeconds;       //　前のUpdateの時の秒数
    Button retryButton;
    Button titleButton;
    string sTgtURL;         //ランキングAPIのURL


    protected override void Awake()
    {
        base.Awake();

        moveSceneManager = MoveSceneManager.Instance;
        soundManager = SoundManager.Instance;
    }

    void Start()
    {

        InstantiateWhenLoadScene();
        InitGame();
    }

    private void Update()
    {
        if (isTimerOn)
        {
            seconds += Time.deltaTime;

            if (seconds >= 60f)
            {
                minute++;
                seconds = seconds - 60;
            }
            //　値が変わった時だけテキストUIを更新
            if ((int)seconds != (int)oldSeconds)
            {
                CountTime = minute.ToString("00") + ":" + ((int)seconds).ToString("00");
            }
            oldSeconds = seconds;
        }

    }

    //シーンがロードされたら呼ばれる
    public void InstantiateWhenLoadScene()
    {
        if(moveSceneManager.SceneName == "Title")
        {
                return;
        }

        if (null == CanvasTr)
        {
            CanvasTr = GameObject.Find("Canvas").transform;
        }

        StartTimer();
    }

    //ゲーム初期化メソッド
    public void InitGame()
    {
        isClear = false;
        isGameOver = false;
    }


    public void Clear()
    {
        if (isClear || isGameOver)
        {
            return;
        }

        isClear = true;
        StopTimer();
        soundManager.StopBGM();
//        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isActive = false;

        //クリア画面のキャンバスを生成
        Instantiate(clearCanvasPrefab, transform.position, Quaternion.identity);

        display = Instantiate(pref);
        display.transform.SetParent(CanvasTr, false);
        display.transform.localScale = Vector3.one;
        display.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

        displayText = display.GetComponent<Text>();

        //ランキング登録
        displayText.text = "ランキング登録中...";
        SetJsonFromWWW();

        //ボタンのコンポーネントを取得
        retryButton = GameObject.Find(retryButtonName).GetComponent<Button>();
        titleButton = GameObject.Find(titleButtonName).GetComponent<Button>();

        //ボタンに、クリックしたときの処理を登録
        retryButton.onClick.AddListener(() => retry(retryButton));  //リトライなので、今と同じシーンを再読み込み
        titleButton.onClick.AddListener(() => GoToTitle()); //タイトル画面に戻る
    }



    public void GameOver()
    {
        if (isGameOver || isClear)
        {
            return;
        }

        soundManager.StopBGM();
        StopTimer();
        isGameOver = true;
//        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isActive = false;

        //ゲームオーバー画面のキャンバスを生成
        Instantiate(gameOverCanvasPrefab, transform.position, Quaternion.identity);

        //ボタンのコンポーネントを取得
        retryButton = GameObject.Find(retryButtonName).GetComponent<Button>();
        titleButton = GameObject.Find(titleButtonName).GetComponent<Button>();

        //ボタンに、クリックしたときの処理を登録
        retryButton.onClick.AddListener(() => retry(retryButton));  //リトライなので、今と同じシーンを再読み込み
        titleButton.onClick.AddListener(() => GoToTitle());  
    }

    public void enterBoss()
    {
        Instantiate(bossPrefab, bossSpawrnPoint, Quaternion.identity);
    }

    public void dropItem()
    {
        //クリアアイテムを生成
        Instantiate(item, itemSpawrnPoint, Quaternion.identity);

    }


    void retry(Button retryButton)
    {
        retryButton.enabled = false;
        Destroy(display);
        ResetTimer();
        moveSceneManager.LoadCurrentScene();
        soundManager.PlayMainBGM();
    }
    void GoToTitle()
    {
        ResetTimer();
        Destroy(display);
        moveSceneManager.GoToTitle();
        soundManager.PlayTitleBGM();
    }

    private void CallbackWebRequestFailed()
    {
        displayText.text = "ランキング登録に失敗しました";
    }



//    private void SetJsonFromWWW()
    private void SetJsonFromWWW()
    {
        sTgtURL = "http://localhost/rankingapi/ranking/setRanking";

        string name = PlayerPrefs.GetString("NAME");
        string clearTime = CountTime;

        StartCoroutine(
            SetRanking(sTgtURL, name,clearTime,WebRequestSuccess,CallbackWebRequestFailed)
        );
        
    }

    private IEnumerator SetRanking(string url, string name, string time, Action<string> cbkSuccess = null, Action cbkFailed = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name); 
        form.AddField("time", time);

        UnityWebRequest webRequest = UnityWebRequest.Post(url, form);

        webRequest.timeout = 5;

        yield return webRequest.SendWebRequest();

        if (webRequest.error != null)
        {
            if (null != cbkFailed)
            {
                cbkFailed();
            }
        }
        else if (webRequest.isDone)
        {
//            Debug.Log($"Success:{webRequest.downloadHandler.text}");
            if (null != cbkSuccess)
            {
                cbkSuccess(webRequest.downloadHandler.text);
            }
        }
    }


    private void WebRequestSuccess(string response)
    {
        displayText.text = "ランキングに登録しました";
    }


    void StartTimer()
    {
        minute = 0;
        seconds = 0f;
        oldSeconds = 0f;

        isTimerOn = true;
    }

    void StopTimer()
    {
        isTimerOn = false;
    }

    void ResetTimer()
    {
        CountTime = "00:00";
    }
}