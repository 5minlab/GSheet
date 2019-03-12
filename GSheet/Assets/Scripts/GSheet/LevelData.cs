using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LevelDataEntry : GSheetDataEntry {
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
	int _nscore;
	public int nScore {
		get { return _nscore;}
		set { _nscore = value;}
	}
	[SerializeField]
	int _nlevel;
	public int nLevel {
		get { return _nlevel;}
		set { _nlevel = value;}
	}
	[SerializeField]
	string _scontent;
	public string sContent {
		get { return _scontent;}
		set { _scontent = value;}
	}
	[SerializeField]
	string _ft;
	public string fT {
		get { return _ft;}
		set { _ft = value;}
	}

}
public class LevelData : GSheetData<LevelDataEntry> {
}