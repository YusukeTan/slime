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
    //�����1�񂾂��Ăяo����A�p�����[�^�̓V�[�������[�h���ꂽ��ɂ̂݌Ăяo�����悤�Ɏw�����܂�//�i�����łȂ��ꍇ�AScene Load�R�[���o�b�N�͍ŏ��̃��[�h�ƌĂ΂�A�K�v����܂���j
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        //�V�[�����ǂݍ��܂�邽�тɌĂяo�����R�[���o�b�N��o�^���܂�
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    */
    //This is called each time a scene is loaded.

    //�����1�񂾂��Ăяo����A�p�����[�^�̓V�[�������[�h���ꂽ��ɂ̂݌Ăяo�����悤�Ɏw�����܂�//�i�����łȂ��ꍇ�AScene Load�R�[���o�b�N�͍ŏ��̃��[�h�ƌĂ΂�A�K�v����܂���j
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        //�V�[�����ǂݍ��܂�邽�тɌĂяo�����R�[���o�b�N��o�^���܂�
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
        //doingSetup��true�̏ꍇ�A�v���[���[�͈ړ��ł��܂���B�^�C�g���J�[�h���A�b�v���Ă���Ԃ̓v���[���[���ړ����Ȃ��悤�ɂ��܂��B
        doingSetup = true;

        //���O�Ō������āA�摜Levelimage�ւ̎Q�Ƃ��擾���܂��B
        levelImage = GameObject.Find("LevelImage");

        //���O�Ō�������GetComponent���Ăяo�����Ƃɂ��A�e�L�X�gLevelText�̃e�L�X�g�R���|�[�l���g�ւ̎Q�Ƃ��擾���܂��B
        levelText = GameObject.Find("LevelText").GetComponent<Text>();

        dialogueCanvasController.dialogText = GameObject.Find("DialogText").GetComponent<Text>();

        //levelText�̃e�L�X�g�𕶎���uDay�v�ɐݒ肵�A���݂̃��x���ԍ���ǉ����܂��B
        levelText.text = "Day " + level;

        //�Z�b�g�A�b�v����levelImage���Q�[���{�[�h�̃A�N�e�B�u�ȃu���b�L���O�v���[���[�̃r���[�ɐݒ肵�܂��B
        levelImage.SetActive(true);

        //levelStartDelay��b�P�ʂŒx��������HideLevelImage�֐����Ăяo���܂��B
        Invoke("HideLevelImage", levelStartDelay);

        //���X�g���̓G�I�u�W�F�N�g�����ׂăN���A���āA���̃��x���ɔ����܂��B
        enemies.Clear();

        //BoardManager�X�N���v�g��SetupScene�֐����Ăяo���A���݂̃��x���ԍ���n���܂��B
        boardScript.SetupScene(level);
    }

    //�v���C���[���t�[�h�|�C���g0�ɓ��B����ƁAGameOver���Ăяo����܂�
    public void GameOver()
    {
        //Set levelText to display number of levels passed and game over message
        levelText.text = "After " + level + " days, you starved.";

        //Enable black background image gameObject.
        levelImage.SetActive(true);

        //����GameManager�𖳌��ɂ��܂��B
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


    //���x���ԂŎg�p����鍕���摜���\���ɂ��܂�
    public void HideLevelImage()
    {
        //levelImage gameObject�𖳌��ɂ��܂��B
        levelImage.SetActive(false);

        //�v���[���[���Ăшړ��ł���悤�ɁAdoingSetup��false�ɐݒ肵�܂�
        doingSetup = false;
    }

    //Update is called every frame.
    void Update()
    {
        //Check that playersTurn or enemiesMoving or doingSetup are not currently true.
        if (playersTurn || enemiesMoving || doingSetup)

            //If any of these are true, return and do not start MoveEnemies.
            return;

        //�_�C�A���O�ɕ\������Ă�ꍇ�A�I��
        if (dialogueCanvasController.dialogText.text != "")
        {
            return;
        }

        //Start moving enemies.
        StartCoroutine(MoveEnemies());
    }


    //������Ăяo���āA�n���ꂽ�G��G�I�u�W�F�N�g�̃��X�g�ɒǉ����܂��B
    public void AddEnemyToList(Enemy script)
    {
        //List�ɃG�l�~�[��ǉ�����
        enemies.Add(script);
    }

    public void RemoveEnemyList(Enemy script)
    {
        enemies.Remove(script);
    }



}