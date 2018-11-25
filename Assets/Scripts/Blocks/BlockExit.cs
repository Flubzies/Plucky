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
            if (BotManager.instance != null) BotManager.instance.DecrementBotCount ();
            else Debug.Log ("Bot Manager is null!");
            return true;
        }
    }
}