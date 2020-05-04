using System;
using System.Collections.Generic;
using System.Linq;
using UnivirsityModels;



namespace Stud.Utils
{
    using FacultiesList = DoubleLinkedList<NamedDoubleLinkedList<NamedDoubleLinkedList<Student>>>;

    public class AverageGradeStatistics
    {
        public float? MaxAverageOfGrades { get; private set; }
        public List<KeyValuePair<NamedDoubleLinkedList<Student>, float>> GroupAndAverageGradePairs { get; private set; }

        public AverageGradeStatistics() { }

        public bool HasResults() => MaxAverageOfGrades.HasValue;

        public AverageGradeStatistics Calculate(FacultiesList facultyList, 
            Func<NamedDoubleLinkedList<Student>, int, bool> shouldIncludeGroupToStatistics)
        {
            GroupAndAverageGradePairs = null;
            float? maxAverage = null;

            var pairsList = new List<KeyValuePair<NamedDoubleLinkedList<Student>, float>>();

            foreach (var f in facultyList)
            {
                foreach (var gr in f)
                {
                    int course = GroupNameParser.ParseHypotheticalCourseNumber(gr.Name);

                    if (!shouldIncludeGroupToStatistics(gr, course)) continue;

                    float average = gr.Average(s => s.AverageGrade);

                    pairsList.Add(new KeyValuePair<NamedDoubleLinkedList<Student>, float>(gr, average));

                    if (!maxAverage.HasValue || maxAverage < average) maxAverage = average;
                }
            }

            if(!maxAverage.HasValue)
            {
                throw new AverageGradeStatisticsHasNoResultsException(
                    "There is nothing to calculate for provided list of faculties");
            }

            MaxAverageOfGrades = maxAverage.Value;
            GroupAndAverageGradePairs = pairsList;

            return this;
        }

        public List<NamedDoubleLinkedList<Student>> GetGroupsWithMaxAverageGrades()
        {
            if(!HasResults())
            {
                throw new InvalidOperationException("You should calculate statistics firs");
            }

            var list = new List<NamedDoubleLinkedList<Student>>();

            foreach(var pair in GroupAndAverageGradePairs)
            {
                if (pair.Value == MaxAverageOfGrades.Value) list.Add(pair.Key);
            }

            return list;
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
