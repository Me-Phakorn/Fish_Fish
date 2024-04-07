public class FishDead : IState<Fish>
{
    private Fish fish;
    public void Enter(Fish fish)
    {
        this.fish = fish;
        this.fish.Dead();
    }

    public void Execute()
    {
        //ไม่ถูกทำงาน
    }

    public void Exit()
    {
        
    }
}