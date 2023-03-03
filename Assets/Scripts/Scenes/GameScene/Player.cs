using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float m_Speed = 1;

    private readonly float m_Gravity = 9.80665f;

    private readonly float m_RotationLimit = 80;

    private readonly float m_360 = 360;

    private CharacterController m_CharacterController;

    private Transform playerCamera;

    private void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        playerCamera = transform.GetChild(0);
    }

    private void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        if (m_CharacterController.isGrounded)
        {
            if (Input.GetKey(KeyCode.W))
            {
                moveDirection += transform.forward * m_Speed;
            }

            if (Input.GetKey(KeyCode.A))
            {
                moveDirection += transform.right * -m_Speed;
            }

            if (Input.GetKey(KeyCode.S))
            {
                moveDirection += transform.forward * -m_Speed;
            }

            if (Input.GetKey(KeyCode.D))
            {
                moveDirection += transform.right * m_Speed;
            }
        }

        moveDirection.y -= (m_Gravity * Time.deltaTime);
        m_CharacterController.Move(moveDirection * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        m_CharacterController.transform.Rotate(0, mouseX, 0);
        playerCamera.Rotate(-mouseY, 0, 0);

        if (playerCamera.localEulerAngles.x < m_360 - m_RotationLimit && playerCamera.localEulerAngles.x > m_360 / 2)
        {
            playerCamera.localEulerAngles = new Vector3(280, playerCamera.localEulerAngles.y, playerCamera.localEulerAngles.z);
        }

        if (playerCamera.localEulerAngles.x > m_RotationLimit && playerCamera.localEulerAngles.x < m_360 / 2)
        {
            playerCamera.localEulerAngles = new Vector3(80, playerCamera.localEulerAngles.y, playerCamera.localEulerAngles.z);
        }
    }
}
