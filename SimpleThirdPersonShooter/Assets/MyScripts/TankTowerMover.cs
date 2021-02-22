using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTowerMover : MonoBehaviour
{
    [SerializeField] public Transform reticleGUI;
    [SerializeField] private Transform cannonTransform;
    [SerializeField] [RangeAttribute(.1f, 1)] private float followRotationSpeed;
    private float reticleDist;
    private Vector3 cannonRotationCentre;

    [System.NonSerialized] public bool isCannonAiming;

    // Start is called before the first frame update
    void Start()
    {
        reticleDist = Vector3.Distance(reticleGUI.position, this.transform.position);
        cannonRotationCentre = -cannonTransform.position;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        // Relative horizontal position of the Reticle w.r.t. the centre of the TOWER
        Vector3 relativeHorReticlePos = new Vector3(
            reticleGUI.position.x - this.transform.position.x,
            0,
            reticleGUI.position.z - this.transform.position.z);

        // Relative vertical position of the Reticle w.r.t the centre of the tower composed with the SCALED local FWD direction (so the cannon rotates only vertically)
        Vector3 relativeVerReticlePos = new Vector3(0, reticleGUI.position.y - this.transform.position.y, 0) + (this.transform.forward) * reticleDist;

        // Rotate tower horizontally (around y-axis) towards reticle, damping the movement
        Quaternion relativeHorReticleRot = Quaternion.LookRotation(relativeHorReticlePos, Vector3.up);
        Quaternion rotateTowardsHorReticle = Quaternion.RotateTowards(this.transform.rotation, relativeHorReticleRot, followRotationSpeed);
        this.transform.rotation = rotateTowardsHorReticle;

        // Rotate cannon vertically (around x-axis) towards reticle, damping the movement as well
        Quaternion relativeVerReticleRot = Quaternion.LookRotation(relativeVerReticlePos, Vector3.up);
        Quaternion rotateTowardsVerReticle = Quaternion.RotateTowards(cannonTransform.rotation, relativeVerReticleRot, followRotationSpeed);
        cannonTransform.rotation = rotateTowardsVerReticle;

        this.isCannonAiming = Quaternion.Angle(this.transform.rotation, relativeHorReticleRot) + Quaternion.Angle(cannonTransform.rotation, relativeVerReticleRot) == 0;
    }
}
