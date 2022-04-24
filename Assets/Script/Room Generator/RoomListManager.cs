using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Room List")]
public class RoomListManager : ScriptableObject
{
    public GameObject fallback;
    public GameObject minimapTile;
    public List<GameObject> rooms = new List<GameObject>();
}
