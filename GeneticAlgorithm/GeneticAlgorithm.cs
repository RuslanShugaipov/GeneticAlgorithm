using org.mariuszgromada.math.mxparser;

namespace GeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        public List<Chromosome> Population { get; private set; }
        public int Generation { get; private set; }
        public int GenerationCount { get; private set; }

        private Function Function;
        private double MutationProbability;
        private double CrossoverProbability;
        private int PopulationSize;
        private Random random;

        public GeneticAlgorithm(Function function, int populationSize, double mutationProbability, double crossoverProbability, int generationCount)        {
            Function = function;
            MutationProbability = mutationProbability;
            CrossoverProbability = crossoverProbability;
            PopulationSize = populationSize;
            random = new Random();

            Population = new List<Chromosome>();
            Generation = 0;
            GenerationCount = generationCount;

            InitPopulation();
            NewGeneration();
        }

        public double[] Run()
        {
            while (Generation < GenerationCount)
            {
                CalculateFitnessForPopulation();
                var NewPopulation = Crossover();
                Population = NewPopulation;
                Mutation();
                Generation++;
            }

            double[] result = new double[2];
            Chromosome bestChromosome = new Chromosome(Population[0].range);
            double minValue = Double.MaxValue;
            foreach(Chromosome chromosome in Population)
            {
                if(minValue > chromosome.Fitness)
                {
                    bestChromosome = chromosome;
                    minValue = bestChromosome.Fitness;
                }
            }
            result[0] = bestChromosome.Decode(bestChromosome.Genes[0]);
            result[1] = bestChromosome.Decode(bestChromosome.Genes[1]);
            return result;
        }

        private void InitPopulation()
        {
            for (int i = 0; i < PopulationSize; ++i)
            {
                //при создании особи (хромосомы) задаем интервал значений для аргументов функции
                Chromosome chromosome = new Chromosome(new Tuple<double, double>(0, 10));
                Population.Add(chromosome);
            }
        }

        private void NewGeneration()
        {
            if (Generation == 0)
            {
                for (int i = 0; i < PopulationSize; ++i)
                {
                    // j < 2, где 2 - число генов
                    for (int j = 0; j < 2; ++j)
                    {
                        Chromosome x = Population[i];
                        x.Genes[j] = x.Code(Utils.GetRandomDoubleInRange(x.range.Item1, x.range.Item2));
                    }
                }
            }
            Generation++;
        }

        private List<Chromosome> Crossover()
        {
            //потомство
            List<Chromosome> offspring = new List<Chromosome>();
            Chromosome descendant1, descendant2;
            for (int k = 0; k < PopulationSize;)
            {
                Chromosome parent1 = TournamentSelection();
                Chromosome parent2 = TournamentSelection();
                //если случайно будет выбран один и тот же родитель
                while (parent1 == parent2)
                {
                    parent2 = TournamentSelection();
                }
                if (CrossoverProbability > random.NextDouble())
                {
                    var descendants = parent1.Crossover(parent2);
                    descendant1 = descendants.Item1;
                    descendant2 = descendants.Item2;
                }
                else
                {
                    descendant1 = parent1;
                    descendant2 = parent2;
                }
                offspring.Add(descendant1);
                offspring.Add(descendant2);
                k = k + 2;
            }

            return offspring;
        }

        private void Mutation()
        {
            foreach (Chromosome chromosome in Population)
            {
                chromosome.Mutation(MutationProbability);
            }
        }

        private Chromosome TournamentSelection(int t = 3)
        {
            double sumFitness = 0;
            List<Chromosome> selectedChromosomes = new List<Chromosome>();
            for (int i = 0; i < t; ++i)
            {
                selectedChromosomes.Add(Population[random.Next(0, PopulationSize)]);
            }
            double minValue = Double.MaxValue;
            int minIndex = 0;
            for (int i = 0; i < selectedChromosomes.Count; ++i)
            {
                if (minValue > selectedChromosomes[i].Fitness)
                {
                    minValue = selectedChromosomes[i].Fitness;
                    minIndex = i;
                }
            }
            return selectedChromosomes[minIndex];
        }

        private void CalculateFitnessForPopulation()
        {
            foreach (Chromosome chromosome in Population)
            {
                chromosome.CalculateFitness(Function);
            }
        }
    }
}
