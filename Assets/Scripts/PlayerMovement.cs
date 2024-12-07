using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float moveSpeed = 10f;
    private Vector3 currVelocity;
    public float gravCustom = -9.8f;
    public Transform groundCheck;
    public float radiusGroundCheck;
    public LayerMask groundLayer;
    public float jumpHeight = 3f;
    private bool isGrounded;
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, radiusGroundCheck, groundLayer);
        if(isGrounded && currVelocity.y < 0)
        {
            currVelocity.y = -2f;
        }
        float xIn = Input.GetAxis("Horizontal");
        float zIn = Input.GetAxis("Vertical");

        Vector3 inputVec = new Vector3(xIn, 0, zIn);
        if(inputVec.magnitude > 1f)
        {
            inputVec.Normalize();
        }

        Vector3 moveDirection = transform.right * inputVec.x + transform.forward * inputVec.z;

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            currVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravCustom);
        }

        currVelocity.y += gravCustom * Time.deltaTime;
        controller.Move(currVelocity * Time.deltaTime);
    }
}
