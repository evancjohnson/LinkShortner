using System;
using System.Collections.Generic;
using System.Text;

namespace LinkShortner.Code
{
    public static class RandomString
    {
        public static string GetRandomString(int length)
        {
            StringBuilder sb = new StringBuilder();
            int numGuidsToConcat = ((length - 1) / 32) + 1;
            for (int i = 1; i <= numGuidsToConcat; i++)
            {
                sb.Append(Guid.NewGuid().ToString("N"));
            }

            return sb.ToString(0, length);
        }
    }
}
