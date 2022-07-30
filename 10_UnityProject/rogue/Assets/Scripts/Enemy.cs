using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    public int playerDamage;                             //�U�����Ƀv���C���[���獷�������t�[�h�|�C���g�̗ʁB
    public int hp = 3;


    private DialogueCanvasController dialogueCanvasController;
    private Animator animator;                            //�G��Animator�R���|�[�l���g�ւ̎Q�Ƃ��i�[����Animator�^�̕ϐ��B
    private Transform target;                            //�e�^�[���Ɉړ����悤�Ƃ���ړIobject
    private bool skipMove;                                //�G���^�[�����X�L�b�v���邩�A���̃^�[�����ړ����邩�ǂ��������肷��u�[���l�B

    //Start�́A��{�N���X�̉��zStart�֐����I�[�o�[���C�h���܂��B
    protected override void Start()
    {
        // Enemy�I�u�W�F�N�g�̃��X�g�ɒǉ����āA���̓G��GameManager�̃C���X�^���X�ɓo�^���܂��B
        //����ɂ��AGameManager���ړ��R�}���h�𔭍s�ł���悤�ɂȂ�܂��B
        GameManager.instance.AddEnemyToList(this);
        dialogueCanvasController = DialogueCanvasController.instance;

        //�Y�t���ꂽAnimator�R���|�[�l���g�ւ̎Q�Ƃ��擾���ĕۑ����܂��B
        animator = GetComponent<Animator>();

        //�^�O���g�p����Player GameObject�������Atransform��ۑ����܂��B
        target = GameObject.FindGameObjectWithTag("Player").transform;

        //�X�^�[�g�֐��𒊏ۃN���X����Ă�
        base.Start();
    }


    //Enemy���^�[�����X�L�b�v���邽�߂ɕK�v�ȋ@�\���܂߂�ɂ́AMovingObject��AttemptMove�֐����I�[�o�[���C�h���܂��B
    //��{�I��AttemptMove�֐��̓���̏ڍׂɂ��ẮAMovingObject�̃R�����g���Q�Ƃ��Ă��������B
//    protected override void AttemptMove<T>(int xDir, int yDir)
    protected override bool AttemptMove<T>(int xDir, int yDir)
    {
        //skipMove��true���ǂ������m�F���Atrue�̏ꍇ��false�ɐݒ肵�āA���̃^�[�����X�L�b�v���܂��B
        if (skipMove)
        {
//            skipMove = false;
//            return false;
        }

        //MovingObject����AttemptMove�֐����Ăяo���܂��B
        base.AttemptMove<T>(xDir, yDir);

        //Enemy���ړ������̂ŁAskipMove��true�ɐݒ肵�Ď��̈ړ����X�L�b�v���܂��B
        skipMove = true;
        return true;
    }
    //MoveEnemy�͖��^�[��GameManger�ɂ���ČĂяo����A�e�G�Ƀv���C���[�Ɍ������Ĉړ�����悤�Ɏw�����܂��B
    public void MoveEnemy()
    {
        // X����Y���̈ړ������̕ϐ���錾���܂��B�����͈̔͂�-1����1�ł��B
        //�����̒l�ɂ��A��{�I�ȕ����i��A���A���A�E�j��I���ł��܂��B
        int xDir = 0;
        int yDir = 0;

        //�G�ƃv���C���[��x���̍���0��菬����=x���������ꍇ
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            //�^�[�Q�b�g�i�v���[���[�j�̈ʒu��y���W�����̓G�̈ʒu��y���W���傫���ꍇ�́Ay����1�i��Ɉړ��j��ݒ肵�܂��B �����łȂ��ꍇ�́A-1�ɐݒ肵�܂��i���Ɉړ����܂��j�B
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }
        //����ȊO�̏ꍇ
        else
        {
            //�^�[�Q�b�g��x�ʒu���G��x�ʒu���傫�����ǂ������m�F���܂��B�����ł���΁Ax������1�i�E�Ɉړ��j�ɐݒ肵�A�����łȂ����-1�i���Ɉړ��j�ɐݒ肵�܂��B
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }


        //�G�l�~�[�͈ړ����Ă��āA�v���[���[�ɑ�������\�������邽�߁AAttemptMove�֐����Ăяo���ăW�F�l���b�N�p�����[�^�[Player��n���܂��B
        AttemptMove<Player>(xDir, yDir);
    }


    // OnCantMove�́AEnemy���v���[���[����L����X�y�[�X�Ɉړ����悤�Ƃ���ƌĂяo����AMovingObject��OnCantMove�֐����I�[�o�[���C�h���܂�
    //�܂��A��������Ɨ\�z�����R���|�[�l���g�A���̏ꍇ��Player�ɓn�����߂Ɏg�p����ėp�p�����[�^�[T���󂯎��܂�
    protected override void OnCantMove<T>(T component)
    {
        //hitPlayer��錾���A���������R���|�[�l���g�Ɠ������Ȃ�悤�ɐݒ肵�܂��B
        Player hitPlayer = component as Player;

        //hitPlayer��LoseFood�֐����Ăяo���āA���Z����t�[�h�|�C���g�̗ʂł���playerDamage��n���܂��B
        hitPlayer.LoseFood(playerDamage);

        //�A�j���[�^�̍U���g���K�[��ݒ肵�āA�G�̍U���A�j���[�V�������g���K�[���܂��B
        animator.SetTrigger("EnemyAttack");

    }

    public void DamageEnemy(int loss)
    {
        //Call the RandomizeSfx function of SoundManager to play one of two chop sounds.
        //        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);

        //Set spriteRenderer to the damaged wall sprite.

        //Subtract loss from hit point total.
        hp -= loss;



        //If hit points are less than or equal to zero:
        if (hp <= 0)
        {
            //Disable the gameObject.
            gameObject.SetActive(false);
            GameManager.instance.RemoveEnemyList(this);
            dialogueCanvasController.ActivateCanvasWithText(loss.ToString() + "�̃_���[�W��^�����B�G��|����");
            dialogueCanvasController.DeactivateCanvasWithDelay(1.0f);
        }
        else
        {
            dialogueCanvasController.ActivateCanvasWithText(loss.ToString() + "�̃_���[�W��^����");
            dialogueCanvasController.DeactivateCanvasWithDelay(1.0f);
        }
    }
}