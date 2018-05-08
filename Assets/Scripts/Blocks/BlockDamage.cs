using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockClasses
{
    [SelectionBase]
    public class BlockDamage : Block
    {
        public override bool BlockEffect(IBot bot_)
        {
            Debug.Log("Damage Effect.");
            bot_.GetHealth.Damage(int.MaxValue);
            bot_.DeathEffect(0.8f);
            return true;
        }
    }
}