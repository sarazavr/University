using System;
using System.Text.RegularExpressions;

namespace Stud.Utils
{
    public static class GroupNameParser
    {
        public static string YEAR_REGEX_PART_KEY = "YEAR";
        public static string MAGISTER_REGEX_PART_KEY = "MAGISTER";

        public static string PATTERN = 
            $"^[A-Za-z][A-Za-z]-(?<{YEAR_REGEX_PART_KEY}>\\d\\d)(?<{MAGISTER_REGEX_PART_KEY }>[m,M])?-\\d$";

        public static Regex REGEX = new Regex(PATTERN);

        public static bool IsValid(string name)
        {
            return name != null && REGEX.IsMatch(name);
        }

        // Returns number of course for group considering THE STUDY IS ENDLESS, MEH
        public static int ParseHypotheticalCourseNumber(string groupName, int bacalavrsStudyYears = 4)
        {
            if (groupName == null)
            {
                throw new ArgumentException("Group name is null");
            }

            var match = REGEX.Match(groupName);

            if (!match.Success)
            {
                throw new ArgumentException("Group name has incorrect format");
            }

            string lastTwoDigitsOfYear = match.Groups[YEAR_REGEX_PART_KEY].Value;
            bool isMagisters = match.Groups[MAGISTER_REGEX_PART_KEY].Value != string.Empty;

            int enterYear = int.Parse($"20{lastTwoDigitsOfYear}");

            int yearsDiff = DateTime.Now.Year - enterYear;

            return isMagisters ? yearsDiff + bacalavrsStudyYears : yearsDiff;
        }
    }
}
