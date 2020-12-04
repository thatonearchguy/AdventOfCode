using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    class Program
    {
        
        static void Main(string[] args)
        {
            /*double trees = 1;
            trees *= Day3Part1(1, 1, 1);
            trees *= Day3Part1(3, 3, 1);
            trees *= Day3Part1(5, 5, 1);
            trees *= Day3Part1(7, 7, 1);
            trees *= Day3Part1(1, 1, 2);
            */
            Console.WriteLine(Day4Part2());
        }
        public static List<string> Day4Helper()
        {
            var myDocuments = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
				"input.txt");
            var readLines = File.ReadAllText(myDocuments);
            List<string> workList = readLines.Split("\n\n").ToList();
            for (int i = 0; i < workList.Count; i++) workList[i] = Regex.Replace(workList[i], @"\n", " ");
            return workList;
        }
        public static int Day4Part1()
        {
            var Passports = Day4Helper();
            List<string> Required = new List<string>{"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"};
            Required.Sort();
            var workList = new List<string>();
            var foundIds = new List<string>();
            var counter = 0;
            foreach(var element in Passports)
            {
                workList = element.Split(" ").ToList();
                foreach(var metric in workList) foundIds.Add(metric.Substring(0, 3));
                foundIds.Sort();
                foundIds.Remove("cid");
                if (foundIds.SequenceEqual(Required)) counter += 1;
                foundIds.Clear();
            }
            return counter;
        }
        public static bool ByrCheck(List<string> fields)
        {
            if (Convert.ToInt32(fields[0].Split(":")[1]) >= 1920 && Convert.ToInt32(fields[0].Split(":")[1]) <= 2002) return true;
            else return false;
        }
        public static bool EclCheck(List<string> fields)
        {
            var x = Convert.ToString(fields[1].Split(":")[1]);
            if (x == "amb" || x == "blu" || x == "brn" || x == "gry" || x == "grn" || x == "hzl" || x == "oth") return true;
            else return false;
        }
        public static bool EyrCheck(List<string> fields)
        {
            if (Convert.ToInt32(fields[2].Split(":")[1]) >= 2020 && Convert.ToInt32(fields[2].Split(":")[1]) <= 2030) return true;
            else return false;
        }
        public static bool HclCheck(List<string> fields)
        {
            if (fields[3].Split(":").Skip(1).Any(c => !"0123456789abcdefABCDEF".Contains(c)) && fields[3].Split(":")[1].Length == 7 && fields[3].Split(":")[1][0] == '#') return true;
            else return false;
        }
        public static bool HgtCheck(List<string> fields)
        {
            try
            {
                var parsed = fields[4].Split(":")[1].ToString();
                var height = Convert.ToInt32(parsed.Substring(0, parsed.Length-2));
                var unit = parsed.Substring(parsed.Length-2, 2);
                if (unit == "in" && height >= 59 && height <= 76) return true;
                if (unit == "cm" && height >= 150 && height <= 193) return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }
        public static bool IyrCheck(List<string> fields)
        {
            if (Convert.ToInt32(fields[5].Split(":")[1]) >= 2010 && Convert.ToInt32(fields[5].Split(":")[1]) <= 2020) return true;
            else return false;
        }
        public static bool PidCheck(List<string> fields)
        {
            try
            {
               if(fields[6].Split(":").Skip(1).Any(c => !"0123456789".Contains(c)) && fields[6].Split(":")[1].Length == 9) return true;
               else return false;
            }
            catch
            { 
                return false;
            }
        }
        public static int Day4Part2()
        {
            var Passports = Day4Helper();
            var workList = new List<string>();
            var counter = 0;
            foreach(var element in Passports)
            {
                workList = element.Split(" ").ToList();
                workList.Sort();
                workList.RemoveAll(x => x.Contains("cid"));
                if (PidCheck(workList) && ByrCheck(workList) && EclCheck(workList) && EyrCheck(workList) && HclCheck(workList) && HgtCheck(workList) && IyrCheck(workList)) 
                {
                    counter += 1; //Not all on one line for debugging purposes. Also it forgets to report one, interesting. 
                }
                else continue;
            }
            return counter;
        }
        public static int mod(int x, int m){return (x%m + m)%m;}
        static int Day3Part1(int horizontalCounter, int right, int down)
        {
            var readLines = inputReader("input");
            //var horizontalCounter = 3;
            var counter = 0;
            for (int i = down; i < readLines.Count; i+=down)
            {
                if (readLines[i][horizontalCounter] == '#') counter += 1;
                horizontalCounter = mod(horizontalCounter+right, readLines[0].Length);
            }
            return counter;
        }
        static List<List<string>> Day2Helper()
        {
            var readLines = inputReader("input");
            var splitLists = new List<List<string>>();
            for(int i = 0; i < readLines.Count; i ++)
            {
                splitLists.Add(readLines[i].Split(" ").ToList());
                foreach(var element in splitLists[i][0].Split("-"))
                {
                    splitLists[i].Insert(0, element);
                }
                splitLists[i].RemoveAt(2);
            }
            return splitLists;
        }
        static int Day2Part2()
        {
            var counter = 0;
            var splitLists = Day2Helper();
            foreach(var line in splitLists)
            {
                if(line[3][Convert.ToInt32(line[0])-1] == line[2][0] 
                && line[3][Convert.ToInt32(line[1])-1] != line[2][0]
                || line[3][Convert.ToInt32(line[1])-1] == line[2][0] 
                && line[3][Convert.ToInt32(line[0])-1] != line[2][0]) counter += 1;
                //var instances = line[3].Count(x => x == line[2][0]);
                //if(instances <= Convert.ToInt32(line[0]) && instances >= Convert.ToInt32(line[1])) counter += 1;
            }
            return counter;
        }
        static int Day2Part1()
        {
            var counter = 0;
            var splitLists = Day2Helper();
            foreach(var line in splitLists)
            {
                var instances = line[3].Count(x => x == line[2][0]);
                if(instances <= Convert.ToInt32(line[0]) && instances >= Convert.ToInt32(line[1])) counter += 1;
            }
            return counter;
        }
        static int Day1Part1(){
            var charles = inputReader("input");
            var workList = new List<int>();
            foreach (var element in charles) workList.Add(Convert.ToInt32(element));
            for(int i = 0; i < workList.Count; i ++)
            {
                for(int j = 0; j < workList.Count; j ++)
                {
                    if (i == j) continue;
                    else{
                        if (workList[i] + workList[j] == 2020) return workList[i] * workList[j];
                    }
                }
            }
            return 0;
        }
        static int Day1Part2(){
            var charles = inputReader("input");
            var workList = new List<int>();
            foreach (var element in charles) workList.Add(Convert.ToInt32(element));
            for(int i = 0; i < workList.Count; i ++)
            {
                for(int j = 0; j < workList.Count; j ++)
                {
                    for(int k = 0; k < workList.Count; k ++)
                    {
                        if (i == j || j == k || i == j && j == k) continue;
                        else{
                            if (workList[i] + workList[j] + workList[k] == 2020) return workList[i] * workList[j] * workList[k];
                        }
                    }
                }
            }
            return 0;
        }
        static List<string> inputReader(string filename)
        {
            var myDocuments = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
				$"{filename}.txt");
			var readLines = File.ReadAllLines(myDocuments);
            return readLines.ToList(); 
        }
    }
}