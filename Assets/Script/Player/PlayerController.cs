using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    [HideInInspector]
    public Vector3 inputDir;
    [HideInInspector]
    public Vector3 moveDir = Vector3.zero;


    [Header("Movement")]
    public float maxSpeed;
    public float accelRate; // in seconds
    [Header("Dash")]
    public float dashDistance;
    public float speed;


    private Vector3 position;
    private Vector3 dirToMouse;
    private Coroutine dashCoroutine;
    private Vector3 worldmousePosition;
    

    void Awake() {
        controller = GetComponent<CharacterController>();
        Global.player = this.gameObject;
        Global.canMove = true;
        Global.playerController = this;
    }

    void Update() 
    {
        inputDir = Vector3.zero;
        if(Global.canMove) {
            inputDir = MovementInput();
            MoveCharacter(inputDir);
        }
        

        dirToMouse = (Global.ScreenToWorldPosition(Input.mousePosition)-transform.position).normalized;
        if(Input.GetKeyDown(KeyCode.Space)) Dash(dirToMouse,dashDistance,speed);

        if(Input.GetKeyDown(KeyCode.T)) Global.roomManager.currentRoom.SetDoorState(true);
    }


    private void MoveCharacter(Vector3 input)
    {
        //accelerate
        moveDir = Vector3.MoveTowards(moveDir, input, (1 / accelRate) * Time.deltaTime);
        //move
        controller.Move(moveDir * Time.deltaTime * maxSpeed);
        //debug
        /*
        position = transform.position;
        position.y += 1;
        Debug.DrawRay(position, inputDir * 3, Color.red);
        position.y += 1;
        Debug.DrawRay(position, moveDir * 3, Color.cyan); 
        */  
    }

    private Vector3 MovementInput(){
        Vector3 input = Vector3.zero;
        //checker
        if(!Global.canMove) return Vector3.zero;

        //wasd
        input.x += Input.GetKey(KeyCode.D) ? 1 : 0;
        input.x += Input.GetKey(KeyCode.A) ? -1 : 0;
        input.z += Input.GetKey(KeyCode.W) ? 1 : 0;
        input.z += Input.GetKey(KeyCode.S) ? -1 : 0;

        input.Normalize();
        return input;
    }

    private void Dash(Vector3 dir,float distance,float speed){
        if(dashCoroutine!=null) return;
        dashCoroutine = StartCoroutine(DashCoroutine(dir,distance,speed));
    }

    private IEnumerator DashCoroutine(Vector3 dir,float distance,float speed){
        //check for distance
        RaycastHit hit;
        int mask = LayerMask.GetMask(new string[2] {"Wall","HalfWall"});
        bool collision = Physics.SphereCast(transform.position,0.4f,dir,out hit,distance,mask);
        if(collision&&hit.distance < 0.5f) yield break;
        collision = Physics.SphereCast(transform.position,0.51f,dir,out hit,distance,mask);
        
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + dir.normalized*(collision?hit.distance:distance);
        Global.canMove = false;
        float t=0;
        float startDist = Vector3.SqrMagnitude(endPos-transform.position);
        while(t<1){
            transform.position = Vector3.MoveTowards(transform.position,endPos,speed*Time.deltaTime);
            yield return null;
            t = 1-(Vector3.SqrMagnitude(endPos-transform.position)/startDist);
        }
        Global.ScreenShake(0.05f,4,0.3f);
        Global.canMove = true;
        dashCoroutine = null;
    }

    public IEnumerator MoveDirection(Vector3 dir,float duration){
        Global.canMove = false;
        dir.Normalize();
        while(duration>0){
            MoveCharacter(dir);
            yield return null;
            duration -= Time.deltaTime;
        }
        Global.canMove = true;
    }

    public void Knockback(Vector3 dir, float force){
        StartCoroutine(KnockbackCoroutine(dir,force));
    }

    private IEnumerator KnockbackCoroutine(Vector3 dir, float force){
        float scale = 1;
        Global.canMove = false;
        dir.Normalize();
        while(scale>0.4f){
            controller.Move(dir*scale*scale*scale*0.02f*force);
            scale -= Time.deltaTime;
            yield return null;
        }
        Global.canMove = true;
    }
}
