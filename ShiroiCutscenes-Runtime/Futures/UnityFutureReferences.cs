using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Futures {
    [Serializable]
    public class GameObjectFutureReference : FutureReference<GameObject> { }

    [Serializable]
    public class TransformFutureReference : FutureReference<Transform> { }

    [Serializable]
    public class AnimatorFutureReference : FutureReference<Animator> { }

    [Serializable]
    public class AudioSourceFutureReference : FutureReference<AudioSource> { }

    [Serializable]
    public class RigidbodyFutureReference : FutureReference<Rigidbody> { }

    [Serializable]
    public class ObjectFutureReference : FutureReference<Object> { }
}