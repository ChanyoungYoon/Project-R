using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1Loader : MonoBehaviour
{
    private RoomGenerator generator;
    private RoomManager manager;

    void Awake() {
        generator = GetComponent<RoomGenerator>();
        generator.GenerateOnce();
        Global.tileGrid = generator.tileGrid;
        manager = transform.GetComponentInChildren<RoomManager>();
    }

    void Start() {
        manager.Spawn();
    }
}
