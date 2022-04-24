using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    public float speed = 8f;
    private Rigidbody bulletRigidbody;
    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletRigidbody.velocity = transform.forward * speed;

        Destroy(gameObject, 3.0f);
    }

    //회전값 고정 : http://devkorea.co.kr/bbs/board.php?bo_table=m03_qna&wr_id=80458
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Global.EnemyAttack(other, damage);
    }
}
