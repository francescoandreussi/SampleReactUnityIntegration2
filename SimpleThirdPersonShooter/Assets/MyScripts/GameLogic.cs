using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private int totalTargetsNum;
    [SerializeField] private int activeTargetsNum;
    [SerializeField] private int maxSecsBetweenSpawns;
    
    public static int currentActiveTargets;

    private float timeElapsed = 0;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnTargets", 0, Random.Range(0, maxSecsBetweenSpawns));
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

    }

    private void SpawnTargets()
    {
        if (currentActiveTargets < activeTargetsNum)
        {
            GameObject newTarget = Instantiate(targetPrefab, new Vector3(Random.Range(-45, 45), -5, Random.Range(-45, 45)), Quaternion.identity);
            currentActiveTargets++;
        }
    }
}
