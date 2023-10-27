using System;

namespace GradeCalculator
{

    class Program
    {

        public record ReturnVal(bool success, string return_text); //This is a record used for finding out if a function was successful if it is not successful we put false and then return text is an error message to return

        public class BaseClass //A class for making classes
        {
            public string ClassName { get; protected set; } = "";
            public List<ClassStudent> Students { get; protected set; } = new List<ClassStudent>(); //A collection of students that exist inside of the class

            public BaseClass(string name)
            {
                ClassName = name;
            }

            public ReturnVal AddStudent(BaseStudent student) //Adding a student to the class
            {
                if (Students.Count == 8) //Making sure there is less than 8 students to a class
                {
                    return new ReturnVal(false, "This class is full");
                }

                foreach(ClassStudent existing_student in Students) //Making sure that a student is not already in the class before adding them
                {
                    if(existing_student.FirstName == student.FirstName && existing_student.LastName == student.LastName)
                    {
                        return new ReturnVal(false, "This student is already added");
                    }
                }

                int mark = 0;
                bool test = false;
                while (!test)
                {
                    Console.WriteLine("\nWhat mark did they get in this class: ");
                    test = int.TryParse(Console.ReadLine(), out mark);
                    if (test) //If the value entered was a value
                    {
                        if (!(mark >= 0 && mark <= 100)) // 0 - 100 for a percentage
                        {
                            Console.WriteLine("Please enter a valid mark: ");
                            test = !test;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter only a numerical value");
                    }
                }

                test = false;
                while (!test)
                {
                    Console.WriteLine("\nAre they on the lower(0) or higher grade(1) ");
                    int grade_val = 0;
                    test = int.TryParse(Console.ReadLine(), out grade_val); // Has to enter a numerical value
                    if (test)
                    {
                        if (grade_val == 0 || grade_val == 1) // If another numerical value then they entered the wrong value
                        {
                            if (grade_val == 0)
                            {
                                Students.Add(new ClassStudent(student.FirstName, student.LastName, mark, false));
                                Console.WriteLine("Lower");
                                return new ReturnVal(true, "Student has been added");
                            }
                            //Only ran if the grade value was not 0 therefore higher
                            Students.Add(new ClassStudent(student.FirstName, student.LastName, mark, true)); 
                            Console.WriteLine("Added higher");
                            return new ReturnVal(true, "Student has been added");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter only a numerical value");
                    }
                }
                return new ReturnVal(false, "Student was not added");
            }
        }

        public class BaseStudent //Class that only contains the basic data of a student
        {
            public string FirstName { get; protected set; } = "";
            public string LastName { get; protected set; } = "";

            public BaseStudent(string first_name, string last_name)
            {
                FirstName = first_name;
                LastName = last_name;
            }
        }

        public class ClassStudent : BaseStudent //The class used by the classes to know each students marks and their grade type
        {
            public int Mark { get; protected set; } = 0;
            public bool Grading { get; protected set; } = false; // false = lower, true = higher

            public bool Pass()
            {
                string grade = CalcGrade(Mark, Grading);
                if (grade == "F" || grade == "U") return false;
                return true;
            }

            public ClassStudent(string first_name, string last_name, int mark, bool grade_type) : base(first_name, last_name)
            {
                Mark = mark;
                Grading = grade_type;
            }
        }

        static string CalcGrade(int mark, bool grade_type) // Used to calcuate what grade is inputted
        {
            if (grade_type == true) // Higher values only
            {
                if (mark >= 91) return "A*";
                else if (mark >= 81) return "A";
                else if (mark >= 71) return "B";
                else if (mark >= 61) return "C";
                else if (mark >= 51) return "D";
                else if (mark >= 31) return "E";
                else if (mark >= 11) return "F";
                else if (mark >= 0) return "U";
            }

            //As the if statement always returned these do not need to be in an else statement

            if (mark >= 91) return "B";
            else if (mark >= 81) return "C";
            else if (mark >= 61) return "D";
            else if (mark >= 41) return "E";
            else if (mark >= 21) return "F";
            else if (mark >= 0) return "U";

            return "";
        }

        static void ClassInfo(BaseClass class_given) // Displays a table of the studnets in the class and their info
        {
            double higher_average_grade = -1;
            double lower_average_grade = -1;
            int val = 0;

            var higher_students = class_given.Students.Where(student => student.Grading == true); //Gets a collection of studetns where they are on the higher grading
            int higher_count = higher_students.Count();
            if (higher_count > 0) // Only is used if there is more than one student in the collection
            {
                val = val++; // For tracking position of the student
                higher_average_grade = higher_students.Average(x => x.Mark);
                higher_students = higher_students.OrderByDescending(x => x.Mark); // Orders the student to the top and bottom student
                foreach (ClassStudent student in higher_students)
                {
                    string name = student.FirstName + " " + student.LastName;
                    Console.WriteLine("| " + val + " | " + name.PadRight(40) + "|" + student.Mark.ToString().PadRight(5) + "|" + CalcGrade(student.Mark, true).PadRight(5) + "|" + (student.Pass() ? "Passed" : "Failed").PadRight(10) + "| Higher |");
                }
                Console.WriteLine("\nThe average grade for the higher students is: " + higher_average_grade + ": A grade " + CalcGrade(Convert.ToInt32(higher_average_grade), true));
            }

            val = 0;
            var lower_students = class_given.Students.Where(student => student.Grading == false);
            int lower_count = lower_students.Count();
            if (lower_count > 0)
            {
                lower_average_grade = lower_students.Average(x => x.Mark);
                lower_students = lower_students.OrderByDescending(x => x.Mark);
                foreach (ClassStudent student in lower_students)
                {
                    string name = student.FirstName + " " + student.LastName;
                    Console.WriteLine("| " + val + " | " + name.PadRight(40) + "|" + student.Mark.ToString().PadRight(5) + "|" + CalcGrade(student.Mark, false).PadRight(5) + "|" + (student.Pass() ? "Passed" : "Failed").PadRight(10) + "| Lower |");
                }
                Console.WriteLine("\nThe average grade for the lower students is: " + lower_average_grade + ": A grade " + CalcGrade(Convert.ToInt32(lower_average_grade), true));
            }
        }

        static BaseClass CreateNewClass()
        {
            string class_name = "";
            Console.WriteLine("\n____Class Creation____\n");
            while (true)
            {
                Console.WriteLine("What is the name of the class: ");
                class_name = Console.ReadLine() ?? "";
                if (class_name != "") return new BaseClass(class_name);
                Console.WriteLine("Please enter a name for the class");
            }
        }

        static ReturnVal CreateNewStudent(ref List<BaseStudent> all_students)
        {
            string first_name = "";
            string last_name = "";

            Console.WriteLine("\n___Student Creation___\n");
            Console.WriteLine("What is the first name of the student: ");
            first_name = Console.ReadLine() ?? "";
            Console.WriteLine("\nWhat is the last name of the student: ");
            last_name = Console.ReadLine() ?? "";

            foreach (BaseStudent student in all_students) // Checks to make sure that they do not already exist
            {
                if (student.FirstName == first_name && student.LastName == last_name)
                {
                    return new ReturnVal(false, "This student already exists");
                }
            }

            all_students.Add(new BaseStudent(first_name, last_name));

            return new ReturnVal(true, "Student Created");
        }

        static void StartMenu() // Only runs at the start to introduce program
        {
            Console.WriteLine("_____Welcome To Lilith's School_____");
            Console.WriteLine("In this school there are classes of 8 students");
            Console.WriteLine("This program will get data for the students\n");
        }

        static ReturnVal GetStudentID(ref List<BaseStudent> all_students)
        {
            int val = 0;
            bool found = false;
            for (int i = 0; i < all_students.Count(); i++) // Displays students with a numerical value
            {
                Console.WriteLine(i+1 + ": " + all_students[i].FirstName + " " + all_students[i].LastName);
            }
            Console.WriteLine("Enter the value of the student: ");
            found = int.TryParse(Console.ReadLine() ?? "X", out val);
            val = val - 1;
            if (val >= 0 && val < all_students.Count()) // If the value is in the range of students 
            {
                return new ReturnVal(true, val.ToString()); // Returns the value the user entered
            }
            return new ReturnVal(false, "Value out of range");
        }

        static ReturnVal GetClassID(ref List<BaseClass> all_classes)
        {
            int val = 0;
            bool found = false;
            for (int i = 0; i < all_classes.Count(); i++) // Dispalys classes with a numerical value
            {
                Console.WriteLine(i + 1 + ": " + all_classes[i].ClassName);
            }
            Console.WriteLine("Enter the value of the class: ");
            found = int.TryParse(Console.ReadLine() ?? "X", out val);
            val = val - 1;
            if (val >= 0 && val < all_classes.Count()) // If the value is in the range of classes
            {
                return new ReturnVal(true, val.ToString()); // Returns the value the user entered
            }
            return new ReturnVal(false, "Value out of range");
        }

        static ReturnVal Menu(ref List<BaseStudent> all_students, ref List<BaseClass> all_classes)
        {
            Console.WriteLine("\nWhat would you like to do: ");
            Console.WriteLine("1. Create a new Student: ");
            Console.WriteLine("2. Create a new Class: ");
            Console.WriteLine("3. Add student to a Class: ");
            Console.WriteLine("4. Get information from a class: ");
            Console.WriteLine("5. Quit: ");
            int input_val = 0;
            bool correct = int.TryParse(Console.ReadLine() ?? "x", out input_val);
            ReturnVal return_val = new ReturnVal(false, ""); // Value that will be returned to the user at the end
            if (correct)
            {
                switch (input_val)
                {
                    case 1:
                        return_val = CreateNewStudent(ref all_students);
                        break;
                    case 2:
                        all_classes.Add(CreateNewClass());
                        return_val = new ReturnVal(true, "Class created");
                        break;
                    case 3:
                        return_val = GetStudentID(ref all_students);
                        if (return_val.success == true)
                        {
                            int student_id = Convert.ToInt32(return_val.return_text);
                            return_val = GetClassID(ref all_classes);
                            int class_id = Convert.ToInt32(return_val.return_text);
                            if (return_val.success == true)
                            {
                                return_val = all_classes[class_id].AddStudent(all_students[student_id]);
                            }
                        }
                        break;
                    case 4:
                        return_val = GetClassID(ref all_classes);
                        if (return_val.success == false)
                        {
                            return return_val;
                        }
                        else
                        {
                            ClassInfo(all_classes[Convert.ToInt32(return_val.return_text)]);
                            return_val = new ReturnVal(true, "");
                        }
                        break;
                    case 5:
                        Console.WriteLine("Thank you for using the Lilith's school program");
                        Environment.Exit(0);
                        break;
                    default:
                        return_val = new ReturnVal(false, "That option does not exist");
                        break;
                }
            }
            return return_val; //Returned every tun
        }

        static void Main(string[] args)
        {
            List<BaseStudent> all_students = new List<BaseStudent>();
            List<BaseClass> all_classes = new List<BaseClass>();
            StartMenu();
            ReturnVal return_val = new ReturnVal(false, "empty");
            while (true)
            {
                return_val = Menu(ref all_students, ref all_classes);
                Console.WriteLine(return_val.return_text);
            }
        }
    }
}