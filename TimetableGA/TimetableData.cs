using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableGA.Entitites;

namespace TimetableGA
{
    internal class TimetableData
    {
        private const int BITSTR_LENGTH = 64;

        private readonly List<string> _types = new List<string>() { "Lecture", "Practice" };
        private readonly List<string> _days = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        private List<string> _times;
        private List<Teacher> _teachers;
        private List<Room> _rooms;
        private List<Module> _modules;

        private Dictionary<string, string> _encodedTypes;
        private Dictionary<string, string> _encodedDays;
        private Dictionary<string, string> _encodedTimes;
        private Dictionary<string, Teacher> _encodedTeachers;
        private Dictionary<string, Room> _encodedRooms;
        private Dictionary<string, Module> _encodedModules;

        #region ForInsideUse
        private string BuildRandomTimetableEntry()
        {
            Random randomGenerator = new Random();
            List<string> moduleKeys = _encodedModules.Keys.ToList();
            List<string> teachersKeys = _encodedTeachers.Keys.ToList();
            List<string> roomsKeys = _encodedRooms.Keys.ToList();
            List<string> daysKeys = _encodedDays.Keys.ToList();
            List<string> timesKeys = _encodedTimes.Keys.ToList();
            List<string> typesKeys = _encodedTypes.Keys.ToList();
            string bitString = moduleKeys[randomGenerator.Next(0, moduleKeys.Count)] + teachersKeys[randomGenerator.Next(0, teachersKeys.Count)]
                + roomsKeys[randomGenerator.Next(0, roomsKeys.Count)]
                + daysKeys[randomGenerator.Next(0, daysKeys.Count)]
                + timesKeys[randomGenerator.Next(0, timesKeys.Count)]
                + typesKeys[randomGenerator.Next(0, typesKeys.Count)];
            return bitString;
        }

        private List<string> DecodeTimetableEntry(string entry)
        {
            List<string> separatedEntry = SeparateBitString(entry);
            List<string> result = new List<string>();
            result.Add(_encodedModules[separatedEntry[0]].ModuleName);
            result.Add(_encodedTeachers[separatedEntry[1]].TeacherName);
            result.Add(_encodedRooms[separatedEntry[2]].RoomName);
            result.Add(_encodedDays[separatedEntry[3]]);
            result.Add(_encodedTimes[separatedEntry[4]]);
            result.Add(_encodedTypes[separatedEntry[5]]);
            return result;
        }

        private List<string> SeparateBitString(string entry)
        {
            List<string> res = Enumerable.Range(0, entry.Length / BITSTR_LENGTH).Select(i => entry.Substring(i * BITSTR_LENGTH, BITSTR_LENGTH)).ToList();
            return res;
        }

        private Dictionary<string, string> EncodeTypes()
        {
            return Encoder<string>.EncodeList(_types);
        }

        private Dictionary<string, string> EncodeTimes()
        {
            return Encoder<string>.EncodeList(_times);
        }

        private Dictionary<string, string> EncodeDays()
        {
            return Encoder<string>.EncodeList(_days);
        }

        private Dictionary<string, Teacher> EncodeTeachers()
        {
            return Encoder<Teacher>.EncodeList(_teachers);
        }

        private Dictionary<string, Room> EncodeRooms()
        {
            return Encoder<Room>.EncodeList(_rooms);
        }

        private Dictionary<string, Module> EncodeModules()
        {
            return Encoder<Module>.EncodeList(_modules);
        }

        #endregion

        public TimetableData(List<string> times, List<string> teachers, Dictionary<string, int> rooms, List<string> modules)
        {
            _times = times;

            _teachers = new List<Teacher>();
            foreach (var name in teachers) _teachers.Add(new Teacher(name));

            _rooms = new List<Room>();
            foreach (var key in rooms.Keys) _rooms.Add(new Room(key, rooms[key]));

            _modules = new List<Module>();
            foreach (var name in modules) _modules.Add(new Module(name));

            _encodedTypes = EncodeTypes();
            _encodedTimes = EncodeTimes();
            _encodedDays = EncodeDays();
            _encodedTeachers = EncodeTeachers();
            _encodedRooms = EncodeRooms();
            _encodedModules = EncodeModules();
        }

        public int FindClassesCapacity(string name)
        {
            foreach (var room in _rooms)
            {
                if (room.RoomName == name) return room.MaxNumberOfStudents;
            }
            return -1;
        }

        #region ForOutsideUse
        public List<string> BuildRandomTimetable(int numberOfClasses)
        {
            List<string> res = new List<string>();
            for (int i = 0; i < numberOfClasses; i++) res.Add(BuildRandomTimetableEntry());
            return res;
        }

        public List<List<string>> DecodeTimetable (List<string> timetable)
        { 
            List<List<string>> res = new List<List<string>>();
            foreach (var day in _days)
            {
                foreach (var time in _times)
                {
                    foreach (var entry in timetable)
                    {
                        List<string> decoded = DecodeTimetableEntry(entry);
                        if (decoded.Contains(day))
                        {
                            if(decoded.Contains(time)) res.Add(decoded);
                        }
                    }
                }
            }           
            return res;
        }

        public ConsoleTable BuildTable(List<List<string>> items)
        {
            var table = new ConsoleTable("Subject", "Teacher", "Room", "Day", "Time", "Type");
            foreach (var row in items) table.AddRow(row.ToArray());
            return table;
        }
        #endregion

    }
}
