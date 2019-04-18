using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {
    private PlayerHealth playerHealth;//補到血呼叫
    public GameObject Health;
    private bool playerEatHealth=false;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }
    private void PlayerEatHealthAction()
    {
        playerEatHealth = true;
    }
    //Event註冊.取消
    private void OnEnable()
    {
        PlayerHealth.PlayerEatHealthEvent += PlayerEatHealthAction;
    }
    private void OnDisable()
    {
        PlayerHealth.PlayerEatHealthEvent -= PlayerEatHealthAction;
    }

    private void HealthProduce()
    {
        Health.SetActive(false);
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (playerEatHealth == true)
        {
            HealthProduce();
        }
	}
}
