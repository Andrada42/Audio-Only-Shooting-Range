using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    public float range = 100f;
    

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
            // Debug.Log("Ma uit la obiectul: " + hit.collider.name);
        }
    }

    void Shoot()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // out hit => metoda Raycast poate pune date in hit
        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log("Am lovit obiectul: " + hit.collider.name);
        }

        
    }
}
