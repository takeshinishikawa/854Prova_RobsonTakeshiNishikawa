using System;
using System.Text;
using System.Linq;

public class batalhaNaval
{
    public static void Main(string[] args)
    {
        string answer = "";
        bool isMultiplayer = false;
        singleOrMulti(ref answer, ref isMultiplayer);
        string firstPlayer = "";
        string secondPlayer = "";
        getNames(ref firstPlayer, ref secondPlayer, isMultiplayer);
        char[,] firstPlayerTable = new char[10, 10];
        char[,] secondPlayerTable = new char[10, 10];
        fillTables(ref firstPlayerTable, ref secondPlayerTable, firstPlayer, secondPlayer, isMultiplayer);
        getShots(firstPlayer, secondPlayer, firstPlayerTable, secondPlayerTable, isMultiplayer);
        Console.ReadLine();
    }
    public static void singleOrMulti(ref string answer, ref bool isMultiplayer)
    {
        while (true)
        {
            Console.WriteLine("O jogo será entre dois jogadores reais? Digite 'Sim' ou 'Nao':");
            answer = Console.ReadLine();
            string formattedAnswer = answer.ToUpper();
            if (formattedAnswer == "SIM" || formattedAnswer == "NAO")
            {
                if (formattedAnswer == "SIM")
                    isMultiplayer = true;
                break;
            }
            else
                Console.WriteLine($"\"{answer}\" não é uma resposta válida");
        }
    }
    public static void getNames(ref string firstPlayer, ref string secondPlayer, bool isMultiplayer)
    {
        if (isMultiplayer)
        {
            while (firstPlayer == "")
            {
                Console.WriteLine("Qual o nome do primeiro jogador?");
                firstPlayer = Console.ReadLine().ToUpper();
            }
            while (secondPlayer == "")
            {
                Console.WriteLine("Qual o nome do segundo jogador?");
                secondPlayer = Console.ReadLine().ToUpper();
            }
        }
        else
        {
            firstPlayer = "HUMANO";
            secondPlayer = "COMPUTADOR";
        }
    }
    public static void fillVertical(ref Dictionary<char, int> vertical)
    {
        vertical.Add('A', 0);
        vertical.Add('B', 1);
        vertical.Add('C', 2);
        vertical.Add('D', 3);
        vertical.Add('E', 4);
        vertical.Add('F', 5);
        vertical.Add('G', 6);
        vertical.Add('H', 7);
        vertical.Add('I', 8);
        vertical.Add('J', 9);
    }
    public static void fillTypes(ref Dictionary<string, int> qttType)
    {
        qttType.Add("PS", 1);
        qttType.Add("NT", 2);
        qttType.Add("DS", 3);
        qttType.Add("SB", 4);
    }
    public static void fillLenght(ref Dictionary<string, int> lenghtType)
    {
        lenghtType.Add("PS", 5);
        lenghtType.Add("NT", 4);
        lenghtType.Add("DS", 3);
        lenghtType.Add("SB", 2);
    }
    public static void fillTables(ref char[,] firstPlayerTable, ref char[,] secondPlayerTable, string firstPlayer, string secondPlayer, bool isMultiplayer)
    {
        bool firstPlayerPositioned = false;
        bool secondPlayerPositioned = false;
        while (!firstPlayerPositioned || !secondPlayerPositioned)
        {
            var vertical = new Dictionary<char, int>();
            var qttType = new Dictionary<string, int>();
            var lenghtType = new Dictionary<string, int>();
            fillVertical(ref vertical);
            fillTypes(ref qttType);
            fillLenght(ref lenghtType);
            decimal sum = qttType.Values.Sum();
            while (sum > 0)
            {
                Console.WriteLine("========================================================================");
                if (!firstPlayerPositioned)
                    Console.WriteLine($"{firstPlayer}, agora você irá posicionar suas embarcações no mapa.");
                else
                    Console.WriteLine($"{secondPlayer}, agora você irá posicionar suas embarcações no mapa. ");

                Console.WriteLine("Qual o tipo de embarcação?");
                string ship = "";
                //Loop to where the computer's ships are included
                if (!isMultiplayer && firstPlayerPositioned)
                {
                    foreach (var item in qttType)
                    {
                        if (item.Value != 0)
                        {
                            ship = item.Key;
                            Console.WriteLine(ship);
                            break;
                        }
                    }
                }
                else
                    ship = Console.ReadLine().ToUpper();
                if (qttType.ContainsKey(ship) && qttType[ship] != 0)
                {
                GetCoordinates:
                    int lineInit = 0;
                    int colInit = 0;
                    int lineEnd = 0;
                    int colEnd = 0;
                    if (!isMultiplayer && firstPlayerPositioned)
                        getComputerCoordinates(ref lineInit, ref colInit, ref lineEnd, ref colEnd, vertical, ship, lenghtType, secondPlayerTable);
                    else
                        getCoordinates(ref lineInit, ref colInit, ref lineEnd, ref colEnd, vertical);
                    if (!firstPlayerPositioned && isFree(lineInit, colInit, lineEnd, colEnd, firstPlayerTable, "") & checkLenght(lineInit, colInit, lineEnd, colEnd, ship, lenghtType))
                        includeShip(lineInit, colInit, lineEnd, colEnd, ref firstPlayerTable);
                    else if (firstPlayerPositioned && isFree(lineInit, colInit, lineEnd, colEnd, secondPlayerTable, secondPlayer) & checkLenght(lineInit, colInit, lineEnd, colEnd, ship, lenghtType))
                        includeShip(lineInit, colInit, lineEnd, colEnd, ref secondPlayerTable);
                    else
                    {
                        Console.WriteLine("Tente novamente.");
                        goto GetCoordinates;
                    }
                    qttType[ship] -= 1;
                    Console.Write("Restam ");
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.Write($"{qttType[ship]}", Console.BackgroundColor);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" embarcação(ões) do tipo ");
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.Write($"{ship}", Console.BackgroundColor);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine(" para você adicionar.");
                    sum--;
                }
                else if (qttType.ContainsKey(ship) && qttType[ship] == 0)
                {
                    Console.Write($"Você já adicionou todos as suas embarcações do tipo ");
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.Write($"{ship}", Console.BackgroundColor);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine(".");
                }
                else
                {
                    Console.WriteLine("Tipo inválido. O jogo possui os tipos:");
                    Console.WriteLine("PS - Porta-Aviões (5 quadrantes)");
                    Console.WriteLine("NT - Navio-Tanque (4 quadrantes)");
                    Console.WriteLine("DS - Destroyers (3 quadrantes)");
                    Console.WriteLine("SB - Submarinos (2 quadrantes)");
                }
            }
            if (!firstPlayerPositioned)
                firstPlayerPositioned = true;
            else if (!secondPlayerPositioned)
                secondPlayerPositioned = true;
        }
    }
    public static void getCoordinate(ref int line, ref int col, Dictionary<char, int> vertical, string firstPlayer, string secondPlayer, bool firstPlayerTurn, bool isMultiplayer, char[,] firstPlayerShots, char[,] secondPlayerShots)
    {
    AskForCoordinate:
        Console.ReadKey();
        Console.Clear();
        string shot = "";
        if (firstPlayerTurn)
        {
            Console.WriteLine($"Veja abaixo os seus tiros, {firstPlayer}.");
            printTable(firstPlayerShots);
            Console.WriteLine($"{firstPlayer}, onde deseja atirar?");
            shot = Console.ReadLine().ToUpper();
        }
        else if (!firstPlayerTurn && isMultiplayer)
        {
            Console.WriteLine($"Veja abaixo os seus tiros, {secondPlayer}.");
            printTable(secondPlayerShots);
            Console.WriteLine($"{secondPlayer}, onde deseja atirar?");
            shot = Console.ReadLine().ToUpper();
        }
        else
        {
            Console.WriteLine($"Veja abaixo os seus tiros, {secondPlayer}.");
            printTable(secondPlayerShots);
            Console.WriteLine($"{secondPlayer}, onde deseja atirar?");
        ComputerShot:
            shot = generateCoordinate();
            int shotLine = 0;
            int shotCol = 0;
            int needle = 0;
            shotLine = vertical[shot[needle]];
            needle++;

            while (needle < shot.Length && shot[needle] >= '0' && shot[needle] <= '9')
            {
                shotCol *= 10;
                shotCol += (int)Char.GetNumericValue(shot[needle]);
                needle++;
            }
            shotCol--;
            if (secondPlayerShots[shotLine, shotCol] != '\0')
                goto ComputerShot;
            Console.WriteLine(shot);
        }
        if (shot.Length < 2)
        {
            Console.Write("Coordenada inválida. ");
            goto AskForCoordinate;
        }
        int i = 0;
        if (shot[i] >= 'A' && shot[i] <= 'J')
        {
            line = vertical[shot[i]];
            i++;
        }
        else
        {
            Console.Write("Valor de linha inválido. São aceitas somente linhas de A à J. ");
            goto AskForCoordinate;
        }
        while (i < shot.Length && shot[i] >= '0' && shot[i] <= '9')
        {
            col *= 10;
            col += (int)Char.GetNumericValue(shot[i]);
            i++;
        }
        col--;
        if (col >= 10 || col < 0)
        {
            Console.Write("Valor de coluna inválido. São aceitas somente linhas de 1 à 10. ");
            col = 0;
            goto AskForCoordinate;
        }
    }
    public static void getComputerCoordinates(ref int lineInit, ref int colInit, ref int lineEnd, ref int colEnd, Dictionary<char, int> vertical, string ship, Dictionary<string, int> lenghtType, char[,] secondPlayerTable)
    {
        Console.WriteLine("Qual a sua posição?");
    AskComputerPosition:
        lineInit = 0;
        colInit = 0;
        string start = generateCoordinate();
        int i = 0;
        lineInit = vertical[start[i]];
        i++;
        while (i < start.Length && start[i] >= '0' && start[i] <= '9')
        {
            colInit *= 10;
            colInit += (int)Char.GetNumericValue(start[i]);
            i++;
        }
        colInit--;
    RollTheDices:
        Random random = new Random();
        const string direction = "01"; //if '0' is true and '1' is false
        bool isHorizontal = false;
        bool addToCoordinate = false;
        if (direction[random.Next(direction.Length)].ToString() == "0")
            isHorizontal = true;
        if (direction[random.Next(direction.Length)].ToString() == "0")
            addToCoordinate = true;
        if (isHorizontal && addToCoordinate)
        {
            lineEnd = lineInit;
            colEnd = colInit + lenghtType[ship] - 1;
        }
        else if (!isHorizontal && addToCoordinate)
        {
            lineEnd = lineInit + lenghtType[ship] - 1;
            colEnd = colInit;
        }
        else if (isHorizontal && !addToCoordinate)
        {
            lineEnd = lineInit;
            colEnd = colInit - lenghtType[ship] + 1;
        }
        else if (!isHorizontal && !addToCoordinate)
        {
            lineEnd = lineInit - lenghtType[ship] + 1;
            colEnd = colInit;
        }
        if (lineEnd < 0 || lineEnd > 9 || colEnd < 0 || colEnd > 9)
            goto RollTheDices;
        if (!isFree(lineInit, colInit, lineEnd, colEnd, secondPlayerTable, "COMPUTER") || !checkLenght(lineInit, colInit, lineEnd, colEnd, ship, lenghtType))
            goto AskComputerPosition;
        StringBuilder result = new StringBuilder();
        int initialLine = lineInit;
        int initialCol = colInit + 1;
        int endLine = lineEnd;
        int endCol = colEnd + 1;
        result.AppendFormat("{0}{1}{2}{3}", vertical.FirstOrDefault(x => x.Value == initialLine).Key, initialCol, vertical.FirstOrDefault(y => y.Value == endLine).Key, endCol);
        Console.WriteLine(result);
    }
    public static string generateCoordinate()
    {
        const string chars = "ABCDEFGHIJ";
        const string numbers = "0123456789";
        StringBuilder result = new StringBuilder();
        string line;
        string col;
        Random random = new Random();
        line = chars[random.Next(chars.Length)].ToString();
        col = numbers[random.Next(numbers.Length)].ToString();
        result.AppendFormat("{0}{1}", line, Int32.Parse(col) + 1);
        return result.ToString();
    }
    public static void getCoordinates(ref int lineInit, ref int colInit, ref int lineEnd, ref int colEnd, Dictionary<char, int> vertical)
    {
    AskPosition:
        colInit = 0;
        colEnd = 0;
        Console.WriteLine("Qual a sua posição?");
        var position = Console.ReadLine().ToUpper();
        int i = 0;
        if (!checkMinLen(position))
            goto AskPosition;
        if (position[i] >= 'A' && position[i] <= 'J')
        {
            lineInit = vertical[position[i]];
            i++;
        }
        else
        {
            Console.WriteLine("Valor inválido. São aceitas somente linhas de A a J. ");
            goto AskPosition;
        }
        while (position[i] >= '0' && position[i] <= '9')
        {
            colInit *= 10;
            colInit += (int)Char.GetNumericValue(position[i]);
            i++;
        }
        colInit--;
        if (position[i] >= 'A' && position[i] <= 'J')
        {
            lineEnd = vertical[position[i]];
            i++;
        }
        else
        {
            Console.WriteLine("Valor inválido. São aceitas somente linhas de A a J. ");
            goto AskPosition;
        }
        while (i < position.Length && position[i] >= '0' && position[i] <= '9')
        {
            colEnd *= 10;
            colEnd += (int)Char.GetNumericValue(position[i]);
            i++;
        }
        colEnd--;
        if (lineInit >= 0 && lineInit <= 9 && colInit >= 0 && colEnd <= 9)
            return;
        else
        {
            Console.Write("Valor de coluna inválido. São aceitas somente linhas de 1 à 10. ");
            goto AskPosition;
        }
    }
    public static bool isFree(int lineInit, int colInit, int lineEnd, int colEnd, char[,] PlayerTable, string secPlayer)
    {
        int minLine;
        int maxLine;
        int minCol;
        int maxCol;

        if (lineInit <= lineEnd)
        {
            minLine = lineInit;
            maxLine = lineEnd;
        }
        else
        {
            minLine = lineEnd;
            maxLine = lineInit;
        }
        if (colInit <= colEnd)
        {
            minCol = colInit;
            maxCol = colEnd;
        }
        else
        {
            minCol = colEnd;
            maxCol = colInit;
        }
        //Check if it is NOT diagonal
        if (minLine != maxLine && minCol != maxCol)
            return false;
        if (minLine == maxLine)
        {
            while (minCol <= maxCol)
            {
                if (PlayerTable[minLine, minCol] != '\0')
                {
                    if (secPlayer != "COMPUTER")
                        Console.WriteLine("Uma embarcação não pode sobrepor a outra. ");
                    return false;
                }
                minCol++;
            }
            return true;
        }
        else
        {
            while (minLine <= maxLine)
            {
                if (PlayerTable[minLine, minCol] != '\0')
                {
                    if (secPlayer != "COMPUTER")
                        Console.WriteLine("Uma embarcação não pode sobrepor a outra. ");
                    return false;
                }
                minLine++;
            }
            return true;
        }
    }
    public static void includeShip(int lineInit, int colInit, int lineEnd, int colEnd, ref char[,] PlayerTable)
    {
        int minLine;
        int maxLine;
        int minCol;
        int maxCol;

        if (lineInit <= lineEnd)
        {
            minLine = lineInit;
            maxLine = lineEnd;
        }
        else
        {
            minLine = lineEnd;
            maxLine = lineInit;
        }
        if (colInit <= colEnd)
        {
            minCol = colInit;
            maxCol = colEnd;
        }
        else
        {
            minCol = colEnd;
            maxCol = colInit;
        }
        if (minLine == maxLine)
        {
            while (minCol <= maxCol)
            {
                PlayerTable[minLine, minCol] = 'S';
                minCol++;
            }
            return;
        }
        else
        {
            while (minLine <= maxLine)
            {
                PlayerTable[minLine, minCol] = 'S';
                minLine++;
            }
            return;
        }
    }
    public static void printTable(char[,] table)
    {
        int line = 0;
        string lines = "ABCDEFGHIJ";
        Console.WriteLine("_____________________");
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.Write("|1|2|3|4|5|6|7|8|9|10|", Console.BackgroundColor);
        Console.BackgroundColor = ConsoleColor.Black;
        Console.WriteLine();
        Console.WriteLine("-------------------------------");
        while (line < 10)
        {
            int col = 0;
            Console.Write("|");
            while (col < 10)
            {
                if (table[line, col] != '\0')
                {
                    if (table[line, col] == 'A')
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.Write($"{table[line, col]}", Console.BackgroundColor);
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else if (table[line, col] == 'X')
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.Write($"{table[line, col]}", Console.BackgroundColor);
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                        Console.Write($"{table[line, col]}");
                }
                else
                    Console.Write(' ');
                Console.Write("|");
                col++;
            }
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write($"Linha - {lines[line]}", Console.BackgroundColor);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("|");
            Console.WriteLine("-------------------------------");
            line++;
        }
    }
    public static bool checkLenght(int lineInit, int colInit, int lineEnd, int colEnd, string ship, Dictionary<string, int> lenghtType)
    {
        int lenght = lenghtType[ship];
        int diff = 0;
        if (lineInit == lineEnd && colInit > colEnd)
            diff = (colInit - colEnd) + 1;
        else if (lineInit == lineEnd && colInit < colEnd)
            diff = (colInit - colEnd) - 1;
        else if (lineInit > lineEnd && colInit == colEnd)
            diff = (lineInit - lineEnd) + 1;
        else if (lineInit < lineEnd && colInit == colEnd)
            diff = (lineInit - lineEnd) - 1;
        else
        {
            Console.WriteLine("O intervalo indicado não corresponde ao tamanho da embarcação.");
            return false;
        }
        if (diff < 0)
            diff *= -1;
        if (diff == lenght)
            return true;
        else
        {
            Console.WriteLine("O intervalo indicado não corresponde ao tamanho da embarcação.");
            return false;
        }
    }
    public static bool checkMinLen(string position)
    {
        if (position.Length < 4)
        {
            Console.Write("Valor inválido. ");
            return false;
        }
        return true;
    }
    public static bool notShot(int line, int col, char[,] PlayerShots)
    {
        if (PlayerShots[line, col] == '\0')
            return true;
        Console.Write("Você já atirou neste local. ");
        return false;
    }
    public static void checkEnemyTable(int line, int col, ref char[,] shooterTable, char[,] enemytable)
    {
        if (enemytable[line, col] != '\0')
        {
            shooterTable[line, col] = 'X';
            Console.WriteLine("Parabéns, você acertou uma embarcação!");
        }
        else
        {
            shooterTable[line, col] = 'A';
            Console.WriteLine("Você errou o tiro...");
        }
    }
    public static int countHits(char[,] PlayerShots, char letter)
    {
        int counter = 0;
        foreach (var item in PlayerShots)
        {
            if (item == letter)
                counter++;
        }
        return counter;
    }
    public static void getShots(string firstPlayer, string secondPlayer, char[,] firstPlayerTable, char[,] secondPlayerTable, bool isMultiplayer)
    {
        char[,] firstPlayerShots = new char[10, 10];
        char[,] secondPlayerShots = new char[10, 10];
        bool firstPlayerTurn = true;
        var vertical = new Dictionary<char, int>();
        fillVertical(ref vertical);
        int totalTargets1 = countHits(secondPlayerTable, 'S');
        int totalTargets2 = countHits(firstPlayerTable, 'S');
        Console.WriteLine("========================================================================");
        Console.WriteLine("Escolha seus alvos e que vença o melhor!!!");
        while (true)
        {
        GetCoordinate:
            int line = 0;
            int col = 0;

            getCoordinate(ref line, ref col, vertical, firstPlayer, secondPlayer, firstPlayerTurn, isMultiplayer, firstPlayerShots, secondPlayerShots);
            if (firstPlayerTurn && notShot(line, col, firstPlayerShots))
            {
                checkEnemyTable(line, col, ref firstPlayerShots, secondPlayerTable);
                firstPlayerTurn = false;
            }
            else if (!firstPlayerTurn && notShot(line, col, secondPlayerShots))
            {
                checkEnemyTable(line, col, ref secondPlayerShots, firstPlayerTable);
                firstPlayerTurn = true;
            }
            else
            {
                Console.WriteLine("Coordenada inválida.");
                goto GetCoordinate;
            }
            if (firstPlayerTurn)
            {
                int hitTheShot1 = countHits(firstPlayerShots, 'X');
                int hitTheShot2 = countHits(secondPlayerShots, 'X');
                if (totalTargets1 == hitTheShot1 && totalTargets2 == hitTheShot2)
                {
                    Console.WriteLine("===================================================================");
                    Console.WriteLine("Houve um empate!");
                    break;
                }
                else if (totalTargets1 == hitTheShot1)
                {
                    Console.WriteLine("===================================================================");
                    Console.WriteLine($"Parabéns, {firstPlayer} você venceu!");
                    break;
                }
                else if (totalTargets2 == hitTheShot2)
                {
                    Console.WriteLine("===================================================================");
                    Console.WriteLine($"Parabéns, {secondPlayer} você venceu!");
                    break;
                }
            }
        }
    }
}
