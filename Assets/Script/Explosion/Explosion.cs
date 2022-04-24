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

        if(checkPlayer) // �����ȿ� �÷��̾ ���� ���
        {
            StartCoroutine(Boom());
        }
    }
    IEnumerator Boom()
    {
        nav.isStopped = true;
        yield return new WaitForSeconds(3.0f); // 3�ʰ� ���� ��¡

        Collider[] col = new Collider[10]; // �ִ� 10����
        hitCount = Physics.OverlapSphereNonAlloc(transform.position, 3.0f, col); // ������ �������� üũ
        for (int i = 0; i < hitCount; i++)
        {
            if (col[i].gameObject.layer == LayerMask.NameToLayer("Enemy") || col[i].gameObject.layer == LayerMask.NameToLayer("EnemyHitbox") ||
                col[i].gameObject.layer == LayerMask.NameToLayer("Player")) // �������� Player, Enemy ���� ����
            {
                Global.Attack_Damage(col[i], damage);
            }
        }

        Destroy(gameObject); // ���� �� ���
    }
}
