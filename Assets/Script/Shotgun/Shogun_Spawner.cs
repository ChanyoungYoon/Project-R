using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shogun_Spawner : MonoBehaviour
{
    public GameObject bulletPrefab; 
    public GameObject bulletPrefab1;
    public GameObject bulletPrefab2;

    private Transform player;
    private Vector3 targetPosition;
    SphereCollider collider;

    private float bulletDelay = 3.0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        collider = GetComponent<SphereCollider>();
    }

    void Update()
    {
        
    }
    public IEnumerator Shotgun()
    {
        Quaternion leftRotation = Quaternion.Euler(new Vector3(0, 20, 0));
        Quaternion rightRotation = Quaternion.Euler(new Vector3(0, -20, 0));
        Quaternion bullet1Rotation1 = transform.rotation * leftRotation;
        Quaternion bullet1Rotation2 = transform.rotation * rightRotation;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        GameObject bullet1 = Instantiate(bulletPrefab1, transform.position, bullet1Rotation1);
        GameObject bullet2 = Instantiate(bulletPrefab2, transform.position, bullet1Rotation2);
        

        yield return new WaitForSeconds(bulletDelay);
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
            StartCoroutine("Shotgun", 3.0f);

            Invoke("ShootDelay", 3.0f);
        }
    }

}
