using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private RoomGenerator generator;
    private Vector2Int tileCoord;
    public Room currentRoom;
    private Coroutine teleportToRoomCoroutine;
    
    void Awake() {
        generator = transform.parent.GetComponent<RoomGenerator>();
        Global.roomManager = this;
    }

    public void Spawn(){
        currentRoom = generator.spawnRoom;
        Global.TeleportPlayerTo(currentRoom.playerSpawn.position);
        tileCoord = currentRoom.tileCoord;
        SwitchRoom(tileCoord);
    }

    public void RoomMove(Door targetDoor,string name){
        if(teleportToRoomCoroutine==null && currentRoom.isOpen) teleportToRoomCoroutine = StartCoroutine(TeleportToRoom(targetDoor,name));
    }

    private IEnumerator TeleportToRoom(Door targetDoor,string dir){
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(1f);
        TransitionType type = TransitionType.DownToUp;
        Vector3 targetPos = targetDoor.GetComponent<BoxCollider>().center+targetDoor.transform.position;
        Vector3 offsetDir = Vector3.zero;
        targetPos.y = 0;
        Vector2Int destCoord = Vector2Int.zero;

        if(dir.Equals("door_U")){
            offsetDir = Vector3.forward;
            destCoord = tileCoord+Vector2Int.up;
        }else if(dir.Equals("door_D")){
            type = TransitionType.UpToDown;
            offsetDir = Vector3.back;
            destCoord = tileCoord+Vector2Int.down;
        }else if(dir.Equals("door_R")){
            type = TransitionType.LeftToRight;
            offsetDir = Vector3.right;
            destCoord = tileCoord+Vector2Int.right;
        }else if(dir.Equals("door_L")){
            type = TransitionType.RightToLeft;
            offsetDir = Vector3.left;
            destCoord = tileCoord+Vector2Int.left;
        }
        Global.Transition(type,wait.waitTime);
        StartCoroutine(Global.playerController.MoveDirection(offsetDir,wait.waitTime));

        yield return wait;

        SwitchRoom(destCoord);
        Global.TeleportPlayerTo(targetPos - offsetDir*3f);
        StartCoroutine(Global.playerController.MoveDirection(offsetDir,wait.waitTime));

        yield return wait;
        
        teleportToRoomCoroutine = null;
    }

    private void SwitchRoom(Vector2Int dest){
        //deactivate previous room
        currentRoom.OnRoomExit();
        //locate next room
        tileCoord = dest;
        currentRoom = generator.tileGrid[dest.x,dest.y].roomObj.GetComponent<Room>();
        //activate next room
        currentRoom.OnRoomEnter();
    }
}
