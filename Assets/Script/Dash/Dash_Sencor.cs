using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_Sencor : MonoBehaviour
{
    private float waitTime;
    SphereCollider collider;

    void Start()
    {
        collider = GetComponent<SphereCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(Global.Check_Player(other))
        {
            collider.enabled = false;
            Dash dash = this.GetComponentInParent<Dash>();
            dash.checkPlayer = true;
            if (dash.wallHit)
                waitTime = 6.0f;
            else
                waitTime = 3.0f;
            Invoke("CoolDown", waitTime); // �뽬 ���� �� �ٷ� ��� ���ϰ� ���
        }
    }
    private void CoolDown()
    {
        collider.enabled = true;
    }
}
