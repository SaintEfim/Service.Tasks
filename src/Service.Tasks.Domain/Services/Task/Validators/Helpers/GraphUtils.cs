namespace Service.Tasks.Domain.Services.Task.Validators.Helpers;

public static class GraphUtils
{
    public static bool HasCycle<T>(
        IEnumerable<(T vertex, List<T> neighbors)> edges,
        IEnumerable<T> vertices)
        where T : notnull
    {
        var colors = vertices.ToDictionary(s => s, _ => 0);
        var graph = edges.ToDictionary(k => k.vertex, v => v.neighbors);

        return graph.Keys
            .Where(current => colors[current] == 0)
            .Any(current => Dfs(graph, colors, current));
    }

    private static bool Dfs<T>(
        Dictionary<T, List<T>> graph,
        Dictionary<T, int> colors,
        T current)
        where T : notnull
    {
        colors[current] = 1;

        graph.TryGetValue(current, out var neighbors);
        if (neighbors != null)
        {
            foreach (var next in neighbors)
            {
                switch (colors[next])
                {
                    case 0:
                        if (Dfs(graph, colors, next))
                        {
                            return true;
                        }

                        break;
                    case 1:
                        return true;
                }
            }
        }

        colors[current] = 2;

        return false;
    }
}
