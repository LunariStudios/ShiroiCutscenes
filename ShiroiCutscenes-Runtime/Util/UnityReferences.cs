using System;
using UnityEngine;
using UnityEngine.Playables;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Util {
    [Serializable]
    public class GameObjectReference : Reference<GameObject> { }

    [Serializable]
    public class TransformReference : Reference<Transform> { }

    [Serializable]
    public class AnimatorReference : Reference<Animator> { }

    [Serializable]
    public class AudioSourceReference : Reference<AudioSource> { }

    [Serializable]
    public class RigidbodyReference : Reference<Rigidbody> { }

    [Serializable]
    public class ObjectReference : Reference<Object> { }

    [Serializable]
    public class DirectorReference : Reference<PlayableDirector> { }
}