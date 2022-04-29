using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_AI : MonoBehaviour
{
    public enum State
    {
        IDLE,
        RUNNING,
        STUNNED,
        JUMPING,
        DEAD
    };

    public State state = State.IDLE;
    public float speed = 1.0f;
    public float health = 100.0f;
    public Transform lava;

    void TransitionTo(State transition_to)
    {
        OnExit(transition_to);
        State currentState = state;
        state = transition_to;
        OnEnter(currentState);
    }
    void OnExit(State entering)
    {
        print("Exiting: " + state.ToString() + " to: " + entering.ToString());
    }
    void OnEnter(State exiting)
    {
        print("Entering: " + state.ToString() + " from: " + exiting.ToString());

        switch (state)
        {
            case State.IDLE:
                {
                    SpriteRenderer sprite = GetComponent<SpriteRenderer>();
                    sprite.color = Color.white;
                    break;
                }
            case State.RUNNING:
                {
                    SpriteRenderer sprite = GetComponent<SpriteRenderer>();
                    sprite.color = Color.cyan;
                    break;
                }
            case State.DEAD:
                {
                    SpriteRenderer sprite = GetComponent<SpriteRenderer>();
                    sprite.color = Color.red;
                    break;
                }
        }
    }
    void OnUpdate()
    {
        print("Current State: " + state.ToString());

        switch(state)
        {
            case State.IDLE:
            {
                HandleInLava();

                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                bool playerHasGivenInput = horizontal != 0.0f || vertical != 0.0f;
                bool playerIsDead = health <= 0.0f;
                
                if(playerIsDead)
                {
                    TransitionTo(State.DEAD);
                }
                else if (playerHasGivenInput)
                {
                    TransitionTo(State.RUNNING);
                }
                else
                {
                    // Play IDLE animation.
                }

                break;
            }
            case State.RUNNING:
            {
                HandleInLava();

                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                bool playerHasGivenInput = horizontal != 0.0f || vertical != 0.0f;
                bool playerIsDead = health <= 0.0f;
                
                if (playerIsDead)
                {
                    TransitionTo(State.DEAD);
                }
                else if (playerHasGivenInput)
                {
                    Vector3 movement_direction = new Vector3(horizontal, vertical, 0.0f);
                    movement_direction.Normalize();

                    Vector3 movement_this_frame = movement_direction * speed * Time.deltaTime;

                    transform.position += movement_this_frame;
                }
                else
                {
                    TransitionTo(State.IDLE);
                }

                break;
            }
        }
    }
    private void Start()
    {
        OnEnter(state);
    }

    void HandleInLava()
    {
        Vector3 thisToLava = lava.position - transform.position;
        bool isInLava = thisToLava.magnitude < (this.transform.localScale.x / 2) + (lava.transform.localScale.x / 2);
        if(isInLava)
        {
            print("ASJDHASDHASBDAHSBDHUABSDhiABXDHBASHUDBASHUDBAUSBDIUHABSDBASHUDBAUHSDB");
            health -= 10.0f * Time.deltaTime;
        }
    }

    void Update()
    {
        OnUpdate();
    }
}

// Implement your own state machine for a non-player entity
    // Goblin
           // Chase player
           // Dead state
           // if lower than 10hp, run away 


// CHasing