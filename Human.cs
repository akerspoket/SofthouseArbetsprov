using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softhouse
{
    // Abstract so you can't create a human
    abstract class Human
    {
        // This is the info that is shared between persons and family memebers, aka humans
        protected string m_firstName = null;
        protected string m_mobile = null;
        protected string m_homePhone = null;
        protected string m_street = null;
        protected string m_city = null;
        protected string m_citycode = null;

        // Protected so you have to call a child class to access it
        protected Human(string firstName)
        {
            m_firstName = firstName;
        }

        public void M_AddContactInfo(string mobile, string home)
        {
            // See if we have already been provided a mobile number
            if (m_mobile != null)
            {
                // Should we always overwrite? return error for now to give the info to the user
                throw new ArgumentException("Contact " + m_firstName + 
                    " already has mobile number " + m_mobile + " and home number " + m_homePhone);
            }
            m_mobile = mobile;
            m_homePhone = home;
        }


        public void M_AddAddressInfo(string street, string city, string cityCode)
        {
            // See if we have already been provided a street
            if (m_street != null)
            {
                // Should we always overwrite? return error for now to give the info to the user
                throw new ArgumentException("Contact " + m_firstName +
                    " already has address " + street + ", " + city + ", " + cityCode);
            }
            m_street = street;
            m_city = city;
            m_citycode = cityCode;
        }

        internal void M_WriteBaseInfo(ref List<string> parsedInput)
        {
            // If we have been given a address, the street will be set.
            if (m_street != null)
            {
                parsedInput.Add("<address>");
                parsedInput.Add("<street>" + m_street + "</street>");
                
                // See if we were also provided a city
                if (m_city != null)
                {
                    parsedInput.Add("<city>" + m_city + "</city>");
                }
                // See if we were alo provided a city code
                if (m_citycode != null)
                {
                    parsedInput.Add("<zipcode>" + m_citycode + "</zipcode>");
                }

                // Address done
                parsedInput.Add("</address>");
            }
            // If we have been given a phone, the mobile will be set
            if (m_mobile != null)
            {
                parsedInput.Add("<phone>");
                parsedInput.Add("<mobile>" + m_mobile + "</mobile>");

                // See if we were also provided a home number
                if (m_homePhone != null)
                {
                    parsedInput.Add("<landline>" + m_homePhone + "</landline>");
                }

                // Phone done
                parsedInput.Add("</phone>");
            }
        }

        public abstract void M_WriteInformation(ref List<string> parsedInput);
    }
}
