using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementLibrary.Contracts
{
    public record UpdateGradeRequest(
        decimal English,
        decimal Informatic,
        decimal PE
    );
}
