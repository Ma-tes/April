using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    internal partial class AnimationSelecter<T, TOutput>
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

        public static TOutPut GetAnimationOutput<TSource, TOutPut>(IEnumerable<AnimationSelecter<TSource, TOutPut>> animations, TSource source)
        {
            return animations.First(n => n.Source.Equals(source)).Output;
        }
    }

    internal partial class AnimationSelecter 
    {
        public static void PlayAnimationClip(GameObject target, AnimationClip animationClip, WrapMode wrapMode = WrapMode.Default)
        {
            Animation animationComponent = target.GetComponent<Animation>();
            if (animationComponent is null)
                Debug.LogWarning($"Your gameobject doesn't have any Animation component");
            else 
            {
                animationComponent.AddClip(animationClip, $"{animationClip.name}");
                animationComponent.clip = animationClip;
                animationClip.wrapMode = wrapMode;
                animationComponent.Play();
            }
        }
    }
}
