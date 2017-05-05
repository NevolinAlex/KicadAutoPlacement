using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace KicadAutoPlacement.GenAlgorithm
{
    public class GeneticAlgorithm
    {
        /// <summary>
        /// Максимальное время жизни хромосомы в пуле
        /// </summary>
        public static int MaxChromosomeAge = 5;
        /// <summary>
        /// Размер пула хромосом
        /// </summary>
        public static int PoolSize = 100;
        /// <summary>
        /// Число лучших хромосом выживающих после селекции
        /// </summary>
        public static int SelectionCount = 60;

        public int IterationCount { get; set; }
        private List<Chromosome> Pool;
        private List<Chromosome> Buffer;
        public GeneticAlgorithm()
        {
            IterationCount = 0;
            Pool = new List<Chromosome>(PoolSize);
            Buffer = new List<Chromosome>();
            GenerateNewChromosomes(PoolSize);
        }
        public void Start()
        {
            while (Console.KeyAvailable == false)
            {
                MakeIteartion();
            }
        }

        public Chromosome GetBestChromosome()
        {
            return Buffer[Buffer.Count - 1];
        }
        private void MakeIteartion()
        {
            IterationCount++;
            GetOld();
            Mutate();
            Crossing();
            GetEvaluation();
            MakeSelection();
            GenerateNewChromosomes(PoolSize - Pool.Count());
        }

        private void GetOld()
        {
            foreach (var chromosome in Pool)
            {
                chromosome.Age++;
            }
        }
        private void GenerateNewChromosomes(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Pool.Add(new Chromosome());
            }
        }
        private void MakeSelection()
        {
            Pool = Pool.Where(x => x.Age <= GeneticAlgorithm.MaxChromosomeAge)
                .OrderBy(x => x.Valuation)
                .Take(SelectionCount)
                .ToList();
        }
        private void Mutate()
        {
            for (int i = 0; i < PoolSize; i++)
            {
                Pool.AddRange(Pool[i].Mutate());
            }
        }

        private void Crossing()
        {
            for (int i = 0; i < PoolSize/2; i++)
            {
                Pool.AddRange(Chromosome.Crossing(Pool[i], Pool[i+PoolSize/2]));
            }
        }

        private void GetEvaluation()
        {
            Chromosome bestPoolChromosome = new Chromosome();
            foreach (var chromosome in Pool)
            {
                chromosome.CalculateValuation();
                if (chromosome.Valuation < bestPoolChromosome.Valuation)
                {
                    bestPoolChromosome = chromosome;
                }
            }
            int bestBufferValuation = int.MaxValue;
            foreach (var chromosome in Buffer)
            {
                if (chromosome.Valuation < bestBufferValuation)
                    bestBufferValuation = chromosome.Valuation;

            }
            if (bestPoolChromosome.Valuation < bestBufferValuation)
            {
                Console.WriteLine(String.Format("Itertion: {0} \n Number of intersection: {1}",IterationCount, bestPoolChromosome.Valuation));
                Buffer.Add(new Chromosome(bestPoolChromosome));
            }

        }
    }
}
