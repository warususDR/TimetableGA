using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableGA
{
    internal class GeneticAlgorithm
    {
        private const int ITEM_SIZE = 64;
        private const int CHROMOSOME_SIZE = ITEM_SIZE * 6;

        private readonly TimetableData _timetableData;
        private readonly Random _random;

        private int _numberOfClasses;

        private int Fitness(string chromosome)
        {
            //0 module 1 teacher 2 room 3 day 4 time 5 type

            List<List<string>> currentTimetable = _timetableData.DecodeTimetable(SeparateChromosome(chromosome));
            int numberOfLectures = 0;
            int numberOfPractices = 0;
            int numberOfConflicts = 0;

            for(int i = 0; i < currentTimetable.Count; i++ )
            {
                List<string> entry1 = currentTimetable[i];

                if (entry1[5] == "Lecture")
                {
                    numberOfLectures++;
                    // лекції тільки в аудиторії де більше 25 місць
                    if (_timetableData.FindClassesCapacity(entry1[2]) < 25) numberOfConflicts++;
                }

                if (entry1[5] == "Practice")
                {
                    numberOfPractices++;
                    // практики в аудиторії де менше 25 місць
                    if (_timetableData.FindClassesCapacity(entry1[2]) > 25) numberOfConflicts++;
                }

                for (int j = 0; j < currentTimetable.Count; j++)
                {
                    List<string> entry2 = currentTimetable[j];
                    if(entry1 != entry2)
                    {
                        //однаковий вчитель
                        if (entry1[1] == entry2[1])
                        {
                            // однаковий час в однаковий день
                            if (entry1[4] == entry2[4] && entry1[3] == entry2[3]) numberOfConflicts++;

                        }
                        //різні вчителі
                        else
                        {
                            // одна і та сама лекція
                            if (entry1[5] == "Lecture" && entry2[5] == "Lecture" && entry1[0] == entry2[0]) numberOfConflicts++;
                        }
                    }
                }
            }
            //надто багато практик/лекцій
            if (numberOfPractices > 14 || numberOfLectures > 14) numberOfConflicts++;
            return numberOfConflicts;

        }

        private List<String> SeparateChromosome(string chromosome)
        {
            return Enumerable.Range(0, chromosome.Length / CHROMOSOME_SIZE).Select(i => chromosome.Substring(i * CHROMOSOME_SIZE, CHROMOSOME_SIZE)).ToList();

        }

        private string Select(List<string> population, List<int> scores, int k = 2)
        {
            int selectionIndex  = Random.Next(0, population.Count);
            List<int> indexes = new List<int>();
            for(int i = 0; i < k; i++)
            {
                int randIndex = Random.Next(0, population.Count);
                indexes.Add(randIndex);
            }
            foreach(var index in indexes)
            {
                if (scores[index] < scores[selectionIndex]) selectionIndex = index;
            }
            return population[selectionIndex];
        }

        private List<string> Crossover(string chromosome1, string chromosome2, double probability)
        {
            string child1 = chromosome1;
            string child2 = chromosome2;
            double randProbability = Random.NextDouble();
            if (randProbability < probability)
            {
                int crossPoint = Random.Next(1, (chromosome1.Length / ITEM_SIZE) - 2) * ITEM_SIZE;
                child1 = chromosome1.Substring(0, crossPoint) + chromosome2.Substring(crossPoint);
                child2 = chromosome2.Substring(0, crossPoint) + chromosome1.Substring(crossPoint);
            }
            return new List<string> { child1, child2 };
        }

        private string Mutate(string chromosome, double probability)
        {
            string MutatedChromosome = chromosome;
            double randProbability = Random.NextDouble();
            if (randProbability < probability)
            {
                MutatedChromosome = GenerateChromosome();
            }
            return MutatedChromosome;
        }

        private string GenerateChromosome()
        {
            List<string> chromosome = _timetableData.BuildRandomTimetable(NumberOfClasses);
            return String.Join(String.Empty, chromosome.ToArray());
        }

        public GeneticAlgorithm(TimetableData timetableData, int numberOfClasses)
        {
            _timetableData = timetableData;
            NumberOfClasses = numberOfClasses;
            _random = new Random();
        }

        public List<List<string>> RunAlgorithm(double crossoverProb = 0.8, double mutationProb = 1 / CHROMOSOME_SIZE, double iterations = 100, int populationSize = 500)
        {
            List<string> currentPopulation = new List<string>();
            for (int i = 0; i < populationSize; i++)
            {
                currentPopulation.Add(GenerateChromosome());
            }

            string bestChromosome = currentPopulation[0];

            // генеруємо популяції задану кількість ітерацій
            for(int iteration = 0; iteration < iterations; iteration++)
            {
                List<int> fitnesses = new List<int>();

                //оцінити популяцію
                foreach (var chromosome in currentPopulation) fitnesses.Add(Fitness(chromosome));

                //найкращий представник популяції
                int bestScore = fitnesses[0];
                for(int i = 1; i < fitnesses.Count; i++)
                {
                    if (fitnesses[i] < bestScore)
                    {
                        bestScore = fitnesses[i];
                        bestChromosome = currentPopulation[i];
                        if (bestScore == 0) return _timetableData.DecodeTimetable(SeparateChromosome(bestChromosome));
                    }
                }

                // дебажне виведення
                List<List<string>> decodedCurrBestTimetable = _timetableData.DecodeTimetable(SeparateChromosome(bestChromosome));
                _timetableData.BuildTable(decodedCurrBestTimetable).Write();
                Console.WriteLine($"Iteration: {iteration} Score: {bestScore}");
                Console.WriteLine();

                //селекція
                List<string> selectedChromosomes = new List<string>();
                for(int i = 0; i < currentPopulation.Count; i++)
                {
                    selectedChromosomes.Add(Select(currentPopulation, fitnesses));
                }

                //кросовер та мутація
                List<string> children = new List<string>();
                for(int i = 0; i < currentPopulation.Count; i+=2)
                {
                    string chromosome1 = selectedChromosomes[i];
                    string chromosome2 = selectedChromosomes[i+1];
                    foreach(var child in Crossover(chromosome1, chromosome2, crossoverProb))
                    {
                        Mutate(child, mutationProb);
                        children.Add(child);
                    }
                }

                currentPopulation = children;

            }

            List<List<string>> decodedBestTimetable = _timetableData.DecodeTimetable(SeparateChromosome(bestChromosome));
            return decodedBestTimetable;
        }
        public Random Random => _random;

        public int NumberOfClasses { get => _numberOfClasses; set => _numberOfClasses = value; }
    }
}
