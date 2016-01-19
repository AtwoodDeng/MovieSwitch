using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeLine :MonoBehaviour {

	static public string[,] tokenInfoListStr = new string[,]
	{
		{"0.1" , "natureTreeFlower" 	, "1"  , "1" 	, "1" 	, "20" , "3", "true" , "true" , "false" , "false" },
		{"14" , "natureRiver" 			, "-1" , "-1" 	, "2" 	, "-1" , "3" , "true" , "true", "false" , "false"},
		{"4" , "natureSea" 				, "-1" , "-1" 	, "1" 	, "-1" , "3" , "true" , "true", "false" , "false"},
		{"5" , "natureTreeGreen" 		, "-1" , "-1" 	, "2" 	, "-1", "3" , "true" , "true", "false" , "false" },
		{"7" , "natureWaterfall" 		, "-1" , "-1" 	, "1" 	, "-1", "3" , "true" , "true", "false" , "false" },
		// 40s

		// traditional songs
		{"6" , "villageSong" 			, "-1" , "-1" 	, "2" 	, "-1", "3" , "true" , "true", "false" , "false" },
		{"5" , "MidEastDrum" 			, "-1" , "-1" 	, "1" 	, "20", "3" , "true" , "true", "false" , "false" },
		{"5" , "NAMusic2" 				, "-1" , "-1" 	, "2" 	, "20", "3" , "true" , "true", "false" , "false" },
		{"10" , "YunnanDance" 			, "-1" , "-1" 	, "1" 	, "20", "3" , "true" , "true", "false" , "false" },
		{"4" , "AfricanDance" 			, "-1" , "-1" 	, "2" 	, "20" , "3" , "true" , "true", "false" , "false"},
		{"10" , "ScotlandMusic" 		, "-1" , "-1" 	, "1" 	, "10", "3" , "true" , "true", "false" , "false" },
		//97s

		//trans
		{"6" , "TransOld" 				, "-1" , "-1" 	, "1" 	, "18" , "1" , "true" , "true", "false" , "false"},
//		{"1" , "TransOld2" 				, "-1" , "-1" 	, "1" 	, "18" , "1" , "true" , "true", "false" , "false"},
		{"4" , "TransNew" 				, "-1" , "-1" 	, "1" 	, "15" , "4" , "true" , "true", "true" , "false"},
		{"13" , "FoodOld" 				, "-1" , "-1" 	, "1" 	, "15" , "2" , "true" , "false", "false" , "false"},
		{"2" , "FoodOld2" 				, "-1" , "-1" 	, "1" 	, "15" , "1" , "true" , "true", "false" , "false"},
		{"3" , "FoodNew" 				, "-1" , "-1" 	, "1" 	, "16" , "4" , "true" , "true", "true" , "false"},
		{"15" , "HouseOld" 				, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "false"},
		{"2" , "HouseBamboo" 			, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "false"},
		{"3" , "HouseNew" 				, "-1" , "-1" 	, "1" 	, "25" , "4" , "true" , "true", "true" , "false"},
		{"4" , "HouseNewBuild" 			, "-1" , "-1" 	, "1" 	, "25" , "4" , "true" , "true", "true" , "false"},
		{"10" , "CityTokyo" 			, "-1" , "-1" 	, "1" 	, "22" , "4" , "true" , "true", "true" , "false"},
		{"4" , "CityDubai" 				, "-1" , "-1" 	, "1" 	, "22" , "4" , "true" , "true", "true" , "false"},
		{"4" , "CityShanghai" 			, "-1" , "-1" 	, "2" 	, "19" , "3" , "true" ,  "true", "false" , "false"},
		//155s

		//OverWhelm
		{"15" , "BusyCityc" 			, "-1" , "-1" 	, "1" 	, "20" , "3" , "true" , "true", "false" , "true"},
		{"4" , "BusyCity2c" 			, "-1" , "-1" 	, "1" 	, "15" , "3" , "true" , "true", "false" , "true"},
		{"4" , "BusyHuman2c" 			, "-1" , "-1" 	, "1" 	, "16" , "3" , "true" , "true", "false" , "true"},
		{"4" , "BusyHuman4c" 			, "-1" , "-1" 	, "1" 	, "18" , "3" , "true" , "true", "false" , "true"},
		{"4" , "BusyHuman3c" 			, "-1" , "-1" 	, "1" 	, "12" , "3" , "true" , "true", "false" , "true"},
		{"4" , "BusyHumanc" 			, "-1" , "-1" 	, "1" 	, "10" , "3" , "true" , "true", "false" , "true"},

		//TV
		//182s
		{"5" , "TVnews1cc" 				, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"2" , "TVmusic1cc" 			, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"2" , "TVfilm1cc" 				, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"1" , "TVadv0cc" 				, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"1" , "TVsport1cc" 			, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"1" , "TVadv1cc" 				, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.5" , "TVmusic2cc" 			, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.5" , "TVfilm2cc" 				, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.5" , "TVnews2cc" 				, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.33" , "TVsport2cc" 			, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.33" , "TVadv2cc" 				, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.2" , "TVmusic3cc" 			, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.2" , "TVfilm3cc" 				, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.2" , "TVnews3cc" 				, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.2" , "TVsport3cc" 			, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.2" , "TVadv3cc" 				, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.2" , "TVmusic4cc" 			, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.2" , "TVfilm4cc" 				, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.2" , "TVnews4cc" 				, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.1" , "TVsport4cc" 			, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.1" , "TVadv4cc" 				, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.1" , "TVmusic5cc" 			, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.1" , "TVnews5cc" 				, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.1" , "TVadv5cc" 				, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.5" , "TVsport5cc" 			, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.5" , "TVmusic6cc" 			, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.5" , "TVnews6cc" 				, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.5" , "TVsport6cc" 			, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"0.5" , "TVnews7cc" 				, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"1" , "TVnews8cc" 				, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"1" , "TVadv7cc" 				, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		{"1" , "TVadv8cc" 				, "-1" , "-1" 	, "1" 	, "-1" , "1" , "true" , "true", "false" , "true"},
		
		//reverse
		{"7" , "AfterCity2" 			, "-1" , "-1" 	, "3" 	, "12" , "3" , "true" , "false", "false" , "true"},
		{"5" , "Reverse6" 				, "-1" , "-1"	, "2" 	, "12" , "5" , "true" , "false", "false" , "true"},
		{"4" , "Reverse7normal" 		, "-1" , "-1"	, "2" 	, "12" , "5" , "true" , "false", "false" , "false"},
		{"4" , "Scene1" 				, "-1" , "-1" 	, "2" 	, "12" , "4" , "true" , "false", "false" , "false"},
		{"4" , "Reverse4normal" 		, "-1" , "-1" 	, "3" 	, "12" , "9" , "true" , "false", "false" , "false"},
		{"4" , "Reverse5normal" 		, "-1" , "-1" 	, "1" 	, "12" , "9" , "false" , "false", "false" , "false"},
		{"5" , "Reverse1normal" 		, "-1" , "-1" 	, "2" 	, "11" , "3" , "false" , "false", "false" , "false"},

		//Disaster
		{"7" , "Disaster4" 				, "0" , "0" 	, "2" 	, "17" , "2" , "false" , "false", "false" , "false"},
		{"9" , "Disaster5" 				, "2" , "2" 	, "3" 	, "26.5" , "3" , "false" , "false", "false" , "false"},
		{"7" , "Disaster3" 				, "2" , "0" 	, "2" 	, "19.2" , "2" , "false" , "false", "false" , "false"},
		{"4" , "Disaster1" 				, "1" , "2" 	, "1" 	, "14.7" , "1" , "false" , "false", "false" , "false"},
		{"3.5" , "Disaster2" 			, "0" , "3" 	, "2" 	, "11.5" , "1" , "false" , "false", "false" , "false"},
		{"2" , "Disaster6" 				, "-1" , "-1" 	, "1" 	, "9" , "2" , "false" , "false", "false" , "false"},


		{"13" , "Crowd720new" 			, "0" , "0" 	, "5" 	, "-1" , "5" , "false" , "false", "false" , "false"},



	};

	static public List<TokenInfo> tokenInfoList;

	public class TokenInfo{
		public float time;
		public string name;
		public int i = -1;
		public int j = -1;
		public int size = 0;
		public float duration = -1f;
		public int maxSize = 3;
		public bool move = true;
		public bool resize = true;
		public bool eat = false;
		public bool delete = false;
	};
	
	static public TokenInfo GetTokenInfo( float itTime , int index )
	{
//		string[] info = tokenInfo[index];
		TokenInfo tokenInfo = new TokenInfo();
		tokenInfo.time = itTime + float.Parse( tokenInfoListStr[index,0] );
		tokenInfo.name = tokenInfoListStr[index,1];

		tokenInfo.i = int.Parse(tokenInfoListStr[index,2]);
		tokenInfo.j = int.Parse(tokenInfoListStr[index,3]);
		tokenInfo.size = int.Parse(tokenInfoListStr[index,4]);
		tokenInfo.duration = float.Parse(tokenInfoListStr[index,5]);
		tokenInfo.maxSize = int.Parse(tokenInfoListStr[index,6]);

		tokenInfo.move = bool.Parse(tokenInfoListStr[index,7]);
		tokenInfo.resize = bool.Parse(tokenInfoListStr[index,8]);
		tokenInfo.eat = bool.Parse(tokenInfoListStr[index,9]);
		tokenInfo.delete = bool.Parse(tokenInfoListStr[index,10]);
		return tokenInfo;
	}

	void Start(){
		tokenInfoList = new List<TokenInfo>();
		float time = 0;
		for(int i = 0 ; i <= tokenInfoListStr.GetUpperBound(0) ; ++ i )
		{
			TokenInfo newTokenInfo = GetTokenInfo(time,i);
			time = newTokenInfo.time;
			tokenInfoList.Add(newTokenInfo);
		}
	}
};
