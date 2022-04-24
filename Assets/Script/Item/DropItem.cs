using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public GameObject reinforcePrefab;
    public bool isDie = false;
    Health health;
    //private void OnEnable() { Health.isDie += this.EnemyDie; }
    //private void OnDisable() { Health.isDie -= this.EnemyDie; }
    private void Start()
    {
        health = this.GetComponent<Health>();
    }
    private void Update()
    {
        if (health.isDie())
        {
            EnemyDie();
        }
    }
    private void EnemyDie()
    {
        Drop();
    }
    public void Drop()
    {
        Instantiate(reinforcePrefab, transform.position, transform.rotation);
    }
}
