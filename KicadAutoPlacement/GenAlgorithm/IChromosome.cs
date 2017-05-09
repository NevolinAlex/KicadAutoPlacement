using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KicadAutoPlacement.GenAlgorithm
{
    public interface IChromosome
    {
        int Age { get; set; }
        int Valuation { get; set; }
        int CalculateValuation();
        List<Chromosome> Crossing(Chromosome chromosome1, Chromosome chromosome2);
        List<Chromosome> Mutate();
    }
}
