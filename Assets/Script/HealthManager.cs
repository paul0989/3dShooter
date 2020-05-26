using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

    public readonly float AwakeTime = 5f;    
    public static HealthManager _Instance;
    // Static Manager

    // 容器
    public Health[] healthObjects;
    public float[] nextAwakeTime;

    private void Awake()
    {
        // 省略初始化
        //healthObjects = new Health[ 3 ];
        _Instance = this;

        // 讓記錄空間和 healthObjects 大小一致
        nextAwakeTime = new float[healthObjects.Length];

        // 初始化物件名稱
        for (int i = 0; i < healthObjects.Length; i++)
        {
            healthObjects[i].transform.name = i.ToString();
        }
    }
        
    public void HealthObjectCostEnergy(GameObject healthObject)
    // 補血方法 給膠囊呼叫
    {        
        int objectNum = System.Convert.ToInt32(healthObject.name);
        // 取得 傳入物件 名稱 的數字型別

        //int objectNum = System.Convert.ToInt32( healthObject.name );
        healthObjects[objectNum].gameObject.SetActive(false);
        nextAwakeTime[objectNum] = Time.time + AwakeTime;
    }

    // 檢查時間
    void Update()
    {
        for (int i = 0; i < nextAwakeTime.Length; i++)
        {            
            if (healthObjects[i].gameObject.activeSelf)
            // 跳過已經是啟動狀態的物件
            {
                continue;
            }

            
            if (Time.time >= nextAwakeTime[i])
            // 判斷時間
            {
                healthObjects[i].gameObject.SetActive(true);
            }
        }
    }
}
