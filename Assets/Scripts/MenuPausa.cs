using UnityEngine;

public class MenuPausa : MonoBehaviour
{

    public GameObject ObjetoMenuPausa;
    public bool Pausa = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Pausa == false)
            {
                ObjetoMenuPausa.SetActive(true);
                Pausa = true;
            }
        }
    }
}
