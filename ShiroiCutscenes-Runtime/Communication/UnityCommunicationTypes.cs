using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Communication {
    [Serializable]
    public sealed class Vector2Input : Input<Vector2> { }

    [Serializable]
    public sealed class Vector3Input : Input<Vector3> { }

    [Serializable]
    public sealed class Vector4Input : Input<Vector4> { }

    [Serializable]
    public sealed class ObjectInput : Input<Object> { }

    [Serializable]
    public sealed class GameObjectInput : Input<GameObject> { }

    [Serializable]
    public sealed class ComponentInput : Input<Component> { }

    [Serializable]
    public sealed class BehaviourInput : Input<Behaviour> { }

    [Serializable]
    public sealed class MonoBehaviourInput : Input<MonoBehaviour> { }

    [Serializable]
    public sealed class Vector2Output : Output<Vector2> { }

    [Serializable]
    public sealed class Vector3Output : Output<Vector3> { }

    [Serializable]
    public sealed class Vector4Output : Output<Vector4> { }

    [Serializable]
    public sealed class ObjectOutput : Output<Object> { }

    [Serializable]
    public sealed class GameObjectOutput : Output<GameObject> { }

    [Serializable]
    public sealed class ComponentOutput : Output<Component> { }

    [Serializable]
    public sealed class BehaviourOutput : Output<Behaviour> { }

    [Serializable]
    public sealed class MonoBehaviourOutput : Output<MonoBehaviour> { }
}