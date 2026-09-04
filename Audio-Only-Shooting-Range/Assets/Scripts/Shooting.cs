using System;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;
using NaughtyAttributes;

public class Shooting : MonoBehaviour
{
    public float range = 100f;

    [SerializeField, BoxGroup("FMOD Events")]
    public EventReference shotEvent;

    [SerializeField, BoxGroup("FMOD Events")]
    public EventReference acquireTargetEvent;


    private bool targetAcquired = false;
    private MouseLook mouseLook;
    private float mainSensitivity;


    void Start()
    {
        mouseLook = GetComponent<MouseLook>();

        if (mouseLook == null)
            Debug.Log("Nu s-a gasit componenta MouseLook pe acest obiect");
        else
            mainSensitivity = mouseLook.sensitivity;
    }

    void Update()
    {
        CheckForTarget();

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            Shoot();
    }

    private void CheckForTarget()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            if (hit.collider.transform.parent != null &&
                hit.collider.transform.parent.CompareTag("Target"))
            {
                // Debug.Log("Ma uit la obiectul: " + hit.collider.name);
                if (!targetAcquired)
                    RuntimeManager.PlayOneShot(acquireTargetEvent);

                if (mouseLook != null)
                    mouseLook.sensitivity = mouseLook.onTargetSensitivity;

                targetAcquired = true;
            }
            else
            {
                if (mouseLook != null)
                    mouseLook.sensitivity = mainSensitivity;
                targetAcquired = false;
            }
        }
    }

    void Shoot()
    {
        RuntimeManager.PlayOneShot(shotEvent); // creaza o instanta audio temporara, o reda si o distruge dupa ce se termina

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // out hit => metoda Raycast poate pune date in hit
        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log("Am lovit obiectul: " + hit.collider.name);
        
            if (hit.collider.transform.parent != null &&
                hit.collider.transform.parent.CompareTag("Target"))
            {
                if (GameManager.instance != null)
                    GameManager.instance.AddToScore(10);

                Destroy(hit.collider.transform.parent.gameObject);
            }
        }
    }
}
