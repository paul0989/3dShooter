using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    public GameObject enemy;
    public int EnemyMaxNumber ;    
    private ObjectPool objectPool;
    public float delayTime = 1f;
    //生怪時間
    public float repeatRate = 3f;
    //重複時間

    //生怪點陣列
    public Transform[] spawnPoints;
    private bool playerIsDead = false;

    
    private void playerDeathAction()
    //player死亡後不生怪
    {
        playerIsDead = true;
    }

    private void OnEnable()
    {
        //PlayerHealth.PlayerDeathEvent += playerDeathAction;
        try
        {
            PlayerHealth.PlayerDeathEvent += playerDeathAction;
        }
        catch
        {
            Debug.Log("catch");
        }
    }

    private void OnDisable()
    {
        PlayerHealth.PlayerDeathEvent -= playerDeathAction;
    }

    private void Spawn()
    {
        if (playerIsDead)
        {
            CancelInvoke("Spawn");
            return;
        }
        int pointIndex = Random.Range(0,spawnPoints.Length); 
        //亂數
        
        Instantiate(enemy,spawnPoints[pointIndex].position,
          spawnPoints[pointIndex].rotation);
        //生怪改使用物件池
        /*GameObject enemyObj = objectPool.GetPoolObject();
                
        enemyObj.GetComponent<EnemyHealth>().Alive(this);
        //初始化Enemy參數
        enemyObj.transform.position = spawnPoints[pointIndex].position;
        enemyObj.transform.rotation = spawnPoints[pointIndex].rotation;
        enemyObj.gameObject.SetActive(true);*/
        //初始化Enemy生怪位置/角度
    }

    public void HandleEnemyDeath(GameObject iGameObject)
    {        
        objectPool.ReturnPoolObject(iGameObject);
        //把從enemyHeath得到的達成回池條件怪物回池
    }

    // Use this for initialization
    void Start()
    {
        objectPool = new ObjectPool(enemy);
        //初始化物件池
        InvokeRepeating("Spawn", delayTime, repeatRate);
        //                       延遲時間,重複執行週期
    }

}
