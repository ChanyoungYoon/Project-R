using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum RoomContext{
    Normal,
    Hallway,
    Hub,
    Spawn,
}

public enum HallwayConnectionType{
    Horizontal,
    Vertical,
    UR,
    RD,
    DL,
    LU,
}

public class Room : MonoBehaviour{
    public RoomContext roomContext;
    public HallwayConnectionType connectionType;

    public Door upDoor;
    public Door downDoor;
    public Door rightDoor;
    public Door leftDoor;

    [HideInInspector]
    public bool up;
    [HideInInspector]
    public bool down;
    [HideInInspector]
    public bool right;
    [HideInInspector]
    public bool left;

    public Transform playerSpawn;
    [HideInInspector]
    public Vector2Int tileCoord;

    public int enterCount = 0;
    public List<GameObject> monsters = new List<GameObject>();
    public bool isOpen = false;


    public void Initialize(){
        //spawn monsters
    }

    public void OnRoomEnter(){
        gameObject.SetActive(true);
        enterCount += 1;

        if(monsters.Count > 0){
            //close door
            SetDoorState(false);
        }
        //else SetDoorState(true);
    }

    public void OnRoomExit(){
        gameObject.SetActive(false);
    }

    public void SetDoorState(bool state){
        if(isOpen == state) return;
        if(state){
            //open door
            
        }else{
            //close door
        }
        upDoor.isOpen = state;
        downDoor.isOpen = state;
        rightDoor.isOpen = state;
        leftDoor.isOpen = state;
        isOpen = state;
    }

    #region Dev Tools
    [ContextMenu("Assign Doors By Name")]
    private void AssignDoors(){
        upDoor = transform.Find("door_U").gameObject.GetComponent<Door>();
        downDoor = transform.Find("door_D").gameObject.GetComponent<Door>();
        rightDoor = transform.Find("door_R").gameObject.GetComponent<Door>();
        leftDoor = transform.Find("door_L").gameObject.GetComponent<Door>();
    }

    [ContextMenu("Switch to Doors")]
    private void SwitchDoor(){
        upDoor.GetComponent<Door>().SwitchDoor();
        downDoor.GetComponent<Door>().SwitchDoor();
        rightDoor.GetComponent<Door>().SwitchDoor();
        leftDoor.GetComponent<Door>().SwitchDoor();
    }
    [ContextMenu("Switch to Walls")]
    private void SwitchWall(){
        upDoor.GetComponent<Door>().SwitchWall();
        downDoor.GetComponent<Door>().SwitchWall();
        rightDoor.GetComponent<Door>().SwitchWall();
        leftDoor.GetComponent<Door>().SwitchWall();
    }
    #endregion 
}
