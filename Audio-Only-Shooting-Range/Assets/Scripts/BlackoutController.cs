using UnityEngine;
using UnityEngine.InputSystem;

public class BlackoutController : MonoBehaviour
{
    public GameObject blackScreenUI;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
        {
            bool currentState = blackScreenUI.activeSelf;
            blackScreenUI.SetActive(!currentState);
        }
    }
}
