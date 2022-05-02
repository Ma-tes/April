using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    internal interface IKeyAction<T>
    {
        public KeyCode Key { get; }

        public IEnumerable<T> ActionMoves();
    }
}
