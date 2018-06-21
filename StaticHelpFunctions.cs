using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softhouse
{
    public static class StaticHelpFunctions
    {
        public static bool M_CorrectLine(string s)
        {
            // Make sure the row starts with P| T| A| or F|
            if (s.StartsWith("P|") || s.StartsWith("T|") || s.StartsWith("A|") || s.StartsWith("F|"))
            {
                return true;
            }
            else
                return false;
        }
    }
}
