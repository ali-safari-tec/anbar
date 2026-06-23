using System.Collections.Generic;
using System.Linq;

namespace anbar.Tools
{
    public class Calculater
    {
        public class Order
        {
            public double Length { get; set; }
            public double Quantity { get; set; }
        }

        public class Combination
        {
            public double Sum { get; set; }
            public Dictionary<double, double> Used { get; set; } = new Dictionary<double, double>();
        }

        public class BarPlan
        {
            public double BarIndex { get; set; }
            public double Sum { get; set; }
            public double Leftover { get; set; }
            public Dictionary<double, double> Cuts { get; set; } = new Dictionary<double, double>();
        }

        public class HalfBar
        {
            public double LengthH { get; set; }
            public double QuantityH { get; set; }
        }

        public class CuttingOptimizer
        {
            public Combination FindBestCombination(List<Order> orders, int index, double currentSum, double barLength, double minLeftover, double maxLeftover)
            {
                if (index >= orders.Count)
                {
                    double leftover = barLength - currentSum;
                    if (leftover < minLeftover || leftover > maxLeftover)
                        return new Combination { Sum = -1 };
                    return new Combination { Sum = currentSum };
                }
                Combination best = new Combination { Sum = -1 };
                for (double q = 0; q <= orders[index].Quantity; q++)
                {
                    double newSum = currentSum + q * orders[index].Length;
                    if (newSum > barLength)
                        break;
                    Combination comb = FindBestCombination(orders, index + 1, newSum, barLength, minLeftover, maxLeftover);
                    if (q > 0 && comb.Sum != -1)
                    {
                        if (comb.Used.ContainsKey(orders[index].Length))
                            comb.Used[orders[index].Length] += q;
                        else
                            comb.Used[orders[index].Length] = q;
                    }
                    if (comb.Sum > best.Sum)
                        best = comb;
                }
                return best;
            }

            public List<BarPlan> OptimizeCutting(double numberOfBars, double barLength, List<Order> orders, List<HalfBar> halfBars, double minLeftover, double maxLeftover)
            {
                List<BarPlan> plans = new List<BarPlan>();
                int barIndex = 1;
                List<Order> remainingOrders = orders.Select(o => new Order { Length = o.Length, Quantity = o.Quantity }).ToList();

                foreach (var halfBar in halfBars)
                {
                    while (halfBar.QuantityH > 0 && remainingOrders.Any(o => o.Quantity > 0))
                    {
                        List<Order> ordersCopy = remainingOrders.Select(o => new Order { Length = o.Length, Quantity = o.Quantity }).ToList();
                        Combination comb = FindBestCombination(ordersCopy, 0, 0, halfBar.LengthH, minLeftover, maxLeftover);
                        if (comb.Sum == -1)
                            break;

                        BarPlan plan = new BarPlan
                        {
                            BarIndex = barIndex,
                            Sum = comb.Sum,
                            Leftover = halfBar.LengthH - comb.Sum,
                            Cuts = new Dictionary<double, double>(comb.Used)
                        };
                        plans.Add(plan);

                        foreach (var used in comb.Used)
                        {
                            Order order = remainingOrders.First(o => o.Length == used.Key);
                            order.Quantity -= used.Value;
                        }

                        halfBar.QuantityH--;
                        barIndex++;
                    }
                }

                while (remainingOrders.Any(o => o.Quantity > 0) && barIndex <= numberOfBars)
                {
                    List<Order> ordersCopy = remainingOrders.Select(o => new Order { Length = o.Length, Quantity = o.Quantity }).ToList();
                    Combination comb = FindBestCombination(ordersCopy, 0, 0, barLength, minLeftover, maxLeftover);
                    if (comb.Sum == -1)
                        break;

                    BarPlan plan = new BarPlan
                    {
                        BarIndex = barIndex,
                        Sum = comb.Sum,
                        Leftover = barLength - comb.Sum,
                        Cuts = new Dictionary<double, double>(comb.Used)
                    };
                    plans.Add(plan);

                    foreach (var used in comb.Used)
                    {
                        Order order = remainingOrders.First(o => o.Length == used.Key);
                        order.Quantity -= used.Value;
                    }

                    barIndex++;
                }

                if (remainingOrders.Any(o => o.Quantity > 0))
                {
                    BarPlan remainingPlan = new BarPlan
                    {
                        BarIndex = barIndex,
                        Sum = remainingOrders.Sum(o => o.Length * o.Quantity),
                        Leftover = 0,
                        Cuts = new Dictionary<double, double>()
                    };
                    foreach (var order in remainingOrders)
                    {
                        if (order.Quantity > 0)
                        {
                            remainingPlan.Cuts[order.Length] = order.Quantity;
                        }
                    }
                    plans.Add(remainingPlan);
                }
                return plans;
            }
        }
    }
}
