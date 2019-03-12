using Google.GData.Client;
using Google.GData.Spreadsheets;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class GSheetDataEditor<Data,Entry> : Editor
	where Data : GSheetData<Entry>
	where Entry : GSheetDataEntry
{
	Data sheet;
	void OnEnable() {
		sheet = target as Data;
	}
	
	static readonly float buttonWidth = 25f;
	static readonly float fieldMinWidth = 20f;

	void PropertyField(object entry, PropertyInfo property) {
		if (property.PropertyType == typeof(int)) {
			int valueNow = (int)property.GetValue (entry,null);
			int valueNew = EditorGUILayout.IntField(valueNow,GUILayout.MinWidth(fieldMinWidth));
			if(valueNew != valueNow) {
				property.SetValue (entry,valueNew,null);
			}
		} else if (property.PropertyType == typeof(float)) {
			float valueNow = (float)property.GetValue (entry,null);
			float valueNew = EditorGUILayout.FloatField(valueNow,GUILayout.MinWidth(fieldMinWidth));
			if(valueNew != valueNow) {
				property.SetValue (entry,valueNew,null);
			}
		} else if (property.PropertyType == typeof(string)) {
			string valueNow = (string)property.GetValue (entry,null);
			string valueNew = EditorGUILayout.TextField(valueNow,GUILayout.MinWidth(fieldMinWidth));
			if(valueNew != valueNow) {
				property.SetValue (entry,valueNew,null);
			}
		} else {
			Debug.LogError("unknown type : " + property.Name);
		}
	}
	public override void OnInspectorGUI ()
	{
		if (GUILayout.Button ("Download From Google Spread Sheet")) {
			Download();
			EditorUtility.SetDirty(sheet);
		}

		sheet.SpreadSheetName = EditorGUILayout.TextField ("Spread Sheet Name", sheet.SpreadSheetName);
		sheet.WorkSheetName = EditorGUILayout.TextField ("Work Sheet Name", sheet.WorkSheetName);
		
		GUILayout.BeginHorizontal ();
		Type EntryType = typeof(Entry);
		PropertyInfo[] properties = EntryType.GetProperties ();
		for (int p=0; p<properties.Length; p++) {
			GUILayout.Label(properties[p].Name,GUILayout.MinWidth(fieldMinWidth));
		}
		GUILayout.Space (4 * buttonWidth + 16);
		GUILayout.EndHorizontal ();

		List<Entry> list = sheet.data;
		for (int i=0; list != null && i < list.Count; i++) {
			GUILayout.BeginHorizontal();
			object entry = list[i];
			for(int p=0;p<properties.Length;p++) {
				PropertyField (entry,properties[p]);
			}
			
			if(GUILayout.Button ("+",GUILayout.Width(buttonWidth))) {
				list.Insert(i,Activator.CreateInstance(EntryType) as Entry);
			} 
			if(GUILayout.Button ("-",GUILayout.Width(buttonWidth))) {
				list.RemoveAt(i);
			} 
			if(GUILayout.Button ("▲",GUILayout.Width (buttonWidth))) {
				if(i > 0) {
					list[i] = list[i-1];
					list[i-1] = entry as Entry;
				}
			} 
			if(GUILayout.Button ("▼",GUILayout.Width (buttonWidth))) {
				if(i < list.Count-1) {
					list[i] = list[i+1];
					list[i+1] = entry as Entry;
				}
			}
			
			GUILayout.EndHorizontal();
		}
		if (GUI.changed) {
			EditorUtility.SetDirty(sheet);
		}

	}
	
	void Download() {
		GSheetSettings setting = GSheetUtility.GetSettings ();
		if (setting == null)
			return;
		SpreadsheetsService service = setting.GetService ();

		WorksheetEntry worksheet = setting.GetWorkSheet (service, sheet.SpreadSheetName, sheet.WorkSheetName);

		AtomLink listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
		
		// Fetch the list feed of the worksheet.
		ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
		ListFeed listFeed = service.Query(listQuery);
		
		sheet.data = new List<Entry> ();
		Type t = typeof(Entry);
		BindingFlags bindingFlag = BindingFlags.NonPublic | BindingFlags.Instance;
		foreach (ListEntry row in listFeed.Entries)
		{
			object entry = Activator.CreateInstance(t);
			foreach (ListEntry.Custom element in row.Elements)
			{
                //필드 이름이 비어있는 경우 _clrrx, _cyevm... 같은 형태로 연결되더라
                var blankGoogleSpreadsheetRegex = new Regex(@"_\w\w\w\w\w");
                if(blankGoogleSpreadsheetRegex.IsMatch(element.LocalName)) {
                    continue;
                }
                // 타입으로 추정 안되면 건너뛰기
                var allowedPrefix = new HashSet<string>() { "s", "f", "n" };
                bool prefixFound = false;
                foreach(var prefix in allowedPrefix) {
                    if (element.LocalName.StartsWith(prefix)) {
                        prefixFound = true;
                        break;
                    }
                }
                if(prefixFound == false) {
                    continue;
                }


                string fieldName = string.Format("_{0}",element.LocalName.ToLower());
				FieldInfo field = t.GetField(fieldName,bindingFlag);
				if(field == null) {
					Debug.LogError("null field : " + element.LocalName);
					continue;
				}
				SetValue(field,entry,element.Value);
			}
			sheet.data.Add (entry as Entry);
		}
	}
	void SetValue(FieldInfo field, object entry, string value) {
        // GSheet에서 숫자에 실수로 공백 넣으면 0으로 예외처리
		if (field.FieldType== typeof(int)) {
            int val = 0;
            if(value != "") {
                val = int.Parse(value);
            }
			field.SetValue(entry, val);

		} else if (field.FieldType == typeof(float)) {
            float val = 0;
            if(value != "") {
                val = float.Parse(value);
            }
			field.SetValue(entry, val);

		} else if (field.FieldType == typeof(string)) {
			field.SetValue(entry,value);
		} else {
			Debug.LogError("unknown type : " + field.Name);
		}
	}
}
