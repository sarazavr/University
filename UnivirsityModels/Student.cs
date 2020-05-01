using System;

namespace UnivirsityModels
{
    public class Student : ICloneable, IComparable<Student>
    {
        public string Surname { get; set; }
        public string Patronimic { get; set; }
        public string Name { get; set; }
        public ushort BirthYear { get; set; }
        public float AverageGrade { get; set; }
        public string FullName { get => $"{Surname} {Name} {Patronimic}"; }

        public Student() { }
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

            return FullName == student.FullName && BirthYear == student.BirthYear;
        }

        public override string ToString()
        {
            return $"Student {FullName}: " +
                   $"birth year: {BirthYear}, " +
                   $"average grade: {AverageGrade}";
        }


        public object Clone()
        {
            return new Student(Surname, Name, Patronimic, BirthYear, AverageGrade);
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

            int nameComparing = string.CompareOrdinal(FullName, student2.FullName);

            if (nameComparing != 0)
            {
                return nameComparing;
            }

            return BirthYear.CompareTo(student2.BirthYear);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Surname, Patronimic, Name, BirthYear);
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
