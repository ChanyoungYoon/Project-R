using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(350)]
public class FollowView : MonoBehaviour
{
    public Transform targetTransform;
    [Range(0,1)]
    public float lerp = 0.25f;
    [Range(0,10)]
    public float speed = 2;

    private Vector3 position;
    private int pause = 0;

    void Start() {
        if(targetTransform != null) position = targetTransform.transform.position;
    }

    void Update() {
        if(pause<=0 && targetTransform != null){
            position = Vector3.Lerp(targetTransform.position,Global.ScreenToWorldPosition(Input.mousePosition),lerp);
            position = (position-transform.position)*speed*Time.deltaTime*3;
            transform.position = transform.position + position;
        }
        pause -= 1;
    }

    public void TeleportTo(Vector3 pos){
        pause = 0;
        transform.position = pos;
    }
}
