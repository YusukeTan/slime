using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShotBeam : MonoBehaviour
{
    public GameObject bossBeamPrefab;
    public float area;                  //ビームを撃ち始める距離
    public float shotInterval;          //ビームを撃つ間隔
    public float speed;                 //ビーム自体の速度

    private float timeElapsed;
    private GameObject target; 

    void Start()
    {
        target = GameObject.Find("Player");
    }

    void Update()
    {
        //ターゲット(=プレイヤー)との距離がareaより近かったら
        if (Vector2.Distance(transform.position, target.transform.position) < area)
        {
            Vector3 diff = (target.transform.position - transform.position);

            transform.rotation = Quaternion.FromToRotation(Vector3.up, diff); //ターゲットの方を向く

            timeElapsed += Time.deltaTime;

            //経過時間がインターバル以上になったら
            if (shotInterval <= timeElapsed)
            {
                GameObject bossBeam = Instantiate(bossBeamPrefab, transform.position, transform.rotation);
                Rigidbody2D rb = bossBeam.GetComponent<Rigidbody2D>();
                rb.velocity = transform.up * speed; //ターゲットの方に飛んでいく
                Destroy(bossBeam, 1.0f);
                timeElapsed = 0f;
            }
        }
    }
}
