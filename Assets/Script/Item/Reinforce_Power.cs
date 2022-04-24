using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reinforce_Power : MonoBehaviour
{
    [SerializeField] private float upDamage = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (Global.Check_Player(other))
        {
            Debug.Log("power up");
            Global.reinforcePower += upDamage;
            Destroy(gameObject);
        }
    }
}
