using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform playerCamera;

    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float crouchSpeed = 2f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    float xRotation = 0f;
    Vector3 velocity;

    public bool isCrouching = false;
    public bool isDead = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (isDead) return;

        MouseLook();
        Move();
        Jump();
        Crouch();
        Pickup();
    }

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * 200f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * 200f * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float speed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
            speed = sprintSpeed;

        if (isCrouching)
            speed = crouchSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void Jump()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
            controller.height = 1f;
        }
        else
        {
            isCrouching = false;
            controller.height = 2f;
        }
    }

    void Pickup()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 3f))
            {
                PickupItem item = hit.collider.GetComponent<PickupItem>();
                if (item != null)
                {
                    item.Pickup();
                }
            }
        }
    }

    public void Die()
    {
        isDead = true;
        Debug.Log("DU BIST GESTORBEN");
    }
}
