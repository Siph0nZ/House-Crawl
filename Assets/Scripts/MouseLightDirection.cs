using UnityEngine;


public class MouseLightDirection : MonoBehaviour
{
    private UnityEngine.Rendering.Universal.Light2D spotlight;
    private Transform parentTransform;

    void Start()
    {
        // Get the Spotlight 2D component
        spotlight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        // Get the parent transform (the sprite the light is attached to)
        parentTransform = transform.parent;
    }

    void Update()
    {
        // Get the mouse position in world coordinates
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0; // Ensure z is 0 since we're in 2D

        // Calculate the direction from the light to the mouse position
        Vector3 direction = mouseWorldPosition - parentTransform.position;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the spotlight to point towards the mouse
        spotlight.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }
}