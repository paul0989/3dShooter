using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    public GameObject enemy;
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
        Instantiate(enemy,spawnPoints[pointIndex].position,
            spawnPoints[pointIndex].rotation);
    }
    // Use this for initialization
    void Start () {
        //                       延遲時間,重複執行週期
        InvokeRepeating("Spawn",delayTime, repeatRate);
	}
	
}
