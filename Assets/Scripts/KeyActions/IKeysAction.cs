using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    internal interface IKeysAction<TSource>
    {
        public KeyValuePair<KeyCode, TSource>[] Actions { get; }

        public IEnumerable<Vector3> ActionMoves(TSource action);
    }
}
