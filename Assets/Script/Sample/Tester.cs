using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class Tester : MonoBehaviour
{

    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)){

            Collider[] col = new Collider[6];
            int count;
            count = Physics.OverlapSphereNonAlloc(transform.position,1.5f,col);
            
            print(count);
            for (int i = 0; i < count; i++)
            {
                print(col[i].gameObject.name);
            }

        }
    }

    void Something(ref float a, float b){
        a= 20;
    }











    #region Trigger Test
    /*
    void OnCollisionEnter(Collision other) {
        print("collision");
    }

    void OnTriggerEnter(Collider other) {
        print("trigger");
    }
*/
    #endregion


    #region Navmesh Test
    /*
    public GameObject navmesh;

    public GameObject agentObj;
    NavMeshAgent agent;

    void Start() {
        agent = agentObj.GetComponent<NavMeshAgent>();
    }
    [ContextMenu("move")]
    void Move(){
        Vector3 dest;
        dest = Random.insideUnitSphere;
        dest *= 4;
        dest.y = 0;
        agent.SetDestination(dest);
    }
    */
    #endregion
    
    #region Batch Test
    /*
    public int count;
    public GameObject obj;
    public float range;
    [ContextMenu("generate")]
    void Generate(){
        GameObject instance;
        for (int i = 0; i < count; i++)
        {
            instance = Instantiate(obj,transform.position+(Vector3.right*(range/30))*(i%30)+(Vector3.forward*(range/30))*(Mathf.Floor(i/30)),Quaternion.Euler(Vector3.zero+(Vector3.right*90)));
            instance.transform.SetParent(transform);
        }
    }
    [ContextMenu("delete")]
    void Delete(){
        int j = transform.childCount;
        for (int p = 0; p < j; p++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
    [ContextMenu("batch")]
    void Batch(){
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            list.Add(transform.GetChild(i).gameObject);
        }
        StaticBatchingUtility.Combine(list.ToArray(),this.gameObject);
    }
*/
    #endregion
}
