using org.mariuszgromada.math.mxparser;

namespace GeneticAlgorithm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Function function1 = new Function("f(x1, x2) = 4 * (x1 - 5)^2 + (x2 - 6)^2");
            Function function2 = new Function("f(x1, x2) = x1^2 + 3 * x2^2 + 2 * x1 * x2");

            int populationSize = 10;
            double mutationProbability = 0.125;
            double crossoverProbability = 0.7;
            int generationCount = 70;

            GeneticAlgorithm algorithm = new GeneticAlgorithm(function2, populationSize,mutationProbability, crossoverProbability, generationCount);
            double[] result = algorithm.Run();

            label1.Text = String.Format("x1 = {0}, x2 = {1}", result[0], result[1]);
        }
    }
}