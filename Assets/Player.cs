using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] float movingDistance;
    [SerializeField] float movingSmooth;
    [SerializeField] float jumpHeight;
    [SerializeField] float moveCD;

    private CharacterController controller;
    private bool moving;
    private float hor;
    private float verVelocity;
    private float timeSinceLastMove = Mathf.Infinity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        timeSinceLastMove += Time.deltaTime;
        ProcessMovement();
        MoveTo();
    }

    private void ProcessMovement()
    {
        if (controller.isGrounded) Debug.Log("Grounded");
        else Debug.Log("Not Grounded");
        if (controller.isGrounded == false) return;
        hor += Input.GetAxisRaw("Horizontal") * movingDistance;
        hor = Mathf.Clamp(hor, -movingDistance, movingDistance);
        timeSinceLastMove = 0;
        Debug.Log("Start Moving");
    }

    private void MoveTo()
    {
        var x = Mathf.MoveTowards(transform.position.x, hor, movingSmooth * Time.deltaTime);
        controller.Move(new Vector3(x, 0, 0));
    }

    private bool IsMoving()
    {
        return timeSinceLastMove < moveCD;
    }
}
