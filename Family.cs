using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softhouse
{
    class Family : Human
    {
        string m_born;
        
        public Family(string firstName, string born): base(firstName)
        {
            m_born = born;         
        }
        public override void M_WriteInformation(ref List<string> parsedInput)
        {
            parsedInput.Add("<family>");
            // This should always be true
            if (m_firstName != null)
            {
                parsedInput.Add("<name>" + m_firstName + "</name>");
            }
            // Make sure we have been provided a birth year
            if (m_born != null)
            {
                parsedInput.Add("<born>" + m_born+ "</born>");
            }
            // Write address and phone number
            base.M_WriteBaseInfo(ref parsedInput);

            // End of family member
            parsedInput.Add("</family>");
        }
    }
}
