using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Heatmap_BombDrop : MonoBehaviour
{
    private static List<Vector3> m_deathPositions = new List<Vector3>();
    private static GameObject heatmapPrefab;
    private static string m_path = "Assets/Resources/Text/";

    void Start()
    {

}

[MenuItem("Tools/Heatmap/Generate")]
    static void ReadDeathData()
    {
        m_deathPositions.Clear();
        string filePath = m_path + SceneManager.GetActiveScene().name ; //Creates and uses a file per scence. This application uses your scene name to generate death textfile. 
        heatmapPrefab = (GameObject)Resources.Load("prefabs/deathPrefab", typeof(GameObject));//Prefab to use to render death positions.

        //Read the text from directly from the txt file
        string fullPath = filePath + ".txt";
        StreamReader reader = new StreamReader(fullPath);
        string deathCoords = "";
        while ((deathCoords = reader.ReadLine()) != null) {//going through the text file line by line and adding it to a list of vectors.
            m_deathPositions.Add(stringToVec(deathCoords));
            deathCoords = "";
        }
        reader.Close();
       renderDeathData();
    }

    
    public static Vector3 stringToVec(string _st)
    {
        Vector3 result = new Vector3();
        string[] vals = _st.Split(',');
        if (vals.Length == 3)
        {
            result.Set(float.Parse(vals[0]), float.Parse(vals[1]), float.Parse(vals[2]));
        }
        return result;
    }

    public static void renderDeathData()
    {
        foreach (Vector3 deathPos in m_deathPositions) {
            Instantiate(heatmapPrefab, deathPos, Quaternion.identity);
        }
    }

    [MenuItem("Tools/Heatmap/Clear")]
    public static void destroyHeatmapObjects()
    {
            GameObject[] hMap_Spheres = GameObject.FindGameObjectsWithTag("heatmap");
            for (int i = 0; i < hMap_Spheres.Length; i++)
            {
                GameObject.DestroyImmediate(hMap_Spheres[i]);
            }
    }
}
