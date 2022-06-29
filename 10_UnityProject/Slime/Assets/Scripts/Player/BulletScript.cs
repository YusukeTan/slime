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
		//オブジェクトを取得
		player = GameObject.FindWithTag("Player");
		//rigidbody2Dコンポーネントを取得
		Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
		//向いている向きに弾を飛ばす
		rigidbody2D.velocity = new Vector2(speed * player.transform.localScale.x, rigidbody2D.velocity.y);
		//画像の向きを合わせる
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
		//5秒後に消滅
		Destroy(gameObject, 5);
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D targetRigidBody = other.GetComponent<Rigidbody2D>();
        if (!targetRigidBody) return;

        EnemyBase targetHealth = targetRigidBody.GetComponent<EnemyBase>(); //当たった物のEnemyBase取得
        if (!targetHealth) return;

		float damage = 1f;
        targetHealth.TakeDamage(damage); //ダメージ処理
		Destroy(gameObject); //弾を破棄
    }


}
