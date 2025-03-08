using DynamicTimeTableGenerator.Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamicTimeTableGenerator.Controllers
{
    public class TimetableController : Controller
    {
        public IActionResult CreateTimetable() => View();

        public IActionResult AllocateSubjects(int workingDays, int subjectsPerDay, int totalSubjects)
        {
            var model = new TimeTable
            {
                WorkingDays = workingDays,
                SubjectsPerDay = subjectsPerDay,
                TotalSubjects = totalSubjects,
                Subjects = Enumerable.Repeat(new SubjectHours(), totalSubjects).ToList()
            };
            if (TempData.ContainsKey("ErrorMessage"))
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult ViewTimetable(TimeTable model)
        {
            int allocatedHours = model.Subjects.Sum(s => s.Hours);

            if (allocatedHours != model.TotalHours)
            {
                TempData["ErrorMessage"] = $"Total subject hours ({allocatedHours}) must match Total Hours for the Week ({model.TotalHours}).";

                return RedirectToAction("AllocateSubjects", new
                {
                    workingDays = model.WorkingDays,
                    subjectsPerDay = model.SubjectsPerDay,
                    totalSubjects = model.TotalSubjects
                });
            }

            model.Timetable = GenerateTimetableGrid(model);
            return View("ViewTimetable", model);
        }

        private List<List<string>> GenerateTimetableGrid(TimeTable model)
        {
            var subjectList = model.Subjects
                .SelectMany(s => Enumerable.Repeat(s.SubjectName, s.Hours))
                .ToList();

            var rand = new Random();
            var timetable = new List<List<string>>();

            for (int i = 0; i < model.SubjectsPerDay; i++)
            {
                var row = new List<string>();

                for (int j = 0; j < model.WorkingDays; j++)
                {
                    if (subjectList.Any())
                    {
                        int index = rand.Next(subjectList.Count);
                        row.Add(subjectList[index]);
                        subjectList.RemoveAt(index);
                    }
                    else
                    {
                        row.Add(string.Empty);
                    }
                }
                timetable.Add(row);
            }
            return timetable;
        }
    }
}
