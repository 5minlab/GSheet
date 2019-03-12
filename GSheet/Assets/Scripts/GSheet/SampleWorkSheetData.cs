using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SampleWorkSheetDataEntry : GSheetDataEntry {
	[SerializeField]
	string _skey;
	public string sKey {
		get { return _skey;}
		set { _skey = value;}
	}
	[SerializeField]
	int _nnumber;
	public int nNumber {
		get { return _nnumber;}
		set { _nnumber = value;}
	}
	[SerializeField]
	string _sname;
	public string sName {
		get { return _sname;}
		set { _sname = value;}
	}
	[SerializeField]
	float _fbounty;
	public float fBounty {
		get { return _fbounty;}
		set { _fbounty = value;}
	}
	[SerializeField]
	string _sjob;
	public string sJob {
		get { return _sjob;}
		set { _sjob = value;}
	}

}
public class SampleWorkSheetData : GSheetData<SampleWorkSheetDataEntry> {
}