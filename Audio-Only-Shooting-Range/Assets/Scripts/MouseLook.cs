using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 10f;

    private float xRotation = 0f;
    private float yRotation = 0f;
    //    sus <= --xRotation++ => jos
    // stanga <= --yRotation++ => dreapta

    void Start()
    {
        // Ascunde cursorul si il blocheaza in mijlocul ecranului
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Mouse.current == null)
            return;

        // Miscarea mouse-ului
        // mouseDelta = nr de pixeli cu care s-a deplasat mouse-ul fata de frame-ul anterior
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        // Time.deltaTime   = durata dintre frame-ul curent si cel anterior, in secunde
        // * Time.deltaTime => nu depinde de nr de FPS, sensibilitatea se va simti la fel
        float mouseX = mouseDelta.x * sensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * sensitivity * Time.deltaTime;

        // mouseX > 0 => vrem sa rotim camera spre dreapta => yRotation += mouseX
        // mouseY > 0 => vrem sa rotim camera spre sus     => xRotation -= mouseY
        yRotation += mouseX;
        xRotation -= mouseY;

        // Nu putem sa ne rotim cu capul in jos / sub noi
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
