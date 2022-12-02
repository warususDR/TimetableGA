using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableGA.Entitites
{
    internal class Module
    {
        private string _moduleName;

        public Module(string moduleName)
        {
            _moduleName = moduleName;
        }

        public string ModuleName { get => _moduleName; }
    }
}
