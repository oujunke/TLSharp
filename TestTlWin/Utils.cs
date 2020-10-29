using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTlWin
{
    public class Utils
    {
        public static int[] GetRange(int start, int count, bool isPositive = false)
        {
            var nums = new int[count];
            for (int i = 0; i < count; i++)
            {
                nums[i] = isPositive?(start + i):(start - i);
            }
            return nums;
        }
    }
}
