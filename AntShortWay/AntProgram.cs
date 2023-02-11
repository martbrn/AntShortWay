namespace AntShortWay
{
    public class AntProgram
    {
        static void Main(string[] args)
        {
            #region Data Structure
            Node NodeA = new Node() { Food = "A" };
            Node NodeB = new Node() { Food = "B" };
            Node NodeC = new Node() { Food = "C" };
            Node NodeD = new Node() { Food = "D" };

            //Food A
            NodeA.Ways.Add(new Way() { Node = NodeB, Distance = 6 });
            NodeA.Ways.Add(new Way() { Node = NodeC, Distance = 10 });
            NodeA.Ways.Add(new Way() { Node = NodeD, Distance = 2 });

            //Food B
            NodeB.Ways.Add(new Way() { Node = NodeA, Distance = 3 });
            NodeB.Ways.Add(new Way() { Node = NodeC, Distance = 1 });
            NodeB.Ways.Add(new Way() { Node = NodeD, Distance = 8 });

            //Food C
            NodeC.Ways.Add(new Way() { Node = NodeA, Distance = 2 });
            NodeC.Ways.Add(new Way() { Node = NodeB, Distance = 1 });
            NodeC.Ways.Add(new Way() { Node = NodeD, Distance = 9 });

            //Food D
            NodeD.Ways.Add(new Way() { Node = NodeA, Distance = 10 });
            NodeD.Ways.Add(new Way() { Node = NodeB, Distance = 2 });
            NodeD.Ways.Add(new Way() { Node = NodeC, Distance = 5 });
            #endregion

            List<Node> graph = new List<Node>() { NodeA, NodeB, NodeC, NodeD };

            // Play with the numberS of iterations (100 it) to look the short route e.g
            var algorithm = new Algorithm(graph, 100, NodeA);
            algorithm.Run();

            Console.WriteLine(algorithm.GetAllRoutes);
        }
    }

    public class Algorithm
    {
        private List<Node> _graph { get; set; }
        private int _n;
        private Node _origin { get; set; }
        private List<Route> _solutions { get; set; }
        public string GetAllRoutes
        {
            get
            {
                string result = "";
                foreach (var route in _solutions)
                {
                    foreach (var node in route.Nodes)
                    {
                        result += node.Food + ", ";
                    }
                    result += " " + route.TotalDistance + "\n";
                }
                return result;
            }
        }

        public Algorithm(List<Node> graph, int n, Node origin)
        {
            _graph = graph;
            _n = n;
            _origin = origin;
        }

        public void Run()
        {
            _solutions = new List<Route>();

            for (int i = 0; i < _n; i++)
            {
                _solutions.Add(Generate());
            }
            _solutions = _solutions.OrderBy(d => d.TotalDistance).ToList();
        }

        public Route Generate()
        {
            var solution = new Route();
            solution.Nodes.Add(_origin);
            Node current = _origin;

            for (int i = 0; i < _graph.Count - 1; i++)
            {
                Node next;
                do
                {
                    next = NextNode(current);
                } while (solution.Nodes.Contains(next));

                solution.Nodes.Add(next);
                solution.TotalDistance += current.Ways.Where(d => d.Node.Food == next.Food).First().Distance;
                current = next;
            }

            solution.Nodes.Add(_origin);
            solution.TotalDistance += current.Ways.Where(d => d.Node.Food == _origin.Food).First().Distance;
            return solution;
        }

        private Node NextNode(Node current)
        {
            int nextNode = new Random().Next(0, _graph.Count - 1);
            return current.Ways[nextNode].Node;
        }
    }

    // Food
    public class Node
    {
        public string Food { get; set; }
        public List<Way> Ways { get; set; }
        public Node() { Ways = new List<Way>(); }
    }

    // Way
    public class Way
    {
        public Node Node { get; set; }
        public int Distance { get; set; }
    }

    // Route
    public class Route
    {
        public List<Node> Nodes { get; set; }
        public int TotalDistance { get; set; }
        public Route() { Nodes = new List<Node>(); TotalDistance = 0; }
    }
}