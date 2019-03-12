using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class GSheetDataEntry {
}

public abstract class GSheetData<Entry> : ScriptableObject
	where Entry : GSheetDataEntry
{
	public string SpreadSheetName;
	public string WorkSheetName;
	public List<Entry> data;
}