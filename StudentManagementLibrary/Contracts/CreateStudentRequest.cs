using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementLibrary.Contracts
{
    public record CreateStudentRequest(
        int Id,
        string Name,
        string Email,
        string PhoneNumber,

    // 0: Male, 1: Female
        bool Gender,
        string Address
    );
}
