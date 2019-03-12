using UnityEngine;
using UnityEditor;
using System.IO;
using Google.GData.Client;
using Google.GData.Spreadsheets;

using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class UnsafeSecurityPolicy {
	public static bool Validator(
		object sender,
		X509Certificate certificate,
		X509Chain chain,
		SslPolicyErrors policyErrors) {

        //*** Just accept and move on...
        // SetLogCallbackDefined can only be called from the main thread.
        // Constructors and field initializers will be executed from the loading thread when loading a scene.
        // Don't use this function in the constructor or field initializers, instead move initialization code to the Awake or Start function.
        // 1. Debug.Log 호출하다 예외가 발생
        // 2. Validator 관련 제대로 안돌아감. return true까지 도달하지 못함
        // 3. 근데 싱글턴은 이미 초기화 되어버림
        // 4. 유니티를 종료하지 않는 이상 몇번을 호출하든 get access token이 안돌아간다
        //Debug.Log ("Validation successful!");
        return true;    
	}

	public static void Instate() {

		ServicePointManager.ServerCertificateValidationCallback = Validator;
	}
}

public class GSheetSettings : ScriptableObject {

	bool isInit = false;
	void Init() {
		if (isInit) {
			return;
		}
		isInit = true;
		UnsafeSecurityPolicy.Instate ();
	}
	static readonly string SCOPE = "https://spreadsheets.google.com/feeds https://docs.google.com/feeds";
	static readonly string REDIRECT_URI = "urn:ietf:wg:oauth:2.0:oob";
    static readonly string TOKEN_TYPE = "refresh";

    public string CLIENT_ID;
	public string CLIENT_SECRET;

	public string ACCESS_CODE;

	public string ACCESS_TOKEN;
	public string REFRESH_TOKEN;

    OAuth2Parameters GetParameters() {
		Init ();
		OAuth2Parameters parameters = new OAuth2Parameters();
		
		parameters.ClientId = CLIENT_ID;
		parameters.ClientSecret = CLIENT_SECRET;
		parameters.RedirectUri = REDIRECT_URI;
		parameters.Scope = SCOPE;
		parameters.AccessCode = ACCESS_CODE;

		parameters.AccessToken = ACCESS_TOKEN;
		parameters.RefreshToken = REFRESH_TOKEN;

		return parameters; 
	}

	public void GetAccessCode() {
		Init ();
		OAuth2Parameters parameters = new OAuth2Parameters();
		
		parameters.ClientId = CLIENT_ID;
		parameters.ClientSecret = CLIENT_SECRET;
		parameters.RedirectUri = REDIRECT_URI;
		parameters.Scope = SCOPE;

		string authorizationUrl = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);


		Application.OpenURL (authorizationUrl);
	}


	public void GetAccessToken() {
        /*
        https://github.com/kimsama/Unity-QuickSheet/blob/50dfaed0397c511ac9da9ee42f64143b3a63e02e/Assets/QuickSheet/GDataPlugin/Editor/GDataDB/GDataDB/Impl/GDataDBRequestFactory.cs#L104-L122
        */
        Init();
		OAuth2Parameters parameters = new OAuth2Parameters();

        parameters.ClientId = CLIENT_ID;
        parameters.ClientSecret = CLIENT_SECRET;
        parameters.RedirectUri = REDIRECT_URI;

        parameters.Scope = SCOPE;
        parameters.AccessType = "offline"; // IMPORTANT 
        parameters.TokenType = TOKEN_TYPE; // IMPORTANT 

        parameters.AccessCode = ACCESS_CODE;

        OAuthUtil.GetAccessToken(parameters);
		OAuthUtil.RefreshAccessToken (parameters);

		ACCESS_TOKEN = parameters.AccessToken;
		REFRESH_TOKEN = parameters.RefreshToken;

		EditorUtility.SetDirty (this);
	}
	public SpreadsheetsService GetService() {
		Init ();
		GOAuth2RequestFactory requestFactory =
			new GOAuth2RequestFactory(null, "MySpreadsheetIntegration-v1", GetParameters());
		SpreadsheetsService service = new SpreadsheetsService("MySpreadsheetIntegration-v1");
		service.RequestFactory = requestFactory;
		return service;
	}

	public WorksheetEntry GetWorkSheet(SpreadsheetsService service, string spreadSheetName, string workSheetName) {
		Init ();
		if (service == null) {
			return null;
		}
		SpreadsheetQuery query = new SpreadsheetQuery();
		
		query.Title = spreadSheetName;
		query.Exact = true;
		
		//Iterate over the results
		var feed = service.Query(query);
		
		if (feed.Entries.Count == 0) {
			Debug.LogError ("can't find spreadsheet : " + spreadSheetName);
			return null;
		}

		SpreadsheetEntry spreadsheet = (SpreadsheetEntry)feed.Entries[0];
		WorksheetFeed wsFeed = spreadsheet.Worksheets;
		WorksheetEntry worksheet = null;
		for (int i=0; i<wsFeed.Entries.Count; i++) {
			if(wsFeed.Entries[i].Title.Text == workSheetName) {
				worksheet = wsFeed.Entries[i] as WorksheetEntry;
				break;
			}
		}
		if (worksheet == null) {
			Debug.LogError("can't find worksheet : " + workSheetName);
		}
		return worksheet;
	}




	public string ScriptPath;
	public string EditorScriptPath;
	public string DataAssetPath;
	
	public TextAsset FieldTemplate;
	public TextAsset DataTemplate;
	public TextAsset DataEditorTemplate;
}
