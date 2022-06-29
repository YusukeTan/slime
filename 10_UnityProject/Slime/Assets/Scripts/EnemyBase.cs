using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float startingHealth = 100f; //開始時点の体力
    [SerializeField] protected AudioSource m_DamageAudio;
    [SerializeField] float area;                            //動き始める距離

    protected float m_CurrentHealth;
    protected bool m_Dead;
    protected SoundManager soundManager;
    protected GameObject target;
    private PlayerController playerController;

    protected virtual void Awake()
    {
        soundManager = SoundManager.Instance;
    }

    protected virtual void Start()
    {
        target = GameObject.Find("Player");
        playerController = target.GetComponent<PlayerController>();
    }

    protected virtual void Update()
    {

    }

    protected bool IsNear()
    {
        if (playerController.isActive)
        {
            return Vector2.Distance(transform.position, target.transform.position) < area;
        }
        else
        {
            return false;
        }
    }

    protected void TurnTarget()
    {
        Vector3 diff = (target.transform.position - transform.position);

        transform.rotation = Quaternion.FromToRotation(Vector3.up, diff); //ターゲットの方を向く
    }

    protected virtual void OnEnable()
    {
        m_CurrentHealth = startingHealth;
        m_Dead = false;
    }

    public virtual void TakeDamage(float amount)
    {
        m_CurrentHealth -= amount;
        //        m_DamageAudio.Play();   //ダメージを受ける音
        soundManager.PlayDamageSE();


        if (m_CurrentHealth <= 0 && !m_Dead)
        {
            OnDeath();
        }
    }


    protected virtual void OnDeath()
    {
        m_Dead = true;

        gameObject.SetActive(false);
    }
}
