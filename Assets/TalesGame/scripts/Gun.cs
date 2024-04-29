using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] Blast ringPrefab;
    [SerializeField] Transform shootPoint;

    private int ringsPerShot = 1;

    private AudioManager audio;
    private bool shooting;
    private int shot;
    private float timeSinceShot;
    [SerializeField] float timeBetweenShots;

    // Start is called before the first frame update
    void Start()
    {
        shooting = false;
        shot = 0;
        timeSinceShot = 0;
        GameObject audioObject = GameObject.Find("AudioManager");
        if (audioObject != null )
        {
            audio = audioObject.GetComponent<AudioManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shooting && timeSinceShot > timeBetweenShots)
        {
            Blast ring = Instantiate(ringPrefab, shootPoint.position, shootPoint.rotation);
            ring.setGun(this);
            shot++;
            timeSinceShot = 0;
        } else
        {
            timeSinceShot += Time.deltaTime;
        }

        if (shot >= ringsPerShot)
        {
            shooting = false;
            shot = 0;
        }
    }

    public void Shoot()
    {
        if (!shooting)
        {
            setShooting(true);
            audio.BlastSound();
        }
    }

    public void setShooting(bool state)
    {
        shooting = state;
    }
    public bool isShooting()
    {
        return shooting;
    }
}
