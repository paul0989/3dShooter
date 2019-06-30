using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 簡易物件池
public class SimpleObjectPool
{
    private GameObject sampleObject;
    private Queue objectContainer = new Queue();
    // 2 3

    public SimpleObjectPool( GameObject iPoolSampleObject )
    {
        sampleObject = iPoolSampleObject;
        Debug.LogWarning( "SimpleObjectPool 初始化 : " + iPoolSampleObject.name );
    }

    public GameObject GetPoolObject()
    {
        Debug.LogWarning( "取得物件 : " + sampleObject.name );
        
        GameObject poolObj;

        // 如果池子有物件
        if( objectContainer.Count > 0 )
        {
            poolObj = (GameObject)objectContainer.Dequeue();
        }
        // 如果沒有物件
        else
        {
            poolObj = GameObject.Instantiate( sampleObject );
        }
        
        poolObj.name = sampleObject.name + "(Active)";
        return poolObj;
    }

    public void ReturnPoolObject( GameObject iGameObject )
    {
        iGameObject.SetActive(false);
        iGameObject.name = sampleObject.name + "(Sleep)";
        objectContainer.Enqueue( iGameObject );
    }
}