using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class CompileTimeTracker
{
    private const string StartTimeKey = "CompileTimeTracker.StartTime";

    static CompileTimeTracker()
    {
        EditorApplication.update += TrackCompilation;
    }

    private static void TrackCompilation()
    {
        if (EditorApplication.isCompiling)
        {
            if (!EditorPrefs.HasKey(StartTimeKey))
            {
                EditorPrefs.SetString(
                    StartTimeKey,
                    EditorApplication.timeSinceStartup.ToString("R")
                );

                Debug.Log("Compilation started.");
            }
        }
        else if (EditorPrefs.HasKey(StartTimeKey))
        {
            double startTime = double.Parse(
                EditorPrefs.GetString(StartTimeKey)
            );

            double finishTime = EditorApplication.timeSinceStartup;
            double compileTime = finishTime - startTime;

            EditorPrefs.DeleteKey(StartTimeKey);

            Debug.Log(
                $"Script compilation time: {compileTime:F3} seconds"
            );
        }
    }
}