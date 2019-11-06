using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : NetworkBehaviour {

    [SerializeField]
    private float speed = 6f;

    [SerializeField]
    private float sprintMultiplier = 1.5f;

    [SerializeField]
    public float sensitivity = 3f;

    [SerializeField]
    private float jumpPower = 20f;

    private PlayerMotor motor;
    public bool Paused = false;


    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);
    }

    private void Update()
    {
        if(isLocalPlayer && !Paused)
        {
            float _xMove = Input.GetAxis("Horizontal");
            float _zMove = Input.GetAxis("Vertical");

            Vector3 _moveHorizontal = transform.right * _xMove;
            Vector3 _moveVertical = transform.forward * _zMove;

            //Final movement vector
            float finalSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                finalSpeed = speed * sprintMultiplier;
            }
            else
            {
                finalSpeed = speed;
            }

            Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * finalSpeed;

            //apply the movement
            motor.Move(_velocity);


            //calculate rotation as a 3d vector
            float _yRot = Input.GetAxis("Mouse X");

            Vector3 _rotation = new Vector3(0, _yRot, 0) * sensitivity;

            motor.Rotate(_rotation);


            //calculate camera rotation as a vector
            float _xRot = Input.GetAxis("Mouse Y");

            Vector3 _cameraRot = new Vector3(_xRot, 0, 0) * sensitivity;

            motor.RotateCamera(_cameraRot);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                motor.Jump(jumpPower);
            }
        }

    }

}
