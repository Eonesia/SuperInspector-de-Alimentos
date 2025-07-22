using UnityEngine;

public class abrirPuerta : MonoBehaviour, IInteractable
{
    [Header("Rotación de apertura (en grados):")]
    [SerializeField] private Vector3 rotacionApertura = new Vector3(0, 90, 0);

    [Header("Velocidad de rotación (grados por segundo):")]
    [SerializeField] private float velocidadRotacion = 180f;

    [Header("Objeto externo opcional para rotar igual")]
    [SerializeField] private Transform objetoExtraARotar;

    [Header("Opcional: permitir rotación distinta para el objeto extra")]
    [SerializeField] private bool noMismaRotacion = false;

    [SerializeField] private Vector3 rotacionExtraApertura = new Vector3(0, 90, 0);

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
        // Rotación de la puerta
        rotacionInicial = transform.rotation;
        rotacionAbierta = rotacionInicial * Quaternion.Euler(rotacionApertura);
        rotacionObjetivo = rotacionInicial;

        // Rotación del objeto extra
        if (objetoExtraARotar != null)
        {
            rotacionInicialExtra = objetoExtraARotar.rotation;
            Vector3 apertura = noMismaRotacion ? rotacionExtraApertura : rotacionApertura;
            rotacionAbiertaExtra = rotacionInicialExtra * Quaternion.Euler(apertura);
            rotacionObjetivoExtra = rotacionInicialExtra;
        }
    }

    public void Interact()
    {
        if (rotando) return;

        rotacionObjetivo = estaAbierto ? rotacionInicial : rotacionAbierta;

        if (objetoExtraARotar != null)
        {
            rotacionObjetivoExtra = estaAbierto ? rotacionInicialExtra : rotacionAbiertaExtra;
        }

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

        // Rotar el objeto extra
        if (objetoExtraARotar != null)
        {
            objetoExtraARotar.rotation = Quaternion.RotateTowards(
                objetoExtraARotar.rotation,
                rotacionObjetivoExtra,
                velocidadRotacion * Time.deltaTime
            );
        }

        bool principalListo = Quaternion.Angle(transform.rotation, rotacionObjetivo) < 0.1f;
        bool extraListo = objetoExtraARotar == null || Quaternion.Angle(objetoExtraARotar.rotation, rotacionObjetivoExtra) < 0.1f;

        if (principalListo && extraListo)
        {
            transform.rotation = rotacionObjetivo;

            if (objetoExtraARotar != null)
                objetoExtraARotar.rotation = rotacionObjetivoExtra;

            rotando = false;
        }
    }
}
