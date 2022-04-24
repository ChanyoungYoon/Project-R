using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Get_Point : MonoBehaviour
{

    public static Get_Point instance;

    public float range;

    private void Awake()
    {
        instance = this;
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result) // ����Ʈ �������� üũ
    {

        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;

        return false;
    }

    public Vector3 GetRandomPoint(Transform point = null, float radius = 0) // �����ǰ� ��ȯ
    {
        Vector3 _point;

        if (RandomPoint(point == null ? transform.position : point.position, radius == 0 ? range : radius, out _point))
        {
            Debug.DrawRay(_point, Vector3.up, Color.black, 1); // ��ȯ ������ �����

            return _point;
        }

        return point == null ? Vector3.zero : point.position;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range); // ���� üũ
    }

#endif

}