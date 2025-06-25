using UnityEngine;


public class abrirPuerta : MonoBehaviour, IInteractable
{
    [Header("Rotación de apertura (en grados):")]
    [SerializeField] private Vector3 rotacionApertura = new Vector3(0, 90, 0);

    [Header("Velocidad de rotación (grados por segundo):")]
    [SerializeField] private float velocidadRotacion = 180f;

    [Header("Objeto externo opcional para rotar igual")]
    [SerializeField] private Transform objetoExtraARotar;

    private Quaternion rotacionInicial;
    private Quaternion rotacionAbierta;
    private Quaternion rotacionObjetivo;

    private Quaternion rotacionInicialExtra;
    private Quaternion rotacionAbiertaExtra;
    private Quaternion rotacionObjetivoExtra;

    private bool estaAbierto = false;
    private bool rotando = false;

    private void Start()
    {
        rotacionInicial = transform.rotation;
        rotacionAbierta = rotacionInicial * Quaternion.Euler(rotacionApertura);
        rotacionObjetivo = rotacionInicial;

        if (objetoExtraARotar != null)
        {
            rotacionInicialExtra = objetoExtraARotar.rotation;
            rotacionAbiertaExtra = rotacionInicialExtra * Quaternion.Euler(rotacionApertura);
            rotacionObjetivoExtra = rotacionInicialExtra;
        }
    }

    public void Interact()
    {
        if (rotando) return;

        rotacionObjetivo = estaAbierto ? rotacionInicial : rotacionAbierta;

        if (objetoExtraARotar != null)
            rotacionObjetivoExtra = estaAbierto ? rotacionInicialExtra : rotacionAbiertaExtra;

        estaAbierto = !estaAbierto;
        rotando = true;
    }

    private void Update()
    {
        if (!rotando) return;

        // Rotar el objeto principal
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            rotacionObjetivo,
            velocidadRotacion * Time.deltaTime
        );

        // Rotar el objeto extra si está asignado
        if (objetoExtraARotar != null)
        {
            objetoExtraARotar.rotation = Quaternion.RotateTowards(
                objetoExtraARotar.rotation,
                rotacionObjetivoExtra,
                velocidadRotacion * Time.deltaTime
            );
        }

        // Comprobar si ya llegaron ambos
        bool principalListo = Quaternion.Angle(transform.rotation, rotacionObjetivo) < 0.1f;
        bool extraListo = objetoExtraARotar == null || Quaternion.Angle(objetoExtraARotar.rotation, rotacionObjetivoExtra) < 0.1f;

        if (principalListo && extraListo)
        {
            if (objetoExtraARotar != null)
                objetoExtraARotar.rotation = rotacionObjetivoExtra;

            transform.rotation = rotacionObjetivo;
            rotando = false;
        }
    }
}