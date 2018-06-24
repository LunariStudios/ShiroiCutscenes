using System;
using System.Reflection.Emit;
using Shiroi.Cutscenes.Tokens;
using Shiroi.Cutscenes.Util;
using UnityEngine;

namespace Shiroi.Cutscenes.Futures {
    [Serializable]
    public sealed class ExpectedFuture : IComparable<ExpectedFuture> {
        [SerializeField]
        private string typeName;

        [SerializeField]
        private string name;

        /**
         * The id of this future, this is the number that will be used when trying to find a future. 
         */
        [SerializeField]
        private int id;

        /**
         * The provider of this future, this is the token that is provider for providing this future
         */
        [SerializeField]
        private Token provider;

        public ExpectedFuture(string name, int id, Token provider, Type futureType) {
            this.name = name;
            this.id = id;
            this.provider = provider;
            typeName = futureType.AssemblyQualifiedName;
            cachedType = futureType;
        }

        public int Id {
            get {
                return id;
            }
        }

        public Token Provider {
            get {
                return provider;
            }
        }

        private Type cachedType;

        public Type Type {
            get {
                return cachedType ?? (cachedType = LoadType());
            }
        }

        private Type LoadType() {
            return Type.GetType(typeName);
        }

        public string Name {
            get {
                return name;
            }
            set {
                name = value;
            }
        }


        public int CompareTo(ExpectedFuture other) {
            return id.CompareTo(other.id);
        }

        public override string ToString() {
            return string.Format(
                "ExpectedFuture(Type: {0}, Name: {1}, ID: {2}, Provider: {3})",
                typeName,
                name,
                id,
                provider);
        }
    }
}