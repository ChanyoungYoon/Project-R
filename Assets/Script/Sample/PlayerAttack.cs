using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Vector3 attackPosition;
    private int hitCount = 0;
    public bool attacking;
    private float damage;
    private float final_damage;
    private int attackSequence = 1;

    private float timeToAttack = 0.25f;
    private float timer = 0f;
    private bool attackSuccess;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !attacking)
        {
            Attack();
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            Invoke("Special_Attack", 2.0f);
        }
        if(attacking)
        {
            timer += Time.deltaTime;

            if(timer > timeToAttack)
            {
                timer = 0;
                attacking = false;
            }
        }
    }

    public void Attack()
    {
        attacking = true;
        attackSuccess = false;
        damage = 3.0f;
        final_damage = damage + Global.reinforcePower;
        Collider[] col = new Collider[10];
        attackPosition = (Global.ScreenToWorldPosition(Input.mousePosition) - transform.position).normalized * 2.0f;
        attackPosition = new Vector3(attackPosition.x + transform.position.x, transform.position.y, attackPosition.z + transform.position.z);

        Vector3 size = new Vector3(2, 2, 2);
        if (attackSequence == 2)
        {
            size = new Vector3(2, 2, 4);
            final_damage *= 1.5f;
        }
        else if (attackSequence == 3)
        {
            size = new Vector3(4, 2, 4);
            final_damage *= 2.0f;
        }

        hitCount = Physics.OverlapBoxNonAlloc(attackPosition, size, col, transform.rotation);
        for (int i = 0; i < hitCount; i++)
        {
            if (col[i].gameObject.layer == LayerMask.NameToLayer("Enemy") || col[i].gameObject.layer == LayerMask.NameToLayer("EnemyHitbox"))
            {
                Global.Attack_Damage(col[i], final_damage);
                attackSuccess = true;
            }
        }
        if (attackSuccess)
        {
            //Debug.Log(final_damage);
            attackSequence++;
            if (attackSequence > 3)
            {
                attackSequence = 1;
            }
        }
        
    }


    public void Special_Attack()
    {
        damage = 10.0f;
        final_damage = damage + Global.reinforcePower;
        Collider[] col = new Collider[10];
        attackPosition = Global.ScreenToWorldPosition(Input.mousePosition);
        hitCount = Physics.OverlapSphereNonAlloc(attackPosition, 3.0f, col);
        for (int i = 0; i < hitCount; i++)
        {
            if (col[i].gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Global.Attack_Damage(col[i], damage);
            }
        }
    }


    //private void OnDrawGizmos()
    //{
    //    if (!Application.isPlaying) return;
    //    attackPosition = (Global.ScreenToWorldPosition(Input.mousePosition) - transform.position).normalized * 2.0f;


    //    attackPosition = new Vector3(attackPosition.x + transform.position.x, transform.position.y, attackPosition.z + transform.position.z);
    //    Gizmos.color = Color.black;
    //    Vector3 size = new Vector3(4, 2, 2);
    //    Gizmos.DrawCube(attackPosition, size);
    //}

}
