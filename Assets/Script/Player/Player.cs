using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField]
    Camera playerCamera;

    private void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }

    protected override void Update()
    {
        //Executes the GetInput function
        GetInput();
        LookMouseCursor();


        base.Update();
    }

    private void GetInput()
    {
        Vector3 moveVector;

        moveVector.x = Input.GetAxisRaw("Horizontal");
        moveVector.y = 0.0f;
        moveVector.z = Input.GetAxisRaw("Vertical");

        direction = moveVector;
    }

    public void LookMouseCursor()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitResult;

        if(Physics.Raycast(ray, out hitResult))
        {
            Vector3 mouseDir = new Vector3(hitResult.point.x, transform.position.y, hitResult.point.z) - transform.position;
            animator.transform.forward = mouseDir;
        }
    }
}
