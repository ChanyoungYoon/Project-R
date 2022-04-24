using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//자주 쓰이는 메소드,싱글턴 레퍼런스 여기서 불러와서 쓰셈 매번 Find, GetComponent하지 말고
//정의는 Awake()에서, 불러오는건 Start()에서
static public class Global
{
    ////constant, variables
    public const float pixelsPerUnit = 16;
    static public float pixelsPerPixel;
    static public float reinforcePower;
    static public float evasionRate;
    static public bool canMove;


    ////references
    static public GameObject player;
    static public PlayerController playerController;
    //camera
    static public Camera mainCamera;
    static public Camera kinescopeCamera;
    static public GameObject virtualCameraTransform;
    //fx
    static public EffectManager fxManager;
    //room
    static public Tile[,] tileGrid;
    static public Vector2Int tileCoord;
    static public Room currentRoom;
    static public RoomManager roomManager;


    ////methods
    static public Func<Vector3,Vector3> ScreenToWorldPosition; //screen pos는 Input.mousePosition과 같은 형식. 화면 왼쪽아래가 (0,0,0), 오른쪽 위가 (Screen.width,Screen.height,0)
    static public Func<Vector3,Vector3> WorldToScreenPosition;
    static public Action<TransitionType,float> Transition;
    static public Action<float,float,float> ScreenShake; //진도, 주파수(속도), 길이 - [적당한 소형진동 : 0.05,2,0.5]
    static public void TeleportPlayerTo(Vector3 position){
        CharacterController cc = player.GetComponent<CharacterController>();
        Vector3 dPos = virtualCameraTransform.transform.position - player.transform.position;

        cc.enabled = false;
        player.transform.position = position;
        cc.enabled = true;
        
        virtualCameraTransform.GetComponent<FollowView>().TeleportTo(position + dPos);
    }
    static public void Attack_Damage(Collider col, float damage)
    {
        if (col.GetComponent<Health>() != null)
        {
            Health health = col.GetComponent<Health>();
            health.Damage(damage);
        }
    }
    static public bool Check_Player(Collider col) // OntriggerEnter 함수 사용시 other 받아오면 됩니다.
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return true;
        }
        return false;
    }

    static public void EnemyAttack(Collider col, int damage)
    {
        if (col.tag == "Player")
        {
            Health health = col.GetComponent<Health>();
            int ran = UnityEngine.Random.Range(0, 100);
            if(ran > evasionRate)
            {
                health.Damage(damage);
            }
        }
    }
}
