using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1.8f;
    private void Update()
    {
        transform.Rotate(0, 0, 360 * Time.deltaTime * rotationSpeed);
    }
}
