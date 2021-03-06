﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KicadAutoPlacement.GenAlgorithm;
using KicadAutoPlacement.Solvers;

namespace KicadAutoPlacement
{
    public class PrintedCircuitBoard
    {
        public List<Module> Modules; // list of elements
        public List<Net> NetList; // list of nets
        public static double DistanceBetweenModules { get; set; } = 1.2;
        public PrintedCircuitBoard(){}

        /// <summary>
        /// Конструктор копирующий конфигурацию платы 
        /// </summary>
        /// <param name="printedCircuitBoard"></param>
        public PrintedCircuitBoard(PrintedCircuitBoard printedCircuitBoard)
        {
            Modules = new List<Module>();
            NetList = new List<Net>();
            foreach (var curModule in printedCircuitBoard.Modules)
            {
                Module module = new Module(curModule.Name);
                module.Path = curModule.Path;
                module.Position = new Point(curModule.Position);
                module.Rotate = curModule.Rotate;
                if (curModule.IsLockedX())
                    module.LockXCoordinate();
                if (curModule.IsLockedY())
                    module.LockYCoordinate();
                module.LeftUpperBound = new Point(curModule.LeftUpperBound);
                module.RighLowerBound = new Point(curModule.RighLowerBound);
                foreach (var curPad in curModule.Pads)
                {

                    Pad pad = new Pad();
                    pad.Number = curPad.Number;
                    pad.Position = new Point(curPad.Position.X, curPad.Position.Y);
                    if (curPad.Net == null)
                        continue;
                    Net net = null;
                    try
                    {
                        foreach (var edge in NetList)
                        {
                            if (edge.Number == curPad.Net.Number && edge.Name == curPad.Net.Name)
                                net = edge;
                        }
                    }
                    catch (NullReferenceException e)
                    {
                    }
                    if (net == null)
                    {
                        pad.Net = new Net(curPad.Net.Name, curPad.Net.Number);
                        pad.Net.Pad1 = pad;
                        pad.Net.Pads.Add(pad);
                        NetList.Add(pad.Net);
                    }
                    else
                    {
                        pad.Net = net;
                        pad.Net.Pad2 = pad;
                        pad.Net.Pads.Add(pad);
                    }
                    pad.Module = module;
                    module.Pads.Add(pad);
                }
                Modules.Add(module);
            }
            // TODO:make a clone PrintedCircuitBoard from another PrintedCircuitBoard : Solved
        }

        /// <summary>
        /// Подсчет всех пересечений в данной конфигурации платы
        /// </summary>
        /// <returns></returns>
        public int GetIntersectionsNumber()
        {
            // TODO: get number of the intersections
            var netlist = GetAllNets(this);
            int intersectionCount = 0;
            for (int i = 0; i < netlist.Count; i++)
            {

                for (int j = i + 1; j < netlist.Count; j++)
                {
                    if (GeometricSolver.AreSegmentsIntersect(netlist[i].Item1, netlist[i].Item2, netlist[j].Item1,
                        netlist[j].Item2))
                        intersectionCount++;
                }
            }
            #region
            //for (int i =0; i < NetList.Count; i++)
            //{ 
            //    Point p1 = new Point(NetList[i].Pad1.Module.Position + NetList[i].Pad1.Position);
            //    if (NetList[i].Pad2 == null)
            //        continue;
            //    Point p2 = new Point(NetList[i].Pad2.Module.Position + NetList[i].Pad2.Position);
            //    for (int j = i+1; j < NetList.Count; j++)
            //    {
            //        Point q1 = new Point(NetList[j].Pad1.Module.Position + NetList[j].Pad1.Position);
            //        if (NetList[j].Pad2 ==null)
            //            continue;
            //        Point q2 = new Point(NetList[j].Pad2.Module.Position + NetList[j].Pad2.Position);
            //            if (PrintedCircuitBoard.AreSegmentsIntersect(p1, p2, q1, q2))
            //                intersectionCount++;
            //    }

            //}
            #endregion
            return intersectionCount;
        }
        /// <summary>
        /// Возвращает список всех соединений в виде пар точек(отрезков)
        /// </summary>
        /// <param name="pcb"></param>
        /// <returns></returns>
        public List<Tuple<Point, Point>> GetAllNets(PrintedCircuitBoard pcb)
        {
            List <Tuple<Point,Point>> lst = new List<Tuple<Point, Point>>();
            foreach (var net in pcb.NetList)
            {
                if (net.Pads.Count > 1)
                    lst.AddRange(GetSegmentListFromNet(net));
            }
            return lst;
        }
        /// <summary>
        /// Возвращает список пар точек(отрезков/соединений) которые принадлежат переданному соединению
        /// </summary>
        /// <param name="net"></param>
        /// <returns></returns>
        private List<Tuple<Point,Point>> GetSegmentListFromNet(Net net)
        {
            List<Tuple<Point, Point>> list = new List<Tuple<Point, Point>>();
            var notVisited = net.Pads.ToList();
            Pad nextPad = notVisited[0];
            while (true)
            {
                notVisited.Remove(nextPad);
                Point curPoint = new Point(nextPad.Position + nextPad.Module.Position);
                var minDistance = double.MaxValue;
                foreach (var e in notVisited)
                {
                    Point ePoint = new Point(e.Position + e.Module.Position);
                    var curDistance = GeometricSolver.GetDistance(curPoint, ePoint);
                    if (GeometricSolver.GetDistance(curPoint, ePoint) < minDistance)
                    {
                        minDistance = curDistance;
                        nextPad = e;
                    }
                }
                if (notVisited.Count == 0)
                {
                    list.Add(new Tuple<Point, Point>(new Point(curPoint),new Point(list[0].Item1) ));
                    list = list.OrderBy(x => GeometricSolver.GetDistance(x.Item1, x.Item2))
                        .Take(list.Count - 1)
                        .ToList();
                    break;
                }
                list.Add(new Tuple<Point, Point>(new Point(curPoint), new Point(nextPad.Position + nextPad.Module.Position) ));

            }

            #region comments
            //Point curPoint = null;
            //for (int i = 0; i < net.Pads.Count; i++)
            //{
            //    double minDistance = double.MaxValue;
            //    Point p1 = new Point(net.Pads[i].Position + net.Pads[i].Module.Position);
            //    for (int j = i+1; j < net.Pads.Count; j++)
            //    {
            //        Point p2 = new Point(net.Pads[j].Position + net.Pads[j].Module.Position);
            //        curDistance = GeometricSolver.GetDistance(p1, p2);
            //        if (curDistance < minDistance && !list.Contains(new Tuple<Point, Point>(p1, p2)))
            //        {
            //            curPoint = p2;
            //            minDistance = curDistance;
            //        }

            //    }
            //    if (minDistance != double.MaxValue)
            //        list.Add(new Tuple<Point, Point>(p1, curPoint));
            //}


            #endregion

            return list;
        }

        /// <summary>
        /// Приводит плату к корректному виду без пересечений модулей
        /// </summary>
        /// <param name="modules"> Список модулей которые нужно проверить на пересечение</param>
        public void LeadToCorrectForm(List<Module> modules)
        {
            int swapCount = 0;
            var notVisited = modules.ToList();
            foreach (var e in notVisited)
            {
                if (!Modules.Contains(e))
                    throw new Exception("Invalid arguments for lead to correct form");
            }
            var rnd = new Random();
            while (true)
            {
                if (notVisited.Count == 0)
                    break;
                var curModule = notVisited[notVisited.Count - 1];
                notVisited.Remove(curModule);
                
                foreach (var module in Modules)
                {
                    if (curModule.Equals(module))
                        continue;

                    if (AreModulesIntersect(curModule, module))
                    {
                        swapCount++;
                        if (module.IsLocked())
                            DivideModules(module,curModule);
                        else 
                            DivideModules(curModule,module);
                        if (!notVisited.Contains(module))
                            notVisited.Add(module);
                        if (!GeometricSolver.AreModuleInBounds(module.LeftUpperBound + module.Position,
                            module.RighLowerBound + module.Position,
                            Chromosome.LeftUpperPoint,
                            new Point(Chromosome.LeftUpperPoint.X + Chromosome.WorkspaceWidth,
                                Chromosome.LeftUpperPoint.Y + Chromosome.WorkspaceHeight)))
                        {
                            module.Position = GeometricSolver.GetRandomPointInRange(Chromosome.WorkspaceWidth, Chromosome.WorkspaceHeight, rnd) + Chromosome.LeftUpperPoint;
                        }
                    }
                }
                if (swapCount > 500)
                    break;
            }
        }

        /// <summary>
        /// Проверка на пересечение двух модулей на плоскости
        /// </summary>
        /// <param name="m1">Модуль 1</param>
        /// <param name="m2">Модуль 2</param>
        /// <returns></returns>
        public static bool AreModulesIntersect(Module m1, Module m2)
        {
            Point leftUpperPointM2 = new Point(m2.Position + m2.LeftUpperBound);
            Point rightLowerPointM2 = new Point(m2.Position + m2.RighLowerBound);
            Point leftUpperPointM1 = new Point(m1.Position + m1.LeftUpperBound);
            Point rightLowerPointM1 = new Point(m1.Position + m1.RighLowerBound);
            return ((leftUpperPointM1.X <= leftUpperPointM2.X && leftUpperPointM2.X <= rightLowerPointM1.X ||
                     leftUpperPointM2.X <= leftUpperPointM1.X && leftUpperPointM1.X <= rightLowerPointM2.X) &&
                    (leftUpperPointM1.Y <= leftUpperPointM2.Y && leftUpperPointM2.Y <= rightLowerPointM1.Y ||
                     leftUpperPointM2.Y <= leftUpperPointM1.Y && leftUpperPointM1.Y <= rightLowerPointM2.Y));
        }
        /// <summary>
        /// Вызывается если модули пересекаются, сдвигает второй модуль относительно первого
        /// на минимальное расстояние чтобы избежать пересечения
        /// </summary>
        /// <param name="m1">Модуль 1</param>
        /// <param name="m2">Модуль 2</param>
        private void DivideModules(Module m1, Module m2)
        {
            Point leftUpperPointM2 = new Point(m2.Position + m2.LeftUpperBound);
            Point rightLowerPointM2 = new Point(m2.Position + m2.RighLowerBound);
            Point leftUpperPointM1 = new Point(m1.Position + m1.LeftUpperBound);
            Point rightLowerPointM1 = new Point(m1.Position + m1.RighLowerBound);
            double minDistance = double.MaxValue;
            Direction direction = Direction.Left;
            if (rightLowerPointM2.X - leftUpperPointM1.X < minDistance)
            {
                minDistance = rightLowerPointM2.X - leftUpperPointM1.X;
                direction = Direction.Left;
            }
            if (rightLowerPointM1.X - leftUpperPointM2.X < minDistance)
            {
                direction = Direction.Right;
                minDistance = rightLowerPointM1.X - leftUpperPointM2.X;
            }
            if (-leftUpperPointM1.Y + rightLowerPointM2.Y < minDistance)
            {
                minDistance = -leftUpperPointM1.Y + rightLowerPointM2.Y;
                direction = Direction.Top;
                
            }
            if (-leftUpperPointM2.Y + rightLowerPointM1.Y < minDistance)
            {
                minDistance = -leftUpperPointM2.Y + rightLowerPointM1.Y;
                direction = Direction.Bottom;
            }
            var curDistance = minDistance;
            //if (m1.IsLocked())
            //    curDistance *= 2;
            switch (direction)
            {
                case Direction.Left:
                    m2.Position -= new Point(curDistance + DistanceBetweenModules,0);
                    break;
                case Direction.Right:
                    m2.Position += new Point(curDistance + DistanceBetweenModules, 0);
                    break;
                case Direction.Bottom:
                    m2.Position += new Point(0, curDistance + DistanceBetweenModules);
                    break;
                case Direction.Top:
                    m2.Position -= new Point(0, curDistance + DistanceBetweenModules);
                    break;
            }
        }


    }
}
