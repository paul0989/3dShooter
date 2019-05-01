using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

    public readonly float AwakeTime = 5f;

    // Static Manager
    public static HealthManager _Instance;

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

    // 補血方法 給膠囊呼叫
    public void HealthObjectCostEnergy(GameObject healthObject)
    {
        // 取得 傳入物件 名稱 的數字型別
        int objectNum = System.Convert.ToInt32(healthObject.name);

        //int objectNum = System.Convert.ToInt32( healthObject.name );
        healthObjects[objectNum].gameObject.SetActive(false);
        nextAwakeTime[objectNum] = Time.time + AwakeTime;
    }

    // 檢查時間
    void Update()
    {
        for (int i = 0; i < nextAwakeTime.Length; i++)
        {
            // 跳過已經是啟動狀態的物件
            if (healthObjects[i].gameObject.activeSelf)
            {
                continue;
            }

            // 判斷時間
            if (Time.time >= nextAwakeTime[i])
            {
                healthObjects[i].gameObject.SetActive(true);
            }
        }
    }
}
