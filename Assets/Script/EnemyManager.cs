using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    public GameObject enemy;
    private SimpleObjectPool objectPool;
    public float delayTime = 1f;//生怪時間
    public float repeatRate = 3f;//重複時間
    //生怪點陣列
    public Transform[] spawnPoints;
    private bool playerIsDead = false;

    //player死亡後不生怪
    private void playerDeathAction()
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
        int pointIndex = Random.Range(0,spawnPoints.Length); //亂數

        // 生怪改使用物件池
        //Instantiate(enemy,spawnPoints[pointIndex].position,
        //    spawnPoints[pointIndex].rotation);
        GameObject enemyObj = objectPool.GetPoolObject();

        // 初始化 Enemy 參數
        enemyObj.GetComponent<EnemyHealth>().Alive( this );

        // 設定生怪位置 / 角度
        enemyObj.transform.position = spawnPoints[pointIndex].position;
        enemyObj.transform.rotation = spawnPoints[pointIndex].rotation;
        enemyObj.gameObject.SetActive(true);
    }

    public void HandleEnemyDeath( GameObject iGameObject )
    {
        objectPool.ReturnPoolObject( iGameObject );
    }

    // Use this for initialization
    void Start () {
        // 初始化 物件池
        objectPool = new SimpleObjectPool( enemy );

        //                       延遲時間,重複執行週期
        InvokeRepeating("Spawn",delayTime, repeatRate);
	}
	
}
