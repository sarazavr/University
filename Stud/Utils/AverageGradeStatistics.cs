using System;
using System.Linq;
using UnivirsityModels;



namespace Stud.Utils
{
    using FacultiesList = DoubleLinkedList<NamedDoubleLinkedList<NamedDoubleLinkedList<Student>>>;

    public class AverageGradeStatistics
    {
        public float MaxAverageOfGrades { get; private set; }
        public NamedDoubleLinkedList<Student> GroupWithMaxAverageOfGrades { get; private set; }

        public AverageGradeStatistics() { }
      

        public AverageGradeStatistics Calculate(FacultiesList facultyList, 
            Func<NamedDoubleLinkedList<Student>, int, bool> shouldIncludeGroupToStatistics)
        {
            GroupWithMaxAverageOfGrades = null;
            float? maxAverage = null;

            foreach (var f in facultyList)
            {
                foreach (var gr in f)
                {
                    int course = GroupNameParser.ParseHypotheticalCourseNumber(gr.Name);

                    if (!shouldIncludeGroupToStatistics(gr, course)) continue;

                    float average = gr.Average(s => s.AverageGrade);

                    if (!maxAverage.HasValue || maxAverage < average)
                    {
                        maxAverage = average;
                        GroupWithMaxAverageOfGrades = gr;
                    }
                }
            }

            if(!maxAverage.HasValue)
            {
                throw new AverageGradeStatisticsHasNoResultsException(
                    "There is nothing to calculate for provided list of faculties");
            }

            MaxAverageOfGrades = maxAverage.Value;

            return this;
        }
    }

    public class AverageGradeStatisticsHasNoResultsException : Exception
    {
        public AverageGradeStatisticsHasNoResultsException()
        {
        }

        public AverageGradeStatisticsHasNoResultsException(string message)
            : base(message)
        { }
    }
}
