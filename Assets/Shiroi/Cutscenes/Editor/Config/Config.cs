using System;
using UnityEditor;
using UnityEngine;

namespace Shiroi.Cutscenes.Editor.Config {
    public abstract class Config {
        public Config(string key, string label, string description) {
            Key = key;
            Label = label;
            Description = description;
        }

        public string Description {
            get;
            private set;
        }

        public string Label {
            get;
            private set;
        }

        public string Key {
            get;
            private set;
        }

        public void DrawGUI() {
            EditorGUILayout.BeginVertical(ShiroiStyles.DefaultBackground);
            EditorGUILayout.LabelField(Label, ShiroiStyles.Bold);
            EditorGUILayout.LabelField(Description);
            DoFieldGUI();
            GUILayout.Space(ShiroiStyles.SpaceHeight);
            EditorGUILayout.EndVertical();
        }

        public abstract void DoFieldGUI();
    }

    public abstract class Config<T> : Config {
        public T DefaultValue {
            get;
            private set;
        }

        protected Config(string key, string label, string description, T defaultValue) : base(key, label, description) {
            DefaultValue = defaultValue;
        }

        public event Action<T> OnChanged;

        public T Value {
            get {
                return EditorPrefs.HasKey(Key) ? GetValue(Key) : DefaultValue;
            }
            set {
                SetValue(Key, value);
                if (OnChanged != null) {
                    OnChanged(value);
                }
            }
        }

        protected abstract T GetValue(string key);

        protected abstract void SetValue(string key, T value);

        public static implicit operator T(Config<T> config) {
            return config.Value;
        }
    }

    public class BooleanConfig : Config<bool> {
        public BooleanConfig(string key, string label, string description, bool value) : base(key, label, description,
            value) { }

        protected override bool GetValue(string key) {
            return EditorPrefs.GetBool(key);
        }

        protected override void SetValue(string key, bool value) {
            EditorPrefs.SetBool(key, value);
        }

        public override void DoFieldGUI() {
            Value = EditorGUILayout.Toggle(Value);
        }
    }

    public class IntegerConfig : Config<int> {
        public IntegerConfig(string key, string label, string description, int value) : base(key, label, description,
            value) { }

        protected override int GetValue(string key) {
            return EditorPrefs.GetInt(key);
        }

        protected override void SetValue(string key, int value) {
            EditorPrefs.SetInt(key, value);
        }

        public override void DoFieldGUI() {
            Value = EditorGUILayout.IntField(Value);
        }
    }

    public class FloatConfig : Config<float> {
        public FloatConfig(string key, string label, string description, float value) : base(key, label, description,
            value) { }

        protected override float GetValue(string key) {
            return EditorPrefs.GetFloat(key);
        }

        protected override void SetValue(string key, float value) {
            EditorPrefs.SetFloat(key, value);
        }

        public override void DoFieldGUI() {
            Value = EditorGUILayout.FloatField(Value);
        }
    }

    public class StringConfig : Config<string> {
        public StringConfig(string key, string label, string description, string value) : base(key, label, description,
            value) { }

        protected override string GetValue(string key) {
            return EditorPrefs.GetString(key);
        }

        protected override void SetValue(string key, string value) {
            EditorPrefs.SetString(key, value);
        }

        public override void DoFieldGUI() {
            Value = EditorGUILayout.TextField(Value);
        }
    }
}