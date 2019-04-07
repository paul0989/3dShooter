﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour {
    public int startHealth = 100;//初始血量
    public int score = 10;//擊殺enemy分數
    private int currentHealth;//目前血量
    private Animator anim;
    private bool isDead;//Enemy是否死亡

    private bool isSinking = false;
    //enemy死亡特效
    public AudioClip deadClip;
    //播放enemy被打到/打死的
    private AudioSource enemyAudio;
    private ParticleSystem hitParticle;//被攻擊時的粒子特效

    private void Awake()
    {
        anim = GetComponent<Animator>();//取得動畫
        currentHealth = startHealth;//血量設置
        enemyAudio = GetComponent<AudioSource>();
        hitParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void Death()
    {
        isDead = true;
        anim.SetTrigger("IsDead");
        enemyAudio.clip = deadClip;
        enemyAudio.Play();
        //enemy死亡後enemy不導航.移動.攻擊
        GetComponent<EnemyMovement>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<EnemyAttack>().enabled = false;
        //獲得分數
        ScoreManager.score += score;
    }

    public void TakeDamage(int amount,Vector3 postion)
    {
        if (isDead)//如果enemy死亡,return
        {
            return;
        }
        currentHealth -= amount;//當前血量-=受到傷害
        enemyAudio.Play();//被打中音效
        hitParticle.transform.position = postion;//hitParticle設定,postion=槍打到的位置
        hitParticle.Play();
        if (currentHealth<=0)//當前血量<=0
        {
            Death();//呼叫Death
        }
    }
    //從Unity的Characters>Zombunny>Animation內的Death增加event來呼叫
    public void StartSinking()
    {
        isSinking = true;
        //gameObject兩秒移除
        Destroy(gameObject,2f);
    }
    // Update is called once per frame
    void Update () {
        if (isSinking)
        {
            //屍體往下沉
            transform.Translate(Vector3.down*Time.deltaTime);
        }
	}
}