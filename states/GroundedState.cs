using UnityEngine;
using System.Collections;

public class GroundedState : PlayerState
{
    // standard physics values to be set here

    // upon movement input, do a horizontal movement along the ground, then check if there is still ground underneath
    // can jump
    // can attack

    PlayerState HandleInput()
    {
        if (Input.GetAxisRaw("Horizontal") != 0) // If a movement input is given
        {
            
        }
        if (Input.GetButton("Jump"))
        {
            return new JumpingState();
        }
        if (Input.GetButton("Fire1"))
        {

        }
        return this;
    }
   
}
