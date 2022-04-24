using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison_Create : MonoBehaviour
{
    public GameObject poisonField;

    private float spawnRate = 5.0f; // ���� �ֱ�
    private float timeAfterSpawn =0.0f; // �ֱ� ���� �������� ���� �ð�

    void Update()
    {
        // timeAfterSpawn ����
        timeAfterSpawn += Time.deltaTime;

        // �ֱ� ���� ������������ ������ �ð��� ���� �ֱ⺸�� ũ�ų� ���ٸ�
        if (timeAfterSpawn >= spawnRate)
        {
            // ������ �ð��� ����
            timeAfterSpawn = 0f;

            Instantiate(poisonField, transform.position, transform.rotation);
        }
    }
}
