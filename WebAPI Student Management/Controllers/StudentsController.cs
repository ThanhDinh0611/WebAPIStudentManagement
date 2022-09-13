using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementLibrary.Contracts;
using StudentManagementLibrary.DataAccess;
using System.Security.Cryptography.X509Certificates;
using WebAPI_Student_Management.Models;

namespace WebAPI_Student_Management.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentContext _studentContext = new StudentContext();

        [HttpGet("/Students/All")]
        public IActionResult GetStudentList()
        {
            // Get student list
            List<Student> Students = _studentContext.Students.ToList();

            // Convert to the StudentResponseList
            List<StudentResponse> StudentResponses = new List<StudentResponse>();
            foreach(Student student in Students)
            {
                StudentResponse studentResponse = new StudentResponse(
                    student.Id,
                    student.Name,
                    student.Email,
                    student.PhoneNumber,
                    student.Gender,
                    student.Address);
                StudentResponses.Add(studentResponse);
            }

            // Check if empty
            if(Students == null)
            {
                return NoContent();
            }
            // Create response
            StudentListResponse response = new StudentListResponse(StudentResponses);
            return Ok(response);   
        }

        [HttpPost()]
        public IActionResult CreateStudent(CreateStudentRequest request)
        {
            // Check if id existed
            if (_studentContext.Students.Any(student => student.Id == request.Id))
            {
                return BadRequest();
            }
            // Create Student object
            Student student = new Student();
            student.Id = request.Id;
            student.Name = request.Name;
            student.Email = request.Email;
            student.PhoneNumber = request.PhoneNumber;
            student.Gender = request.Gender;
            student.Address = request.Address;

            // Update to Database
            _studentContext.Students.Add(student);
            _studentContext.SaveChanges();

            // Create response
            StudentResponse response = new StudentResponse(
                student.Id,
                student.Name,
                student.Email,
                student.PhoneNumber,
                student.Gender,
                student.Address);

            // return response
            return CreatedAtAction(
                actionName: nameof(GetStudent),
                routeValues: new {id = student.Id},
                value: response
                
                );
        }

        [HttpGet("{id:int}")]
        public IActionResult GetStudent(int id)
        {
            //Get information from DB
            var student = _studentContext.Students
                .Where(student => student.Id == id)
                .FirstOrDefault();
            if (student == null)
                return NotFound();

            //Create a response
            StudentResponse response = new StudentResponse(
               student.Id,
               student.Name,
               student.Email,
               student.PhoneNumber,
               student.Gender,
               student.Address);
            //Return response
            return Ok(response);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateStudent(int id, UpdateStudentRequest request)
        {
            // Get the student info
            var student = _studentContext.Students
                .Where(student => student.Id == id)
                .FirstOrDefault();
            
            // Check if id existed --> Update
            if (student != null)
            {
                //Update to DB
                student.Name = request.Name;
                student.Email = request.Email;
                student.PhoneNumber = request.PhoneNumber;
                student.Gender = request.Gender;
                student.Address = request.Address;
                _studentContext.SaveChanges();

                // Create response
                StudentResponse response = new StudentResponse(
                    student.Id,
                    student.Name,
                    student.Email,
                    student.PhoneNumber,
                    student.Gender,
                    student.Address);

                return Ok(response);
            }
            // otherwise --> Create new
            else
            {
                // Create a new CreateStudentRequest Contract
                CreateStudentRequest createStudentRequest = new CreateStudentRequest(
                    id,
                    request.Name,
                    request.Email,
                    request.PhoneNumber,
                    request.Gender,
                    request.Address);

                // Call create action
                return CreateStudent(createStudentRequest);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteStudent(int id)
        {
            // Get student info
            var student = _studentContext.Students
                .Where(student => student.Id == id)
                .FirstOrDefault();

            // Check if id is existed
            if (student == null)
            {
                return NoContent();
            }
            else
            {
                // Delete from Database
                _studentContext.Students.Remove(student);
                _studentContext.SaveChanges();

                // Create StudentResponse
                StudentResponse response = new StudentResponse(
                    student.Id,
                    student.Name,
                    student.Email,
                    student.PhoneNumber,
                    student.Gender,
                    student.Address);

                return Ok(response);
            }
        }
    }
}
