using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasionUp : MonoBehaviour
{
    [SerializeField] private int evasionUpRate = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (Global.Check_Player(other))
        {
            Debug.Log("evasion up");
            Global.evasionRate += evasionUpRate;
            if(Global.evasionRate >= 50)
            {
                Global.evasionRate = 50;
            }
            Destroy(gameObject);
        }
    }
}
