using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToPixel : MonoBehaviour
{
    public Transform parent;
    public bool snapPosition = true;
    public bool snapYRot = true;
    public bool snapRot;
    public float snapRotIncrement = 22.5f;

    
    private float translateDivision;
    private float rotationDivision;

    private Vector3 prevPos;
    private Vector3 prevRot;
    private Vector3 newPos;
    private Vector3 newRot;

    void Start() {
        translateDivision = 1/Global.pixelsPerUnit;
        rotationDivision = 1/snapRotIncrement;
    }

    void Update() {
        if(snapPosition && prevPos != parent.position){
            newPos.Set(Mathf.Round(parent.position.x*Global.pixelsPerUnit)*translateDivision,Mathf.Round(parent.position.y*Global.pixelsPerUnit)*translateDivision,Mathf.Round(parent.position.z*Global.pixelsPerUnit)*translateDivision);
        }
        if(snapYRot && prevRot.y != parent.rotation.eulerAngles.y){
            newRot = parent.rotation.eulerAngles;
            newRot.y = Mathf.Round(newRot.y*rotationDivision)*snapRotIncrement;
        }
        transform.position = newPos;
        transform.rotation = Quaternion.Euler(newRot);
        prevPos = parent.position;
    }
}
