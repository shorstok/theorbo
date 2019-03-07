using System.Collections.Generic;

namespace theorbo.MusicTheory
{
    public static class ScaleHelper
    {
        public static int[] GetHalftoneStepsFromBitmask(int mask)
        {
            int? lastNonzeroBitIdx = null;
            var c = 0;

            var result = new List<int>();

            while (mask != 0)
            {
                if ((mask & 1) == 1)
                {
                    if (lastNonzeroBitIdx.HasValue)
                        result.Add(c - lastNonzeroBitIdx.Value);

                    lastNonzeroBitIdx = c;
                }

                mask >>= 1;
                c++;
            }

            return result.ToArray();
        }
    }
}