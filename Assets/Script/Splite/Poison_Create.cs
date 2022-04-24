using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison_Create : MonoBehaviour
{
    public GameObject poisonField;

    private float spawnRate = 5.0f; // 생성 주기
    private float timeAfterSpawn =0.0f; // 최근 생성 시점에서 지난 시간

    void Update()
    {
        // timeAfterSpawn 갱신
        timeAfterSpawn += Time.deltaTime;

        // 최근 생성 시점에서부터 누적된 시간이 생성 주기보다 크거나 같다면
        if (timeAfterSpawn >= spawnRate)
        {
            // 누적된 시간을 리셋
            timeAfterSpawn = 0f;

            Instantiate(poisonField, transform.position, transform.rotation);
        }
    }
}
