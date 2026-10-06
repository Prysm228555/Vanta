using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    private float mouseSensitivityX = 3f;

    [SerializeField]
    private float mouseSensitivityY = 3f;

    [SerializeField]
    private float thrusterForce = 1600f;

    [Header("Joint Options")]
    [SerializeField]
    private float jointSpring = 20f;

    [SerializeField]
    private float jointMaxForce = 50f;

    private PlayerMotor motor;
    private ConfigurableJoint joint;

    private Animator animator;

    void Start()
    {
        // Automaticaly sets motor to the PlayerMotor component
        motor = GetComponent<PlayerMotor>();

        // Automaticaly sets joint to the ConfigurableJoint component
        joint = GetComponent<ConfigurableJoint>();

        // Set default value for the ConfigurableJoint
        SetJointSettings(jointSpring);

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Calculate player velocity
        float xMov = Input.GetAxis("Horizontal");
        float zMov = Input.GetAxis("Vertical");

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        Vector3 velocity = (moveHorizontal + moveVertical) * speed;

        // Play thruster animations
        animator.SetFloat("ForwardVelocity", zMov);

        // Apply movement
        motor.Move(velocity);


        // Calculate player rotation
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0, yRot, 0) * mouseSensitivityX;

        // Apply player rotation
        motor.Rotate(rotation);


        // Calculate camera rotation
        float xRot = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRot * mouseSensitivityY;

        // Apply camera rotation
        motor.RotateCamera(cameraRotationX);


        // Calculate thrusterForce
        Vector3 thrusterVelocity = Vector3.zero;
        if (Input.GetButton("Jump"))
        {
            thrusterVelocity = Vector3.up * thrusterForce;
            SetJointSettings(0f);
        }
        else
        {
            SetJointSettings(jointSpring);
        }

        // Apply Thruster velocity
        motor.ApplyThruster(thrusterVelocity);
    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive { positionSpring = _jointSpring, maximumForce = jointMaxForce };
    }
}
 