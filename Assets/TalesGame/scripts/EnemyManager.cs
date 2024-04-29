using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] List<Clone> clones;
    private List<Clone> activeClones;
    [SerializeField] float timeBetweenShots;
    [SerializeField] GameManager manager;
    private float timeSinceLastShot;
    private int numDead = 0;
    private int numActiveClones;
    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastShot = 0;
        
        activeClones = new List<Clone>(clones);
        numActiveClones = activeClones.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.gameOver && manager.started) {
            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot > timeBetweenShots)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        // Pick a random clone
        int randomIndex = Random.Range(0, activeClones.Count);
        Clone nextToShoot = activeClones[randomIndex];
        // Have him shoot
        nextToShoot.Shoot();
        // Reset the time since last shot
        timeSinceLastShot = 0;
    }

    public void UpdateDeaths()
    {
        // Figure out which clone died, remove it from the active clones
        int indexToRemove = -1;
        for (int i = 0; i < activeClones.Count; i++) { 
            if (activeClones[i].IsDead())
            {
                indexToRemove = i;
            }
        }
        if (indexToRemove != -1)
        {
            activeClones.RemoveAt(indexToRemove);
        }

        // Update the number of dead clones and set gameOver if necessary
        numDead++;
        if (numDead == clones.Count)
        {
            manager.GameOver(true);
        }
    }

}
