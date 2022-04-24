using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tile{
    public Tile(){
        this.occupied = false;
        this.roomObj = null;
    }
    public bool occupied;
    public GameObject roomObj;
    public GameObject minimapObj;
}

public class RoomGenerator : MonoBehaviour
{
    [HideInInspector]
    public Room spawnRoom;
    public Tile[,] tileGrid = new Tile[30,30];
    public Dictionary<GameObject,Vector2> generatedRooms = new Dictionary<GameObject,Vector2>();

    [SerializeField]
    private RoomListManager roomPreset;
    [SerializeField]
    private int gridSize = 5;
    [SerializeField]
    private int minHubDistance=2;
    [SerializeField]
    private int minRoomCount = 11;
    [SerializeField]
    private bool useFallbackRoom = false;
    private List<GameObject> roomList = new List<GameObject>();
    private List<GameObject> hubRoomList = new List<GameObject>();
    private List<GameObject> normalRoomList = new List<GameObject>();
    private List<GameObject> spawnRoomList = new List<GameObject>();
    private List<GameObject> hallwayVertical = new List<GameObject>();
    private List<GameObject> hallwayHorizontal = new List<GameObject>();
    private List<GameObject> hallwayCornerUR = new List<GameObject>();
    private List<GameObject> hallwayCornerRD = new List<GameObject>();
    private List<GameObject> hallwayCornerDL = new List<GameObject>();
    private List<GameObject> hallwayCornerLU = new List<GameObject>();
    private int twigCount = 0;
    private int hallPos;
    private List<GameObject> generatedHubs = new List<GameObject>();
    private List<GameObject> generatedMinimap = new List<GameObject>();
    private GameObject fallbackRoom;
    private GameObject minimapTile;
    private GameObject minimapParent;
    private GameObject instance;


    public System.Action initializeRooms;


    void InitializeGid(){
        for (int i = 0; i < tileGrid.GetLength(0); i++)
        {
            for (int j = 0; j < tileGrid.GetLength(1); j++)
            {
               tileGrid[i,j] = new Tile();
            }
        }
    }

    void ParseRooms(){
        Room roomInfo;
        roomList = roomPreset.rooms;
        fallbackRoom = roomPreset.fallback;
        minimapTile = roomPreset.minimapTile;
        minimapParent = new GameObject("minimap");
        for (int i = 0; i < roomList.Count; i++)
        {
            roomInfo = roomList[i].GetComponent<Room>();
            //hub parse condition
            if (roomInfo.roomContext == RoomContext.Hub){
                hubRoomList.Add(roomList[i]);
            }else if (roomInfo.roomContext == RoomContext.Normal){
                normalRoomList.Add(roomList[i]);
                hubRoomList.Add(roomList[i]);
            }else if (roomInfo.roomContext == RoomContext.Spawn){
                spawnRoomList.Add(roomList[i]);
            }else if (roomInfo.connectionType == HallwayConnectionType.Horizontal){
                hallwayHorizontal.Add(roomList[i]);
            }else if (roomInfo.connectionType == HallwayConnectionType.Vertical){
                hallwayVertical.Add(roomList[i]);
            }else if (roomInfo.connectionType == HallwayConnectionType.UR){
                hallwayCornerUR.Add(roomList[i]);
            }else if (roomInfo.connectionType == HallwayConnectionType.RD){
                hallwayCornerRD.Add(roomList[i]);
            }else if (roomInfo.connectionType == HallwayConnectionType.DL){
                hallwayCornerDL.Add(roomList[i]);
            }else if (roomInfo.connectionType == HallwayConnectionType.LU){
                hallwayCornerLU.Add(roomList[i]);
            }
        }
    }

    public void GenerateOnce(){
        InitializeGid();
        ParseRooms();
        CreateHub();
        CreateMainHallway();
        CreateBranches();
        CreateTwigs();
        ConnectDoors();
    }

    //[ContextMenu("Generate")]
    void Generate(){
        Reset();
        CreateHub();
        CreateMainHallway();
        CreateBranches();
        CreateTwigs();

        ConnectDoors();
    }

    void Reset(){
        foreach (var item in generatedRooms)
        {
            Destroy(item.Key);
        }
        for (int i = 0; i < generatedMinimap.Count; i++)
        {
            Destroy(generatedMinimap[i]);
        }
        generatedRooms.Clear();
        generatedHubs.Clear();
        generatedMinimap.Clear();
        for (int i = 0; i < tileGrid.GetLength(0); i++)
        {
            for (int j = 0; j < tileGrid.GetLength(1); j++)
            {
                tileGrid[i,j].occupied = false;
            }
        }
    }

    void CreateHub(){
        List<Vector2> hubPos = new List<Vector2>();
        Vector2 pos;
        int a = 0;
        bool addToList = false;
        //pick random positions with minimum distance
        for (int i = 0; i < 20; i++)
        {
                addToList = true;
                pos.x = Mathf.RoundToInt(Random.Range(0,gridSize-1));
                pos.y = Mathf.RoundToInt(Random.Range(0,gridSize-1));
                for (int j = 0; j < hubPos.Count; j++)
                {
                    if ((hubPos[j]-pos).sqrMagnitude < (minHubDistance*minHubDistance) || hubPos[j].y==pos.y)
                    {
                        addToList = false;
                        break;
                    }
                }
                if (addToList)
                {
                    hubPos.Add(pos);
                    a++;
                }
        }
        //create hub
        for (int i = 0; i < a; i++)
        {
            InstantiateRoom((int)hubPos[i].x,(int)hubPos[i].y,hubRoomList,true,true,true,true);
        }
    }

    void CreateMainHallway(){
        hallPos = Mathf.RoundToInt(gridSize*0.5f);
        bool lineClear = false;
        //find line clear
        for(int j=0; j< gridSize; j++){
            lineClear = true;
            hallPos += j * (int)Mathf.Pow(-1,j);
            for (int i = 0; i < gridSize; i++)
            {
                if(tileGrid[hallPos,i].occupied){
                    lineClear = false;
                    break;
                }
            }
            if (lineClear) break;
        }
        //create hallway
        int minRange=100;
        int maxRange=-100;
        //find hallway end position
        foreach (var item in generatedRooms)
        {
            if(item.Value.y<minRange) minRange = (int)item.Value.y;
            if(item.Value.y>maxRange) maxRange = (int)item.Value.y;
        }
        int shift = 0;
        int preShift = 0;
        int shiftLength = (int)Random.Range(2,5)+1;
        int shiftDir = (Random.value>0.5f)?1:-1;
        bool shiftClear;
        for (int i = 0; i < gridSize; i++)
        {
            //checks if column is clear for shift
            if (shiftLength>0 && i+shiftLength<gridSize){
                shiftClear = true;
                for (int j = 0; j < shiftLength; j++)
                {
                    if(tileGrid[hallPos+shiftDir,i+j].occupied) shiftClear = false;
                }
                if(shiftClear && Random.value > 0.5f) shift = shiftDir;
            }
            if(shift!=0) shiftLength--;
            if(shiftLength <= 0) shift = 0;
            //place corner tiles
            if (preShift!=shift){
                if(shift!=0){
                    //shift start
                    if(shift>0){
                        InstantiateRoom(hallPos+shift,i,hallwayCornerLU,true,false,false,true);
                        InstantiateRoom(hallPos,i,hallwayCornerRD,false,true,true,false);
                    }else{
                        InstantiateRoom(hallPos+shift,i,hallwayCornerUR,true,false,true,false);
                        InstantiateRoom(hallPos,i,hallwayCornerDL,false,true,false,true);
                    }
                }else{
                    //shift end
                    if(preShift>0){
                        InstantiateRoom(hallPos+preShift,i,hallwayCornerDL,false,true,false,true);
                        InstantiateRoom(hallPos,i,hallwayCornerUR,true,false,true,false);
                    }else{
                        InstantiateRoom(hallPos+preShift,i,hallwayCornerRD,false,true,true,false);
                        InstantiateRoom(hallPos,i,hallwayCornerLU,true,false,false,true);
                    }
                }
            }
            else if (i>minRange-2 && i<maxRange+2) InstantiateRoom(hallPos+shift,i,hallwayVertical,true,true,false,false);
            preShift = shift;
        }
    }

    void CreateBranches(){
        int direction;
        int currentX;
        int currentY;
        bool isOccupied = false;
        for (int i = 0; i < generatedHubs.Count; i++)
        {
            currentX = (int)generatedRooms[generatedHubs[i]].x;
            currentY = (int)generatedRooms[generatedHubs[i]].y;
            direction = currentX>hallPos?-1:1;
            if(currentX==hallPos)direction = 0;

            while(!isOccupied){
                currentX += direction;
                if(tileGrid[currentX,currentY].occupied) {isOccupied = true; break;}
                InstantiateRoom(currentX,currentY,normalRoomList,false,false,true,true);
            }
            isOccupied = false;
        }
    }

    void CreateTwigs(){
        Room room;
        int random;
        Vector2 dir = Vector2.zero;
        Vector2 currentIndex = Vector2.zero;
        if(generatedRooms.Count<minRoomCount)twigCount = minRoomCount-generatedRooms.Count;
        for (int i = 0; i < twigCount; i++)
        {
            random = 1;//(int)Random.Range(1,4);
            dir.Set(0,0);
            currentIndex.Set(0,0);
            //select tile and direction
            while(random>0){
                room = generatedRooms.ElementAt((int)Random.Range(0,(int)generatedRooms.Count)).Key.GetComponent<Room>();
                currentIndex = generatedRooms[room.gameObject];
                if(!tileGrid[(int)currentIndex.x,(int)currentIndex.y+1].occupied)random--;
                if(random==0){
                    dir.Set(0,1);
                    break; 
                }
                if(currentIndex.y>0&&!tileGrid[(int)currentIndex.x,(int)currentIndex.y-1].occupied)random--;
                if(random==0){
                    dir.Set(0,-1);
                    break; 
                }
                if(!tileGrid[(int)currentIndex.x+1,(int)currentIndex.y].occupied)random--;
                if(random==0){
                    dir.Set(1,0);
                    break;
                }
                if(currentIndex.x>0&&!tileGrid[(int)currentIndex.x-1,(int)currentIndex.y].occupied)random--;
                if(random==0){
                    dir.Set(-1,0);
                    break;
                }
            }
            InstantiateRoom((int)(currentIndex.x+dir.x),(int)(currentIndex.y+dir.y),normalRoomList,dir.y<0?true:false,dir.y>0?true:false,dir.x<0?true:false,dir.x>0?true:false);
        }
    }

    void ConnectDoors(){
        Room room;
        Door door = null;
        Room otherRoom;
        Door otherDoor;
        Vector2 coord;
        foreach (var item in generatedRooms)
        {
            room = item.Key.GetComponent<Room>();
            coord = item.Value;
            //check up down right left for rooms
            if (!tileGrid[(int)coord.x,(int)coord.y+1].occupied) room.up = false;
            if (coord.y>0 && !tileGrid[(int)coord.x,(int)coord.y-1].occupied) room.down = false;
            if (!tileGrid[(int)coord.x+1,(int)coord.y].occupied) room.right = false;
            if (coord.x>0 && !tileGrid[(int)coord.x-1,(int)coord.y].occupied) room.left = false;
            if (coord.x==0) room.left = false;
            if (coord.y==0) room.down = false;
            //sync room connections
            if (!room.up && tileGrid[(int)coord.x,(int)coord.y+1].occupied && tileGrid[(int)coord.x,(int)coord.y+1].roomObj.GetComponent<Room>().down) room.up = true;
            if (!room.down && coord.y > 0 && tileGrid[(int)coord.x,(int)coord.y-1].occupied && tileGrid[(int)coord.x,(int)coord.y-1].roomObj.GetComponent<Room>().up) room.down = true;
            if (!room.right && tileGrid[(int)coord.x+1,(int)coord.y].occupied && tileGrid[(int)coord.x+1,(int)coord.y].roomObj.GetComponent<Room>().left) room.right = true;
            if (!room.left && coord.x > 0 && tileGrid[(int)coord.x-1,(int)coord.y].occupied && tileGrid[(int)coord.x-1,(int)coord.y].roomObj.GetComponent<Room>().right) room.left = true;

            //reset door visual
            room.upDoor.SetVisual();
            room.downDoor.SetVisual();
            room.rightDoor.SetVisual();
            room.leftDoor.SetVisual();

            //link map tile
            MinimapTile _mapTile = tileGrid[(int)item.Value.x,(int)item.Value.y].minimapObj.GetComponent<MinimapTile>();
            _mapTile.LinkRight(room.right);
            _mapTile.LinkUp(room.up);
            _mapTile.LinkDown(room.down);
            _mapTile.LinkLeft(room.left);

            //connect up
            if(room.up){
                door = room.upDoor;
                otherRoom = tileGrid[(int)coord.x,(int)coord.y+1].roomObj.GetComponent<Room>();
                otherDoor = otherRoom.downDoor;
                door.isConnected = true;
                otherDoor.isConnected = true;
                door.targetDoor = otherDoor;
                otherDoor.targetDoor = door;
                door.SetVisual();
                otherDoor.SetVisual();
            }
            //connect right
            if(room.right){
                door = room.rightDoor;
                otherRoom = tileGrid[(int)coord.x+1,(int)coord.y].roomObj.GetComponent<Room>();
                otherDoor = otherRoom.leftDoor;
                door.isConnected = true;
                otherDoor.isConnected = true;
                door.targetDoor = otherDoor;
                otherDoor.targetDoor = door;
                door.SetVisual();
                otherDoor.SetVisual();
            }
        }
    }

    void InstantiateRoom(int indexX, int indexY, List<GameObject> from,bool up,bool down,bool right,bool left){
        GameObject roomObj;
        Room room;
        if(useFallbackRoom) roomObj = fallbackRoom;
        else if(from.Equals(hubRoomList) && generatedHubs.Count == 0) {roomObj = spawnRoomList[(int)Random.Range(0,spawnRoomList.Count-1)];}
        else roomObj = from[(int)Mathf.Floor(Random.Range(0,from.Count))];
        instance = Instantiate(roomObj,new Vector3(indexX*50,0,indexY*50),Quaternion.Euler(Vector3.zero));
        instance.SetActive(false);
        room = instance.GetComponent<Room>();

        if(generatedHubs.Count == 0) spawnRoom = room;

        generatedRooms.Add(instance,new Vector2(indexX,indexY));
        tileGrid[indexX,indexY].occupied = true;
        tileGrid[indexX,indexY].roomObj = instance;

        if(from.Equals(hubRoomList)) generatedHubs.Add(instance);

        room.up = up;
        room.down = down;
        room.right = right;
        room.left = left;

        room.tileCoord.Set(indexX,indexY);

        instance = Instantiate(minimapTile,new Vector3(indexX-15,0,indexY),Quaternion.Euler(Vector3.zero));
        instance.transform.SetParent(minimapParent.transform);
        tileGrid[indexX,indexY].minimapObj = instance;
        generatedMinimap.Add(instance);

        //add to action
        initializeRooms += room.Initialize;
    }
}
