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
        GameObject SplitedEnemy = Instantiate(splitPrefab, transform.position + Vector3(1, 0, 0) , transform.rotation);  //��ġ�� �ٲٷ��� ��� ����? || vector3 �ڷ����� ���� ����, �����ֱ�
        // GameObject bullet = Instantiate(splitPrefab, transform.position + Vector3(0, 0, 1) , transform.rotation);
        // GameObject bullet = Instantiate(splitPrefab, transform.position + Vector3(1, 0, 1) , transform.rotation);
    }
    // https://sylvester127.tistory.com/2 ������ �ִ� ���
    // https://m.blog.naver.com/PostView.naver?isHttpsRedirect=true&blogId=yoohee2018&logNo=220691949344 vector3�� ��ġ ����


}
