using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 7f;
    [SerializeField] float turnSpeed = 12f;
    [SerializeField] Transform cameraTransform;

    CharacterController controller;
    float verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null) cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        Vector2 input = Vector2.zero;
        if (kb.wKey.isPressed) input.y += 1;
        if (kb.sKey.isPressed) input.y -= 1;
        if (kb.dKey.isPressed) input.x += 1;
        if (kb.aKey.isPressed) input.x -= 1;
        input = Vector2.ClampMagnitude(input, 1f);

        Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
        Vector3 move = forward * input.y + right * input.x;

        float speed = kb.leftShiftKey.isPressed ? sprintSpeed : walkSpeed;
        if (controller.isGrounded && verticalVelocity < 0f) verticalVelocity = -1f;
        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        controller.Move((move * speed + Vector3.up * verticalVelocity) * Time.deltaTime);

        if (move.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(move), turnSpeed * Time.deltaTime);
    }
}