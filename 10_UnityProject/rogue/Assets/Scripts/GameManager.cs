using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    //Time to wait before starting level, in seconds.
    public float levelStartDelay = 2f;
    //Delay between each Player turn.
    public float turnDelay = 0.1f;

    //Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;
    public Text levelText;
    public GameObject levelImage;


    //Store a reference to our BoardManager which will set up the level.
    private BoardManager boardScript;
    //Current level number, expressed in game as "Day  1".
    private int level = 3;
    private DialogueCanvasController dialogueCanvasController;


    //List of all Enemy units, used to issue them move commands.
    private List<Enemy> enemies;
    //Boolean to check if enemies are moving.
    private bool enemiesMoving;
    private bool doingSetup = true;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        //Get a component reference to the attached BoardManager script
        boardScript = GetComponent<BoardManager>();
        dialogueCanvasController = DialogueCanvasController.instance;

    }

    private void Start()
    {
        //Call the InitGame function to initialize the first level 
        InitGame();

    }

    /*
    //これは1回だけ呼び出され、パラメータはシーンがロードされた後にのみ呼び出されるように指示します//（そうでない場合、Scene Loadコールバックは最初のロードと呼ばれ、必要ありません）
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        //シーンが読み込まれるたびに呼び出されるコールバックを登録します
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    */
    //This is called each time a scene is loaded.

    //これは1回だけ呼び出され、パラメータはシーンがロードされた後にのみ呼び出されるように指示します//（そうでない場合、Scene Loadコールバックは最初のロードと呼ばれ、必要ありません）
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        //シーンが読み込まれるたびに呼び出されるコールバックを登録します
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    //This is called each time a scene is loaded.
    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        instance.level++;
        instance.InitGame();
    }


    /*
        void OnLevelWasLoaded(int index)
        {
            level++;
            InitGame();
        }
    */
    //Initializes the game for each level.
    void InitGame()
    {
        //doingSetupがtrueの場合、プレーヤーは移動できません。タイトルカードがアップしている間はプレーヤーが移動しないようにします。
        doingSetup = true;

        //名前で検索して、画像Levelimageへの参照を取得します。
        levelImage = GameObject.Find("LevelImage");

        //名前で検索してGetComponentを呼び出すことにより、テキストLevelTextのテキストコンポーネントへの参照を取得します。
        levelText = GameObject.Find("LevelText").GetComponent<Text>();

        dialogueCanvasController.dialogText = GameObject.Find("DialogText").GetComponent<Text>();

        //levelTextのテキストを文字列「Day」に設定し、現在のレベル番号を追加します。
        levelText.text = "Day " + level;

        //セットアップ中にlevelImageをゲームボードのアクティブなブロッキングプレーヤーのビューに設定します。
        levelImage.SetActive(true);

        //levelStartDelayを秒単位で遅延させてHideLevelImage関数を呼び出します。
        Invoke("HideLevelImage", levelStartDelay);

        //リスト内の敵オブジェクトをすべてクリアして、次のレベルに備えます。
        enemies.Clear();

        //BoardManagerスクリプトのSetupScene関数を呼び出し、現在のレベル番号を渡します。
        boardScript.SetupScene(level);
    }

    //プレイヤーがフードポイント0に到達すると、GameOverが呼び出されます
    public void GameOver()
    {
        //Set levelText to display number of levels passed and game over message
        levelText.text = "After " + level + " days, you starved.";

        //Enable black background image gameObject.
        levelImage.SetActive(true);

        //このGameManagerを無効にします。
        enabled = false;
    }

    //Coroutine to move enemies in sequence.
    IEnumerator MoveEnemies()
    {
        //While enemiesMoving is true player is unable to move.
        enemiesMoving = true;

        //Wait for turnDelay seconds, defaults to .1 (100 ms).
        yield return new WaitForSeconds(turnDelay);

        //If there are no enemies spawned (IE in first level):
        if (enemies.Count == 0)
        {
            //Wait for turnDelay seconds between moves, replaces delay caused by enemies moving when there are none.
            yield return new WaitForSeconds(turnDelay);
        }

        //Loop through List of Enemy objects.
        for (int i = 0; i < enemies.Count; i++)
        {
            //Call the MoveEnemy function of Enemy at index i in the enemies List.
            enemies[i].MoveEnemy();

            //Wait for Enemy's moveTime before moving next Enemy, 
            yield return new WaitForSeconds(enemies[i].moveTime);
        }
        //Once Enemies are done moving, set playersTurn to true so player can move.
        playersTurn = true;

        //Enemies are done moving, set enemiesMoving to false.
        enemiesMoving = false;
    }


    //レベル間で使用される黒い画像を非表示にします
    public void HideLevelImage()
    {
        //levelImage gameObjectを無効にします。
        levelImage.SetActive(false);

        //プレーヤーが再び移動できるように、doingSetupをfalseに設定します
        doingSetup = false;
    }

    //Update is called every frame.
    void Update()
    {
        //Check that playersTurn or enemiesMoving or doingSetup are not currently true.
        if (playersTurn || enemiesMoving || doingSetup)

            //If any of these are true, return and do not start MoveEnemies.
            return;

        //ダイアログに表示されてる場合、終了
        if (dialogueCanvasController.dialogText.text != "")
        {
            return;
        }

        //Start moving enemies.
        StartCoroutine(MoveEnemies());
    }


    //これを呼び出して、渡された敵を敵オブジェクトのリストに追加します。
    public void AddEnemyToList(Enemy script)
    {
        //Listにエネミーを追加する
        enemies.Add(script);
    }

    public void RemoveEnemyList(Enemy script)
    {
        enemies.Remove(script);
    }



}