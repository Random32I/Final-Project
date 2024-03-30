using System.Xml.Linq;

public class EVs
{
    static List<Node> nodes = new List<Node>();

    static bool initialized;

    static char StartNodeName = 'H';

    static int[] shortestDistances = new int[23];

    static void Main(string[] args)
    {
        if (!initialized)
        {
            InitializeNodes();
            initialized = true;
        }

        for (int i = 0; i < 23; i++)
        {
            if (StartNodeName == nodes[i].GetName())
            {
                shortestDistances[i] = 0;
            }

            else
            {
                shortestDistances[i] = int.MaxValue - 1;
                nodes[i].SetDistance(int.MaxValue - 1);
            }
        }

        Console.WriteLine($"With a distance of {DistanceToChargingStation()}");
    }

    static int DistanceToChargingStation()
    {
        Node currentNode;
        //Getting the distances of each node
        for (int i = 0; i < 23; i++)    
        {
            currentNode = nodes[IndexOfShortestDistance()];

            for (int j = 0; j < currentNode.GetConnections().Count; j++)
            {
                if (!GetNode(currentNode.GetConnections()[j]).GetVisited())
                {
                    int newDistance = currentNode.GetDistance() + currentNode.GetConnectionWeights()[j];

                    if (newDistance < GetNode(currentNode.GetConnections()[j]).GetDistance())
                    {
                        GetNode(currentNode.GetConnections()[j]).SetDistance(newDistance);
                        shortestDistances[currentNode.GetConnections()[j] - 65] = newDistance;
                    }
                }
            }
            currentNode.SetVisited();
        }

        //Removing unoptimal routes
        for (int i = 0; i < 23; i++)
        {
            for (int j = 0; j < nodes[i].GetConnections().Count; j++)
            {
                if (GetNode(nodes[i].GetConnections()[j]).GetDistance() > nodes[i].GetConnectionWeights()[j] + nodes[i].GetDistance())
                {
                    nodes[i].RemoveNode(nodes[i].GetConnections()[j]);
                }
            }
        }

        //Finding the closest Charging station

        int[] chargingStations = new int[4]
    {
            shortestDistances[7],
            shortestDistances[10],
            shortestDistances[16],
            shortestDistances[19]
    };

        int lowestValue = int.MaxValue;

        //set to node H as a default, will never be not assigned though
        Node closestCharger = GetNode('H');

        for (int i = 0; i < 4; i++)
        {
            if (chargingStations[i] < lowestValue)
            {
                lowestValue = chargingStations[i];

                switch (i)
                {
                    case 0:
                        closestCharger = GetNode('H');
                        break;
                    case 1:
                        closestCharger = GetNode('K');
                        break;
                    case 2:
                        closestCharger = GetNode('Q');
                        break;
                    case 3:
                        closestCharger = GetNode('T');
                        break;
                }
            }
        }

        //Recommending Path to Nearest Charging station
        currentNode = closestCharger;

        List<char> route = new List<char>();

        while (currentNode.GetName() != StartNodeName)
        {
            route.Add(currentNode.GetName());
            currentNode = FindCloserNode(currentNode);
        }
        //Adds the Root Node
        route.Add(currentNode.GetName());

        Console.Write("The shortest route will be: ");
        for (int i = route.Count - 1; i >= 0; i--)
        {
            if (i != 0)
            {
                Console.Write($"{route[i]}, ");
            }
            else
            {
                //Write the last element without a comma
                Console.Write($"{route[i]} \n");
            }
        }

        return lowestValue;
    }

    static Node FindCloserNode(Node currentNode)
    {
        Node closestNode = currentNode;

        for(int i = 0; i < currentNode.GetConnections().Count; i++)
        {
            if (GetNode(currentNode.GetConnections()[i]).GetDistance() < closestNode.GetDistance())
            {
                closestNode = GetNode(currentNode.GetConnections()[i]);
            }
        }

        return closestNode;
    }

    static int IndexOfShortestDistance()
    {
        int smallestDistance = int.MaxValue;
        int index = 0;
        for (int i = 0; i < 23; i++)
        {
            if (shortestDistances[i] < smallestDistance && !nodes[i].GetVisited())
            {
                smallestDistance = shortestDistances[i];
                index = i;
            }
        }

        return index;
    }

    static void InitializeNodes()
    {
        for (int i = 0; i < 23; i++)
        {
            Node currentNode = new Node();

            //65 is the ASCII code for A
            currentNode.SetName((char)(65 + i));

            //designating charging stations
            switch (currentNode.GetName())
            {
                case 'H':
                case 'K':
                case 'Q':
                case 'T':
                    currentNode.SetChargingStation();
                    break;
            }

            //initializing connections
            switch (currentNode.GetName())
            {
                case 'A':
                    currentNode.AddNode('B', 6);
                    currentNode.AddNode('F', 5);
                    break;
                case 'B':
                    currentNode.AddNode('A', 6);
                    currentNode.AddNode('C', 5);
                    currentNode.AddNode('G', 6);
                    break;
                case 'C':
                    currentNode.AddNode('B', 5);
                    currentNode.AddNode('D', 7);
                    currentNode.AddNode('H', 5);
                    break;
                case 'D':
                    currentNode.AddNode('C', 7);
                    currentNode.AddNode('E', 7);
                    currentNode.AddNode('I', 8);
                    break;
                case 'E':
                    currentNode.AddNode('D', 7);
                    currentNode.AddNode('I', 6);
                    currentNode.AddNode('N', 15);
                    break;
                case 'F':
                    currentNode.AddNode('A', 5);
                    currentNode.AddNode('G', 8);
                    currentNode.AddNode('J', 7);
                    break;
                case 'G':
                    currentNode.AddNode('B', 6);
                    currentNode.AddNode('F', 8);
                    currentNode.AddNode('H', 9);
                    currentNode.AddNode('K', 8);
                    break;
                case 'H':
                    currentNode.AddNode('C', 5);
                    currentNode.AddNode('G', 9);
                    currentNode.AddNode('I', 12);
                    break;
                case 'I':
                    currentNode.AddNode('D', 8);
                    currentNode.AddNode('E', 6);
                    currentNode.AddNode('H', 12);
                    currentNode.AddNode('M', 10);
                    break;
                case 'J':
                    currentNode.AddNode('F', 7);
                    currentNode.AddNode('K', 5);
                    currentNode.AddNode('O', 7);
                    break;
                case 'K':
                    currentNode.AddNode('G', 8);
                    currentNode.AddNode('J', 5);
                    currentNode.AddNode('L', 7);
                    break;
                case 'L':
                    currentNode.AddNode('K', 7);
                    currentNode.AddNode('M', 7);
                    currentNode.AddNode('P', 7);
                    break;
                case 'M':
                    currentNode.AddNode('I', 10);
                    currentNode.AddNode('L', 7);
                    currentNode.AddNode('N', 9);
                    break;
                case 'N':
                    currentNode.AddNode('E', 15);
                    currentNode.AddNode('M', 9);
                    currentNode.AddNode('R', 7);
                    break;
                case 'O':
                    currentNode.AddNode('J', 7);
                    currentNode.AddNode('P', 13);
                    currentNode.AddNode('S', 9);
                    break;
                case 'P':
                    currentNode.AddNode('L', 7);
                    currentNode.AddNode('O', 13);
                    currentNode.AddNode('Q', 8);
                    currentNode.AddNode('U', 11);
                    break;
                case 'Q':
                    currentNode.AddNode('P', 8);
                    currentNode.AddNode('R', 9);
                    break;
                case 'R':
                    currentNode.AddNode('N', 7);
                    currentNode.AddNode('Q', 9);
                    currentNode.AddNode('W', 10);
                    break;
                case 'S':
                    currentNode.AddNode('O', 9);
                    currentNode.AddNode('T', 9);
                    break;
                case 'T':
                    currentNode.AddNode('S', 9);
                    currentNode.AddNode('U', 8);
                    break;
                case 'U':
                    currentNode.AddNode('P', 11);
                    currentNode.AddNode('T', 8);
                    currentNode.AddNode('V', 8);
                    break;
                case 'V':
                    currentNode.AddNode('U', 8);
                    currentNode.AddNode('W', 5);
                    break;
                case 'W':
                    currentNode.AddNode('R', 10);
                    currentNode.AddNode('V', 5);
                    break;
            }

            nodes.Add(currentNode);
        }
    }

    static Node GetNode(char name)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].GetName() == name)
            {
                return nodes[i];
            }
        }
        return default;
    }
}