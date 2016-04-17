using UnityEngine;
using UnityEngine.UI;

public class TileMapController : MonoBehaviour
{
    public Text _saveFileName;
    public Text _loadFileName;
    public IsometricMapGenerator IsometricMapGenerator;

    public void Save()
    {
        IsometricMapGenerator.SaveMap(_saveFileName.text);
    }

    public void Load()
    {
        IsometricMapGenerator.LoadMap(_loadFileName.text);
    }
}
