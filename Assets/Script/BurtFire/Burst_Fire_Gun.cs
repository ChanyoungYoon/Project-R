using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst_Fire_Gun : MonoBehaviour
{
    public GameObject bulletPrefab;

    public int burstSize = 5;
    public float bulletDelay = 0.41f;

    private Transform player;
    private Vector3 targetPosition;
    SphereCollider collider;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        collider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //���� : https://answers.unity.com/questions/1135134/how-do-you-do-burst-fire-in-c.html
    //+ https://sylvester127.tistory.com/2
    public IEnumerator FireBurst()
    {
        for (int i = 0; i < burstSize; i++)
        { 
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

            yield return new WaitForSeconds(bulletDelay);
        }
    }

    public void ShootDelay()
    {
        collider.enabled = false;

        collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("attack");
            targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(targetPosition);
            StartCoroutine("FireBurst",5.0f);
            
            Invoke("ShootDelay", 10.0f);
        }
    }
}
