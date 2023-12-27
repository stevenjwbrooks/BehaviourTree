using System.Linq;

namespace BehaviourTree
{
    public static class ArrayExtensions
    {
        /// <summary>
        ///  Fisher-Yates shuffle
        /// </summary>
        public static T[] Shuffle<T>(this T[] items, IRandomProvider randomProvider)
        {
            var n = items.Length;
            var newArray = items.ToArray();

            while (n > 1)
            {
                n--;
                var k = randomProvider.NextRandomInteger(n + 1);
                (newArray[k], newArray[n]) = (newArray[n], newArray[k]);
            }

            return newArray;
        }
    }
}
