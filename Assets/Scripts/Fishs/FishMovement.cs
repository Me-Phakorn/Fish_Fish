public class FishMovement : IState<Fish>
{
    private Fish fish;
    public void Enter(Fish fish)
    {
        this.fish = fish;
    }

    public void Execute()
    {
        fish.Movement();
    }

    public void Exit()
    {
        
    }
}