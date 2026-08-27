using System.Collections.Frozen;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using CampusPractice.FluentApi;
using CampusPractice.FluentApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

using var db = new CampusContext();

// ----------------------------------------- Section A -------------------------------------------------------------


// Students whose last name starts with A
var q1 = db.Students.Where(s => s.LastName.StartsWith('A')).Select(s => s.FirstName).ToList();
q1.ForEach(Console.WriteLine);


// Get the Title and Credits of every course worth more than 3 credits, ordered by Credits descending.
var q2 = db.Courses.Where(c => c.Credits > 3).OrderByDescending(c => c.Credits).Select(c => c.Title + " " + c.Credits).ToList();
q2.ForEach(Console.WriteLine);


//Get the full names ("First Last") of every instructor hired before 2016, as a List<string>.
var q3 = db.Instructors.Where(i => i.HireDate.Year < 2016).Select(i => i.FirstName + " " + i.LastName).ToList<string>();
q3.ForEach(Console.WriteLine);


//Count how many students are in the database.
var q4 = db.Students.Count();
Console.WriteLine(q4);


//Get the single course titled "Calculus I" (use the method that throws if zero or more than one match).
var q5 = db.Courses.Single(c => c.Title.Equals("Calculus I"));


//Get the first 3 students ordered by EnrollmentDate.
var q6 = db.Students.Take(3).OrderBy(s => s.EnrollmentDate).Select(s => s.FirstName).ToList();
q6.ForEach(Console.WriteLine);


//Check whether any course has more than 3 credits (boolean result, don't load all courses to check).
var q7 = db.Courses.Any(c => c.Credits > 3);


//Get distinct City values from StudentAddress.
var q8 = db.StudentAddresses.Select(c => c.City).Distinct().ToList();
q8.ForEach(Console.WriteLine);


//Get students' Email domains only (the part after @) as a distinct list.
var q9 = db.Students.Select(s => s.Email.Substring(s.Email.IndexOf('@') + 1)).Distinct().ToList();
q9.ForEach(Console.WriteLine);


//Skip the first 2 departments and take the next 2, ordered by Name.
var q10 = db.Departments.OrderBy(d => d.Name).Select(d => d.Name).Skip(2).Take(2).ToList();
q10.ForEach(Console.WriteLine);

// ----------------------------------------- Section B -------------------------------------------------------------

//Get every instructor together with their department's Name, without lazy-loading (i.e., you're not allowed a second round-trip per instructor).
var q11 = db.Instructors.Select(i => new {name = i.FirstName + " " + i.LastName, Department = i.Department.Name}).ToList();
q11.ForEach(Console.WriteLine);


//Get a Course by id, including its Department and Instructor (Instructor may be null — handle that).
var q12 = db.Courses.Include(c => c.Department).Include(c=>c.Instructor).FirstOrDefault(c => c.CourseId == 2);


//Get all departments with their instructors and each instructor's courses, in one query (two levels deep).
var q13 = db.Departments.Include(d => d.Instructors).ThenInclude(i => i.Courses).ToList();


//For each student who has an address, project { StudentName, City }. Students without an address should not appear (use the right join type on purpose).
var q14 = db.Students.Where(s => s.Address != null).Select(s => new {StudentName = s.FirstName, City=s.Address!.City}).ToList();
q14.ForEach(Console.WriteLine);


//For each student regardless of whether they have an address, project { StudentName, City }, using "N/A" when there's no address.
var q15 = db.Students.Select(s => new {StudentName = s.FirstName, City=(s.Address == null?"N/A":s.Address.City)}).ToList();
q15.ForEach(Console.WriteLine);


//Load all courses with more than 5 enrollees, including the Enrollments collection but using AsSplitQuery — and explain in a comment why you'd choose that over the default single query here.
var q16 = db.Courses.Include(c => c.Enrollments).Where(c => c.Enrollments.Count() > 5).AsSplitQuery().ToList();
q16.ForEach(Console.WriteLine);


//Get instructors who have zero courses assigned (careful: this needs the "not any" pattern, not Count() == 0 after loading everything).
var q17 = db.Instructors.Where(i => !i.Courses.Any()).ToList();
q17.ForEach(Console.WriteLine);


// ----------------------------------------- Section C -------------------------------------------------------------

//Group courses by DepartmentId and return { DepartmentId, CourseCount }.
var q18 = db.Courses.GroupBy(c => c.DepartmentId).Select(g => new{Id = g.Key, CourseCount = g.Count()}).ToList();
q18.ForEach(Console.WriteLine);


//For each department, compute the average Credits of its courses.
var q19 = db.Courses.GroupBy(c => c.DepartmentId).Select(g => new {Id = g.Key, AvgCredits = g.Average(c => c.Credits)}).ToList();
Console.WriteLine(q19);


//Find the student(s) with the highest average Grade across all their graded enrollments (exclude nulls).
var q20 = db.Enrollments.Where(e => e.Grade != null).GroupBy(e => e.StudentId).Select(g => new {studentId = g.Key, AvgGrade = g.Average(g => g.Grade)}).Take(5).ToList();
q20.ForEach(Console.WriteLine);


//Get the number of enrollments per course, sorted descending, top 5 only.
var q21 = db.Enrollments.GroupBy(e => e.CourseId).Select(g => new {Course = g.Key, Enrollments = g.Count()}).OrderByDescending(x => x.Enrollments).Take(5).ToList();
q21.ForEach(Console.WriteLine);


//Group enrollments by Grade bucket (>= 3.7 = "A", >= 3.0 = "B", else "C or below"; nulls = "Ungraded") and count each bucket. (Think about whether this can run entirely in SQL or needs AsEnumerable() partway through — and why.)
var q22 = db.Enrollments.Select(e => e.Grade == null?"Ungraded" : e.Grade >= 3.7m ? "A": e.Grade >= 3.0m? "B" : "C or below").GroupBy(b => b).Select(g => new {Bucket = g.Key, Count = g.Count()}).ToList();
q22.ForEach(Console.WriteLine);


//Find departments that have no instructors at all (left join + null check, or Any-based).
var q23 = db.Departments.LeftJoin(db.Instructors, d => d.DepartmentId, i => i.DepartmentId, (d, i) => new { Department = d.Name, Instructor = i }).Where(x => x.Instructor == null).ToList();
q23.ForEach(Console.WriteLine);