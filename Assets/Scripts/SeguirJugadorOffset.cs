using UnityEngine;

public class SeguirJugadorOffset : MonoBehaviour
{
    [Header("Transform del jugador que se va a seguir")]
    public Transform jugador;

    [Header("Offset local respecto al jugador")]
    public Vector3 offset = new Vector3(0, 1, 1.5f);

    void LateUpdate()
    {
        if (jugador == null) return;

        // Aplica posición con offset en base a la rotación del jugador
        transform.position = jugador.position + jugador.TransformDirection(offset);

        // Opcional: copiar rotación (si quieres que rote igual que el jugador)
        transform.rotation = jugador.rotation;
    }
}
