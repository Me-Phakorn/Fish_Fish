using UnityEngine;

public class BossIdle : IState<Boss>
{
    private Boss fish;

    public void Enter(Boss fish)
    {
        this.fish = fish;
    }

    public void Execute()
    {
        fish.Idle();
    }

    public void Exit()
    {
        Debug.Log("Exit Idle");
    }
}