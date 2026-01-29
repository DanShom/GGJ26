using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "NewSaveManager", menuName = "NewSaveManager")]
public class SaveManager : ScriptableObject
{
    [SerializeField] private GameData gameData;

    private List<Saveable> saveableObjects;

    private FileDataHandler Handler;

    public static SaveManager instance { get; private set; }
    public string fileName = "SaveGame.game";
    public bool EncriptData = false;

    private void Awake()
    {
        init();
    }

    public void init()
    {
        if(instance != null && instance != this)
        {
            Debug.LogError("Found more than one Save Manager in the scene.");
        }
        instance = this;
        //if (saveableObjects == null) { saveableObjects = FindAllSaveables(); }
        saveableObjects = FindAllSaveables();
        if (Handler == null)
        {
            Handler = new FileDataHandler(Application.persistentDataPath, fileName, EncriptData);
        }
    }


    private void Start()
    {
        LoadGame();
    }

    private void OnApplicationQuit()
    {

        SaveGame();
    }
    [ContextMenu("Generate guid id for all savebls")]
    private void SetIds()
    {
        var saveables = FindAllSaveables();
        foreach (var a in saveables)
        {
            //a.SetId(System.Guid.NewGuid().ToString() );
        }
    }

    public void DeleteGame()
    {
        init();
        Handler.Delete("01");
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public void SaveGame()
    {
        init();
        foreach (var a in saveableObjects)
        {
            a.SaveData(ref this.gameData);
        }

        Handler.Save(this.gameData, "01");
        Debug.Log("Game Saved");
    }
    public void LoadGame()
    {
        init();
        this.gameData = Handler.Load("01");
        Debug.Log(this.gameData);
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data defaults.");
            NewGame();
        }

        foreach(var a in saveableObjects)
        {
            a.LoadData(this.gameData);
        }
        //Debug.Log(Handler.GetDataDirPath());
        Debug.Log("Game Loaded");
    }


    private List<Saveable> FindAllSaveables()
    {
        IEnumerable<Saveable> saveableObject = FindObjectsOfType<MonoBehaviour>(true).OfType<Saveable>();

        return new List<Saveable>(saveableObject);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SaveManager))]
public class SaveManagerCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SaveManager ge = (SaveManager)target;
        if (GUILayout.Button("SaveGame"))
        {
            ge.init();
            ge.SaveGame();
        }
        if (GUILayout.Button("Load Game"))
        {
            ge.init();
            ge.LoadGame();
        }
        if (GUILayout.Button("Delete Game"))
        {
            ge.DeleteGame();
        }
    }
}
#endif