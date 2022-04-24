using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dash : MonoBehaviour
{
    [SerializeField]
    private float speed = 4.0f; // ���� ���ǵ� ����
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
    private Quaternion tmpRotation; // �뽬 �� �ٽ� Rotation�� �����ϱ� ���� �ӽú���
    

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        nav.speed = speed;
        target = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾� ã��
    }

    private void Update()
    {
        if (!dash && !nav.hasPath)
        {
            nav.SetDestination(Get_Point.instance.GetRandomPoint(transform, radius)); // Get_Point���� �޾ƿ� ������ġ�� ���� Nav�̵�
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
        yield return new WaitForSeconds(2.0f); // 2�ʰ� ��¡

        tmpRotation = transform.rotation;
        transform.LookAt(target.position);
        rigid.AddForce(transform.forward * dashPower, ForceMode.Impulse); // target�Ĵٺ��� ������ �ٷ� ���� Rotation������ ����
        transform.rotation = tmpRotation;
        wallHit = Physics.SphereCast(transform.position, 0.1f, transform.forward, out hit, 0.5f, LayerMask.GetMask("Wall"));

        yield return new WaitForSeconds(0.7f); // ���� �ð� �ø��� �� ��� �ָ� ����
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
        Gizmos.DrawWireSphere(transform.position, radius); // ���� üũ
    }

#endif
}