using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleController : MonoBehaviour
{
    [SerializeField] float m_PaddleSpeed;

    InputActions m_Input;

    Transform m_Transform;

    Vector2 MotionVector;

    Vector2 WindowLimits;

    float PaddleWidth;

    private void OnEnable()
    {
        m_Input = new InputActions();

        m_Input.Player.Enable();

        m_Input.Player.Move.performed += OnMovement;
        m_Input.Player.Move.started += OnMovement;
        m_Input.Player.Move.canceled += OnMovement;

        m_Transform = transform;

        FindWindowLimits();

        PaddleWidth = m_Transform.localScale.x;
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            Vector2 temp = context.ReadValue<Vector2>();

            MotionVector = new Vector2(temp.x, 0);
        }
        else
        {
            MotionVector = Vector2.zero;
        }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float Xpos = m_Transform.position.x;
        float HalfPaddle = PaddleWidth / 2;
        float Xmin = -WindowLimits.x;
        float Xmax = WindowLimits.x;
        float MotionValue = MotionVector.x;

        if ((Xpos - HalfPaddle < Xmin) && (MotionValue < 0)) return;
        if (m_Transform.position.x + (PaddleWidth / 2) > WindowLimits.x && MotionVector.x > 0) return;

        m_Transform.Translate(MotionVector * m_PaddleSpeed * Time.deltaTime);
        //MotionVector = Vector2.zero;
    }

    private void OnDisable()
    {
        m_Input.Player.Move.performed -= OnMovement;


        m_Input.Player.Disable();  
    }


    void FindWindowLimits()
    {
        WindowLimits = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
}
