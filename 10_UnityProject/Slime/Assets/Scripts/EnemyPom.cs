using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�|���Ƃ������O�̎G���G
public class EnemyPom : EnemyBase
{

    [SerializeField] float interval;       //�߂Â��Ԋu
    [SerializeField] float speed;          //�߂Â��X�s�[�h
    [SerializeField] float waitTime = 0; //��~��Ԃ̑҂�����

    private float timeElapsed;
    float step;             //�ړ�����
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

        //�^�[�Q�b�g(=�v���C���[)�Ƃ̋�����area���߂�������
        if (IsNear())
        {
            TurnTarget();

            step = speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

            timeElapsed += Time.deltaTime;

            //�o�ߎ��Ԃ��C���^�[�o���ȏ�ɂȂ�����
            if (interval <= timeElapsed)
            {
                StartCoroutine(WaitTimer(waitTime)); //waitTime������~
                timeElapsed = 0;
            }

        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        //GameOver�ɓ���������
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
