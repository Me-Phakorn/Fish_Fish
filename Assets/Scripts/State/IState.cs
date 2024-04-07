public interface IState<T> {
    void Enter(T t);
    void Execute();
    void Exit();
}