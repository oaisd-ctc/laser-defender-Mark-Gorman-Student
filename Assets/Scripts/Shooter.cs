using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifetime = 5f;
    [SerializeField] float FiringBaseRate = 0.2f;

    [Header("AI")]
    [SerializeField] bool isAI;
    [SerializeField] float firingVarianceRate = 0f;
    [SerializeField] float firingMinRate = 0.1f;

    [HideInInspector] public bool isFiring;
    AudioPlayer audioPlayer;

    Coroutine firingCoroutine;

    void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    void Start()
    {
        if(isAI)
        {
            isFiring = true;
        }
    }

    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if (isFiring && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        else if(!isFiring && firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    IEnumerator FireContinuously()
    {
        while(true)
        {
            GameObject instance = Instantiate(projectilePrefab,
                                            transform.position,
                                            Quaternion.identity);
            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = transform.up * projectileSpeed;
            }

            Destroy(instance, projectileLifetime);

            float fireSpeed = Random.Range(FiringBaseRate - firingVarianceRate,
                                        FiringBaseRate + firingVarianceRate);
            fireSpeed = Mathf.Clamp(fireSpeed, firingMinRate, float.MaxValue);

            audioPlayer.PlayShootingClip();

            yield return new WaitForSeconds(fireSpeed);
        }
    }
}
