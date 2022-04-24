using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Door : MonoBehaviour
{
    [HideInInspector]
    public bool isConnected = false;
    public bool isOpen = false;
    //[HideInInspector]
    public Door targetDoor;
    
    [SerializeField]
    private GameObject doorObj;
    [SerializeField]
    private GameObject wallObj;

    private Vector3 position;
    private Coroutine teleportCoroutine;

    void OnTriggerStay(Collider other) {
        if(isConnected && other.transform.tag == "Player") Teleport();
    }

    void OnDisable() {
        teleportCoroutine = null;
    }

    private void Teleport(){
        Global.roomManager.RoomMove(targetDoor,name);
    }

    public void SetVisual(){
        FindDoorWallObj();
        doorObj.SetActive(isConnected);
        wallObj.SetActive(!isConnected);
    }

    [ContextMenu("Assign Objects by name")]
    private void FindDoorWallObj(){
        doorObj = transform.Find("door").gameObject;
        wallObj = transform.Find("wall").gameObject;
    }

    #region Dev Tools
    public void SwitchDoor(){
        FindDoorWallObj();
        doorObj.SetActive(true);
        wallObj.SetActive(false);
    }
    public void SwitchWall(){
        FindDoorWallObj();
        doorObj.SetActive(false);
        wallObj.SetActive(true);
    }

    void OnDrawGizmos() {
        if(!isConnected) return;
        if(isOpen) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position,1);
    }
    #endregion
}
