public class BossDead : IState<Boss>
{
    private Boss fish;
    public void Enter(Boss fish)
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