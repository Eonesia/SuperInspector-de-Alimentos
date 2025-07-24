using UnityEngine;

public class SalvaObjetos : MonoBehaviour
{
    [Header("Destino al que se teletransportan los objetos")]
    public Transform destino;

    private void OnTriggerEnter(Collider other)
    {
        if (destino != null)
        {
            // Teletransportar el objeto que entró al trigger
            other.transform.position = destino.position;
        }
        else
        {
            Debug.LogWarning("No se ha asignado un destino en el teletransportador.");
        }
    }
}
