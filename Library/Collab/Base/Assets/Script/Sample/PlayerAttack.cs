using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Vector3 attackPosition;
    private int hitCount = 0;
    private bool attacking;

    private float timeToAttack = 0.25f;
    private float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {

            Attack();
        }

        if(attacking)
        {
            timer += Time.deltaTime;

            if(timer >= timeToAttack)
            {
                timer = 0;
                attacking = false;
                
            }
        }
        
    }

    public void Attack()
    {
        Collider[] col = new Collider[3];
        attackPosition = (Global.ScreenToWorldPosition(Input.mousePosition) - transform.position).normalized;

        if ((Global.ScreenToWorldPosition(Input.mousePosition) - transform.position).x < 0)
        {
            attackPosition.x = -attackPosition.x;
        }
        if ((Global.ScreenToWorldPosition(Input.mousePosition) - transform.position).z < 0)
        {
            attackPosition.z = -attackPosition.z;
        }

        attackPosition = new Vector3(attackPosition.x + transform.position.x, transform.position.y, attackPosition.z + transform.position.z);
        Debug.Log(attackPosition.ToString("F2"));
        Gizmos.DrawSphere(attackPosition, 1.5f);
        hitCount = Physics.OverlapSphereNonAlloc(attackPosition, 1.5f, col);
        
        attacking = true;
        Debug.Log(hitCount);

        for (int i = 0; i < hitCount; i++)
        {
            print(col[i].gameObject.name);
        }
    }
    private void OnDrawGizmos()
    {
        attackPosition = (Global.ScreenToWorldPosition(Input.mousePosition) - transform.position).normalized;

        if ((Global.ScreenToWorldPosition(Input.mousePosition) - transform.position).x < 0)
        {
            attackPosition.x = -attackPosition.x;
        }
        if ((Global.ScreenToWorldPosition(Input.mousePosition) - transform.position).z < 0)
        {
            attackPosition.z = -attackPosition.z;
        }


        attackPosition = new Vector3(attackPosition.x + transform.position.x, transform.position.y, attackPosition.z + transform.position.z);
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(attackPosition, 1.5f);
    }

}
