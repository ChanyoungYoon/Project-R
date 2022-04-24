using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prediction : MonoBehaviour {
    public GameObject bulletPrefab;

    public int burstSize = 1;

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
    
    public IEnumerator PredictionGun()
    {
    
        { 
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 800);

            yield return new WaitForSeconds(3.0f);
        }
    }

    public void ShootDelay()
    {
        collider.enabled = false;

        collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
            if(Global.Check_Player(other))
        {
            targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(targetPosition);
            StartCoroutine("PredictionGun",3.0f);

            Invoke("ShootDelay", 3.0f);
        }
    }

}
