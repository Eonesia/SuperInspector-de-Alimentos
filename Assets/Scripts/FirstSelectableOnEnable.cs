using UnityEngine;
using UnityEngine.EventSystems;

public class FirstSelectableOnEnable : MonoBehaviour
{
    public GameObject firstButton;

    private void OnEnable()
    {
        if (firstButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstButton);
        }
    }
}
