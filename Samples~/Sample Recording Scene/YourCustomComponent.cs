using System;
using UnityEngine;

namespace StepanLem.ReplaySystem.SampleScene
{
    public class YourCustomComponent : MonoBehaviour
    {
        public string MyString;
        public float MyFloat;
        public bool MyBool;
        public EnumOfSomething MyEnum;
        public SomeStruct MyStruct;
    }

    public enum EnumOfSomething { First = 1, Last = 2 };

    [Serializable]
    public struct SomeStruct
    {
        public int Property1;
        public bool Property2;
    }
}


