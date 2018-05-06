using ManagerClasses;
using UnityEngine;

namespace BlockClasses
{
    [SelectionBase]
    public class BlockExit : Block
    {
        public override bool BlockEffect (IBot bot_)
        {
            bot_.DeathEffect (0.8f);
            BotManager.instance.DecrementBotCount ();
            return true;
        }
    }
}