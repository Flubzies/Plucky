using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockClasses
{
    public class BlockTurn : Block
    {
        TrailRenderer[] _tr;
        ParticleSystem _ps;

        protected override void Awake ()
        {
            base.Awake ();
            _tr = GetComponentsInChildren<TrailRenderer> ();
            _ps = GetComponent<ParticleSystem> ();
            ParticleSystem.ShapeModule s = _ps.shape;
            s.mesh = GetComponent<MeshFilter> ().mesh;
            Debug.Log (s.mesh);
        }

        public override bool BlockEffect (IBot bot_)
        {
            bot_.BotRotation (Quaternion.LookRotation (transform.forward));
            return true;
        }

        public void PlaceBlock (Vector3 pos_)
        {
            foreach (TrailRenderer tr in _tr) tr.Clear ();
            transform.position = pos_;
        }
    }
}