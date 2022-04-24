using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Track : MonoBehaviour
{
    [SerializeField]
    private float speed = 6.0f; // 추적 스피드 설정

    private Transform target;
    private int damage = 1;
    NavMeshAgent nav;
    Rigidbody rigid;
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.speed = speed;
        rigid = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 찾음
    }

    void Update()
    {
        nav.SetDestination(target.position); // 추적 시작
    }

    private void FixedUpdate()
    {
        FreezeVelocity(); // 충돌시 밀거나 돌아가는 현상 방지
    }
    void FreezeVelocity()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        Global.EnemyAttack(other, damage);
    }
}
