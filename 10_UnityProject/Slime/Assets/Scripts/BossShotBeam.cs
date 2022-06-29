using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShotBeam : MonoBehaviour
{
    public GameObject bossBeamPrefab;
    public float area;                  //�r�[���������n�߂鋗��
    public float shotInterval;          //�r�[�������Ԋu
    public float speed;                 //�r�[�����̂̑��x

    private float timeElapsed;
    private GameObject target; 

    void Start()
    {
        target = GameObject.Find("Player");
    }

    void Update()
    {
        //�^�[�Q�b�g(=�v���C���[)�Ƃ̋�����area���߂�������
        if (Vector2.Distance(transform.position, target.transform.position) < area)
        {
            Vector3 diff = (target.transform.position - transform.position);

            transform.rotation = Quaternion.FromToRotation(Vector3.up, diff); //�^�[�Q�b�g�̕�������

            timeElapsed += Time.deltaTime;

            //�o�ߎ��Ԃ��C���^�[�o���ȏ�ɂȂ�����
            if (shotInterval <= timeElapsed)
            {
                GameObject bossBeam = Instantiate(bossBeamPrefab, transform.position, transform.rotation);
                Rigidbody2D rb = bossBeam.GetComponent<Rigidbody2D>();
                rb.velocity = transform.up * speed; //�^�[�Q�b�g�̕��ɔ��ł���
                Destroy(bossBeam, 1.0f);
                timeElapsed = 0f;
            }
        }
    }
}
