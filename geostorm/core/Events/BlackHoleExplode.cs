using System;
using System.Numerics;

namespace geostorm.core.Events
{
    class BlackHoleExplode : Event
    {
        public BlackHole blackHole;
        public int frameCount;

        public BlackHoleExplode(BlackHole blackHole)
        {
            this.blackHole = blackHole;
            frameCount = 0;
        }
    }
}
