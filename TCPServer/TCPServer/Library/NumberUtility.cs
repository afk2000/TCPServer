using System;

namespace TCPServer.Library
{
    public static class NumberUtility
    {
        private const string terminate = "terminate";
        public static bool IsValidFormat(string incoming)
        {
            if (incoming.Length != 9)
            {
                return false;
            }

            //Char.IsNumber?
            int value;
            return int.TryParse(incoming, out value);
        }

        public static bool IsTerminate(string incoming)
        {
            if (incoming == terminate)
            {
                return true;
            }
            return false;
        }

        public static string[] GetStringArray(string joined)
        {
            var delimiter = new string[] { @"|" };
            return joined.Split(delimiter, StringSplitOptions.None);
        }
    }
}
