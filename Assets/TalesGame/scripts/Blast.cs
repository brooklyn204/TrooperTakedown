using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour
{
    [SerializeField] Gun gun;
    [SerializeField] float initSize;
    [SerializeField] float scalePerFrame = 0.0001f;
    [SerializeField] float range;
    [SerializeField] float speed;
    [SerializeField] string playerTag = "Player";
    [SerializeField] string weaponTag = "Lightsaber";
    [SerializeField] string enemyTag = "Clone";

    // Start is called before the first frame update
    void Start()
    {
        // Set up initial scale
        Vector3 scale = Vector3.one;
        scale.x = initSize;
        scale.y = initSize;
        scale.z = transform.localScale.z;
        transform.localScale = scale;
        // Rotate model to face forward
        Vector3 rotationToAdd = new Vector3(270, 0, 0);
        transform.Rotate(rotationToAdd);
        transform.parent = null;

    }

    // Update is called once per frame
    void Update()
    {
        // Move forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
            
        // Increase size
        transform.localScale = new Vector3(transform.localScale.x + scalePerFrame, transform.localScale.y + scalePerFrame, transform.localScale.z);

        // If blast is out of range
        if (isOutOfRange()) { 
            Destroy(gameObject);
        }
           
    }

    public bool isOutOfRange() { 
        return Vector3.Distance(transform.position, gun.transform.position) > range; 
    }
    public void setGun(Gun newGun) { gun = newGun; }

    void OnTriggerEnter(Collider other)
    {
        // Get GameObject hit
        Transform otherTransform = other.transform;
        while (otherTransform.parent != null && otherTransform.tag != playerTag && otherTransform.tag != weaponTag && otherTransform.tag != enemyTag) {
            otherTransform = otherTransform.parent;
        }
        
        GameObject objectHit = otherTransform.gameObject;
        
        if (objectHit.tag == weaponTag)
        {
            Lightsaber lightsaber = objectHit.GetComponent<Lightsaber>();
            if (lightsaber != null && lightsaber.player.state == PlayerStates.Slash)
            {
                speed = -speed;
            }
        } else if (objectHit.tag == playerTag || objectHit.tag == enemyTag) // If the object is the player or an enemy, play the dying animation and delete the blast
        {
            objectHit.GetComponent<Animator>().SetTrigger("bodyHit");
            Destroy(gameObject);
        } 
    }

}