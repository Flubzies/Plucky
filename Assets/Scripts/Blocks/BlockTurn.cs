using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockClasses
{
    public class BlockTurn : Block, IPlaceable
    {
        TrailRenderer[] _tr;

        protected override void Awake ()
        {
            base.Awake ();
            _tr = GetComponentsInChildren<TrailRenderer> ();
        }

        public override bool BlockEffect (IBotMovement bot_)
        {
            bot_.BotRotation (Quaternion.LookRotation (transform.forward));
            return true;
        }

        public Transform GetGhostBlock { get { return _blockGhost; } }

        public void PlaceBlock (Vector3 pos_)
        {
            foreach (TrailRenderer tr in _tr) tr.Clear ();
            transform.position = pos_;
        }

    }
}