using UnityEngine;

public class CrossHairTarget : MonoBehaviour
{
    RaycastHit hit;
    Ray ray;
    Camera mainCamera ;


    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        ray.origin = mainCamera.transform.position;
        ray.direction = mainCamera.transform.forward;

        if (Physics.Raycast(ray, out hit))
        {
            transform.position = hit.point;
        }
        else
        {
            transform.position = ray.origin + ray.direction * 1000.0f;
        }
        Debug.DrawLine(ray.origin, hit.point, Color.red);
    }
}
