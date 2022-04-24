using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


//보스 패턴4에 이용되는 스크립트


public class Smog : MonoBehaviour
{
    public GameObject bossScript;
    private int damage = 1;
    public bool isPoisoned;
    public static bool inPoisonArea;
    // Start is called before the first frame update
    void Start()
    {
        isPoisoned = GameObject.Find("Boss").GetComponent<BossScript>().isPoisoned;
    }

    void CheckingCode()
    {
        Debug.Log("(Smog)inPoisonArea = " + inPoisonArea);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            inPoisonArea = true;
            //Debug.Log("(Stay)inPoisonArea = " + inPoisonArea);
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        
        if (isPoisoned == false && other.tag == "Player")
        {
            StartCoroutine(GiveDamage(other, damage));

        }
    }

    public void OnTriggerExit(Collider other)
    {
        
        if (other.tag == "Player")
        {
            inPoisonArea = false;
            //Debug.Log("(Exit)inPoisonArea = " + inPoisonArea);
        }
    }

    IEnumerator GiveDamage(Collider collider, int damage)
    {
        isPoisoned = true;
        for(int i = 0; i < 3; i ++)
        {
            Global.EnemyAttack(collider, damage);
            yield return new WaitForSeconds(1f);
        }
        isPoisoned = false;
    }

    /*private void OnCollisionEnter(Collision collision)
    {

        Debug.Log("other.tag = " + collision.collider.tag.ToString());

    }*/
}
