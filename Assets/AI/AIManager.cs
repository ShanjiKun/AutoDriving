using System;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using System.IO;

public class AIManager {

	//	Define classes
	public static string QD1_TIEPTUC 	= "TiepTuc";
	public static string QD1_GIAMRETRAI = "GiamReTrai";
	public static string QD1_GIAMREPHAI = "GiamRePhai";
	public static string QD1_DUNG		= "Dung";
	public static string QD1_TANGTOC	= "TangToc";

	public static string QD2_REGAP 		= "ReGap";
	public static string QD2_REVUA 		= "ReVua";
	public static string QD2_RENHE 		= "ReNhe";

	public static string QD3_TIEPTUC 	= "TiepTuc";
	public static string QD3_DUNG		= "Dung";

	//	Decision type
	public enum DecisionType
	{
		DEC1,
		DEC2,
		DEC3
	}

	//	Ranges
	public static List<int> speedRanges = new List<int>{0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100};
	public static List<int> distanceRanges = new List<int>{0, 3, 6, 9, 12, 15, 18, 21, 24, 27, 30};

	public static bool TrainSystem(string path1, string path2, string path3)
	{
		List<string> trainData1 = getTrainDataFromFile (path1, DecisionType.DEC1);
		bool isState1 = CustomNaiveBayes.sharedDecision1().trainSystemWithData(trainData1);

		List<string> trainData2 = getTrainDataFromFile (path2, DecisionType.DEC2);
		bool isState2 = CustomNaiveBayes.sharedDecision2().trainSystemWithData(trainData2);

		List<string> trainData3 = getTrainDataFromFile (path3, DecisionType.DEC3);
		bool isState3 = CustomNaiveBayes.sharedDecision3().trainSystemWithData(trainData3);

		//	Check for state
		if(!isState1 || !isState2 || !isState3) {
			return false;
		}

		//	Train success
		return true;
	}

	//	Use to get a decision with test data
	public static List<string> getDecisions(string distance, string speed, string left, string right, string ahead, string behind)
	{
		//	Preprocess data
		distance = handleDistance(distance);
		speed = handleSpeed(speed);

		//	Get decision 1
		string test1 = distance + "," + speed + "," + left + "," + right + "," + ahead + "," + behind;
		string dec1 = CustomNaiveBayes.sharedDecision1().getDecisionWithTest (test1);
		if (dec1 == QD1_GIAMRETRAI || dec1 == QD1_GIAMREPHAI) {
			//	Get decision 2
			string test2 = distance + "," + speed;
			string dec2 = CustomNaiveBayes.sharedDecision2().getDecisionWithTest (test2);

			//	Get decision 3
			string test3 = dec2 + "," + left + "," + right;
			string dec3 = CustomNaiveBayes.sharedDecision3().getDecisionWithTest (test3);

			return new List<string>{dec1, dec2, dec3};
		}
		return new List<string>{dec1};
	}

	//	Use to load and preprocess train data from file
	static List<string> getTrainDataFromFile(string path, DecisionType dec)
	{
		List<string> trainData = new List<string> ();

		StreamReader reader =  new StreamReader(path);
		string text = reader.ReadLine ();
		while (text != null) {
			
			//	Preprocess data
			string processData = preprocessTrainData (text, dec);
			trainData.Add (processData);

			//	Read next line
			text = reader.ReadLine ();
		}

		return trainData;
	}

	//	To proprocess train data
	static string preprocessTrainData (string originData, DecisionType dec)
	{
		string processData = originData;
		string[] elems = originData.Split ("," [0]);
		switch (dec) {
		case DecisionType.DEC1:
			{
				string processDis = handleDistance (elems [0]);
				string processSpeed = handleSpeed (elems [1]);
				processData = processDis + "," + processSpeed + "," + elems [2] + "," + elems [3] + "," + elems [4] + "," + elems [5] + "," + elems [6];
			}
			break;
		case DecisionType.DEC2:
			{
				string processDis = handleDistance (elems [0]);
				string processSpeed = handleSpeed (elems [1]);
				processData = processDis + "," + processSpeed + "," + elems [2];
			}
			break;
		default:
			break;
		}

		return processData;
	}

	//	Helpers
	static string handleDistance(string dis)
	{
		int i;
		for (i = 0; i < distanceRanges.Count; i++) {
			if (distanceRanges [i] > Convert.ToInt32(dis))
				break;
		}
		i = i == 0 ? 0 : i-1; 
		return i.ToString();
	}

	static string handleSpeed(string speed)
	{
		int i;
		for (i = 0; i < speedRanges.Count; i++) {
			if (speedRanges [i] > Convert.ToInt32(speed))
				break;
		}
		i = i == 0 ? 0 : i-1; 
		return i.ToString();
	}

}
