using UnityEngine;
using System.Collections.Generic;

public class EnemyNavigationGrid : MonoBehaviour
{
    [SerializeField] private int width = 100;
    [SerializeField] private int height = 100;
    [SerializeField] private float cellSize = 1f;

    [SerializeField] private LayerMask collisionLayer;

    private bool[,] walkable;

    private void Awake()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        walkable = new bool[width, height];

        for (int x = 0; x < width; x++)
        {

            for (int y = 0; y < height; y++)
            {
                Vector2 position = GetWorldPosition(x, y);
                Collider2D collision = Physics2D.OverlapCircle(position, cellSize * 0.4f, collisionLayer);

                walkable[x, y] = collision == null;
            }

        }
    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        return (Vector2)transform.position + new Vector2((x + 0.5f) * cellSize, (y + 0.5f) * cellSize);
    }

    private Vector2Int GetGridPosition(Vector2 worldPosition)
    {
        Vector2 localPosition = worldPosition - (Vector2)transform.position;

        return new Vector2Int(Mathf.FloorToInt(localPosition.x / cellSize), Mathf.FloorToInt(localPosition.y / cellSize));
    }

    private bool IsInsideGrid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    private bool IsWalkable(int x, int y)
    {
        return IsInsideGrid(x, y) && walkable[x, y];
    }

    public Vector2 GetRandomWalkablePosition()
    {
        for (int i = 0; i < 500; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            if (IsWalkable(x, y))
                return GetWorldPosition(x, y);
        }

        return new Vector2(float.NaN, float.NaN);
    }

    public List<Vector2> FindPath(
     Vector2 startPosition,
     Vector2 targetPosition)
    {
        Vector2Int start = GetGridPosition(startPosition);
        Vector2Int target = GetGridPosition(targetPosition);

        if (!IsInsideGrid(start.x, start.y) ||
            !IsInsideGrid(target.x, target.y))
        {
            return new List<Vector2>();
        }

        if (!IsWalkable(target.x, target.y))
        {
            return new List<Vector2>();
        }

        if (!IsWalkable(start.x, start.y))
        {
            start = GetNearestWalkableCell(start);

            if (start.x < 0)
                return new List<Vector2>();
        }

        float[,] costs = new float[width, height];
        Vector2Int[,] parents = new Vector2Int[width, height];
        bool[,] closed = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                costs[x, y] = float.PositiveInfinity;
                parents[x, y] = new Vector2Int(-1, -1);
            }
        }

        List<Vector2Int> open = new List<Vector2Int>();

        costs[start.x, start.y] = 0f;
        open.Add(start);

        // Otto direzioni di movimento.
        int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
        int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };

        while (open.Count > 0)
        {
            // Trova la cella con il costo totale più basso.
            int bestIndex = 0;

            for (int i = 1; i < open.Count; i++)
            {
                Vector2Int a = open[i];
                Vector2Int b = open[bestIndex];

                float scoreA = costs[a.x, a.y] + Heuristic(a, target);
                float scoreB = costs[b.x, b.y] + Heuristic(b, target);

                if (scoreA < scoreB)
                    bestIndex = i;
            }

            Vector2Int current = open[bestIndex];
            open.RemoveAt(bestIndex);

            if (closed[current.x, current.y])
                continue;

            closed[current.x, current.y] = true;

            // La destinazione è stata raggiunta.
            if (current == target)
                return ReconstructPath(start, target, parents);

            // Controlla le otto celle vicine.
            for (int i = 0; i < 8; i++)
            {
                int nx = current.x + dx[i];
                int ny = current.y + dy[i];

                if (!IsWalkable(nx, ny) || closed[nx, ny])
                    continue;

                bool diagonal = dx[i] != 0 && dy[i] != 0;

                // Impedisce di attraversare diagonalmente
                // l'angolo di due ostacoli.
                if (diagonal &&
                    (!IsWalkable(current.x + dx[i], current.y) ||
                     !IsWalkable(current.x, current.y + dy[i])))
                {
                    continue;
                }

                float stepCost = diagonal ? 1.41421356f : 1f;

                float newCost =
                    costs[current.x, current.y] + stepCost;

                if (newCost >= costs[nx, ny])
                    continue;

                costs[nx, ny] = newCost;
                parents[nx, ny] = current;

                Vector2Int neighbour = new Vector2Int(nx, ny);

                if (!open.Contains(neighbour))
                    open.Add(neighbour);
            }
        }

        // Nessun percorso disponibile.
        return new List<Vector2>();
    }

    private Vector2Int GetNearestWalkableCell(Vector2Int origin)
    {
        for (int radius = 1; radius < Mathf.Max(width, height); radius++)
        {
            for (int x = origin.x - radius; x <= origin.x + radius; x++)
            {
                for (int y = origin.y - radius; y <= origin.y + radius; y++)
                {
                    if (Mathf.Abs(x - origin.x) != radius &&
                        Mathf.Abs(y - origin.y) != radius)
                    {
                        continue;
                    }

                    if (IsWalkable(x, y))
                        return new Vector2Int(x, y);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }

    private float Heuristic(Vector2Int a, Vector2Int b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);

        // Distanza diagonale ottimizzata per otto direzioni.
        return Mathf.Max(dx, dy) +
               0.41421356f * Mathf.Min(dx, dy);
    }

    private List<Vector2> ReconstructPath(
        Vector2Int start,
        Vector2Int target,
        Vector2Int[,] parents)
    {
        List<Vector2> path = new List<Vector2>();
        Vector2Int current = target;

        while (current != start)
        {
            path.Add(GetWorldPosition(current.x, current.y));

            current = parents[current.x, current.y];

            if (current.x < 0)
                return new List<Vector2>();
        }

        path.Reverse();
        return path;
    }
}



