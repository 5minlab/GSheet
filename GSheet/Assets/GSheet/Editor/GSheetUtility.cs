using UnityEditor;
using UnityEngine;
using System.IO;

public static class GSheetUtility
{
	/// <summary>
	//	This makes it easy to create, name and place unique new ScriptableObject asset files.
	/// </summary>
	public static void CreateAsset<T> () where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T> ();
		
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (path == "") 
		{
			path = "Assets";
		} 
		else if (Path.GetExtension (path) != "") 
		{
			path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
		}
		
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/New " + typeof(T).ToString() + ".asset");
		
		AssetDatabase.CreateAsset (asset, assetPathAndName);
		
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = asset;
	}

	public static GSheetSettings GetSettings() {
		string[] settings = AssetDatabase.FindAssets ("t:GSheetSettings");
		if (settings.Length == 0) {
			Debug.Log ("can't find settings");
			return null;
		} 
		if (settings.Length > 1) {
			Debug.Log ("settings num > 1 error");
			return null;
		} 
		GSheetSettings setting = AssetDatabase.LoadAssetAtPath (AssetDatabase.GUIDToAssetPath (settings [0]), typeof(GSheetSettings)) as GSheetSettings;
		return setting;
	}
}