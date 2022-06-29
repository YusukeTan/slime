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

        m_ExplosionParticles.gameObject.SetActive(false); //�������o�͔�\��        
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

    //�{�X��HP�Q�[�W�̐ݒ�
    private void SetHealthUi()
    {
        slider.value = m_CurrentHealth;
        //HP�Q�[�W�̐F�B0�Ȃ�ԁA���^���Ȃ��
        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, m_CurrentHealth / startingHealth);
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        gameManager.dropItem(); //�N���A�A�C�e���𗎂Ƃ�

        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);
        m_ExplosionParticles.Play(); //�������o
        m_ExplosionAudio.Play();     //������   

    }
}
