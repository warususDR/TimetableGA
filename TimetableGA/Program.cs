
using ConsoleTables;
using TimetableGA;
using TimetableGA.Entitites;


#region TestData
List<string> times = new List<string> { "8:30 - 9:50", "10:00 - 11:20", "11:40 - 13:00", "13:30 - 14:50", "15:00 - 16:20", "16:30 - 17:50" };
List<string> teachers = new List<string> { "V. V. Bublyk", "M. M. Glybovets", "V. M. Mychalevych", "V. S. Protsenko", "O. V. Oletskiy", "Y. O. Yushechenko" };
Dictionary<string, int> rooms = new Dictionary<string, int> { { "123", 25 }, { "321", 15 }, { "222", 30 }, { "145", 10 }, {"201", 15 }, {"67", 50 } };
List<string> modules = new List<string> { "MOOP", "Artificial Intelligence", "Procedural Programming", "Theory of Relativity", "Computer Virusology", "Game Development" };
#endregion

TimetableData data = new TimetableData(times, teachers, rooms, modules);
GeneticAlgorithm gAlgorithm = new GeneticAlgorithm(data, 20);
List<List<string>> gaResult = gAlgorithm.RunAlgorithm();
ConsoleTable table = data.BuildTable(gaResult);
table.Write();
Console.WriteLine();