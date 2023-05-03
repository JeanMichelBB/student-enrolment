using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using BusinessLayer;

namespace Data
{
    internal class Connect
    {
        private static String cliComConnectionString = GetConnectString();

        internal static String ConnectionString { get => cliComConnectionString; }

        private static String GetConnectString()
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder();
            cs.DataSource = "10.0.0.82";
            cs.InitialCatalog = "College1en";
            cs.UserID = "sa";
            cs.Password = "sysadm";
            return cs.ConnectionString;
        }
    }

    internal class DataTables
    {
        private static SqlDataAdapter adapterStudents = InitAdapterStudents();
        private static SqlDataAdapter adapterEnrollments = InitAdapterEnrollments();
        private static SqlDataAdapter adapterCourses = InitAdapterCourses();
        private static SqlDataAdapter adapterPrograms = InitAdapterPrograms();


        private static DataSet ds = InitDataSet();

        private static SqlDataAdapter InitAdapterStudents()
        {
            SqlDataAdapter r = new SqlDataAdapter(
           "SELECT * FROM Students ORDER BY StId", Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }


        private static SqlDataAdapter InitAdapterEnrollments()
        {
            SqlDataAdapter r = new SqlDataAdapter(
           "SELECT * FROM Enrollments ORDER BY StId", Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }



        private static SqlDataAdapter InitAdapterCourses()
        {
            SqlDataAdapter r = new SqlDataAdapter(
            "SELECT * FROM Courses ORDER BY CId", Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }
       
        private static SqlDataAdapter InitAdapterPrograms()
        {
            SqlDataAdapter r = new SqlDataAdapter(
            "SELECT * FROM Programs ORDER BY ProgId", Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }
        private static DataSet InitDataSet()
        {
            DataSet ds = new DataSet();
            loadPrograms(ds);
            loadCourses(ds);
            loadStudents(ds);
            loadEnrollments(ds);
            return ds;
        }
        private static void loadStudents(DataSet ds)
        {
            adapterStudents.Fill(ds, "Students");

            ds.Tables["Students"].Columns["StId"].AllowDBNull = false;
            ds.Tables["Students"].Columns["stName"].AllowDBNull = false;
            ds.Tables["Students"].Columns["ProgId"].AllowDBNull = false;

            ds.Tables["Students"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Students"].Columns["StId"]};


            ForeignKeyConstraint myFK = new ForeignKeyConstraint("MyFK",
                new DataColumn[]{
                    ds.Tables["Programs"].Columns["ProgId"]
                },
                new DataColumn[] {
                    ds.Tables["Students"].Columns["ProgId"],
                }
            );
            myFK.DeleteRule = Rule.None;
            myFK.UpdateRule = Rule.Cascade;
            ds.Tables["Students"].Constraints.Add(myFK);
        }
        internal static void loadEnrollments(DataSet ds)
        {
            adapterEnrollments.Fill(ds, "Enrollments");

            ds.Tables["Enrollments"].Columns["StId"].AllowDBNull = false;
            ds.Tables["Enrollments"].Columns["CId"].AllowDBNull = false;
            ds.Tables["Enrollments"].Columns["FinalNote"].AllowDBNull = true;

            ds.Tables["Enrollments"].PrimaryKey = new DataColumn[2]
                    { ds.Tables["Enrollments"].Columns["StId"],
                    ds.Tables["Enrollments"].Columns["CId"]};


            ForeignKeyConstraint myFK = new ForeignKeyConstraint("MyFK",
                new DataColumn[]{
                    ds.Tables["Courses"].Columns["CId"]
                },
                new DataColumn[] {
                    ds.Tables["Enrollments"].Columns["CId"],
                }
            );
            myFK.DeleteRule = Rule.None;
            myFK.UpdateRule = Rule.Cascade;
            ds.Tables["Enrollments"].Constraints.Add(myFK);

            ForeignKeyConstraint myFK2 = new ForeignKeyConstraint("MyFK2",
                new DataColumn[]{
                    ds.Tables["Students"].Columns["StId"]
                 },
                new DataColumn[] {
                    ds.Tables["Enrollments"].Columns["StId"],
                  }
            );
            myFK2.DeleteRule = Rule.None;
            myFK2.UpdateRule = Rule.Cascade;
            ds.Tables["Enrollments"].Constraints.Add(myFK2);
        }
        private static void loadCourses(DataSet ds)
        {
            adapterCourses.Fill(ds, "Courses");

            ds.Tables["Courses"].Columns["CId"].AllowDBNull = false;
            ds.Tables["Courses"].Columns["CName"].AllowDBNull = false;
            ds.Tables["Courses"].Columns["ProgId"].AllowDBNull = false;

            ds.Tables["Courses"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Courses"].Columns["CId"]};


            ForeignKeyConstraint myFK = new ForeignKeyConstraint("MyFK",
                new DataColumn[]{
                    ds.Tables["Programs"].Columns["ProgId"]
                },
                new DataColumn[] {
                    ds.Tables["Courses"].Columns["ProgId"],
                }
            );
            myFK.DeleteRule = Rule.None;
            myFK.UpdateRule = Rule.Cascade;
            ds.Tables["Courses"].Constraints.Add(myFK);
        }
        private static void loadPrograms(DataSet ds)
        {
            adapterPrograms.Fill(ds, "Programs");

            ds.Tables["Programs"].Columns["ProgId"].AllowDBNull = false;
            ds.Tables["Programs"].Columns["ProgName"].AllowDBNull = false;

            ds.Tables["Programs"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Programs"].Columns["ProgId"]};
        }
        internal static SqlDataAdapter getAdapterStudents()
        {
            return adapterStudents;
        }
        internal static SqlDataAdapter getAdapterEnrollments()
        {
            return adapterEnrollments;
        }
        internal static SqlDataAdapter getAdapterCourses()
        {
            return adapterCourses;
        }
        internal static SqlDataAdapter getAdapterPrograms()
        {
            return adapterPrograms;
        }
        internal static DataSet getDataSet()
        {
            return ds;
        }
     }

        internal class Students
        {
            private static SqlDataAdapter adapter = DataTables.getAdapterStudents();
            private static DataSet ds = DataTables.getDataSet();

            internal static DataTable GetStudents()
            {
                return ds.Tables["Students"];
            }
            internal static void ReloadStudents()
            {
                adapter.Fill(ds.Tables["Students"]);
            }
            internal static int UpdateStudents()
            {
                if (!ds.Tables["Students"].HasErrors)
                {
                    return adapter.Update(ds.Tables["Students"]);
                }
                else
                {
                    return -1;
                }
            }
        }
        internal class Enrollments
        {
            private static SqlDataAdapter adapter = DataTables.getAdapterEnrollments();
            private static DataSet ds = DataTables.getDataSet();

            private static DataTable displayEnroll = null;

            internal static DataTable GetDisplayEnrollments()
            {
                ds.Tables["Enrollments"].AcceptChanges();
                var query = (
                    from enrollment in ds.Tables["Enrollments"].AsEnumerable()
                    from student in ds.Tables["Students"].AsEnumerable()
                    from course in ds.Tables["Courses"].AsEnumerable()
                    from program in ds.Tables["Programs"].AsEnumerable()
                    where enrollment.Field<string>("StId") ==
                    student.Field<string>("StId")
                    where enrollment.Field<string>("CId") ==
                    course.Field<string>("CId")
                    where student.Field<string>("ProgId") ==
                    program.Field<string>("ProgId")
                    select new
                    {
                        StName = student.Field<string>("StName"),
                        CName = course.Field<string>("CName"),
                        ProgId = student.Field<string>("ProgId"),
                        ProgName = program.Field<string>("ProgName"),
                        CId = enrollment.Field<string>("CId"),
                        StId = enrollment.Field<string>("StId"),
                        FinalNote = enrollment.Field<int?>("FinalNote")
                    });
                    DataTable result = new DataTable();
                    result.Columns.Add("StName");
                    result.Columns.Add("CName");
                    result.Columns.Add("ProgId");
                    result.Columns.Add("ProgName");
                    result.Columns.Add("CId");
                    result.Columns.Add("StId");
                    result.Columns.Add("FinalNote");
                    foreach (var x in query)
                     {
                        object[] allFields = { x.StName, x.CName, x.ProgId, x.ProgName, x.CId, x.StId, x.FinalNote };
                        result.Rows.Add(allFields);
                     }
                    displayEnroll = result;
                    return displayEnroll;
             }
             internal static DataTable GetEnrollments()
            {
                return ds.Tables["Enrollments"];
            }
            internal static void ReloadEnrollments()
            {
                adapter.Fill(ds.Tables["Enrollments"]);
            }
            internal static int UpdateEnrollments()
            {
                if (!ds.Tables["Enrollments"].HasErrors)
                {
                    return adapter.Update(ds.Tables["Enrollments"]);
                }
                else
                {
                    return -1;
                }
            }
            internal static int InsertData(string[] a)
            {
                var test = (
                       from assign in ds.Tables["Enrollments"].AsEnumerable()
                       where assign.Field<string>("StId") == a[0]
                       where assign.Field<string>("CId") == a[1]
                       select assign);

                if (test.Count() > 0)
                {
                    Multi_tier_Project.Form1.DALMessage("This assignment already exists");
                    return -1;
                }
                try
                {
                    DataRow line = ds.Tables["Enrollments"].NewRow();

                    line.SetField("StId", a[0]);
                    line.SetField("CId", a[1]);
                    string finalGrade = null;

                    if (a.Length == 3) { 

                        finalGrade= a[2];
                    }
                    line.SetField("FinalNote", finalGrade);
                    ds.Tables["Enrollments"].Rows.Add(line);

                    adapter.Update(ds.Tables["Enrollments"]);

                    if (displayEnroll != null)
                    {
                        var query = (
                               from Students in ds.Tables["Students"].AsEnumerable()
                               from Courses in ds.Tables["Courses"].AsEnumerable()
                               from Programs in ds.Tables["Programs"].AsEnumerable()
                               where Students.Field<string>("StId") == a[0]
                               where Courses.Field<string>("CId") == a[1]
                               where Programs.Field<string>("ProgId") == Students.Field<string>("ProgId")
                               select new
                               {
                                   StName = Students.Field<string>("StName"),
                                   CName = Courses.Field<string>("CName"),
                                   ProgId = Programs.Field<string>("ProgId"),
                                   ProgName = Programs.Field<string>("ProgName"),
                                   CId = Courses.Field<string>("CId"),
                                   StId = Students.Field<string>("StId")
                               });
                    ;

                    var r = query.Single();
                        displayEnroll.Rows.Add(new object[] {r.StName,r.CName,r.ProgId,r.ProgName,r.CId,r.StId,finalGrade});
                    }
                    return 0;
                }
                catch (Exception e)
                {
                Multi_tier_Project.Form1.DALMessage("Insertion / Update rejected" + e.Message);
                return -1;
                }
            }
           internal static int DeleteData(List<string[]> lId)
            {
                try
                {

                    var lines = ds.Tables["Enrollments"].AsEnumerable()
                                    .Where(s =>
                                       lId.Any(x => (x[0] == s.Field<string>("StId") && x[1] == s.Field<string>("CId"))));

                    foreach (var line in lines)
                    {
                        line.Delete();
                    }

                    adapter.Update(ds.Tables["Enrollments"]);

                    if (displayEnroll != null)
                    {
                        foreach (var p in lId)
                        {
                            var r = displayEnroll.AsEnumerable()
                                    .Where(s => (s.Field<string>("StId") == p[0] && s.Field<string>("CId") == p[1]))
                                    .Single();
                        displayEnroll.Rows.Remove(r);
                        }
                    }
                    return 0;
                }
                catch (Exception e)
                {
                Multi_tier_Project.Form1.DALMessage("Update / Deleteion rejected" + e.Message);
                return -1;
                }
            }
        }
        internal class Courses
        {
            private static SqlDataAdapter adapter = DataTables.getAdapterCourses();
            private static DataSet ds = DataTables.getDataSet();

            internal static DataTable GetCourses()
            {
                return ds.Tables["Courses"];
            }
            internal static void ReloadCourses()
            {
                adapter.Fill(ds.Tables["Courses"]);
            }
            internal static int UpdateCourses()
            {
                if (!ds.Tables["Courses"].HasErrors)
                {
                    return adapter.Update(ds.Tables["Courses"]);
                }
                else
                {
                    return -1;
                }
            }
        }
        internal class Programs
        {
            private static SqlDataAdapter adapter = DataTables.getAdapterPrograms();
            private static DataSet ds = DataTables.getDataSet();

            internal static DataTable GetPrograms()
            {
                return ds.Tables["Programs"];
            }
            internal static void ReloadPrograms()
            {
                adapter.Fill(ds.Tables["Programs"]);
            }
        internal static int UpdatePrograms()
        {
            if (!ds.Tables["Programs"].HasErrors)
            {
                return adapter.Update(ds.Tables["Programs"]);
            }
            else
            {
                return -1;
            }
        }
             internal class Enrollments
        {

        }
    }        
}
