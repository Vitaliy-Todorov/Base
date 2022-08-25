﻿using UnityEngine;

namespace Scripts.Infrastructure.Services.InputService
{
    class KeyboardMouseInputServuce : IInputService
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";

        public Vector3 Axis
        {
            get
            {
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                    return new Vector3(Input.GetAxis(Horizontal), 0, Input.GetAxis(Vertical));
                else
                    return Vector3.zero;
            }
        }
        public bool AttackButtonUp => Input.GetMouseButtonDown(0);
    }
}