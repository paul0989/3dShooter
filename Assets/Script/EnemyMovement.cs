using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {
    private Transform player;//跟隨的目標
    private NavMeshAgent nav;
    // Use this for initialization
    private void Awake()
    {
        //找到Tag(Palyer)
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update () {
        nav.destination = player.position;
	}
}
