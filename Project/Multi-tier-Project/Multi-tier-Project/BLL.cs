using Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BusinessLayer
{

    internal class Students
    {
        internal static int UpdateStudents()
        {


            return Data.Students.UpdateStudents();
        }
    }
    internal class Enrollments
    {
        internal static int UpdateEnrollments()
        {
            return Data.Enrollments.UpdateEnrollments();
        }
        internal static bool FinalGradeIsEmpty(string finalGrade)
        {
            if (finalGrade == "") {return true;}
            else {return false;}
        }
        internal static bool SameProgramStudent(string StId, string CId)
        {
            DataSet ds = DataTables.getDataSet();

            var query = (
                   
                   from student in ds.Tables["Students"].AsEnumerable()
                   from course in ds.Tables["Courses"].AsEnumerable()
                   where student.Field<string>("StId") == StId
                   where course.Field<string>("CId") == CId
                   select new
                   {
                       StProgId = student.Field<string>("ProgId"),
                       CProgId = course.Field<string>("ProgId")
                   });

            var result = query.Single();

            if(result.StProgId == result.CProgId)
            {
                return true;
            }
            return false;
    }

        internal static bool IsValid(Data.Enrollments enroll)
        {
            if (enroll != null)
            {
                return true;
            }
            else
            {
                Multi_tier_Project.Form1.BLLMessage("Business Rules: Addition/Modification rejetée");
                return false;
            }
        }
    }
    internal class Courses
    {
        internal static int UpdateCourses()
        {
            return Data.Courses.UpdateCourses();
        }
    }
    internal class Programs
    {
        internal static int UpdatePrograms()
        {
            return Data.Programs.UpdatePrograms();
        }
    }
}
