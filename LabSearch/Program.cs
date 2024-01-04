using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Threading;

namespace LabSearch
{
    class PathNode
    {
        private readonly char[,] _maze;

        public Point Position { get; }

        public PathNode? Parent { get; }

        public PathNode(char[,] maze, Point position, PathNode parent)
        {
            _maze = maze;
            Position = position;
            Parent = parent;
        }

        public List<PathNode> GetNextNodes()
        {
            var result = new List<PathNode>(4);
            if (Position.X + 1 < _maze.GetLength(0))
            {
                result.Add(new PathNode(_maze, new Point(Position.X + 1, Position.Y), this));
            }
            if (Position.Y + 1 < _maze.GetLength(1))
            {
                result.Add(new PathNode(_maze, new Point(Position.X, Position.Y + 1), this));
            }
            if (Position.X - 1 >= 0)
            {
                result.Add(new PathNode(_maze, new Point(Position.X - 1, Position.Y), this));
            }
            if (Position.Y - 1 >= 0)
            {
                result.Add(new PathNode(_maze, new Point(Position.X, Position.Y - 1), this));
            }

            return result;
        }
    }

    enum PathWay
    {
        Right,
        Bottom,
        Left,
        Top,
        End
    }

    class PathItem
    {
        private readonly char[,] _field;
        private PathWay _currentWay = PathWay.Right;

        public Point Position { get; }
        public PathWay PrevPathWay => _currentWay switch
        {
            PathWay.Right => _currentWay,
            PathWay.Bottom => PathWay.Right,
            PathWay.Left => PathWay.Bottom,
            PathWay.Top => PathWay.Left,
            PathWay.End => PathWay.Top,
            _ => _currentWay
        };

        public PathItem(char[,] field, Point pos)
        {
            this._field = field;
            Position = pos;
        }

        public bool CanGetNextPoint() => _currentWay != PathWay.End;

        public PathItem[] GetNextPoints()
        {
            List<PathItem> items = new List<PathItem>();
            while (CanGetNextPoint())
                items.Add(GetNextPoint());
            return items.ToArray();
        }

        public PathItem GetNextPoint()
        {
            while (CanGetNextPoint())
            {
                switch (_currentWay)
                {
                    case PathWay.Right:
                        _currentWay = PathWay.Bottom;
                        if (Position.X + 1 < _field.GetLength(0))
                        {
                            return new PathItem(_field, new Point(Position.X + 1, Position.Y));
                        }
                        continue;
                    case PathWay.Bottom:
                        _currentWay = PathWay.Left;
                        if (Position.Y + 1 < _field.GetLength(1)) // bottom
                        {
                            return new PathItem(_field, new Point(Position.X, Position.Y + 1));
                        }
                        continue;
                    case PathWay.Left:
                        _currentWay = PathWay.Top;
                        if (Position.X - 1 >= 0) // left
                        {
                            return new PathItem(_field, new Point(Position.X - 1, Position.Y));
                        }
                        continue;
                    case PathWay.Top:
                        _currentWay = PathWay.End;
                        if (Position.Y - 1 >= 0) // top
                        {
                            return new PathItem(_field, new Point(Position.X, Position.Y - 1));
                        }
                        continue;
                    default:
                        return this;
                }
            }

            return this;
        }

        public override bool Equals(object? obj)
        {
            var other = obj as PathItem;
            return other.Position == this.Position;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            int n, m;
            n = 10; m = 10;
            float ratio = 0.1f;

            const char barier = 'X';
            const char free = ' ';
            Point start = new Point(0, 0);
            Point end = new Point(n - 1, m - 1);

            char[,] field = new char[n, m];
            Generate(field, barier, free, ratio, start, end);

            var result = FindPath_Dfs(field, barier, free, start, end);
            Display(field, result);
        }

        private static Point[] FindPath_Bfs(char[,] field, char barier, char free, Point start, Point end)
        {
            static Point[] NodeToPath(PathNode exitNode)
            {
                List<Point> result = new List<Point>();
                while (exitNode != null)
                {
                    result.Add(exitNode.Position);
                    exitNode = exitNode.Parent!;
                }
                result.Reverse();
                return result.ToArray();
            }

            var forCheck = new Queue<PathNode>();
            forCheck.Enqueue(new PathNode(field, start, null!));

            List<PathNode> explored = new List<PathNode>();

            while (forCheck.Count > 0)
            {
                var currentNode = forCheck.Dequeue();
                if (currentNode.Position == end) return NodeToPath(currentNode);

                explored.Add(currentNode);
                foreach (var possibleNode in currentNode.GetNextNodes())
                {
                    if (field[possibleNode.Position.X, possibleNode.Position.Y] == barier) continue;
                    if (explored.Contains(possibleNode)) continue;

                    forCheck.Enqueue(possibleNode);
                }
            }

            return Array.Empty<Point>();
        }

        private static Point[] FindPath_Dfs(char[,] field, char barier, char free, Point start, Point end)
        {
            var path = new Stack<PathItem>();
            path.Push(new PathItem(field, start));

            while (path.Count > 0)
            {
                var currentItem = path.Peek();
                DisplayPoint(field, start, end, barier, path.ToArray(), '*');
                if (currentItem.Position == end) break;

                PathItem? nextItem = null;
                while (currentItem.CanGetNextPoint())
                {
                    nextItem = currentItem.GetNextPoint();
                    if (path.Contains(nextItem)) continue;
                    else break;
                }

                if (nextItem == null || nextItem == currentItem)
                {
                    path.Pop();
                }
                else if (field[nextItem.Position.X, nextItem.Position.Y] == free)
                {
                    path.Push(nextItem);
                }
            }

            return path.Select(p => p.Position).ToArray();
        }

        static void Generate(char[,] filed, char barier, char free, float rate, params Point[] exceptions)
        {
            var random = new Random();
            for (int i = 0; i < filed.GetLength(0); i++)
                for (int j = 0; j < filed.GetLength(1); j++)
                {
                    if (exceptions.Contains(new Point(i, j)))
                    {
                        filed[i, j] = free;
                    }
                    else
                    {
                        if (random.NextSingle() - rate > 0)
                            filed[i, j] = free;
                        else
                            filed[i, j] = barier;
                    }
                }
        }

        private static void DisplayPoint(char[,] field, Point start, Point end, char barier, PathItem[] prevPath, char v)
        {
            Console.ReadKey();
            //Thread.Sleep(10);
            Console.Clear();

            ConsoleColor defBgColor = ConsoleColor.DarkGray;
            Console.BackgroundColor = defBgColor;

            for (int j = 0; j < field.GetLength(1); j++)
            {
                for (int i = 0; i < field.GetLength(0); i++)
                {
                    if (start.X == i && start.Y == j) Console.BackgroundColor = ConsoleColor.DarkGreen;
                    if (end.X == i && end.Y == j) Console.BackgroundColor = ConsoleColor.Magenta;
                    if (field[i, j] == barier) Console.BackgroundColor = ConsoleColor.Red;

                    var currentPoint = new Point(i, j);
                    PathItem? currentPath = null;
                    if ((currentPath = Array.Find(prevPath, p => p.Position == currentPoint)) != null)
                    {
                        v = currentPath.PrevPathWay switch
                        {
                            PathWay.Left => '←',
                            PathWay.Right => '→',
                            PathWay.Top => '↑',
                            PathWay.Bottom => '↓',
                            PathWay.End => '0',
                            _ => v
                        };
                        Console.Write(v);
                    }
                    else
                        Console.Write(field[i, j]);

                    Console.BackgroundColor = defBgColor;
                }
                Console.WriteLine();
            }

            Console.ResetColor();
        }

        static void Display(char[,] field, Point[] path)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                for (int i = 0; i < field.GetLength(0); i++)
                    if (path.Contains(new Point(i, j)))
                        Console.Write('P');
                    else
                        Console.Write(field[i, j]);
                Console.WriteLine();
            }
        }
    }
}