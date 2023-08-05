using UnityEngine;

public class Player : PlayersOrigin
{
    private Game game;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && _isAlive > 0)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.S) && _isAlive > 0)
        {
            Crunch();
        }

        if (_isAlive <= 0)
        {
            game.Died();
        }

    }

    public void SetGame(Game game)
    {
        this.game = game;
    }


}
