using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison_Field : MonoBehaviour
{
    private int damage = 1;
    private BoxCollider boxCol;
    private void Start()
    {
        boxCol = GetComponent<BoxCollider>();
        Destroy(gameObject, 3.0f);
    }
    public void PoisonDelay()
    {
        boxCol.enabled = false;

        boxCol.enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        Global.EnemyAttack(other, damage);
        Invoke("PoisonDelay", 0.8f);
    }
}
