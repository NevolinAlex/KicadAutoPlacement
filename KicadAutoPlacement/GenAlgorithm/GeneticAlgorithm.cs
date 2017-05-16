using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static int SelectionCount = 80;

        public int IterationCount { get; set; }
        public List<Chromosome> Pool;
        private List<Chromosome> Buffer;
        public GeneticAlgorithm()
        {
            IterationCount = 0;
            Pool = new List<Chromosome>(PoolSize);
            Buffer = new List<Chromosome>();
            Buffer.Add(new Chromosome());
            GenerateNewChromosomes(PoolSize);
        }
        public void Start()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            while (Console.KeyAvailable == false)
            {
                MakeIteartion(stopWatch);
            }
            stopWatch.Stop();
        }

        public Chromosome GetBestChromosome()
        {
            return Buffer[Buffer.Count - 1];
        }
        private void MakeIteartion(Stopwatch stopWatch)
        {
            IterationCount++;
            GetOld();
            //Console.WriteLine("Итерация: " + IterationCount);
            Mutate();
            //Console.WriteLine("Время мутации: " + (double)stopwatch.Elapsed.Milliseconds/1000);

            Crossing();
            //Console.WriteLine("Время скрещивания: " + (double)stopwatch.ElapsedMilliseconds/1000);

            GetEvaluation(stopWatch);
            //Console.WriteLine("Время оценки: " + (double)stopwatch.ElapsedMilliseconds / 1000);

            MakeSelection();
            //Console.WriteLine("Время селекции: " + (double)stopwatch.ElapsedMilliseconds / 1000);

            GenerateNewChromosomes(PoolSize - Pool.Count());
           // Console.WriteLine("Время генерации новых хромосом: " + (double)stopwatch.ElapsedMilliseconds / 1000);
           // Console.WriteLine(String.Format("Number of intersection: {0}\n--------------------------------------", Buffer[0].Valuation));
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

        private void GetEvaluation(Stopwatch stopWatch)
        {
            GenAlgorithm.Chromosome bestPoolChromosome = new GenAlgorithm.Chromosome();
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
                Console.WriteLine(String.Format("Itertion: {0} \n Number of intersection: {1}\n Elapsed Time: {2}",IterationCount, bestPoolChromosome.Valuation, stopWatch.ElapsedMilliseconds/1000));
                Buffer[0] = new Chromosome(bestPoolChromosome);
            }

        }
    }

}
