using System;
using System.Collections.Generic;
using System.Linq;
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

        [SerializeField]
        public GameObject Entity;

        public AnimationSelecter(T source, TOutput output, GameObject entity) 
        {
            Source = source;
            Output = output;
            Entity = entity;
        }

        public static AnimationSelecter<TSource, TOutPut> GetCurrentAnimation<TSource, TOutPut>(IEnumerable<AnimationSelecter<TSource, TOutPut>> animations, TSource source)
        {
            return animations.First(n => n.Source.Equals(source));
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
