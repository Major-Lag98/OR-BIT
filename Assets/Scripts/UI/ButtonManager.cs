using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

    [SerializeField] float rotationSpeed = 100;

    bool isRotating = false;

    bool clockwise = false;


    private void Update()
    {
        if (isRotating)
        {
            Transform piece = PieceFlinger.Instance.CurrentPieceBeingFlung.transform;

            if (clockwise) //both with ? null checks
            {
                piece?.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
            }
            else
            {
                piece?.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            }
        }
    }

    public void OnPointerDownRotate(bool _clockwise) //holding down button will rotate
    {
        isRotating = true;
        clockwise = _clockwise;
    }

    public void OnPointerUpRotate() //letting go of button will stop rotation
    {
        isRotating = false;
    }
}
