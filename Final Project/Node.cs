public class Node
{
    char name;
    bool isChargingStation;

    bool visited;

    int distance;

    //Linked List
    List<char> connectedNodes = new List<char>();
    List<int> weights = new List<int>();

    public void AddNode(char name, int weight)
    {
        connectedNodes.Add(name);
        weights.Add(weight);
    }

    public void RemoveNode(char name)
    {
        for (int i = 0; i < connectedNodes.Count; i++)
        {
            if (connectedNodes[i] == name)
            {
                weights.RemoveAt(i);
                connectedNodes.RemoveAt(i);
            }
        }
    }

    public List<char> GetConnections()
    {
        return connectedNodes;
    }
    public List<int> GetConnectionWeights()
    {
        return weights;
    }

    public void SetVisited()
    {
        visited = true;
    }

    public bool GetVisited()
    {
        return visited;
    }

    public void SetDistance(int value)
    {
        distance = value;
    }
    public int GetDistance()
    {
        return distance;
    }

    public char GetName()
    {
        return name;
    }
    public void SetName(char newName)
    {
        name = newName;
    }

    public bool GetChargingStation()
    {
        return isChargingStation;
    }
    public void SetChargingStation()
    {
        isChargingStation = true;
    }
}

