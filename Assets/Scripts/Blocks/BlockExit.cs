using UnityEngine;

namespace BlockClasses
{
    public class BlockExit : Block
    {
        public override bool BlockEffect (IBotMovement bot_)
        {
            // Debug.Log ("Level Complete!");
            return true;
        }
    }
}