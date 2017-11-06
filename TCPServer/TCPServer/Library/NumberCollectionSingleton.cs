using System;
using System.Collections.Concurrent;

namespace TCPServer.Library
{
    public sealed class NumberCollectionSingleton 
    {

        private static volatile NumberCollectionSingleton instance;
        private static object syncRoot = new Object();

        private ConcurrentDictionary<int, byte> NumbersList { get; set; }
        private int newDuplicates { get; set; }
        private int newUniques { get; set; }
        
        public NumberCollectionSingleton()
        {
            NumbersList = new ConcurrentDictionary<int, byte>();
        }
        

        public static NumberCollectionSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new NumberCollectionSingleton();
                    }
                }

                return instance;
            }
        }


        public bool Add(int number)
        {
            if (!NumbersList.TryAdd(number, 0))
            {
                newDuplicates++;
                return false;
            }
            newUniques++;
            return true;
        }

        public int GetLatestDuplicateCount()
        {
            var returnValue = newDuplicates;
            newDuplicates = 0;
            return returnValue;
        }

        public int GetLatestNewUniques()
        {
            var returnValue = newUniques;
            newUniques = 0;
            return returnValue;
        }

        public int GetTotalCount()
        {
            return NumbersList.Count;
        }
    }
}
