using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ポムという名前の雑魚敵
public class EnemyPom : EnemyBase
{

    [SerializeField] float interval;       //近づく間隔
    [SerializeField] float speed;          //近づくスピード
    [SerializeField] float waitTime = 0; //停止状態の待ち時間

    private float timeElapsed;
    float step;             //移動距離
    Rigidbody2D rb;
    bool stop = false;

    protected override void Start()
    {
        base.Start();

        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (stop) return;

        //ターゲット(=プレイヤー)との距離がareaより近かったら
        if (IsNear())
        {
            TurnTarget();

            step = speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

            timeElapsed += Time.deltaTime;

            //経過時間がインターバル以上になったら
            if (interval <= timeElapsed)
            {
                StartCoroutine(WaitTimer(waitTime)); //waitTimeだけ停止
                timeElapsed = 0;
            }

        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        //GameOverに当たったら
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = new Vector2(0,0);
        }


    }

    protected IEnumerator WaitTimer(float waitTime)
    {
        stop = true;

        yield return new WaitForSeconds(waitTime);

        stop = false;
    }

}
