using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBoss : EnemyBase
{

    [SerializeField] Slider slider;
    [SerializeField] Image fillImage;
    [SerializeField] Color fullHealthColor = Color.green;
    [SerializeField] Color zeroHealthColor = Color.red;
    [SerializeField] GameObject explosionPrefab;

    private GameManager gameManager;
    private AudioSource m_ExplosionAudio;
    private ParticleSystem m_ExplosionParticles;

    protected override void Awake()
    {
        base.Awake();

        m_ExplosionParticles = Instantiate(explosionPrefab).GetComponent<ParticleSystem>();
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

        m_ExplosionParticles.gameObject.SetActive(false); //爆発演出は非表示        
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }


    protected override void OnEnable()
    {
        base.OnEnable();

        SetHealthUi();
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        SetHealthUi();
    }

    //ボスのHPゲージの設定
    private void SetHealthUi()
    {
        slider.value = m_CurrentHealth;
        //HPゲージの色。0なら赤、満タンなら緑
        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, m_CurrentHealth / startingHealth);
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        gameManager.dropItem(); //クリアアイテムを落とす

        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);
        m_ExplosionParticles.Play(); //爆発演出
        m_ExplosionAudio.Play();     //爆発音   

    }
}
