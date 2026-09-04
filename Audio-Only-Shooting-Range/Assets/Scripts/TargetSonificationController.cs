using UnityEngine;
using FMODUnity;

public class TargetSonificationController : MonoBehaviour
{
    
    private StudioEventEmitter eventEmitter;
    private Transform mainCameraTransform;


    void Start()
    {
        eventEmitter = GetComponent<StudioEventEmitter>();

        if (Camera.main != null )
            mainCameraTransform = Camera.main.transform;
    }

    
    void Update()
    {
        if (eventEmitter == null || mainCameraTransform == null)
            return;


        // Directia camerei, proiectata pe planul orizontal (y = 0)
        Vector3 camDirFlat = mainCameraTransform.forward;
        camDirFlat.y = 0;
        camDirFlat.Normalize();

        // Vector de la Main Camera spre Target
        Vector3 directionToTarget = (transform.position - mainCameraTransform.position).normalized;

        // Directia vectorului, proiectata pe planul orizontal (y = 0)
        Vector3 tarDirFlat = directionToTarget;
        tarDirFlat.y = 0;
        tarDirFlat.Normalize();

        // Vector3.SignedAngle(vect1, vect2, axa) => calculeaza unghiul dintre 2 vectori, semnul e determinat in functie de axa de rotatie (perpendiculara pe cei 2)
        float azimuthError = Mathf.Abs(Vector3.SignedAngle(camDirFlat, tarDirFlat, Vector3.up));


        float cameraPitch = Mathf.Asin(mainCameraTransform.forward.y) * Mathf.Rad2Deg;
        float targetPitch = Mathf.Asin(directionToTarget.y) * Mathf.Rad2Deg;

        float elevationError = Mathf.Abs(cameraPitch - targetPitch);

        // Debug.Log($"a = {azimuthError} e = {elevationError}");

        // Trimitem valorile calculate
        eventEmitter.SetParameter("AzimuthError", azimuthError);
        eventEmitter.SetParameter("ElevationError", elevationError);
    }
}
