using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Split : MonoBehaviour
{
    public GameObject splitPrefab;

    void Start()
    {
        Splited();
    }

    void Update()
    {

    }

    void Splited()
    {
        Destroy(gameObject, 5);
        GameObject SplitedEnemy = Instantiate(splitPrefab, transform.position + Vector3(1, 0, 0) , transform.rotation);  //위치를 바꾸려면 어떻게 하지? || vector3 자료형의 변수 선언, 더해주기
        // GameObject bullet = Instantiate(splitPrefab, transform.position + Vector3(0, 0, 1) , transform.rotation);
        // GameObject bullet = Instantiate(splitPrefab, transform.position + Vector3(1, 0, 1) , transform.rotation);
    }
    // https://sylvester127.tistory.com/2 딜레이 주는 방법
    // https://m.blog.naver.com/PostView.naver?isHttpsRedirect=true&blogId=yoohee2018&logNo=220691949344 vector3와 위치 관련


}
