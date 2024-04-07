using UnityEngine;

public class FishIdle : IState<Fish>
{
    private Fish fish;

    public void Enter(Fish fish)
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