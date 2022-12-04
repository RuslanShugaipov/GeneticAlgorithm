using System.Collections;
using org.mariuszgromada.math.mxparser;

namespace GeneticAlgorithm
{
    public class Chromosome
    {
        public int[] Genes { get; set; }
        public double Fitness { get; private set; }
        public Tuple<double, double> range { get; set; }

        //количество значений
        private double n;
        //количество разрядов
        private int m;
        //точность
        private double p;

        public Chromosome(Tuple<double, double> range)
        {
            this.range = range;
            Genes = new int[2];
            n = range.Item2 - range.Item1 + 1;
            m = (int)Math.Log2(n) + 1;
            p = (n - 1) / (Math.Pow(2, m) - 1);
        }

        public Chromosome(double x1, double x2, Tuple<double, double> range)
        {
            this.range = range;
            Genes = new int[2] { Code(x1), Code(x2) };
            n = range.Item2 - range.Item1 + 1;
            m = (int)Math.Log2(n) + 1;
            p = (n - 1) / (Math.Pow(2, m) - 1);
        }

        public int Code(double x)
        {
            //целочисленное значение (двоичное представление)
            var g = (int)Math.Floor((x - range.Item1) / p);

            //код Грея
            var grayCode = g ^ (g >> 1);

            return grayCode;
        }

        public double Decode(int x)
        {
            //из кода Грея в двоичный
            int bin;
            for (bin = 0; x != 0; x >>= 1)
            {
                bin ^= x;
            }

            //вещественное значение
            var r = bin * p + range.Item1;

            return r;
        }

        public double CalculateFitness(Function function)
        {
            double x1 = Decode(Genes[0]), x2 = Decode(Genes[1]);
            Argument a1 = new Argument("a1", x1);
            Argument a2 = new Argument("a2", x2);
            Expression e = new Expression("f(a1, a2)", function, a1, a2);
            Fitness = e.calculate();
            return Fitness;
        }

        public Tuple<Chromosome, Chromosome> Crossover(Chromosome otherParent)
        {
            Chromosome descendant1 = new Chromosome(range);
            Chromosome descendant2 = new Chromosome(range);

            //получаем строковое представление хромосомы первого родителя
            BitArray arrayThis1 = new BitArray(new int[2] { Genes[0], 0 });
            arrayThis1.Length = m;
            Utils.Reverse(arrayThis1);

            BitArray arrayThis2 = new BitArray(new int[2] { Genes[1], 0 });
            arrayThis2.Length = m;
            Utils.Reverse(arrayThis2);

            BitArray chromosome1 = Utils.Append(arrayThis1, arrayThis2);
            chromosome1.Length = 2 * m;

            //получаем строковое представление хромосомы второго родителя
            BitArray arrayOther1 = new BitArray(new int[2] { otherParent.Genes[0], 0 });
            arrayOther1.Length = m;
            Utils.Reverse(arrayOther1);

            BitArray arrayOther2 = new BitArray(new int[2] { otherParent.Genes[1], 0 });
            arrayOther2.Length = m;
            Utils.Reverse(arrayOther2);

            BitArray chromosome2 = Utils.Append(arrayOther1, arrayOther2);
            chromosome2.Length = 2 * m;

            int index = new Random().Next(0, 6);
            var splittedChromosome1 = Utils.Split(chromosome1, index, 2 * m);
            var splittedChromosome2 = Utils.Split(chromosome2, index, 2 * m);

            BitArray descendant1BitArray = Utils.Append(splittedChromosome1.Item1, splittedChromosome2.Item2);
            BitArray descendant2BitArray = Utils.Append(splittedChromosome2.Item1, splittedChromosome1.Item2);

            var splittedDescendant1 = Utils.Split(descendant1BitArray, 3, 2 * m);
            var splittedDescendant2 = Utils.Split(descendant2BitArray, 3, 2 * m);

            int[] gene = new int[1];
            Utils.Reverse(splittedDescendant1.Item1);
            splittedDescendant1.Item1.CopyTo(gene, 0);
            descendant1.Genes[0] = gene[0];
            Utils.Reverse(splittedDescendant1.Item2);
            splittedDescendant1.Item2.CopyTo(gene, 0);
            descendant1.Genes[1] = gene[0];

            Utils.Reverse(splittedDescendant2.Item1);
            splittedDescendant2.Item1.CopyTo(gene, 0);
            descendant2.Genes[0] = gene[0];
            Utils.Reverse(splittedDescendant2.Item2);
            splittedDescendant2.Item2.CopyTo(gene, 0);
            descendant2.Genes[1] = gene[0];

            return new Tuple<Chromosome, Chromosome>(descendant1, descendant2);
        }

        public void Mutation(double probability)
        {
            BitArray arrayThis1 = new BitArray(new int[2] { Genes[0], 0 });
            arrayThis1.Length = m;
            Utils.Reverse(arrayThis1);

            BitArray arrayThis2 = new BitArray(new int[2] { Genes[1], 0 });
            arrayThis2.Length = m;
            Utils.Reverse(arrayThis2);

            BitArray chromosome = Utils.Append(arrayThis1, arrayThis2);
            chromosome.Length = 2 * m;

            for(int i = 0; i < chromosome.Count; ++i)
            {
                if(probability > Utils.GetRandomDoubleInRange(0, 1))
                {
                    chromosome[i] = !chromosome[i];
                }
            }
        }
    }
}
