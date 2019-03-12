using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Sheet1DataEntry : GSheetDataEntry {
	[SerializeField]
	string _sname;
	public string sName {
		get { return _sname;}
		set { _sname = value;}
	}

	[SerializeField]
	int _nage;
	public int nAge {
		get { return _nage;}
		set { _nage = value;}
	}

	[SerializeField]
	int _ngender;
	public int nGender {
		get { return _ngender;}
		set { _ngender = value;}
	}

	[SerializeField]
	int	 _nscore;
	public int nScore {
		get { return _nscore;}
		set { _nscore = value;}
	}
}
public class Sheet1Data : GSheetData<Sheet1DataEntry> {
}