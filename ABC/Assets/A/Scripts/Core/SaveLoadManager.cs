using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) SaveGame();
        if (Input.GetKeyDown(KeyCode.F9)) LoadGame();
    }

    public void SaveGame()
    {
        GameStateManager gsm = GameStateManager.Instance;
        Inventory inv = Inventory.Instance;

        PlayerPrefs.SetInt("mainQuestStep", gsm.mainQuestStep);
        PlayerPrefs.SetInt("hasIDCard", gsm.hasIDCard ? 1 : 0);
        PlayerPrefs.SetInt("hasSurfaceClue", gsm.hasSurfaceClue ? 1 : 0);
        PlayerPrefs.SetInt("hasDeepClue", gsm.hasDeepClue ? 1 : 0);
        PlayerPrefs.SetInt("hasStealTeach", gsm.hasStealTeach ? 1 : 0);
        PlayerPrefs.SetInt("hasStealDone", gsm.hasStealDone ? 1 : 0);
        PlayerPrefs.SetInt("hasSneakTeach", gsm.hasSneakTeach ? 1 : 0);
        PlayerPrefs.SetInt("hasGottenClue", gsm.hasGottenClue ? 1 : 0);

        // 保存背包
        string itemsJson = JsonUtility.ToJson(new ItemList(inv.items));
        PlayerPrefs.SetString("inventory", itemsJson);

        PlayerPrefs.Save();
        Debug.Log("游戏已保存 (F5)");
    }

    public void LoadGame()
    {
        GameStateManager gsm = GameStateManager.Instance;
        Inventory inv = Inventory.Instance;

        gsm.mainQuestStep = PlayerPrefs.GetInt("mainQuestStep", 0);
        gsm.hasIDCard = PlayerPrefs.GetInt("hasIDCard", 0) == 1;
        gsm.hasSurfaceClue = PlayerPrefs.GetInt("hasSurfaceClue", 0) == 1;
        gsm.hasDeepClue = PlayerPrefs.GetInt("hasDeepClue", 0) == 1;
        gsm.hasStealTeach = PlayerPrefs.GetInt("hasStealTeach", 0) == 1;
        gsm.hasStealDone = PlayerPrefs.GetInt("hasStealDone", 0) == 1;
        gsm.hasSneakTeach = PlayerPrefs.GetInt("hasSneakTeach", 0) == 1;
        gsm.hasGottenClue = PlayerPrefs.GetInt("hasGottenClue", 0) == 1;

        // 读取背包
        string itemsJson = PlayerPrefs.GetString("inventory", "{\"items\":[]}");
        ItemList loadedItems = JsonUtility.FromJson<ItemList>(itemsJson);
        inv.items = loadedItems.items;

        Debug.Log("游戏已读档 (F9)");
    }

    [System.Serializable]
    public class ItemList
    {
        public List<string> items;
        public ItemList(List<string> items) { this.items = items; }
    }
}