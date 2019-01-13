using System;
using System.Collections;
using System.Collections.Generic;
using Shiroi.Cutscenes.Communication;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Shiroi.Cutscenes.Tokens {
    public class DynamicOutput : Output {
        private Type type;

        public DynamicOutput(Type type, string name) {
            this.type = type;
            Name = name;
        }

        public override Type GetOutputType() {
            return type;
        }

        public void Apply(object obj, Context context) {
            if (!GetOutputType().IsInstanceOfType(obj)) {
                throw new System.Exception(
                    $"Expected object that inherits from {GetOutputType().Name}, but got {obj}!"
                );
            }

            context.Enqueue(Name, obj);
        }
    }

    public class GetObjectFromSceneToken : Token, IOutputContext {
        [SerializeField]
        private string typeName;

        [SerializeField]
        private string outputName;

        [SerializeField]
        private int instanceId;

        public string TypeName {
            get => typeName;
            set {
                typeName = value;
                ResetCache();
            }
        }

        public int InstanceId {
            get => instanceId;
            set {
                instanceId = value;
                ResetCache();
            }
        }

        public string OutputName {
            get => outputName;
            set {
                outputName = value;
                ResetCache();
            }
        }

        private DynamicOutput cachedOutput;

        public Type Type {
            get => typeName == null ? null : Type.GetType(typeName);
            set => typeName = value.AssemblyQualifiedName;
        }

        public DynamicOutput Output {
            get {
                var t = Type;
                if (t == null) {
                    return null;
                }

                return cachedOutput ?? (cachedOutput = new DynamicOutput(t, OutputName));
            }
        }

        public Object ActiveObject {
            get {
                var roots = SceneManager.GetActiveScene().GetRootGameObjects();
                if (Type == typeof(GameObject)) {
                    foreach (var gameObject in roots) {
                        if (RecurseFind(gameObject, out var obj)) {
                            return obj;
                        }
                    }
                } else {
                    foreach (var gameObject in roots) {
                        var components = gameObject.GetComponentsInChildren(Type, true);
                        foreach (var component in components) {
                            if (component.GetInstanceID() == instanceId) {
                                return component;
                            }
                        }
                    }
                }

                return null;
            }
            set => instanceId = value == null ? 0 : value.GetInstanceID();
        }

        private bool RecurseFind(GameObject gameObject, out GameObject o) {
            if (gameObject.GetInstanceID() == instanceId) {
                o = gameObject;
                return gameObject;
            }

            for (var i = 0; i < gameObject.transform.childCount; i++) {
                var c = gameObject.transform.GetChild(i).gameObject;
                if (c.GetInstanceID() != instanceId) {
                    continue;
                }

                o = c;
                return true;
            }

            o = null;
            return false;
        }

        public override IEnumerator Execute(CutsceneExecutor executor) {
            var found = FindObjectsOfType(Type);
            foreach (var o in found) {
                if (o.GetInstanceID() == instanceId) {
                    Output.Apply(found, executor.Context);
                    yield break;
                }
            }
        }

        private void ResetCache() {
            cachedOutput = null;
        }

        public IEnumerable<Output> GetOutputs() {
            var o = Output;
            if (o == null) {
                yield break;
            }

            yield return o;
        }
    }
}