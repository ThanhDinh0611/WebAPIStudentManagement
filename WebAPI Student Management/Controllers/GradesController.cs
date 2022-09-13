using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementLibrary.Contracts;
using StudentManagementLibrary.DataAccess;
using WebAPI_Student_Management.Models;

namespace WebAPI_Student_Management.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private StudentContext _studentContext = new StudentContext();
        [HttpPost]
        public IActionResult CreateGrade(CreateGradeRequest request)
        {
            // Check if the student is not existed --> Bad Request
            if(!_studentContext.Students.Any(student => student.Id == request.StudentId))
            {
                return BadRequest();
            }

            //Check if the grade is existed --> Update
            if(_studentContext.Grades.Any(grade => grade.StudentId == request.StudentId))
            {
                // Create UpdateGradeRequest
                UpdateGradeRequest UpdateRequest = new UpdateGradeRequest(
                    request.English,
                    request.Informatic,
                    request.PE);

                // Call UpdateGrade function
                return UpdateGrade(request.StudentId, UpdateRequest);
            }

            // Create a grade record
            Grade grade = new Grade();
            grade.StudentId = request.StudentId;
            grade.English = request.English;
            grade.Informatic = request.Informatic;
            grade.PE = request.PE;

            // Update to DB
            _studentContext.Add(grade);
            _studentContext.SaveChanges();

            // Create response
            GradeResponse response = new GradeResponse(
                grade.StudentId,
                grade.English,
                grade.Informatic,
                grade.PE);
            return CreatedAtAction(
                actionName: nameof(GetGrade),
                routeValues: new {id = grade.StudentId},
                value: response
                );
        }

        [HttpGet("{studentId:int}")]
        public IActionResult GetGrade(int studentId)
        {
            // Get Grade Info
            var grade = _studentContext.Grades
                .Where(grade => grade.StudentId == studentId)
                .FirstOrDefault();

            // Check if grade existed
            if (grade != null)
            {
                // Create response
                GradeResponse response = new GradeResponse(
                    grade.StudentId,
                    grade.English,
                    grade.Informatic,
                    grade.PE);
                return Ok(response);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPut("{studentId:int}")]
        public IActionResult UpdateGrade(int studentId, UpdateGradeRequest request)
        {
            // Check if the student is not existed --> Bad Request
            if (!_studentContext.Students.Any(student => student.Id == studentId))
            {
                return BadRequest();
            }

            // Get grade info
            var grade = _studentContext.Grades
                .Where(grade => grade.StudentId == studentId)
                .FirstOrDefault();

            //Check if the grade is not existed --> NoContent
            if (grade == null)
            {
                return NoContent();
            }
            else
            {
                // Update Grade info to DB
                grade.English = request.English;
                grade.Informatic = request.Informatic;
                grade.PE = request.PE;
                _studentContext.SaveChanges();

                // Create response
                GradeResponse response = new GradeResponse(
                    grade.StudentId,
                    grade.English,
                    grade.Informatic,
                    grade.PE);
                return Ok(response);
            }
        }

        [HttpDelete("{studentId:int}")]
        public IActionResult DeleteGrade(int studentId)
        {
            // Get grade info
            var grade = _studentContext.Grades
                .Where(grade => grade.StudentId == studentId)
                .FirstOrDefault();
            // Check if grade is not existed --> NoContent
            if (grade == null)
            {
                return NoContent();
            }
            else
            {
                // Delete from DB
                _studentContext.Grades.Remove(grade);
                _studentContext.SaveChanges();

                // Create response
                GradeResponse response = new GradeResponse(
                    grade.StudentId,
                    grade.English,
                    grade.Informatic,
                    grade.PE);
                return Ok(response);
            }
        }
    }
}
