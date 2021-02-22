using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleScript : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(mainCamera);
    }
}
