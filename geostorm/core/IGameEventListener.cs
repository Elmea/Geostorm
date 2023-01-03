using System.Collections.Generic;

namespace geostorm
{
    interface IGameEventListener
    {
        public void HandleEvent(core.GameData data, in core.GameInput input, List<core.Event> events);
    }
}
