using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour {
    public int startHealth = 100;
    //初始血量

    public int score = 10;
    //擊殺enemy分數

    private int currentHealth;
    //目前血量

    private Animator anim;
    private bool isDead;
    //Enemy是否死亡

    //物件池
    private bool isSinking = false;
    private float sinkingDoneTime = 0f;
    //手動計算的死亡時間
    private EnemyManager enemyManager;
    //物件所屬的管理器

    //private bool isSinking = false;
    
    public AudioClip deadClip;
    //enemy死亡特效    
    private AudioSource enemyAudio;
    //播放enemy被打到/打死的
    private ParticleSystem hitParticle;
    //被攻擊時的粒子特效

    private void Awake()
    {
        anim = GetComponent<Animator>();
        //取得動畫
        currentHealth = startHealth;
        //血量設置
        enemyAudio = GetComponent<AudioSource>();
        hitParticle = GetComponentInChildren<ParticleSystem>();
    }

    
    public void Alive(EnemyManager iEnemyManager)
    //讓死掉的enemy復活
    {
        enemyManager = iEnemyManager;
        
        isDead = false;
        isSinking = false;
        currentHealth = startHealth;
        GetComponent<EnemyMovement>().enabled = true;
        GetComponent<NavMeshAgent>().enabled = true;
        GetComponent<EnemyAttack>().enabled = true;
        //重置Enemy狀態
    }

    private void Death()
    {
        isDead = true;
        anim.SetTrigger("IsDead");
        enemyAudio.clip = deadClip;
        enemyAudio.Play();        
        GetComponent<EnemyMovement>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<EnemyAttack>().enabled = false;
        //enemy死亡後enemy不導航.移動.攻擊        
        ScoreManager.score += score;
        //獲得分數
    }

    public void TakeDamage(int amount,Vector3 postion)
    {
        if (isDead)
        //如果enemy死亡,return
        {
            return;
        }
        currentHealth -= amount;
        //當前血量-=受到傷害
        enemyAudio.Play();
        //被打中音效
        hitParticle.transform.position = postion;
        //hitParticle設定,postion=槍打到的位置
        hitParticle.Play();
        if (currentHealth<=0)
        //當前血量<=0
        {
            Death();
            //呼叫Death
        }
    }
    
    public void StartSinking()
    //從Unity的Characters>Zombunny>Animation內的Death增加event來呼叫
    {
        isSinking = true;
        //Destroy(gameObject,2f);    
        //gameObject兩秒移除
        sinkingDoneTime = Time.time + 2f;
        //手動計時當前時間+2秒
    }

    // Update is called once per frame
    void Update ()
    {
        //Debug.Log(this.gameObject.name + " - " + enemyManager);
        if (isSinking)
        {            
            transform.Translate(Vector3.down*Time.deltaTime);
            //屍體往下沉
        }
        if (isSinking && Query_IsSinkingDone())
        //怪物死亡及已過兩秒
        {            
            enemyManager.HandleEnemyDeath(this.gameObject);
            //告知管理器這隻enemy回池條件達成
        }
    }
    private bool Query_IsSinkingDone()
    {
        return Time.time >= sinkingDoneTime;
        //當前時間>=怪物死亡時間+兩秒
    }
}
