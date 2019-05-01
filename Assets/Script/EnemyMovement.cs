using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {
    private Transform player;//跟隨的目標
    private NavMeshAgent nav;
    private Animator animator;
    // Use this for initialization
    private void Awake()
    {
        //找到Tag(Palyer)
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        //get Enemy裡的animator
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        //如果animator裡的第0層的state狀態名稱為"Move"
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Move"))
        {
            nav.destination = player.position;
        }
        
	}
}
