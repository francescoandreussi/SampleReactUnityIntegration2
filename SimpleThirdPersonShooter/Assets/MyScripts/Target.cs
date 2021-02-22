using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] [Range(.01f, 5)] private float animationSpeed;
    [SerializeField] [Range(0, 1)] private float rotationSpeed;
    [SerializeField] private int maxTargetHeight;

    private Vector3 awakePos;
    private Vector3 targetPos;
    private float awakeTime;
    private float targetHeight;

    private void Awake()
    {
        awakeTime = Time.time;
        awakePos = this.transform.position;
        targetHeight = Random.Range(1, maxTargetHeight);
        targetPos = new Vector3(this.transform.position.x, targetHeight, this.transform.position.z);
        Debug.Log(targetPos);
    }

    private void Update()
    {
        if (this.transform.position.y != targetHeight)
        {
            float t = Time.time - awakeTime;
            this.transform.position = Vector3.Slerp(awakePos, targetPos, t / animationSpeed);
        }

        this.transform.Rotate(Vector3.up, rotationSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if the object intersects a box destroy it because there's the possibility of being completely hidden
        if (collision.collider.name.Contains("Box"))
        {
            Debug.Log("target inside of box...destroying it!");
            MarkAsHit();
        }
    }
    // Possible extensions and more complex behaviours here!

    IEnumerator WaitExplosion()
    {
        yield return new WaitForSeconds(.4f);
        Destroy(this.gameObject);
    }

    public void MarkAsHit()
    {
        // Trigger explosion
        this.GetComponent<ParticleSystem>().Play();
        GameLogic.currentActiveTargets--;
        StartCoroutine(WaitExplosion());
    }
}
