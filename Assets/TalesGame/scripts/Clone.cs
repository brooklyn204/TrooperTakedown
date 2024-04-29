using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] Gun stunner;
    [SerializeField] float threshholdAngle;
    [SerializeField] EnemyManager manager;

    private AudioManager audio;
    private bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        GameObject audioObject = GameObject.Find("AudioManager");
        if (audioObject != null)
        {
            audio = audioObject.GetComponent<AudioManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get vector pointing towards the target
        Vector3 targetDirection = target.transform.position - transform.position;
        // Get vector representing direction gun points
        Vector3 stunnerDirection = stunner.gameObject.transform.up;
        // Ignore the y direction, we aren't going to tilt up and down anyway
        targetDirection.y = 0;
        stunnerDirection.y = 0;
        // Get the angle between the direction to the target and the direction the gun points
        float angleToTarget = Vector3.SignedAngle(stunnerDirection, targetDirection, Vector3.up);
        if (Mathf.Abs(angleToTarget) > threshholdAngle)
        {
            transform.Rotate(0.0f, angleToTarget, 0.0f, Space.World);
        }
    }

    public void Shoot()
    {
        stunner.Shoot();
    }

    // When hit with a blast, the death animation is played, which will trigger this function.
    public void OnDeath()
    {
        if (!isDead) // In case the animation is somehow called twice
        {
            isDead = true;
            audio.CloneDeathSound();
            manager.UpdateDeaths();
        }
    }

    public bool IsDead()
    {
        return isDead;
    }
}