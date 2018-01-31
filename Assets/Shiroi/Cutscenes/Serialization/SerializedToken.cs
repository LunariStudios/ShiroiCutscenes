using System;
using Shiroi.Cutscenes.Tokens;
using UnityEngine;

namespace Shiroi.Cutscenes.Serialization {
    [Serializable]
    public struct SerializedToken {
        [SerializeField]
        public string TokenType;

        [SerializeField]
        public SerializedObject TokenData;

        private SerializedToken(string tokenType, SerializedObject tokenData) {
            TokenType = tokenType;
            TokenData = tokenData;
        }

        public IToken Deserialize() {
            var type = Type.GetType(TokenType);
            if (type == null) {
                Debug.LogFormat("[ShiroiCutscenes] Couldn't find type of token '{0}'! Skipping.", TokenType);
                return null;
            }
            var token = (IToken) Activator.CreateInstance(type);
            TokenData.Deserialize(token);
            return token;
        }

        public static SerializedToken From(IToken loadedToken) {
            var typeName = loadedToken.GetType().FullName;
            var obj = SerializedObject.From(loadedToken);
            return new SerializedToken(typeName, obj);
        }
    }
}