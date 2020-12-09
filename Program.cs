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
            */
            Console.WriteLine(Day9(true));
        }
        public static Int64 Day9(bool part2 = false)
        {
            var readlines = inputReader("input");
            var workList = new List<Int64>();
            foreach(var element in readlines) workList.Add(Convert.ToInt64(element));
            if(part2)
            {
                var sumList = new List<Int64>();
                var skip = 0;
                while(true)
                {
                    for(int i = skip; i < workList.Count(); i++)
                    {
                        sumList.Add(workList[i]);
                        if (sumList.Sum()>776203571) 
                        {
                            skip+=1; 
                            sumList.Clear();
                            break;
                        }
                        if (sumList.Sum() == 776203571) return sumList.Max() + sumList.Min();
                    }
                }
            }
            else{
                for (int i = 0; i < workList.Count()-25; i++)
                {
                    if((from n1 in workList from n2 in workList.Skip(i).Take(25) 
                        where n1 != n2 && n1 + n2 == workList[i+24]
                        select new {n1, n2}).Count() == 0) return workList[i+24]; 
                }
            }
            return 0;
        }
        public static int Day8(bool part2 = false)
        {
            var programCounter = 0;
            var accumulator = 0;
            var readLines = inputReader("input");
            for (int i = 0; i < readLines.Count(); i++) readLines[i] = Regex.Replace(readLines[i], @"\b+\b", "");
            var seenAddresses = new List<int>();
            var toReplace = new List<int>();
            if(part2)
            {
                accumulator = 0;
                programCounter = 0;
                for (int i = 0; i < readLines.Count(); i++)
                {
                    if(String.Join("", readLines[i].Take(3))=="jmp") toReplace.Add(i);
                    if(String.Join("", readLines[i].Take(3))=="nop") toReplace.Add(i);
                }
                foreach(var index in toReplace)
                {
                    accumulator = 0;
                    programCounter = 0;
                    var flipped = false;
                    if(readLines[index].Contains("nop")) readLines[index] = "jmp " + String.Join("", readLines[index].Skip(4));
                    else if(readLines[index].Contains("jmp")) 
                    {
                        readLines[index] = "nop " + String.Join("", readLines[index].Skip(4)); 
                        flipped = true;
                    }
                    while(seenAddresses.Contains(programCounter)==false)
                    {
                        seenAddresses.Add(programCounter);
                        if(String.Join("", readLines[programCounter].Take(3)) == "nop") programCounter+=1;
                        if(String.Join("", readLines[programCounter].Take(3)) == "acc")
                        {
                            accumulator += Convert.ToInt32(String.Join("", readLines[programCounter].Skip(4))); 
                            programCounter+=1;
                        }
                        if(String.Join("", readLines[programCounter].Take(3)) == "jmp") programCounter += Convert.ToInt32(String.Join("", readLines[programCounter].Skip(4))); 
                    }
                    seenAddresses.Clear();
                    if(programCounter!=641 && flipped == false) readLines[index] = "nop " + String.Join("", readLines[index].Skip(4));
                    else if (programCounter!=641 && flipped == true) readLines[index] = "jmp " + String.Join("", readLines[index].Skip(4));
                    else break;
                }
            }
            else{
                while(seenAddresses.Contains(programCounter)==false)
                {
                    seenAddresses.Add(programCounter);
                    if(String.Join("", readLines[programCounter].Take(3)) == "nop") programCounter+=1;
                    if(String.Join("", readLines[programCounter].Take(3)) == "acc")
                    {
                        accumulator += Convert.ToInt32(String.Join("", readLines[programCounter].Skip(4))); 
                        programCounter+=1;
                    }
                    if(String.Join("", readLines[programCounter].Take(3)) == "jmp") programCounter += Convert.ToInt32(String.Join("", readLines[programCounter].Skip(4))); 
                }
            }
            return accumulator;
        }
        public static int Day7(bool part2 = false)
        {
            var readLines = inputReader("input");
            var workList = new List<string>();
            var discard = new List<string>{"bag", "bags", "bag,", "bags,", " contain", "bag.", "bags."};
            var goldenColours = new List<string>();
            var newColours = new List<string>();
            var individualColours = new List<string>();
            var containsColours = new List<string>();
            var total = 0;
            for (int i=0; i < readLines.Count(); i++) readLines[i] = Regex.Replace(string.Join(" ", readLines[i].Split(' ').Where(w => !discard.Contains(w))), @"\b contain\b", "");
            foreach (var element in readLines)
            {
                if(String.Join(" ", element.Split(" ").Skip(2)).Contains("shiny gold")) 
                {
                    goldenColours.Add(String.Join(" ", element.Split(" ").Take(2)));
                    total += 1;
                }
            }
            foreach (var element in readLines)
            {
                foreach (var colour in goldenColours){
                    if(String.Join(" ", element.Split(" ").Skip(2)).Contains(colour)) {
                        newColours.Add(String.Join(" ", element.Split(" ").Take(2)));
                    }
                }
            }
            foreach (var element in readLines)
            {
                foreach (var colour in newColours){
                    if(String.Join(" ", element.Split(" ").Skip(2)).Contains(colour)) {
                        newColours.Add(String.Join(" ", element.Split(" ").Take(2)));
                    }
                }
            }
            return goldenColours.Count() + newColours.Distinct().Count();
        }
        public static int Day6(bool part2 = false)
        {
            var readLines = Day4Helper();
            var questions = new List<string>();
            var total = 0;
            foreach (var question in readLines)
            {
                if(part2){
                    questions = question.Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList();
                    foreach(var character in questions[0])
                    {
                        if(questions.Count(x => x.Contains(character) == true) == questions.Count()) total += 1;
                    }
                }
                else{
                    total += Regex.Replace(question, @"\s+", "").Distinct().Count();
                }
            }
            return total;
        }
        public static int Day5(bool part2 = false)
        {
            var readLines = inputReader("input");
            var scores = new List<int>();
            var row = 0;
            var column = 0;
            foreach (var element in readLines)
            {
                row = Convert.ToInt32(element.Substring(0, 7).Replace("B", "1").Replace("F", "0"), 2);
                column = Convert.ToInt32(element.Substring(7, 3).Replace("R", "1").Replace("L", "0"), 2);
                scores.Add((row * 8) + column);
            }
            if (part2)
            {
                scores.Sort();
                for (int i = 0; i < scores.Count()-1; i++)
                {
                    if(scores[i+1]-scores[i]!=1) return scores[i+1]-1;
                }
            }
            else return scores.Max();
            return 0;
        }
        public static List<string> Day4Helper()
        {
            var myDocuments = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
				"input.txt");
            var readLines = File.ReadAllText(myDocuments);
            List<string> workList = readLines.Split("\n\n").ToList();
            //for (var i = 0; i < workList.Count; i++) workList[i] = Regex.Replace(workList[i], @"\n", " ");
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
                workList = element.Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList();
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
               //Unintentional genius, this check can only start if there's 7 items in the list, or it throws an index out of range which I catch below. Very efficient
               //c=> !"0123456789" will return false if it's not an integer
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
                //Putting PidCheck() first actually turned out to make my code super efficient, it auto-rejects anything with six items
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