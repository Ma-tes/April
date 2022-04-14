using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    internal sealed class AnimationSelecter<T, TOutput> where TOutput : Animator
    {
        [SerializeField]
        public T Source;

        [SerializeField]
        public TOutput Output;

        public AnimationSelecter(T source, TOutput output) 
        {
            Source = source;
            Output = output;
        }

        public static TOutPut GetAnimationOutput<TSource, TOutPut>(IEnumerable<AnimationSelecter<TSource, TOutPut>> animations, TSource source) where TOutPut : Animator
        {
            return animations.First(n => n.Source.Equals(source)).Output;
        }
    }
}
