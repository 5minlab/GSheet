using UnityEditor;

[CustomEditor(typeof(Sheet1Data))]
public class Sheet1DataEditor : GSheetDataEditor<Sheet1Data,Sheet1DataEntry> {
	[MenuItem("Assets/Create/Custom/GSheet/Create Sheet1Data")]
	public static void Create() {
		GSheetUtility.CreateAsset<Sheet1Data> ();
	}
}
