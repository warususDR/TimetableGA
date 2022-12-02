using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableGA.Entitites
{
    internal class Teacher
    {
        string _teacherName;

        public Teacher(string teacherName)
        {
            _teacherName = teacherName;
        }

        public string TeacherName { get => _teacherName; }
    }
}
