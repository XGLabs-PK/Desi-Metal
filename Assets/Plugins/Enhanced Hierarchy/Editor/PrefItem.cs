using System;
using UnityEditor;
using UnityEngine;

namespace EnhancedHierarchy
{
    /// <summary>
    /// Generic preference item interface.
    /// </summary>
    public interface IPrefItem
    {
        bool Drawing { get; }
        object Value { get; set; }

        GUIContent Label { get; }

        //void DoGUI();

        GUIEnabled GetEnabledScope();
        GUIEnabled GetEnabledScope(bool enabled);
        GUIFade GetFadeScope(bool enabled);
    }

    /// <summary>
    /// Generic preference item.
    /// </summary>
    [Serializable]
    public sealed class PrefItem<T> : IPrefItem
    {
        [Serializable]
        struct Wrapper
        {
            [SerializeField]
            public T value;
        }

        const string KEY_PREFIX = "EH.";

        string key;
        Wrapper wrapper;
        T defaultValue;

        readonly GUIFade fade;

        public GUIContent Label { get; private set; }

        public bool Drawing => fade.Visible;

        public T DefaultValue
        {
            get => defaultValue;
            set => SetDefaultValue(value);
        }

        public T Value
        {
            get => wrapper.value;
            set => SetValue(value, false);
        }

        bool UsingDefaultValue => !EditorPrefs.HasKey(key);

        object IPrefItem.Value
        {
            get => Value;
            set => Value = (T)value;
        }

        public PrefItem(string key, T defaultValue, string text = "", string tooltip = "")
        {
            this.key = KEY_PREFIX + key;
            this.defaultValue = defaultValue;

            Label = new GUIContent(text, tooltip);
            fade = new GUIFade();

            Preferences.contents.Add(Label);
            Preferences.onResetPreferences += ResetValue;

            if (UsingDefaultValue)
                wrapper.value = Clone(defaultValue);
            else
                LoadValue();
        }

        public void SetDefaultValue(T newDefault)
        {
            if (UsingDefaultValue)
                wrapper.value = Clone(newDefault);

            defaultValue = newDefault;
        }

        void LoadValue()
        {
            try
            {
                if (!EditorPrefs.HasKey(key))
                    return;

                string json = EditorPrefs.GetString(key);

                // if(Preferences.DebugEnabled)
                //    Debug.LogFormat("Loading preference {0}: {1}", key, json);

                wrapper = JsonUtility.FromJson<Wrapper>(json);
            }
            catch (Exception e)
            {
                Debug.LogWarningFormat("Failed to load preference item \"{0}\", using default value: {1}", key,
                    defaultValue);

                Debug.LogException(e);
                ResetValue();
            }
        }

        void SetValue(T newValue, bool forceSave)
        {
            try
            {
                if (Value != null && Value.Equals(newValue) && !forceSave)
                    return;

                wrapper.value = newValue;

                string json = JsonUtility.ToJson(wrapper, Preferences.DebugEnabled);

                // if(Preferences.DebugEnabled)
                //    Debug.LogFormat("Saving preference {0}: {1}", key, json);

                EditorPrefs.SetString(key, json);
            }
            catch (Exception e)
            {
                Debug.LogWarningFormat("Failed to save {0}: {1}", key, e);
                Debug.LogException(e);
            }
            finally
            {
                wrapper.value = newValue;
            }
        }

        void ResetValue()
        {
            if (UsingDefaultValue)
                return;

            if (Preferences.DebugEnabled)
                Debug.LogFormat("Deleted preference {0}", key);

            wrapper.value = Clone(defaultValue);
            EditorPrefs.DeleteKey(key);
        }

        public void ForceSave()
        {
            SetValue(wrapper.value, true);
        }

        T Clone(T other)
        {
            if (typeof(T).IsValueType)
                return other;

            Wrapper wrapper = new Wrapper() { value = other };
            string json = JsonUtility.ToJson(wrapper, Preferences.DebugEnabled);
            Wrapper clonnedWrapper = JsonUtility.FromJson<Wrapper>(json);

            // if(Preferences.DebugEnabled)
            //     Debug.LogFormat("Clone of {0}: {1}", key, json);

            return clonnedWrapper.value;
        }

        public GUIEnabled GetEnabledScope()
        {
            return GetEnabledScope(Value.Equals(true));
        }

        public GUIEnabled GetEnabledScope(bool enabled)
        {
            return new GUIEnabled(enabled);
        }

        public GUIFade GetFadeScope(bool enabled)
        {
            fade.SetTarget(enabled);
            return fade;
        }

        public static implicit operator T(PrefItem<T> pb)
        {
            if (pb == null)
            {
                Debug.LogError("Cannot get the value of a null PrefItem");
                return default;
            }

            return pb.Value;
        }

        public static implicit operator GUIContent(PrefItem<T> pb)
        {
            if (pb == null)
            {
                Debug.LogError("Cannot get the content of a null PrefItem");
                return new GUIContent("Null PrefItem");
            }

            return pb.Label;
        }
    }
}
