using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockClasses
{
    public class BlockDamage : Block
    {
        public override bool BlockEffect (IBotMovement bot_)
        {
            Debug.Log ("Damage Effect.");
            bot_.GetHealth.Damage (int.MaxValue);
            return true;
        }
    }
}