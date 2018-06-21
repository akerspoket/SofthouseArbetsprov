using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softhouse
{
    class Parser
    {
        string[] m_parsedLines = null;
        public void M_ParseLines(string[] input)
        {
            // Create a list to save the parsed info
            List<string> parsedInput = new List<string>();
            parsedInput.Add("<people>");

            // Make a list to save past persons. Person is used to save person data
            List<Person> pastPersons = new List<Person>();
            // Human can be both family and person, used to set variabled
            Human currentHuman = null;
            // Used to assigne family memebers
            Person currentPerson = null;

            int length = input.Length;
            for (int i = 0; i < length; i++)
            {
                // See if there are any faults in the line, if so throw error
                if (!StaticHelpFunctions.M_CorrectLine(input[i]))
                {
                    throw new InvalidArguments("Invalid line at row " + i);
                }
                
                // Everything seems to be in order, go thorugh the prefixes
                char nextInputPrefix = input[i][0];
                string[] splitString = input[i].Split('|');

                // We got a new person
                if (nextInputPrefix == 'P')
                {
                    // See if this is the first person ever, 
                    // otherwise save the last human to the past person list (completed person list)
                    if (currentPerson != null)
                    {
                        pastPersons.Add(currentPerson);
                    }
                    // The person has to have a firstname
                    string firstName = splitString[1];
                    // The person might be without last name
                    string lastName = null;
                    if (splitString.Length >= 3)
                    {
                        lastName = splitString[2];
                    }

                    // Create the person
                    currentPerson = new Person(firstName,lastName);
                    
                    // Set the person the following commands should affect to the new person
                    currentHuman = currentPerson;
                }
                // A new family memeber
                else if (nextInputPrefix == 'F')
                {
                    // Make sure we have a person to attach to 
                    if (currentPerson == null)
                    {
                        throw new InvalidArguments("Family member without person at row " + i);
                    }
                    // Name is always there
                    string name = splitString[1];
                    // Might not have born info
                    string born= null;
                    if (splitString.Length >= 3)
                    {
                        born = splitString[2];
                    }

                    // Create the new family memeber 
                    // save it down to the human we want to affect with following commands
                    currentHuman = new Family(name, born);

                    // Attatch this family memeber to the current person we are affecting
                    currentPerson.M_AddFamily((Family)currentHuman);
                }
                // Phone number info
                else if (nextInputPrefix == 'T')
                {
                    // Make sure we have a human to give info to
                    if (currentHuman == null)
                    {
                        throw new InvalidArguments("Contact info without recipient at row" + i);
                    }
                    // Mobile is always given
                    string mobile = splitString[1];
                    // Home phone/landline is not guaranteed
                    string homePhone = null;
                    if (splitString.Length >= 3)
                    {
                        homePhone = splitString[2];
                    }
                    // Add the phone info to the current human we are affecting
                    currentHuman.M_AddContactInfo(mobile, homePhone);
                }
                // Address info
                else if (nextInputPrefix == 'A')
                {
                    // Make sure we have a human to attach to
                    if (currentHuman == null)
                    {
                        throw new InvalidArguments("Address without recipient at row" + i);
                    }
                    // Street is always given
                    string street = splitString[1];
                    // City and citycode/zip-code is not guaranteed
                    string city = null;
                    string cityCode = null;
                    if (splitString.Length >= 3)
                    {
                        city = splitString[2];
                    }
                    if (splitString.Length >= 4)
                    {
                        cityCode = splitString[3];
                    }

                    // Attatch the info to the current human
                    currentHuman.M_AddAddressInfo(street, city, cityCode);
                }
            }

            // See if we have a person at the end that needs saving to the completed humans persons
            if (currentPerson != null)
            {
                pastPersons.Add(currentPerson);
            }

            // Tell all the persons to write their info
            length = pastPersons.Count;
            for (int i = 0; i < length; i++)
            {
                pastPersons[i].M_WriteInformation(ref parsedInput);
            }

            parsedInput.Add("</people>");
            m_parsedLines = parsedInput.ToArray();
        }
        public void M_PrintParsedLines()
        { }
        public void M_SaveParsedLines(string outputFile)
        {
            File.WriteAllLines(outputFile, m_parsedLines);
        }
    }
}
