using UnityEngine;

namespace AnkleBreaker.Inspector
{
    public class FolderPathAttribute : PropertyAttribute
    {
        public string DefaultPath { get; private set; }

        public FolderPathAttribute(string defaultPath = "")
        {
            DefaultPath = defaultPath;
        }
    }
}

