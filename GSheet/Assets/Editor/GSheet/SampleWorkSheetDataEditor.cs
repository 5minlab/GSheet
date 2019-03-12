using UnityEditor;

[CustomEditor(typeof(SampleWorkSheetData))]
public class SampleWorkSheetDataEditor : GSheetDataEditor<SampleWorkSheetData,SampleWorkSheetDataEntry> {
	[MenuItem("Assets/Create/Custom/GSheet/Create SampleWorkSheetData")]
	public static void Create() {
		GSheetUtility.CreateAsset<SampleWorkSheetData> ();
	}
}
