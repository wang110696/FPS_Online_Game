using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FPS/Map Models")]
public class MapModelsScriptableObject : ScriptableObject
{
    public List<MapModel> MapModels;
}