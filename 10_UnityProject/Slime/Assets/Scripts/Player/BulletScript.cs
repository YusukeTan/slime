using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
	private GameObject player;
	private int speed = 10;
	public AudioSource m_DamageAudio;

	void Start()
	{
		//�I�u�W�F�N�g���擾
		player = GameObject.FindWithTag("Player");
		//rigidbody2D�R���|�[�l���g���擾
		Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
		//�����Ă�������ɒe���΂�
		rigidbody2D.velocity = new Vector2(speed * player.transform.localScale.x, rigidbody2D.velocity.y);
		//�摜�̌��������킹��
		Vector2 temp = transform.localScale;
		if (0 < player.transform.localScale.x)
		{
			temp.x = 1f;
        }
        else
        {
			temp.x = -1f;
		}
		transform.localScale = temp;
		//5�b��ɏ���
		Destroy(gameObject, 5);
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D targetRigidBody = other.GetComponent<Rigidbody2D>();
        if (!targetRigidBody) return;

        EnemyBase targetHealth = targetRigidBody.GetComponent<EnemyBase>(); //������������EnemyBase�擾
        if (!targetHealth) return;

		float damage = 1f;
        targetHealth.TakeDamage(damage); //�_���[�W����
		Destroy(gameObject); //�e��j��
    }


}
