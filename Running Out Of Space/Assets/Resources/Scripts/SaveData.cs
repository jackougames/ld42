using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SaveData : ScriptableObject {

    [Serializable]
    public class KeyValuePair<T> {
        public List<string> keys = new List<string>();
        public List<T> values = new List<T>();

        public void Clear() {
            keys.Clear();
            values.Clear();
        }

        public void SetValue(string key, T value) {

            int index = keys.FindIndex(x => x == key);

            if (index > -1) {
                //Found something
                values[index] = value;
            }
            else {
                keys.Add(key);
                values.Add(value);
            }

        }

        public bool GetValue(string key, ref T value) {

            int index = keys.FindIndex(x => x == key);

            if (index > -1) {
                //Found something
                value = values[index];
                return true;
            }

            return false;
        }
    }

    public KeyValuePair<bool> boolKeyValuePairLists = new KeyValuePair<bool>();
    public KeyValuePair<int> intKeyValuePairLists = new KeyValuePair<int>();
    public KeyValuePair<string> stringKeyValuePairLists = new KeyValuePair<string>();
    public KeyValuePair<Vector3> vector3KeyValuePairLists = new KeyValuePair<Vector3>();

    private void Save<T>(KeyValuePair<T> lists, string key, T value) {
        lists.SetValue(key, value);
    }

    private bool Load<T>(KeyValuePair<T> lists, string key, ref T value) {
        return lists.GetValue(key, ref value);
    }

    //Saving
    public void Save(string key, bool value) {
        Save(boolKeyValuePairLists, key, value);
    }

    public void Save(string key, int value) {
        Save(intKeyValuePairLists, key, value);
    }

    public void Save(string key, string value) {
        Save(stringKeyValuePairLists, key, value);
    }

    public void Save(string key, Vector3 value) {
        Save(vector3KeyValuePairLists, key, value);
    }

    //Loading
    public bool Load(string key, ref bool value) {
        return Load(boolKeyValuePairLists, key, ref value);
    }

    public bool Load(string key, ref int value) {
        return Load(intKeyValuePairLists, key, ref value);
    }

    public bool Load(string key, ref string value) {
        return Load(stringKeyValuePairLists, key, ref value);
    }

    public bool Load(string key, ref Vector3 value) {
        return Load(vector3KeyValuePairLists, key, ref value);
    }
}
