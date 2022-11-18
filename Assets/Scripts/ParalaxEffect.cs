using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;


    Vector2 startingPos;
    float startingZ;

    Vector2 CameraMoveSinceStart => (Vector2)cam.transform.position - startingPos;

    float ZDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    float ClippingPlane => (cam.transform.position.z + (ZDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));
    float ParalaxFactor => Mathf.Abs(ZDistanceFromTarget / ClippingPlane);

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPosition = startingPos + CameraMoveSinceStart*ParalaxFactor;

        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
