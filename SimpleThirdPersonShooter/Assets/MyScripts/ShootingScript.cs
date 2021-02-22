using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ShootingScript : MonoBehaviour
{
    [SerializeField] private Rigidbody tank;
    [SerializeField] private GameObject projectilePrefab; //  projectile model (not used atm)
    [SerializeField] private ParticleSystem shotFlash;
    [SerializeField] private float backlashForce;

    private TankTowerMover aimingScript;
    private Transform mainCamera;
    private Vector3 shootingDir;

    // Start is called before the first frame update
    void Start()
    {
        aimingScript = GetComponentInParent<TankTowerMover>();
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        shootingDir = aimingScript.reticleGUI.position - mainCamera.position;
        Debug.DrawRay(mainCamera.position, shootingDir * 100, Color.red);
        if (CrossPlatformInputManager.GetButtonDown("Fire1") && aimingScript.isCannonAiming)
        {
            Shoot();
        }
    }

    IEnumerator shootAfterLoadingSound()
    {
        yield return new WaitForSeconds(.9f);
        shotFlash.Play();

        // Add backlash
        tank.AddForce(-this.transform.forward * backlashForce, ForceMode.Impulse);

        // Check for hit objects
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.position, shootingDir, out hit))
        {
            Debug.Log(hit.collider.name);
            // If the hit object has tag "Respawn", it WILL have a Target script component attached too
            if (hit.transform.tag.Equals("Respawn"))
            {
                Debug.Log("Target HIT");
                hit.transform.GetComponent<Target>().MarkAsHit();
            }
        }
    }

    void Shoot()
    {
        // Trigger audio and visual effects
        this.GetComponent<AudioSource>().Play();
        StartCoroutine(shootAfterLoadingSound());
    }
}
