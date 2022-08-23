using System;
using UnityEngine;

namespace SCP939infection
{
    [Serializable]
    public sealed class SerializableVector3
    {
        public float x { get; set; } = 0f;
        public float y { get; set; } = 0f;
        public float z { get; set; } = 0f;

        public SerializableVector3()
        {

        }

        public SerializableVector3(float x, float y, float z) : this()
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator Vector3(SerializableVector3 target) => new Vector3(target.x, target.y, target.z);
    }
}