using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public struct ListWrap{
        public List<GameObject> elements;
    }
    [Header("-----SPAWN POSITIONS-----")]
    public List<ListWrap> spawnPoints = new List<ListWrap>();
    
    
    [Header("-----ENEMY GROUP-----")]
    [Header("")]
    public List<ListWrap> enemyGroups = new List<ListWrap>();
    [SerializeField]
    private int currentGroup;

    void Update() {
        if(Input.GetKeyDown(KeyCode.J)){
            SpawnEnemies();
        }
    }

    [ContextMenu("Create")]
    public void SpawnEnemies(){
        GameObject instance;
        currentGroup = Random.Range(0,enemyGroups.Count);
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if(enemyGroups[currentGroup].elements.Count<i) break;
            for (int j = 0; j < spawnPoints[i].elements.Count; j++)
            {
                instance = Instantiate(enemyGroups[currentGroup].elements[i],spawnPoints[i].elements[j].transform.position,Quaternion.Euler(Vector3.zero));
                transform.parent.GetComponent<Room>().monsters.Add(instance);
            }
        }
    }
}
