using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class TankMover : MonoBehaviour
{
    [SerializeField] private float movingSpeed;
    [SerializeField] private float turningSpeed;

    private Transform cameraTransform;
    private Vector3 lastValidDir;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GameObject.FindObjectOfType<Camera>().transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Read the user input (the input coordinate system is LOCAL to the camera orientation, i.e. not aligned to the world axes)
        var x = CrossPlatformInputManager.GetAxis("Horizontal");
        var z = CrossPlatformInputManager.GetAxis("Vertical");
        Vector3 localMovingDir = new Vector3(x, 0, z);

        // Save the direction of the movement in world space (the y-component has to be discarded)
        Vector3 movingDir = cameraTransform.rotation * localMovingDir;
        movingDir.y = 0;
        lastValidDir = !movingDir.Equals(Vector3.zero) ? movingDir.normalized : lastValidDir;

        // movingDir should never go to zero, hence I take the last valid value

        // Turn the tank towards the moving direction
        Quaternion movingRot = Quaternion.LookRotation(lastValidDir, Vector3.up);
        Quaternion rotateTowardsMovingRot = Quaternion.RotateTowards(this.transform.rotation, movingRot, turningSpeed);
        this.transform.rotation = rotateTowardsMovingRot;

        // Compute the angle in redians which the tank has still to cover for being aligned to the moving direction
        float currentAngleRad = Quaternion.Angle(this.transform.rotation, movingRot) * Mathf.Deg2Rad;
        float alignmentFactor = Mathf.Pow(Mathf.Max(0, Mathf.Cos(currentAngleRad)), 3);

        // Translate in the moving direction, with a certain moving speed, dampened when the tank is not aligned to the direction
        // Actually, the DIRECTION is dictated by lastValidDir (which cannot be zero) whereas the INTENSITY of the movement is given by
        // the magnitude of movingDir composed with the movingSpeed and the "alignment-to-moving-direction" factor
        this.transform.position += lastValidDir * movingDir.magnitude * movingSpeed * alignmentFactor;
    }
}
