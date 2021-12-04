var boardsWon = new List<List<List<int?>>>();
var input = File.ReadLines("4.txt");

var draws = input.First().Split(',').Select(int.Parse).ToList();

var boards = input.Skip(1).Chunk(6)
    .Select(lines => lines.Skip(1).Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).Cast<int?>().ToList()).ToList())
    .ToList();

var (winningNumber, winningBoard) = Play(draws, boards);

var sum = 0;
for (int y = 0; y < winningBoard.Count; y++)
{
    for (int x = 0; x < winningBoard[y].Count; x++)
    {
        sum += winningBoard[y][x] ?? 0;
    }
}

Console.WriteLine(winningNumber);
Console.WriteLine(sum);
System.Console.WriteLine(sum * winningNumber);

(int, List<List<int?>>) Play(List<int> draws, List<List<List<int?>>> boards)
{
    foreach (var number in draws)
    {
        EliminateNumber(number, boards);
        var wonBoard = BoardWon();
        if (wonBoard != null)
            return (number, wonBoard);
    }
    throw new ArgumentException("Could not complete game");
}

void EliminateNumber(int number, List<List<List<int?>>> boards)
{
    foreach (var board in boards)
    {
        for (int y = 0; y < board.Count; y++)
        {
            for (int x = 0; x < board[y].Count; x++)
            {
                if (board[y][x] == number)
                    board[y][x] = null;
            }
        }
    }
}

List<List<int?>>? BoardWon()
{
    foreach (var board in boards.Except(boardsWon))
    {
        var won = false;
        for (int y = 0; y < board.Count; y++)
        {
            won |= board[y].All(x => x == null);
        }
        for (int x = 0; x < board[0].Count; x++)
        {
            won |= board.All(b => b[x] == null);
        }

        if (won)
            boardsWon.Add(board);

        if (boardsWon.Count == boards.Count)
            return board;
    }

    return null;
}