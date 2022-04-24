using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dash : MonoBehaviour
{
    [SerializeField]
    private float speed = 4.0f; // 추적 스피드 설정
    private float radius;
    [SerializeField]
    private float dashPower = 10.0f;

    private int damage = 1;
    private Transform target;
    private NavMeshAgent nav;
    private Rigidbody rigid;

    public bool checkPlayer = false;
    public bool wallHit = false;
    private bool dash = false;
    private Quaternion tmpRotation; // 대쉬 후 다시 Rotation값 복원하기 위한 임시변수
    

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        nav.speed = speed;
        target = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 찾음
    }

    private void Update()
    {
        if (!dash && !nav.hasPath)
        {
            nav.SetDestination(Get_Point.instance.GetRandomPoint(transform, radius)); // Get_Point에서 받아온 랜덤위치를 향해 Nav이동
        }

        if (checkPlayer)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        RaycastHit hit;
        checkPlayer = false;
        
        nav.isStopped = true;
        yield return new WaitForSeconds(2.0f); // 2초간 차징

        tmpRotation = transform.rotation;
        transform.LookAt(target.position);
        rigid.AddForce(transform.forward * dashPower, ForceMode.Impulse); // target쳐다보고 돌진후 바로 원래 Rotation값으로 복원
        transform.rotation = tmpRotation;
        wallHit = Physics.SphereCast(transform.position, 0.1f, transform.forward, out hit, 0.5f, LayerMask.GetMask("Wall"));

        yield return new WaitForSeconds(0.7f); // 돌진 시간 늘리면 더 길고 멀리 돌진
        rigid.velocity = Vector3.zero;
        if (wallHit)
            yield return new WaitForSeconds(3.0f);
        nav.isStopped = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Global.EnemyAttack(other, damage);
    }

    

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius); // 범위 체크
    }

#endif
}