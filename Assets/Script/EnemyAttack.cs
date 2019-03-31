using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour {
    public int attackDamage = 10;//怪物攻擊力
    private bool playerInRange;//玩家有無在可攻擊範圍內
    private PlayerHealth playerHealth;//打中時呼叫
    //攻擊的CD時間
    private float timer;
    private float timeBetweenAttacks = 0.5f;

    //當player死亡後轉為idle動作
    private Animator enemyAnimator;
    private bool playerIsDeath = false;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerHealth=player.GetComponent<PlayerHealth>();
        enemyAnimator = GetComponent<Animator>();
    }

    //Event  playerHealth.cs
    private void playerDeathAction()
    {       
        playerIsDeath = true;
        //enemy(Animator>transition)
        enemyAnimator.SetTrigger("PlayerDead");
        //player死亡後enemy不導航.移動
        GetComponent<EnemyMovement>().enabled=false;
        GetComponent<NavMeshAgent>().enabled=false;
    }

    //Event註冊.取消
    private void OnEnable()
    {
        PlayerHealth.PlayerDeathEvent += playerDeathAction;
    }
    private void OnDisable()
    {
        PlayerHealth.PlayerDeathEvent -= playerDeathAction;
    }

    private void OnTriggerEnter(Collider other)//Player在可攻擊範圍內
    {
        if (other.tag==playerHealth.tag)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)//離開這個Trigger
    {
        if (other.tag == playerHealth.tag)
        {
            playerInRange = false;
        }
    }

    private void Attack()
    {
        timer = 0;//攻擊時歸0重新計算下次攻擊時間
        playerHealth.TakeDamage(attackDamage);
    }
    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;//每個frame做deltaTime的增加
        //player在範圍內And player沒死(playerIsDeath==false)
        if (playerInRange && playerIsDeath==false)
        {
            if (timer>=timeBetweenAttacks)
            {
                Attack();
            }
            
        }
	}
}
