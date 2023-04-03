using UnityEngine;

public interface ICharacterController
{
    void Movement();
    void Run();
    void Dodge();
    void Action();
    void Flip(bool value, float posX, float posY);
}

