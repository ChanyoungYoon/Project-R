using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Track : MonoBehaviour
{
    [SerializeField]
    private float speed = 6.0f; // ���� ���ǵ� ����

    private Transform target;
    private int damage = 1;
    NavMeshAgent nav;
    Rigidbody rigid;
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.speed = speed;
        rigid = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾� ã��
    }

    void Update()
    {
        nav.SetDestination(target.position); // ���� ����
    }

    private void FixedUpdate()
    {
        FreezeVelocity(); // �浹�� �аų� ���ư��� ���� ����
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
