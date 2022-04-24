using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Area : MonoBehaviour
{
    private int damage = 3;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Health>() != null)
        {
            Health health = other.GetComponent<Health>();
            health.Damage(damage);
        }
        Debug.Log(transform.localPosition.ToString("F2"));
    }

    public void RightMove()
    {
        transform.localPosition = new Vector3(1, 0, -1);
    }

    public void LeftMove()
    {
        transform.localPosition = new Vector3(-1, 0, -1);
    }

    public void UpMove()
    {
        transform.localPosition = new Vector3(0, 0, -2);
    }

    public void DownMove()
    {
        transform.localPosition = new Vector3(0, 0, 0);
    }

}
