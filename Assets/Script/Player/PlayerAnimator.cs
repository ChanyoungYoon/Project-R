using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    //REQUIRED INPUT
    private Vector2 moveDir;
    //PROCESSED INPUT
    private float velocity;
    private float angle;

    private string currentState;
    private int state = 0;

    private PlayerController playerController;
    private Animator animator;
    private SpriteRenderer sprite;

    private float melee1Timer;

    private const string IDLE_R = "idle_r";
    private const string IDLE_L = "idle_r";
    private const string IDLE_U = "idle_u";
    private const string IDLE_D = "idle_d";

    private const string RUN_R = "run_r";
    private const string RUN_L = "run_r";
    private const string RUN_U = "run_u";
    private const string RUN_D = "run_d";

    private const string RUN_RU = "run_ur";
    private const string RUN_RD = "run_dr";
    private const string RUN_LU = "run_ur";
    private const string RUN_LD = "run_dr";

    private const string MELEE1_R = "melee2_r";

    private const string SLASH1 = "slash1";


    void Awake() {
        playerController = transform.parent.GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update() {
        //process input
        moveDir.x = playerController.moveDir.x;
        moveDir.y = playerController.moveDir.z;
        velocity = moveDir.magnitude;
        if(velocity > 0.1f) angle = -Mathf.Atan2(moveDir.x,-moveDir.y)*Mathf.Rad2Deg+180;

        //animate
        switch (state)
        {
            case 0:
                IdleState();
                break;
            case 1:
                RunState();
                break;
            case 2:
                Melee1State();
                break;
        }
        FlipToDirection();
        if(Input.GetKeyDown(KeyCode.H)) { 
            state = 2; 
            Global.fxManager.Play("slash2",transform.position + new Vector3(0,0,-0.01f),0,Vector3.one,false); 
            playerController.Knockback(Vector3.right,0.2f);
            }
    }

    void IdleState(){
        if(velocity > 0.1f) { state = 1; return; }

        if(angle >= 315 || angle < 45) ChangeToState(IDLE_U,false);
        else if(angle >= 45 && angle < 135 ) ChangeToState(IDLE_R,false);
        else if(angle >= 135 && angle < 225 ) ChangeToState(IDLE_D,false);
        else if(angle >= 225 && angle < 315 ) ChangeToState(IDLE_L,false);
    }

    void RunState(){
        if(velocity <= 0.1f) { state = 0; return; }

        if(angle >= 337.5f || angle < 22.5f ) ChangeToState(RUN_U,true);
        else if(angle >= 22.5f && angle < 67.5f ) ChangeToState(RUN_RU,true);
        else if(angle >= 67.5f && angle < 112.5f ) ChangeToState(RUN_R,true);
        else if(angle >= 112.5f && angle < 157.5f ) ChangeToState(RUN_RD,true);
        else if(angle >= 157.5f && angle < 202.5f ) ChangeToState(RUN_D,true);
        else if(angle >= 202.5f && angle < 247.5f ) ChangeToState(RUN_LD,true);
        else if(angle >= 247.5f && angle < 292.5f ) ChangeToState(RUN_L,true);
        else if(angle >= 292.5f && angle < 337.5f ) ChangeToState(RUN_LU,true);
    }

    void Melee1State(){
        if(!currentState.Equals(MELEE1_R)) { melee1Timer = 100; }
        else if(melee1Timer > animator.GetCurrentAnimatorStateInfo(0).length) melee1Timer = melee1Timer-100+animator.GetCurrentAnimatorStateInfo(0).length;
        if(melee1Timer<=0) { state=0; melee1Timer = animator.GetCurrentAnimatorStateInfo(0).length; IdleState(); return; }

        melee1Timer-=Time.deltaTime;
        ChangeToState(MELEE1_R,false);
    }

    void FlipToDirection(){
        if(angle < 180) sprite.flipX = false;
        else sprite.flipX = true;
    }

    void ChangeToState(string targetState,bool keepNormalizedTime){
        if(currentState==targetState) return;

        animator.Play(targetState,0,keepNormalizedTime?animator.GetCurrentAnimatorStateInfo(0).normalizedTime:0);

        currentState = targetState;
    }
}
