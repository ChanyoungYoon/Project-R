using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://mrbinggrae.tistory.com/34
//https://www.youtube.com/watch?v=0AxnfPg_zSg
public class Character : MonoBehaviour
{
    [SerializeField]
    private float speed;
    protected Vector3 direction;
    protected Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

    }

    protected virtual void Update()
    {
        Move();
        AnimateMovement();

    }

    public void Move()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime);

    }

    public void AnimateMovement()
    {
        //Sets the animation parameter so that he faces the correct direction
        animator.SetFloat("x", direction.x);
        animator.SetFloat("y", direction.y);
    }
}
