using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private float speed = 3.0f;

    public bool checkPlayer = false;

    private int hitCount = 0;
    private Transform target;
    private int damage = 5;

    NavMeshAgent nav;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.speed = speed;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        nav.SetDestination(target.position);

        if(checkPlayer) // 범위안에 플레이어가 들어올 경우
        {
            StartCoroutine(Boom());
        }
    }
    IEnumerator Boom()
    {
        nav.isStopped = true;
        yield return new WaitForSeconds(3.0f); // 3초간 폭발 차징

        Collider[] col = new Collider[10]; // 최대 10마리
        hitCount = Physics.OverlapSphereNonAlloc(transform.position, 3.0f, col); // 범위안 누구인지 체크
        for (int i = 0; i < hitCount; i++)
        {
            if (col[i].gameObject.layer == LayerMask.NameToLayer("Enemy") || col[i].gameObject.layer == LayerMask.NameToLayer("EnemyHitbox") ||
                col[i].gameObject.layer == LayerMask.NameToLayer("Player")) // 데미지는 Player, Enemy 전부 입음
            {
                Global.Attack_Damage(col[i], damage);
            }
        }

        Destroy(gameObject); // 폭발 후 사망
    }
}
