using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("Sensitivity")]
    public float sensitivity = 5.5f;
    public float onTargetSensitivity = 1f;

    [Header("Initial Rotation (Degrees)")]
    [Range(-89f, 89f)] public float initialXRotation = 2f;
    [Range(-89f, 89f)] public float initialYRotation = 2f;


    private float xRotation = 0f;
    private float yRotation = 0f;
    //    sus <= --xRotation++ => jos
    // stanga <= --yRotation++ => dreapta


    void Start()
    {
        xRotation = initialXRotation;
        yRotation = initialYRotation;
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    void Update()
    {
        if (!GameManager.instance.GameIsActive)
        {
            xRotation = initialXRotation;
            yRotation = initialYRotation;
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
            return;
        }

        if (Mouse.current == null)
            return;

        // Miscarea mouse-ului
        // mouseDelta = nr de pixeli cu care s-a deplasat mouse-ul fata de frame-ul anterior
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        // Debug.Log($"Dx = {mouseDelta.x}, Dy ={mouseDelta.y}");

        // Time.deltaTime   = durata dintre frame-ul curent si cel anterior, in secunde
        // * Time.deltaTime => nu depinde de nr de FPS, sensibilitatea se va simti la fel
        float mouseX = mouseDelta.x * sensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * sensitivity * Time.deltaTime;

        // mouseX > 0 => vrem sa rotim camera spre dreapta => yRotation += mouseX
        // mouseY > 0 => vrem sa rotim camera spre sus     => xRotation -= mouseY
        yRotation += mouseX;
        xRotation -= mouseY;

        // Nu putem sa ne rotim cu capul in jos
        xRotation = Mathf.Clamp(xRotation, -89f, 89f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
