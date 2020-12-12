using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine(Day12Part2());
        }
        public static char DirectionToChar(int direction)
        {
            if(direction == 90) return 'E';
            if(direction == 0) return 'N';
            if(direction == 180) return 'S';
            if(direction == 270) return 'W';
            else return 'E';
        }
        public static List<int> PointRotator(List<int> coordinate, int direction)
        {
            var newCoordinate = new List<int>();
            if(direction == 90)
            {
                newCoordinate.Add(coordinate[1]);
                newCoordinate.Add(coordinate[0]*-1);
            }
            if(Math.Abs(direction) == 180)
            {
                newCoordinate.Add(coordinate[0]*-1);
                newCoordinate.Add(coordinate[1]*-1);
            }
            if(direction == 270)
            {
                newCoordinate.Add(coordinate[1]*-1);
                newCoordinate.Add(coordinate[0]);
            }
            if(direction == 0)
            {
                newCoordinate.Add(0);
                newCoordinate.Add(0);
            }
            if(direction == -90) return PointRotator(coordinate, 270);
            if(direction == -270) return PointRotator(coordinate, 90);
            if(Math.Abs(direction)==360) return coordinate;
            return newCoordinate;
        } 
        public static int Day12Part2()
        {
            var readlines = inputReader("input");
            var wayPointPosition = new List<int>{10, 1};
            var xPosition = 0;
            var yPosition = 0;
            var toMove = new char();
            foreach(var line in readlines)
            {
                toMove = line[0];
                if(toMove == 'F') 
                {
                    xPosition += wayPointPosition[0]*Convert.ToInt32(String.Join("", line.ToCharArray().Skip(1)));
                    yPosition += wayPointPosition[1]*Convert.ToInt32(String.Join("", line.ToCharArray().Skip(1)));
                }
                if(toMove == 'N') wayPointPosition[1] += Convert.ToInt32(String.Join("", line.ToCharArray().Skip(1)));
                if(toMove == 'S') wayPointPosition[1] -= Convert.ToInt32(String.Join("", line.ToCharArray().Skip(1)));
                if(toMove == 'E') wayPointPosition[0] += Convert.ToInt32(String.Join("", line.ToCharArray().Skip(1)));
                if(toMove == 'W') wayPointPosition[0] -= Convert.ToInt32(String.Join("", line.ToCharArray().Skip(1)));
                if(toMove == 'R') 
                {
                    wayPointPosition = PointRotator(wayPointPosition, Convert.ToInt32(String.Join("", line.ToCharArray().Skip(1))));
                }
                if(toMove == 'L') 
                {
                    wayPointPosition = PointRotator(wayPointPosition, Convert.ToInt32(String.Join("", line.ToCharArray().Skip(1)))*-1);
                } 
            }
            return Math.Abs(xPosition) + Math.Abs(yPosition);
        }
        public static int Day12Part1()
        {
            var readlines = inputReader("input");
            var direction = 90;
            var xPosition = 0;
            var yPosition = 0;
            var toMove = new char();
            foreach(var line in readlines)
            {
                toMove = line[0];
                if(toMove == 'F') toMove = DirectionToChar(direction);
                if(toMove == 'N') yPosition += Convert.ToInt32(String.Join("", line.ToCharArray().Skip(1)));
                if(toMove == 'S') yPosition -= Convert.ToInt32(String.Join("", line.ToCharArray().Skip(1)));
                if(toMove == 'E') xPosition += Convert.ToInt32(String.Join("", line.ToCharArray().Skip(1)));
                if(toMove == 'W') xPosition -= Convert.ToInt32(String.Join("", line.ToCharArray().Skip(1)));
                if(toMove == 'R') 
                {
                    direction = mod(direction+Convert.ToInt32(String.Join("", line.ToCharArray().Skip(1))), 360);
                }
                if(toMove == 'L') 
                {
                    direction = mod(direction-Convert.ToInt32(String.Join("", line.ToCharArray().Skip(1))), 360);
                }
            }
            return Math.Abs(xPosition) + Math.Abs(yPosition);
        }
        public static void Day11ListPrinter(List<string> list)
        {
            var total = 0;
            foreach(var line in list)
            {
                Console.WriteLine(line);
                total += line.Count(x=>x=='#');
            }
            Console.WriteLine(total);
        }
        public static List<string> Day11ListCopy(List<string> list)
        {
            var returnList = new List<string>();
            foreach (var element in list)
            {
                returnList.Add(element);
            }
            return returnList;
        }
        public static int Day11(bool part2 = false)
        {
            var freshSeats = inputReader("input");
            var duplicateSeats = inputReader("input"); //Easiest way I could see of deep copying the input xD
            var occupiedCounter = 0;
            var iterationCounter = 0; //Unnecessary but I was curious to see how many iterations it took to stabilise the seats.
            var Indexes = new Dictionary<List<int>, List<List<int>>>();
            for(int i = 0; i < freshSeats.Count(); i ++)
            {
                for(int x = 0; x < freshSeats[i].Count(); x ++)
                {
                    Indexes.Add(new List<int>{i, x}, GetIndexes(x, i, freshSeats, part2)); 
                    //Calls GetIndex function to find all the valid seats to consider for every index. 
                }
            }
            while(true){
                freshSeats = Day11ListCopy(duplicateSeats); //Simple function helper to deep copy a list. 
                var DictKeys = Indexes.Keys; //Collection of every key in the dictionary, and hence every index of valid seats in the airport.
                foreach(var Key in DictKeys) 
                {
                    occupiedCounter = 0;
                    foreach(var Seat in Indexes[Key]) //Goes through the visible seats for every index. 
                    {
                        if(freshSeats[Seat[0]][Seat[1]]=='#') occupiedCounter+=1;
                    }
                    StringBuilder line = new StringBuilder(duplicateSeats[Key[0]]);
                    if(freshSeats[Key[0]][Key[1]]=='L' && occupiedCounter == 0) line[Key[1]]='#';
                    if(freshSeats[Key[0]][Key[1]]=='#' && occupiedCounter >=5 && part2) line[Key[1]]='L';
                    if(part2!=true && freshSeats[Key[0]][Key[1]]=='#' && occupiedCounter >=4) line[Key[1]]='L';
                    duplicateSeats[Key[0]] = line.ToString();
                }
                Day11ListPrinter(duplicateSeats);
                iterationCounter += 1;
                if(IsEqual(freshSeats, duplicateSeats)) 
                {
                    Day11ListPrinter(freshSeats);
                    return iterationCounter;
                }
            }
        }
        public static bool IsEqual(List<string> originalSeats, List<string> duplicateSeats)
        {
            for(int i = 0; i < originalSeats.Count(); i ++)
            {
                if(originalSeats[i].SequenceEqual(duplicateSeats[i])==false) return false;
            }
            return true;
        }

        public static List<List<int>> GetIndexes(int xIndex, int yIndex, List<string> seats, bool part2 = false)
        {
            var worklist = new List<int>{0, 1, 0, -1, 1, 0, -1, 0, 1, 1, -1, 1, -1, -1, 1, -1}; 
            //worklist contains all the possible transformations of an index in y, x format. 
            var validIndexes = new List<List<int>>();
            for (int i = 0; i < worklist.Count(); i+=2)
            {
                var multiplier = 1; //Multiplier for part 2
                try
                {
                    if(seats[yIndex+worklist[i]][xIndex+worklist[i+1]]=='L' && seats[yIndex][xIndex]=='L') //Tries to check if this is even a valid index for the specified point in the seats. 
                    {
                        validIndexes.Add(new List<int>{yIndex+worklist[i], xIndex+worklist[i+1]}); //Adds all valid seats that are adjacent to the specified index
                    }
                    if(seats[yIndex+worklist[i]][xIndex+worklist[i+1]]=='.' && seats[yIndex][xIndex]=='L'&& part2) 
                    //If it is part 2, and the located seat is a floor, it increments a multiplier which multiplies the transformation until it finds a seat, or it throws an index out of range exception. 
                    {
                        while(seats[yIndex+(worklist[i]*multiplier)][xIndex+(worklist[i+1]*multiplier)]=='.') multiplier += 1;
                        //If a valid index is found, it is added to the 2D list, otherwise it throws an out of bounds exception which skips that transformation. 
                        validIndexes.Add(new List<int>{yIndex+(worklist[i]*multiplier), xIndex+(worklist[i+1]*multiplier)}); 
                    }
                }
                catch {} //Elegantly handles cases where the index to be checked is outside of the seats. 
            }
            return validIndexes;
        }
        public static int Day10Part1()
        {
            var readlines = Day10Helper();
            readlines.Add(0);
            readlines.Add(readlines[readlines.Count()-1]+3);
            var onejolt = 0;
            var threejolt = 0;
            for(int i = 0; i < readlines.Count()-1; i ++)
            {
                if(readlines[i+1]-readlines[i] == 1) onejolt += 1;
                if(readlines[i+1]-readlines[i] == 3) threejolt += 1;
            }
            return onejolt * threejolt;
        }
        public static List<int> Day10Helper()
        {
            var readlines = IntegerInputReader("input");
            var validJoltages = new List<int>{1, 2, 3};
            var JoltageChain = new List<int>();
            readlines.Sort();
            readlines.Add(readlines[readlines.Count()-1]+3);
            return readlines;
        }
        public static long Day10Part2Take2()
        {
            var readLines = Day10Helper();
            var ways = new Dictionary<long, long>();
            ways.Add(0, 1);
            foreach (var line in readLines)
            {
                ways[line] = 0;
                if(ways.ContainsKey(line - 1)) ways[line]+=ways[line-1];
                if(ways.ContainsKey(line - 2)) ways[line]+=ways[line-2];
                if(ways.ContainsKey(line - 3)) ways[line]+=ways[line-3];
            }      
            return ways[readLines.Max()];      
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
        static List<int> IntegerInputReader(string filename)
        {
            var myDocuments = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
				$"{filename}.txt");
			var readLines = File.ReadAllLines(myDocuments);
            var workList = new List<int>();
            foreach(var element in readLines) workList.Add(Convert.ToInt32(element));
            return workList; 
        }
    }
}