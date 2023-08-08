using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotate : MonoBehaviour
{
    private bool isRotating = false;
    private Vector3 touchStartPosition;
    [SerializeField]
    private float rotationSpeed = 2.0f;

    void Update()
    {
        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Raycast to check if the touch is hitting the object
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject == gameObject)
                        {
                            // Object is touched
                            isRotating = true;
                            touchStartPosition = touch.position;
                        }
                    }
                    break;

                case TouchPhase.Moved:
                    // Rotate the object when the touch is moved
                    if (isRotating)
                    {
                        Vector3 currentTouchPosition = touch.position;
                        float deltaX = currentTouchPosition.x - touchStartPosition.x;

                        // Adjust rotation speed based on touch movement
                        float rotationAmount = deltaX * rotationSpeed * Time.deltaTime;

                        // Rotate the object around its up-axis (you can change this if needed)
                        transform.Rotate(Vector3.up, -rotationAmount);
                    }
                    break;

                case TouchPhase.Ended:
                    // Touch is no longer active
                    isRotating = false;
                    break;
            }
        }
    }
}
