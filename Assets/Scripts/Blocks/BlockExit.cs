using UnityEngine;

namespace BlockClasses
{
    [SelectionBase]
    public class BlockExit : Block
    {
        public override bool BlockEffect (IBot bot_)
        {
            
            return true;
        }
    }
}