using System;

namespace Features.Input
{
    public struct ChangedData<T> where T : struct, IEquatable<T>
    {
        public T data;
        public bool IsChanged { get; private set; }

        public bool PopIsChanged 
        {
            get
            {
                bool value = IsChanged;
                IsChanged = false;
                return value;
            }
        }

        public void MarkAsChanged()
        {
            IsChanged = true;
        }
    }
}