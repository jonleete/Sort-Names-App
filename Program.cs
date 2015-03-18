using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sort
{

    public class Person
    {
        private string Surname;
        private string Firstname;

        public Person(string Surname, string FirstName)
        {
            this.Surname = Surname;
            this.Firstname = FirstName;
        }

        public override string ToString()
        {
            return this.Surname + ", " + this.Firstname;
        }

        public string getSurname()
        {
            return Surname;
        }

        public string getFirstname()
        {
            return Firstname;
        }

        public override bool Equals(Object compare)
        {
            Person comparePerson = (Person)compare;
            if (Surname == comparePerson.Surname && Firstname == comparePerson.Firstname) return true;
            return false;
        }
    }

    public class PersonComparer : IComparer<Person>
    {

        public int Compare(Person x, Person y)
        {
            if (x.getSurname() == y.getSurname())
            {
                int compareValue = x.getFirstname().CompareTo(y.getFirstname());
                return compareValue;
            }
            else
            {
                return x.getSurname().CompareTo(y.getSurname());
            }
        }

    }

    public class Program
    {

        public enum ERROR_CODE
        {
            NoErrors = 0,
            NoFile = 1,
            ArgumentCountMismatch = 2,
            CannotOpenFile = 3
        }

        public static int Main(string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    {
                        Console.WriteLine("Error: No Filename Specified");
                        return (int)ERROR_CODE.NoFile;
                    }
                case 1:
                    {
                        string inputFilename = args[0];
                        StreamReader input = null;

                        try
                        {
                            input = new StreamReader(inputFilename);
                        }
                        catch (Exception) 
                        {
                            Console.WriteLine("Cannot open " + inputFilename);
                            return (int)ERROR_CODE.CannotOpenFile;
                        }
                        
                        List<Person> people = new List<Person>();

                        while (!input.EndOfStream)
                        {
                            string line = input.ReadLine();
                            string[] nameComponents = line.Split(',');
                            nameComponents[1].Replace("  ", string.Empty);
                            people.Add(new Person(nameComponents[0], nameComponents[1]));
                        }
                        
                        people.Sort(new PersonComparer());

                        string outputFilename = inputFilename.Split('.')[0] + "-sorted.txt";
                        TextWriter output = new StreamWriter(outputFilename);

                        foreach (Person person in people) {
                            output.WriteLine(person.ToString());
                        }

                        output.Close();
                        Console.WriteLine("Finished: created " + outputFilename);
                        return (int)ERROR_CODE.NoErrors;
                    }
                default:
                    {
                        Console.WriteLine("Error: sort-names only supports one argument, an input filename");
                        return (int)ERROR_CODE.ArgumentCountMismatch;
                    }
            }
        }
    }
}
