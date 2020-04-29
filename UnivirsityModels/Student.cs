using System;

namespace UnivirsityModels
{
    [Serializable]
    public class Student : ICloneable, IComparable<Student>
    {
        public string Surname { get; }
        public string Patronimic { get; }
        public string Name { get; }
        public ushort BirthYear { get; }
        public float AverageGrade { get; set; }

        public Student(string surname, string name, string patronimic, ushort birthYear, float averageGrade = 0)
        {
            Surname = surname;
            Name = name;
            Patronimic = patronimic;
            BirthYear = birthYear;
            AverageGrade = averageGrade;
        }

        public override bool Equals(object obj)
        {
            if (obj is null || obj.GetType() != GetType()) return false;

            if (ReferenceEquals(this, obj)) return true;

            return Equals((Student)obj);
        }

        public bool Equals(Student student)
        {
            if (student is null) return false;

            return Surname == student.Surname
                   && Name == student.Name
                   && Patronimic == student.Patronimic
                   && BirthYear == student.BirthYear;
        }

        public override string ToString()
        {
            return $"Student {Surname} {Name} {Patronimic}: " +
                   $"birth year: {BirthYear}, " +
                   $"average grade: {AverageGrade}";
        }


        public object Clone()
        {
            return new Student(Surname, Name, Patronimic, BirthYear, AverageGrade);
        }

        public override int GetHashCode()
        {
            return Surname.GetHashCode()
               ^ Patronimic.GetHashCode() * 17
               ^ Name.GetHashCode() * 19
               * BirthYear.GetHashCode();
        }

        public static int CompareTo(Student student1, Student student2)
        {
            return student1 is null ? -1 : student1.CompareTo(student2);
        }

        public int CompareTo(Student student2)
        {
            if (student2 is null)
            {
                return 1;
            }

            int result = string.CompareOrdinal(Surname, student2.Surname);
            if (result != 0)
            {
                return result;
            }

            result = string.CompareOrdinal(Name, student2.Name);
            if (result != 0)
            {
                return result;
            }

            result = string.CompareOrdinal(Patronimic, student2.Patronimic);
            if (result != 0)
            {
                return result;
            }

            return BirthYear.CompareTo(student2.BirthYear);
        }


        public static bool operator ==(Student student1, Student student2)
        {
            return CompareTo(student1, student2) == 0;
        }

        public static bool operator !=(Student student1, Student student2)
        {
            return CompareTo(student1, student2) != 0;
        }

        public static bool operator >(Student student1, Student student2)
        {
            return CompareTo(student1, student2) > 0;
        }

        public static bool operator <(Student student1, Student student2)
        {
            return CompareTo(student1, student2) < 0;
        }

        public static bool operator >=(Student student1, Student student2)
        {
            return CompareTo(student1, student2) >= 0;
        }

        public static bool operator <=(Student student1, Student student2)
        {
            return CompareTo(student1, student2) <= 0;
        }
    }
}
