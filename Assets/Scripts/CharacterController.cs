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
    [HideInInspector] public bool bloquearRotacion = false;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference runAction;
    public InputActionReference lookAction;
    public InputActionReference pauseAction;
    public InputActionReference listaAction;
    public InputActionReference inspeccionAction;
    public InputActionReference cambioccAction;

    public MenuPausa menuPausa;
    public MenuLista menuLista;
    public MenuInspeccion menuInspeccion;
    public MenuCC menuCC;

    [Header("Sonidos de pasos")]
    public AudioSource pasosAudioSource;
    public AudioClip[] pasosCaminar;
    public AudioClip[] pasosCorrer;
    public float intervaloPasosCaminar = 0.5f;
    public float intervaloPasosCorrer = 0.3f;
    [Range(0f, 1f)] public float volumenPasos = 1f;


    private CharacterController controlador;
    private Vector2 inputMovimiento;
    private Vector2 inputLook;
    private float velocidadVertical = 0f;
    private float rotacionVertical = 0f;
    private bool saltar = false;
    private bool corriendo = false;
    private float tiempoUltimoPaso = 0f;

    private System.Action<InputAction.CallbackContext> pausaCallback;
    private System.Action<InputAction.CallbackContext> listaCallback;
    private System.Action<InputAction.CallbackContext> inspeccionCallback;
    private System.Action<InputAction.CallbackContext> cuadernoCallback;

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        runAction.action.Enable();
        lookAction.action.Enable();
        pauseAction.action.Enable();
        listaAction.action.Enable();
        inspeccionAction.action.Enable();
        cambioccAction.action.Enable();

        moveAction.action.performed += ctx => inputMovimiento = ctx.ReadValue<Vector2>();
        moveAction.action.canceled += ctx => inputMovimiento = Vector2.zero;

        jumpAction.action.performed += ctx => saltar = true;

        runAction.action.performed += ctx => corriendo = true;
        runAction.action.canceled += ctx => corriendo = false;

        lookAction.action.performed += ctx => inputLook = ctx.ReadValue<Vector2>();
        lookAction.action.canceled += ctx => inputLook = Vector2.zero;

        pausaCallback = ctx => { if (menuPausa != null) menuPausa.AlternarPausa(); };
        pauseAction.action.performed += pausaCallback;

        listaCallback = ctx => { if (menuLista != null) menuLista.AlternarLista(); };
        listaAction.action.performed += listaCallback;

        inspeccionCallback = ctx => { if (menuInspeccion != null) menuInspeccion.AlternarInspeccion(); };
        inspeccionAction.action.performed += inspeccionCallback;

        cuadernoCallback = ctx => { if (menuCC != null) menuCC.AlternarCuaderno(); };
        cambioccAction.action.performed += cuadernoCallback;
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
        runAction.action.Disable();
        lookAction.action.Disable();
        pauseAction.action.Disable();
        listaAction.action.Disable();
        inspeccionAction.action.Disable();
        cambioccAction.action.Disable();

        if (pauseAction != null) pauseAction.action.performed -= pausaCallback;
        if (listaAction != null) listaAction.action.performed -= listaCallback;
        if (inspeccionAction != null) inspeccionAction.action.performed -= inspeccionCallback;
        if (cambioccAction != null) cambioccAction.action.performed -= cuadernoCallback;
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

        // Reproducir pasos
        if (enSuelo && inputMovimiento.magnitude > 0.1f)
        {
            float intervalo = corriendo ? intervaloPasosCorrer : intervaloPasosCaminar;

            if (Time.time - tiempoUltimoPaso > intervalo)
            {
                ReproducirPaso();
                tiempoUltimoPaso = Time.time;
            }
        }
    }

    void RotarCamara()
    {
        if (bloquearRotacion) return;

        Vector2 deltaRaton = Mouse.current != null && Mouse.current.delta.IsActuated()
            ? Mouse.current.delta.ReadValue() * sensibilidadRaton
            : Vector2.zero;

        Vector2 lookInput = deltaRaton != Vector2.zero ? deltaRaton * Time.deltaTime : inputLook * sensibilidadMando * Time.deltaTime;

        rotacionVertical -= lookInput.y;
        rotacionVertical = Mathf.Clamp(rotacionVertical, -limiteRotacionVertical, limiteRotacionVertical);

        camaraTransform.localRotation = Quaternion.Euler(rotacionVertical, 0f, 0f);
        transform.Rotate(Vector3.up * lookInput.x);
    }

    void ReproducirPaso()
    {
        if (pasosAudioSource == null) return;

        AudioClip[] clips = corriendo ? pasosCorrer : pasosCaminar;
        if (clips.Length == 0) return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];
        pasosAudioSource.PlayOneShot(clip, volumenPasos);
    }

}

