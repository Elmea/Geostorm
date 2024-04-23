using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geostorm.core
{
    interface Isystem
    {
        public void Update(in GameInput input, GameData data, List<Event> events);
    }
}
