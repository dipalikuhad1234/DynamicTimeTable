using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DynamicTimeTableGenerator.Models
{
    public class TimeTable
    {
        [Range(1, 7, ErrorMessage = "Working days must be between 1 and 7.")]
        public int WorkingDays { get; set; }

        [Range(1, 8, ErrorMessage = "Subjects per day must be between 1 and 8.")]
        public int SubjectsPerDay { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Total subjects must be greater than 0.")]
        public int TotalSubjects { get; set; }
        public int TotalHours => WorkingDays * SubjectsPerDay;

        public int TotalAllocatedHours => Subjects.Sum(s => s.Hours);

        public List<SubjectHours> Subjects { get; set; } = new();

        public List<List<string>> Timetable { get; set; } = new();
        public bool IsValid() => TotalAllocatedHours == TotalHours;
    }

    public class SubjectHours
    {
        [Required(ErrorMessage = "Subject name is required.")]
        public string SubjectName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Hours must be a positive number.")]
        public int Hours { get; set; }
    }
}
