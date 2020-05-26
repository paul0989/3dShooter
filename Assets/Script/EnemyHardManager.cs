using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHardManager : MonoBehaviour {
    public GameObject enemy;
    public float delayTime = 1f;
    //生怪時間

    public float repeatRate = 3f;
    //重複時間
    
    public Transform[] spawnPoints;
    private bool playerIsDead = false;
    //生怪點陣列

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
    }

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("Spawn", delayTime, repeatRate);
        //                       延遲時間,重複執行週期
    }

}
