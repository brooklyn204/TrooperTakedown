using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ahsoka : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float speed;
    [SerializeField] float turnSpeed;
    [SerializeField] AnimationClip slashClip;
    [SerializeField] GameObject center;
    [SerializeField] float moveRadius;
    [SerializeField] GameManager manager;

    private AudioManager audio;
    private InputAction runAction;
    public PlayerStates state;
    private float direction;
    // Start is called before the first frame update
    void Start()
    {
        state = PlayerStates.Idle;

        GameObject audioObject = GameObject.Find("AudioManager");
        if (audioObject != null)
        {
            audio = audioObject.GetComponent<AudioManager>();
        }
    }

    void FixedUpdate()
    {
        // Rotate according to turn direction (or don't, if direction is 0)
        transform.Rotate(0.0f, turnSpeed * direction, 0.0f, Space.World);

        if (state == PlayerStates.Run)
        {
            Vector3 moveDirection = transform.forward;
            float distanceToCenter = Vector3.Distance(transform.position, center.transform.position);
            bool inBounds = distanceToCenter < moveRadius;

            float newDistanceToCenter = Vector3.Distance(transform.position + speed * transform.forward, center.transform.position);
            bool movingTowardsCenter = newDistanceToCenter < distanceToCenter;

            // Only move if the character is in the area allowed or moving back towards the center
            if (inBounds || movingTowardsCenter)
            {
                transform.position = transform.position + speed * transform.forward;
            }
        }
        
    }

    public void OnRun(InputValue value)
    {
        // cannot run while slashing or dying
        if (state == PlayerStates.Slash || state == PlayerStates.Death)
        {
            return;
        }

        float result = value.Get<float>();


        if (result == 1)
        {
            state = PlayerStates.Run;
            animator.SetBool("Running", true);
        }
        if (result == 0)
        {
            state = PlayerStates.Idle;
            animator.SetBool("Running", false);
        }
        
    }

    public void OnTurn(InputValue value)
    {
        float result = value.Get<float>();
        direction = result;
    }

    public void OnSwing()
    {
        // Can't swing if dead or running
        if (state == PlayerStates.Death || state == PlayerStates.Run)
        {
            return;
        }
        animator.SetBool("Slashing",true);
        state = PlayerStates.Slash;
    }


    public void FinishSwing()
    {
        animator.SetBool("Slashing", false);
        state = PlayerStates.Idle;
    }

    // When a Blast hits a character, it just plays the death animation (since it doesn't know what type of object it hit).
    // If the death animation is playing, then we must be in the death state (this is a little funky but gets the job done).
    public void OnDeath()
    {
        state = PlayerStates.Death;
        audio.AhsokaDeathSound();
        manager.GameOver(false);
    }
}


public enum PlayerStates
{
    Idle,
    Run,
    Slash,
    Death
};