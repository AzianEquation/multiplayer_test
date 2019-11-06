using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    [SerializeField]
    private Camera cam;


    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 camRot = Vector3.zero;


    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    //Gets a movement vector
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }
    public void RotateCamera(Vector3 _camRot)
    {
        camRot = _camRot;
    }

    //Run every physics iteration
    private void FixedUpdate()
    {
        performMovement();
        performRotation();
    }

    void performMovement()
    {
        if(velocity != Vector3.zero)
        {
            float DistanceToTheGround = GetComponent<Collider>().bounds.extents.y;
            bool isGrounded = Physics.Raycast(transform.position, Vector3.down, DistanceToTheGround + 0.1f);
            if(isGrounded)
            {
                rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
            }
            else
            {
                rb.MovePosition(transform.position + (velocity * 0.7f) * Time.fixedDeltaTime);
            }

        }
    }
    void performRotation()
    {
        rb.MoveRotation(transform.rotation * Quaternion.Euler(rotation));
        if(cam != null)
        {
            cam.transform.Rotate(-camRot);
        }
    }

    public void Jump(float _Power)
    {
        float DistanceToTheGround = GetComponent<Collider>().bounds.extents.y;
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, DistanceToTheGround + 0.1f);

        if(isGrounded)
        {
            rb.AddForce(Vector3.up * _Power, ForceMode.Impulse);
        }
    }


}
