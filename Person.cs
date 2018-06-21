using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softhouse
{
    class Person:Human
    {
        string m_lastName;
        List<Family> m_family = new List<Family>();
        public Person(string firstName, string lastName): base(firstName)
        {
            m_lastName = lastName;
        }
        public override void M_WriteInformation(ref List<string> parsedInput)
        {
            parsedInput.Add("<person>");
            // Make sure we have a firsts name, this should always be true
            if (m_firstName != null)
            {
                parsedInput.Add("<firstname>" + m_firstName + "</firstname>");
            }
            // See if we have been provided a last name
            if (m_lastName != null)
            {
                parsedInput.Add("<lastname>" + m_lastName + "</lastname>");
            }
            // Write the other info. Address and phone number
            base.M_WriteBaseInfo(ref parsedInput);
            // Make the family memebers write their info
            foreach (var item in m_family)
            {
                item.M_WriteInformation(ref parsedInput);
            }
            // Done
            parsedInput.Add("</person>");
        }
        public void M_AddFamily(Family newMember)
        {
            m_family.Add(newMember);
        }
    }
}
