using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableGA.Entitites
{
    internal class Room
    {
        private string _roomName;
        private int _maxNumberOfStudents;

        public Room(string roomName, int maxNumberOfStudents)
        {
            _roomName = roomName;
            _maxNumberOfStudents = maxNumberOfStudents;
        }

        public string RoomName { get => _roomName;}
        public int MaxNumberOfStudents { get => _maxNumberOfStudents;}
    }
}
