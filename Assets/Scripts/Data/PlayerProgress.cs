
using System;
using System.Collections.Generic;

namespace Scripts.Data
{
    [Serializable]
    public partial class PlayerProgress
    {
        public WorldData WorldData;

        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
        }
    }
}