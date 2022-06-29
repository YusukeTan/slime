using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{

    [SerializeField] float dashSpeed;     //ダッシュ速度
    [SerializeField] float gravity;       //重力　
    [SerializeField] float jumpSpeed;     //ジャンプする速度
    [SerializeField] float jumpHeight;    //ジャンプの高さ
    [SerializeField] GameObject bullet;
    [SerializeField] GroundCheck ground; 
    [SerializeField] AudioClip shotSound; //弾を撃つときの音
    [SerializeField] AudioClip gameoverSound;
    [SerializeField] AudioClip clearSound;
    [SerializeField] float defaultSpeed = 0;

    public bool isActive = false;

    private AudioSource audioSource;
    private GameManager gameManager;
    private float speed = 0;
    private float ySpeed = 0;
    private float direction = 1;
    private bool jump = false;          //ジャンプキーが押されている
    private bool isGrounded = false;
    private bool isDead = false;
    private bool isClear = false;
    private bool enterBoss = false;
    private bool dash = false;
    private bool isJump = false;        //ジャンプ状態
    private float jumpPos = 0.0f;       //ジャンプした位置
    private float jumpTime = 0f;        //ジャンプしている時間
    private float inputH = 0;                   //横キーの入力
    private Animator animator;
    private Rigidbody2D rigidBody2D;

    void Start()
    {
        InitCharacter();

        animator = GetComponent<Animator>();
        gameManager = GameManager.Instance;
        rigidBody2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void InitCharacter()
    {
//        Speed = defaultSpeed;
        direction = transform.localScale.x; 

        isActive = true;
    }


    void Update()
    {
        GetInput();
        //zボタンが押され、かつ死んでいなければ
        if (Input.GetKeyDown(KeyCode.Z) && !isDead)
        {
            Instantiate(bullet, transform.position, transform.rotation);
            audioSource.clip = shotSound;
            audioSource.Play();
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isDead)
        {
            if(dash)
            {
                dash = false;
            }
            else
            {
                dash = true;
            }
        }

    }

    private void OnEnable()
    {
        isDead = false;
        isClear = false;
    }

    void FixedUpdate()
    {
        Move();
    }

    void GetInput()
    {
        if (!isActive)
        {
            return;
        }

        jump = Input.GetKey("up");
        inputH = Input.GetAxisRaw("Horizontal"); //横のインプット
    }

    void Move()
    {
        if (!isActive)
        {
            return;
        }

//        float ySpeed = -gravity; //重力がかかっている
        ySpeed = -gravity; //重力がかかっている

        //接地判定
        isGrounded = ground.IsGround();

        //移動速度の計算処理
        if (inputH != 0)
        {
            direction = Mathf.Sign(inputH); //向き。右なら1、左なら-1
            speed = defaultSpeed * direction;
            Vector2 temp = transform.localScale;
            temp.x = inputH * 0.37f; //0.37fはプレイヤー画像の大きさ
            transform.localScale = temp;

            //スペースキーが押されてる場合（ダッシュ時）
//            if (Input.GetKey(KeyCode.Space))
            if (dash)
            {
                //通常スピード(横)にダッシュスピードをかける
                    speed = speed * dashSpeed;
            }

        }
        else
        {
            speed = 0;
        }

        //接地しているなら
        if (isGrounded)
        {
            if(jump) //ジャンプキーが押された
            {
                ySpeed = jumpSpeed;             //上方向にジャンプ速度
                jumpPos = transform.position.y; //ジャンプした位置を記録
                isJump = true;
                jumpTime = 0f;
            }
            else
            {
                isJump = false;
            }
        }
        //接地しておらず、ジャンプ状態なら
        else if (isJump)
        {
            //ジャンプキーを押されている。かつ、ジャンプした位置+決めたジャンプ高さより現在地が下
            //かつジャンプしている時間が1s未満ならジャンプを継続する
            if (jump && jumpPos + jumpHeight > transform.position.y && jumpTime < 1f)
            {
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;
            }
            else
            {
                isJump = false;
            }
        }

        //実際の移動処理
        rigidBody2D.velocity = new Vector2(speed, ySpeed);
    }
    


    void Dead()
    {
        if (!isDead)
        {
            isDead = true;
            audioSource.clip = gameoverSound;
            audioSource.Play();

            //アニメーションを更新
            UpdateAnimation();

            isActive = false;
            enterBoss = false;                          //ボスエリアに入ってないことにする
            rigidBody2D.velocity = new Vector2(0, 0);   //プレイヤーの動きを止める

            gameManager.GameOver();
        }
    }
    private void Clear()
    {
        isActive = false;
        isClear = true;
        rigidBody2D.velocity = new Vector2(0, 0); //プレイヤーの動きを止める

        gameManager.Clear();

        audioSource.clip = clearSound;
        audioSource.Play();

        //アニメーションを更新
        UpdateAnimation();


    }

    private void UpdateAnimation()
    {
        animator.SetBool("Die", isDead);
        animator.SetBool("Smile", isClear);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //GameOverに当たったら
        if(collision.gameObject.tag == "GameOver")
        {
            Dead();
        }
        //BossAreaに当たったら
        else if (collision.gameObject.tag == "BossArea")
        {
            if(!enterBoss)  //まだボスエリアに入っていなければ
            {
                gameManager.enterBoss();
                enterBoss = true;
            }
        }
        //Clearに当たったら
        else if (collision.gameObject.tag == "Clear")
        {
            Clear();
        }
    }

    
    void OnCollisionEnter2D(Collision2D col)
    {
        //プレイヤーが床に当たったら
        if (transform.parent == null && col.gameObject.tag == "Floor")
        {
            var emptyObject = new GameObject();
            emptyObject.transform.parent = col.gameObject.transform;
            transform.parent = emptyObject.transform; //プレイヤーの親をFloorにする。床の移動にプレイヤーがついていく
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (transform.parent != null && col.gameObject.tag == "Floor")
        {
            transform.parent = null; //床から離れたらプレイヤーの親をなくす
        }
    }


}