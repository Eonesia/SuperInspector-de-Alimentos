using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadCaminar = 5f;
    public float velocidadCorrer = 10f;
    public float fuerzaSalto = 3f;
    public float gravedad = -9.81f;

    [Header("Rotación de cámara")]
    public Transform camaraTransform;
    public float sensibilidadRaton = 1f;
    public float sensibilidadMando = 150f;
    public float limiteRotacionVertical = 80f;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference runAction;
    public InputActionReference lookAction; // <-- Joystick derecho
    public InputActionReference pauseAction;

    public MenuPausa menuPausa;
    private CharacterController controlador;
    private Vector2 inputMovimiento;
    private Vector2 inputLook;
    private float velocidadVertical = 0f;
    private float rotacionVertical = 0f;
    private bool saltar = false;
    private bool corriendo = false;

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        runAction.action.Enable();
        lookAction.action.Enable();

        moveAction.action.performed += ctx => inputMovimiento = ctx.ReadValue<Vector2>();
        moveAction.action.canceled += ctx => inputMovimiento = Vector2.zero;

        jumpAction.action.performed += ctx => saltar = true;

        runAction.action.performed += ctx => corriendo = true;
        runAction.action.canceled += ctx => corriendo = false;

        lookAction.action.performed += ctx => inputLook = ctx.ReadValue<Vector2>();
        lookAction.action.canceled += ctx => inputLook = Vector2.zero;

        pauseAction.action.Enable();
        pauseAction.action.performed += ctx => menuPausa.AlternarPausa();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
        runAction.action.Disable();
        lookAction.action.Disable();
        pauseAction.action.Disable();
    }

    void Start()
    {
        controlador = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        MoverJugador();
        RotarCamara();
    }

    void MoverJugador()
    {
        bool enSuelo = controlador.isGrounded;

        if (enSuelo && velocidadVertical < 0)
            velocidadVertical = -2f;

        Vector3 direccionMovimiento = transform.right * inputMovimiento.x + transform.forward * inputMovimiento.y;

        float velocidadActual = corriendo ? velocidadCorrer : velocidadCaminar;
        controlador.Move(direccionMovimiento * velocidadActual * Time.deltaTime);

        if (saltar && enSuelo)
        {
            velocidadVertical = Mathf.Sqrt(fuerzaSalto * -2f * gravedad);
        }

        saltar = false;

        velocidadVertical += gravedad * Time.deltaTime;
        controlador.Move(Vector3.up * velocidadVertical * Time.deltaTime);
    }

    void RotarCamara()
    {
        Vector2 deltaRaton = Mouse.current != null && Mouse.current.delta.IsActuated()
            ? Mouse.current.delta.ReadValue() * sensibilidadRaton
            : Vector2.zero;

        Vector2 lookInput = deltaRaton != Vector2.zero ? deltaRaton * Time.deltaTime : inputLook * sensibilidadMando * Time.deltaTime;

        rotacionVertical -= lookInput.y;
        rotacionVertical = Mathf.Clamp(rotacionVertical, -limiteRotacionVertical, limiteRotacionVertical);

        camaraTransform.localRotation = Quaternion.Euler(rotacionVertical, 0f, 0f);
        transform.Rotate(Vector3.up * lookInput.x);
    }
}
