using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Add_Health : MonoBehaviour
{
    //private void OnEnable() { Health.isDie += this.EnemyDie; }
    //private void OnDisable() { Health.isDie -= this.EnemyDie; }
    Health health;
    private void Start()
    {
        health = this.GetComponent<Health>();
    }
    private void Update()
    {
        if(health.isDie())
        {
            EnemyDie();
        }
    }
    private void EnemyDie()
    {
        Destroy(gameObject);
    }
}
