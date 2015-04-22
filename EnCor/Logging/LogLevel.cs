using System;

namespace EnCor.Logging
{
    public class LogLevel : IComparable
    {
        public static readonly LogLevel Fatal = new LogLevel("Fatal", 5);
        public static readonly LogLevel Error = new LogLevel("Error", 4);
        public static readonly LogLevel Warning = new LogLevel("Warning", 3);
        public static readonly LogLevel Information = new LogLevel("Information", 2);
        public static readonly LogLevel Debug = new LogLevel("Debug", 1);

        private readonly string _name;
        private readonly int _value;

        private LogLevel(string name, int value)
        {
            _name = name;
            _value = value;
        }


        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            var b = obj as LogLevel;
            if (b == null)
            {
                throw new ArgumentException(string.Format("obj {0} is not LogLevel, cannot compare it to LogLevel.", obj));
            }

            return _value.CompareTo(b._value);
        }

        public override bool Equals(object obj)
        {
            var b = obj as LogLevel;
            if (b != null)
            {
                return b._value == _value;
            }
            return obj.Equals(this);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        #endregion

        public override string ToString()
        {
            return _name;
        }
    }
}
