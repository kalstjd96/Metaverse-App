public interface IStampState
{
    void Enter(int areaNum);
    void Update();
    void Exit(bool isCompelete = false);
}
