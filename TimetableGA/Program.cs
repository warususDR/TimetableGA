
using ConsoleTables;
using TimetableGA;
using TimetableGA.Entitites;


// test data
List<string> times = new List<string> { "8:30 - 9:50", "10:00 - 11:20", "11:40 - 13:00", "13:30 - 14:50", "15:00 - 16:20", "16:30 - 17:50" };
List<string> teachers = new List<string> { "В. В. Бублик", "М. М. Глибовець", "В. М. Михалевич", "В. С. Проценко" };
Dictionary<string, int> rooms = new Dictionary<string, int> { { "123", 25 }, { "321", 15 }, { "222", 30 } };
List<string> modules = new List<string> { "МООП", "ОШІ", "Процедурне програмування" };

TimetableData data = new TimetableData(times, teachers, rooms, modules);
List<string> encodedRandomTimetable = data.BuildRandomTimetable(50);
List<List<string>> decodedTimetable = data.DecodeTimetable(encodedRandomTimetable);
ConsoleTable table = data.BuildTable(decodedTimetable);
table.Write();
Console.WriteLine();