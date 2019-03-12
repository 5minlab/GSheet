using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(GSheetSettings))]
public class GSheetsettingsEditor : Editor {
	GSheetSettings settings;
	void OnEnable() {
		settings = target as GSheetSettings;
	}
	public override void OnInspectorGUI ()
	{
		settings.CLIENT_ID = EditorGUILayout.TextField ("Client ID", settings.CLIENT_ID);
		settings.CLIENT_SECRET = EditorGUILayout.TextField ("Client Secret", settings.CLIENT_SECRET);

		if (GUILayout.Button ("Get Access Code")) {
			settings.GetAccessCode();
		}

		settings.ACCESS_CODE = EditorGUILayout.TextField ("Access Code", settings.ACCESS_CODE);

		if (GUILayout.Button ("Get Access Token")) {
			settings.GetAccessToken();
		}
		//settings.ACCESS_TOKEN = EditorGUILayout.TextField ("Access Token", settings.ACCESS_TOKEN);
		EditorGUILayout.LabelField ("Access Token", settings.ACCESS_TOKEN);
		EditorGUILayout.LabelField ("Refresh Token", settings.REFRESH_TOKEN);

		EditorGUILayout.Separator ();

		settings.ScriptPath = EditorGUILayout.TextField ("Script Path", settings.ScriptPath);
		settings.EditorScriptPath = EditorGUILayout.TextField ("Editor Script Path", settings.EditorScriptPath);
		settings.DataAssetPath = EditorGUILayout.TextField ("Data Asset Path", settings.DataAssetPath);

		EditorGUILayout.Separator ();

		settings.FieldTemplate = EditorGUILayout.ObjectField ("Field Template",settings.FieldTemplate, typeof(TextAsset), false) as TextAsset;
		settings.DataTemplate = EditorGUILayout.ObjectField ("Data Template",settings.DataTemplate, typeof(TextAsset), false) as TextAsset;
		settings.DataEditorTemplate = EditorGUILayout.ObjectField ("Data Editor Template",settings.DataEditorTemplate, typeof(TextAsset), false) as TextAsset;

		if (GUI.changed) {
			EditorUtility.SetDirty(settings);
		}
	}

	[MenuItem("Assets/Create/Custom/GSheet/Create GSheet Settings")]
	public static void CreateSettings() {
		var setting = AssetDatabase.FindAssets ("t:GSheetSettings");
		if (setting.Length > 0) {
			Debug.Log ("setting already exist : " + AssetDatabase.GUIDToAssetPath(setting[0]));
		} else {
			GSheetUtility.CreateAsset<GSheetSettings>();
		}
	}
}
