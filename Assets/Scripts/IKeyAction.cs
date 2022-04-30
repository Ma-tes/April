using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    internal interface IKeyAction
    {
        public KeyCode Key { get; }

        public IEnumerable<Vector3> ActionMoves();
    }
}
