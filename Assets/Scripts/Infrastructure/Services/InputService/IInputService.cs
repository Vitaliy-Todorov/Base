using UnityEngine;

namespace Scripts.Infrastructure.Services.InputService
{
    public interface IInputService : IService
    {
        Vector3 Axis { get; }

        bool AttackButtonUp { get; }
    }
}