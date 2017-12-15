using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    PlayerState currentState;
    PlayerState nextState;
    
    // Use this for initialization
    void Start()
    {
        currentState = new GroundedState();
    }

    // Update is called once per frame
    void Update()
    {
        //nextState = currentState.HandleInput();

    }
}
