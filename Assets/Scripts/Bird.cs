using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Scripts.Enums;

public class Bird : MonoBehaviour
{
    public event EventHandler OnGameOver;
    public event EventHandler OnStartedPlaying;

    private GameState state;
    
    private static Bird instance;
    public static Bird GetInstance() { return instance; }

    private Rigidbody2D rigidbody2D;
    private const float JUMP_AMOUNT = 100f;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        instance = this;
        rigidbody2D.bodyType = RigidbodyType2D.Static;
        state = GameState.WaitingToStart;
    }
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        switch(state)
        {
            case GameState.WaitingToStart:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                    state = GameState.Playing;
                    rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    if (OnStartedPlaying != null) {  OnStartedPlaying(this, EventArgs.Empty); }
                }
                break;
            case GameState.Playing:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                    Jump();
                }
                break;
            case GameState.GameOver:
                break;
            default:
                break;
        }
        
    }

    private void Jump() 
    {
        rigidbody2D.velocity = Vector2.up * JUMP_AMOUNT;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        rigidbody2D.bodyType = RigidbodyType2D.Static;
        if (OnGameOver != null) { OnGameOver(this, EventArgs.Empty); }
    }
}
