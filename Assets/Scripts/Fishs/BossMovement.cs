public class BossMovement : IState<Boss>
{
    private Boss fish;
    public void Enter(Boss fish)
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