using System;
using UnityEngine;

public class Player : PlayersOrigin
{

    void Update()
    {
         // if (!isAnimating) 
         // {
            if (Input.GetKeyDown(KeyCode.W) && _isAlive > 0)
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.S) && _isAlive > 0)
            {
                Crunch();
            }

         // }

    }

}
