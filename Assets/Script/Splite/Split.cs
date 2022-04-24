using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Split : MonoBehaviour
{
    [SerializeField]
    private float speed = 6.0f;

    private Transform target;
    public GameObject splitEnemy1;
    public GameObject splitEnemy2;
    private Vector3 sommonPosition1;
    private Vector3 sommonPosition2;
    private int damage = 1;
    private bool sommon = false;

    Health health;
    NavMeshAgent nav;
    Rigidbody rigid;

    //private void OnEnable() { Health.isDie += this.EnemyDie; }
    //private void OnDisable() { Health.isDie -= this.EnemyDie; }
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.speed = speed;
        rigid = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        health = this.GetComponent<Health>();
    }
    void Update()
    {
        nav.SetDestination(target.position);
        if (health.isDie())
        {
            EnemyDie();
        }
    }

    private void FixedUpdate()
    {
        FreezeVelocity();
    }
    void FreezeVelocity()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    private void SommonSplitEnemy()
    {
        sommonPosition1 = transform.position + new Vector3(1, 0, 0);
        sommonPosition2 = transform.position + new Vector3(-1, 0, 0);
        if (!sommon)
        {
            Instantiate(splitEnemy1, sommonPosition1, transform.rotation);
            Instantiate(splitEnemy2, sommonPosition2, transform.rotation);
            sommon = true;
        }
    }

    private void EnemyDie()
    {
        SommonSplitEnemy();
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Global.EnemyAttack(other, damage);
    }
}
