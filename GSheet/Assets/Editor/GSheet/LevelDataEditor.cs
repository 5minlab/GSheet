using UnityEditor;

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : GSheetDataEditor<LevelData,LevelDataEntry> {
	[MenuItem("Assets/Create/Custom/GSheet/Create LevelData")]
	public static void Create() {
		GSheetUtility.CreateAsset<LevelData> ();
	}
}
