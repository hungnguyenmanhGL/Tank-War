using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS WORK ON BEST ON KEY CONTROLLER
public class TankMover : MonoBehaviour
{
    private Rigidbody2D tankBody;
    private Vector2 movementVector;

    public float maxSpeed = 70f;
    public float rotateSpeed = 200f;
    public float acceleration = 70f;
    public float deacceleration = 50f;
    public float currentSpeed = 0;
    public float currentForwardDirection = 1;

    // Start is called before the first frame update
    void Start()
    {
        tankBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movementVector = movementVector.normalized;

    }

    private void FixedUpdate()
    {
        Move(movementVector);
        tankBody.velocity = (Vector2)(transform.up) * currentSpeed * currentForwardDirection * Time.fixedDeltaTime;
        tankBody.MoveRotation(transform.rotation * Quaternion.Euler(0, 0, -movementVector.x * rotateSpeed * Time.fixedDeltaTime));
    }

    private void CalculateSpeed(Vector2 movementVector)
    {
        if (Mathf.Abs(movementVector.y) > 0)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            currentSpeed -= deacceleration * Time.deltaTime;
        }
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    }

    void Move(Vector2 movementVector)
    {
        this.movementVector = movementVector;
        CalculateSpeed(movementVector);
        //OnSpeedChange?.Invoke(this.movementVector.magnitude);
        if (movementVector.y > 0)
        {
            if (currentForwardDirection == -1)
                currentSpeed = 0;
            currentForwardDirection = 1;
        }
        else if (movementVector.y < 0)
        {
            if (currentForwardDirection == 1)
                currentSpeed = 0;
            currentForwardDirection = -1;
        }
    }
}
