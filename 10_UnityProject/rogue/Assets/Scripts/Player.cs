using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;


public class Player : MovingObject
{
    public float restartLevelDelay = 1f;            //レベルを再始動するまでの秒単位の遅延時間。(ステージのこと)
    public int pointsPerFood = 10;                  //フードオブジェクトを拾うときにプレーヤーのフードポイントに追加するポイントの数。
    public int enemyDamage = 1;                     //プレイヤーが壁を割ったときに壁に与えるダメージ。
    public Text foodText;
    //1 of 2 Audio clips to play when player moves.
    public AudioClip moveSound1;
    //2 of 2 Audio clips to play when player moves.
    public AudioClip moveSound2;

    private DialogueCanvasController dialogueCanvasController;
//    [SerializeField] private DialogueCanvasController dialogueCanvasController;
    private Animator animator;                    //プレーヤーのアニメーターコンポーネントへの参照を格納するために使用されます。
    private AnimatorStateInfo stateInfo;
    private bool isExecute;
    private int food;                            //レベル中(このステージ中)にプレイヤーのフードポイントの合計を保存するために使用されます。
    float oldPosition;                           //移動前の座標の合計値


    //MovingObjectのStart関数をオーバーライドします
    protected override void Start()
    {
        //プレーヤーのアニメーターコンポーネントへのコンポーネント参照を取得する
        animator = GetComponent<Animator>();
        
        dialogueCanvasController = DialogueCanvasController.instance;


        //レベル間のGameManager.instanceに保存されている現在のフードポイントの合計を取得します。
        food = GameManager.instance.playerFoodPoints;

        foodText.text = "Food:" + food;

        oldPosition = transform.position.x + transform.position.y;

        //MovingObject基本クラスのStart関数を呼び出します。
        base.Start();
    }


    //この関数は、動作が無効または非アクティブになったときに呼び出されます。（エリア移動の時に呼び出される）
    private void OnDisable()
    {
        //Playerオブジェクトが無効になっている場合は、現在のローカルフードの合計をGameManagerに保存して、次のレベルで再ロードできるようにします。
        GameManager.instance.playerFoodPoints = food;
    }


    private void Update()
    {
        //プレイヤーの番でない場合、関数を終了します。
        if (!GameManager.instance.playersTurn)
        {
            return;
        }

        //ダイアログに表示されてる場合、終了
        if (dialogueCanvasController.dialogText.text != "")
        {
            return;
        }

        int horizontal = 0;      //水平移動方向を格納するために使用されます
        int vertical = 0;        //垂直移動方向を格納するために使用されます。

        if (animator)
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }


        //入力マネージャーから入力を取得し、整数に丸め、水平に保存してx軸の移動方向を設定します
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));

        //入力マネージャーから入力を取得し、整数に丸め、垂直に保存してy軸の移動方向を設定します
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        //水平に移動するかどうかを確認し、移動する場合は垂直にゼロに設定します。(ズレ防止)
        if (horizontal != 0)
        {
            vertical = 0;
        }
        //水平または垂直にゼロ以外の値があるかどうかを確認します
        if (horizontal != 0 || vertical != 0)
        {
            if (!isMoving)
            {
                //ジェネリックパラメーターWallを渡してAttemptMoveを呼び出します。
                //これは、プレイヤーが壁に遭遇した場合プレイヤーがwallクラスを操作するためです。
                //プレーヤーを移動する方向を指定するパラメーターとして、水平方向と垂直方向に渡します。
//                AttemptMove<Wall>(horizontal, vertical);
                AttemptMove<Enemy>(horizontal, vertical);
            }
        }
    }

    // AttemptMoveは、基本クラスMovingObjectのAttemptMove関数をオーバーライドします
    // AttemptMoveはジェネリックパラメーターTを受け取ります。これは、プレーヤーの場合はWallタイプであり、xおよびy direcの整数も受け取ります
//    protected override void AttemptMove<T>(int xDir, int yDir)
    protected override bool AttemptMove<T>(int xDir, int yDir)
    {
        //プレイヤーが移動するたびに、フードポイントの合計から減算します。
        food--;
        foodText.text = "Food:" + food;

        //基本クラスのAttemptMoveメソッドを呼び出し、コンポーネントT（この場合はWall）と移動するxおよびy方向を渡します。
        bool canMove = base.AttemptMove<T>(xDir, yDir);

        //ヒットにより、Moveで行われたLinecastの結果を参照できます。
        RaycastHit2D hit;

        //If Move returns true, meaning Player was able to move into an empty space.
        //        if (Move(xDir, yDir, out hit))


        /*
        if (Move(xDir, yDir, out hit))
        {
                //Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
                SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
            Debug.Log("移動完了");
        }
        */


        //プレイヤーが移動してフードポイントを失ったので、ゲームが終了したかどうかを確認します。
        CheckIfGameOver();

        //        StartCoroutine(WaitMove());
        //プレーヤーのターンが終わったので、GameManagerのplayersTurnブール値をfalseに設定します。
        if (!isMoving)
        {
            GameManager.instance.playersTurn = false;
        }
        else
        {
            if (canMove)
                //                        StartCoroutine(WaitMove());
                GameManager.instance.playersTurn = false;

        }

        return canMove;
    }


    IEnumerator WaitMove()
    {

        //別案：whileはカッコの中身がtrueの間ループを続ける
        while (GameManager.instance.playersTurn)
        {
            if (oldPosition != Mathf.Floor(transform.position.x) + Mathf.Floor(transform.position.y))
            {
                if (isInt(transform.position.x) && isInt(transform.position.y))
                {
                    oldPosition = transform.position.x + transform.position.y;
                    GameManager.instance.playersTurn = false;
                    yield return null;
                }
            }
            yield return null;
        }
    }

    bool isInt(float f)
    {
        if (f == (int)f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //OnCantMoveは、MovingObjectの抽象関数OnCantMoveをオーバーライドします。
    //これは、プレーヤーの場合はプレーヤーが攻撃して破壊できる壁である一般的なパラメーターTを取ります。
    protected override void OnCantMove<T>(T component)
    {
        //hitWallを、パラメーターとして渡されたコンポーネントと等しくなるように設定します。
//        Wall hitWall = component as Wall;
        Enemy hitEnemy = component as Enemy;

        //攻撃している壁のDamageWall関数を呼び出します。
        hitEnemy.DamageEnemy(enemyDamage);

        //プレーヤーの攻撃アニメーションを再生するには、プレーヤーのアニメーションコントローラーの攻撃トリガーを設定します。
        animator.SetTrigger("chop");

        GameManager.instance.playersTurn = false;
    }

    //Playerとトリガーがぶつかった時
    //OnTriggerEnter2Dは、トリガー設定したオブジェクトとぶつかると呼び出される
    private void OnTriggerEnter2D(Collider2D other)
    {
        //衝突したトリガーのタグがExitであるか確認してください。
        if (other.tag == "Exit")
        {
            Destroy(other.GetComponent<GameObject>());
            //1秒後に次のレベル（ステージ）を開始するために、Restart関数を呼び出します。
            Invoke("Restart", restartLevelDelay);

            //レベルが終わったので、プレーヤーオブジェクトを無効にします。
            enabled = false;
        }

        //衝突したトリガーのタグがFoodであるか確認してください。
        else if (other.tag == "Food")
        {
            //プレイヤーの現在のフードの合計にpointsPerFoodを追加します。
            food += pointsPerFood;

            //食べ物を拾った時に加算して表示
            foodText.text = "+" + pointsPerFood + "Food:" + food;

            //アイテム取得後、非表示
            other.gameObject.SetActive(false);
        }
    }

    //Restartは呼び出されたときにシーンをリロードします。
    private void Restart()
    {

        //最後にロードされたシーンをロードします。この場合はMain、ゲーム内の唯一のシーンです。 そして、それを「シングル」モードでロードして、既存のものを置き換えます
        //現在のシーンのすべてのシーンオブジェクトをロードしません。
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);

    }


    //LoseFoodは、敵がプレイヤーを攻撃したときに呼び出されます。
    //失うポイントの数を指定するパラメーター損失をとります。
    public void LoseFood(int loss)
    {
        //プレーヤーアニメーターのトリガーを設定して、playerHitアニメーションに遷移します。
        animator.SetTrigger("hit");

        //プレイヤーの合計から失われたフードポイントを差し引きます。
        food -= loss;

        dialogueCanvasController.ActivateCanvasWithText(loss.ToString()+"のダメージを受けた");
        dialogueCanvasController.DeactivateCanvasWithDelay(1.0f);

        foodText.text = "-" + loss + "Food:" + food;

        //ゲームが終了したかどうかを確認します
        CheckIfGameOver();
    }


    //CheckIfGameOverは、プレーヤーがフードポイントを超えているかどうかをチェックし、足りない場合はゲームを終了します。
    private void CheckIfGameOver()
    {
        //フードポイントの残りが0より低い、または同じ場合
        if (food <= 0)
        {
            //GameManagerのGameOver関数を呼び出します。
            GameManager.instance.GameOver();
        }
    }



}