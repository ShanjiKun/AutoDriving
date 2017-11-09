//using System;
//using System.IO;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.RegularExpressions;
//using UnityEngine;
//
//public class NaiveBayes : MonoBehaviour {
//
//	public static string QD1_TIEPTUC 	= "TiepTuc";
//	public static string QD1_GIAMRETRAI = "GiamReTrai";
//	public static string QD1_GIAMREPHAI = "GiamRePhai";
//	public static string QD1_DUNG		= "Dung";
//	public static string QD1_TANGTOC	= "TangToc";
//
//	public static string QD2_REGAP 		= "ReGap";
//	public static string QD2_REVUA 		= "ReVua";
//	public static string QD2_RENHE 		= "ReNhe";
//
//	public static string QD3_TIEPTUC 	= "TiepTuc";
//	public static string QD3_DUNG		= "Dung";
//
//	//	Pathfile
//	public static string parentPath = "Assets/AI/TrainingData/";
//	//	public static string parentPath = "Assets/AI/TrainingData_Tam/";
//
//	//	Range
//	public static List<int> distanceRanges = new List<int>{0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100};
//	public static List<int> speedRanges = new List<int>{0, 3, 6, 9, 12, 15, 18, 21, 24, 27, 30};
//
//	public static List<string> AI(string distance, string speed, string left, string right, string ahead, string behind)
//	{
//		string test1 = distance + " " + speed + " " + left + " " + right + " " + ahead + " " + behind;
//
//		print ("Test: " + test1);
//
//		string dec1 = BAYESDecision1 (test1);
//
//		if (dec1 == QD1_GIAMRETRAI || dec1 == QD1_GIAMREPHAI) {
//			string test2 = distance + " " + speed;
//			string dec2 = BAYESDecision2 (test2);
//
//			string test3 = dec2 + " " + left + " " + right;
//			string dec3 = BAYESDecision3 (test3);
//
//			return new List<string>{dec1, dec2, dec3};
//		}
//
//		return new List<string>{dec1};
//	}
//
//	//	Decisions
//	private static string BAYESDecision1(string test)
//	{
//		List<Document> trainData1 = getTrainData1();
//		var c = new Classifier(trainData1);
//		var res0 = c.IsInClassProbability(QD1_TIEPTUC, test);
//		var res1 = c.IsInClassProbability(QD1_GIAMRETRAI, test);
//		var res2 = c.IsInClassProbability(QD1_GIAMREPHAI, test);
//		var res3 = c.IsInClassProbability(QD1_DUNG, test);
//		var res4 = c.IsInClassProbability(QD1_TANGTOC, test);
//
//		print (res0 + " " + res1 + " " + res2 + " " + res3 + " " + res4);
//
//		List<double> res = new List<double> ();
//		res.Add (res0);
//		res.Add (res1);
//		res.Add (res2);
//		res.Add (res3);
//		res.Add (res4);
//
//		// Find out max of res
//		double max = res0;
//		int index = 0;
//		for (int i = 1; i < res.Count; i++) {
//			if (res [i] > max) {
//				max = res [i];
//				index = i;
//			}
//		}
//
//		// Return dec1
//		switch (index) {
//		case 1:
//			return QD1_GIAMRETRAI;
//		case 2:
//			return QD1_GIAMREPHAI;
//		case 3:
//			return QD1_DUNG;
//		case 4:
//			return QD1_TANGTOC;
//		default:
//			return QD1_TIEPTUC;
//		}
//	}
//
//	private static string BAYESDecision2(string test)
//	{
//		List<Document> trainData2 = getTrainData2();
//		var c = new Classifier(trainData2);
//		var res0 = c.IsInClassProbability(QD2_REGAP, test);
//		var res1 = c.IsInClassProbability(QD2_REVUA, test);
//		var res2 = c.IsInClassProbability(QD2_RENHE, test);
//
//		List<double> res = new List<double> ();
//		res.Add (res0);
//		res.Add (res1);
//		res.Add (res2);
//
//		// Find out max of res
//		double max = res0;
//		int index = 0;
//		for (int i = 1; i < res.Count; i++) {
//			if (res [i] > max) {
//				max = res [i];
//				index = i;
//			}
//		}
//
//		// Return dec2
//		switch (index) {
//		case 1:
//			return QD2_REVUA;
//		case 2:
//			return QD2_RENHE;
//		default:
//			return QD2_REGAP;
//		}
//	}
//
//	private static string BAYESDecision3(string test)
//	{
//		List<Document> trainData3 = getTrainData3();
//		var c = new Classifier(trainData3);
//		var res0 = c.IsInClassProbability(QD3_TIEPTUC, test);
//		var res1 = c.IsInClassProbability(QD3_DUNG, test);
//
//		List<double> res = new List<double> ();
//		res.Add (res0);
//		res.Add (res1);
//
//		// Find out max of res
//		double max = res0;
//		int index = 0;
//		for (int i = 1; i < res.Count; i++) {
//			if (res [i] > max) {
//				max = res [i];
//				index = i;
//			}
//		}
//
//		// Return dec3
//		switch (index) {
//		case 1:
//			return QD3_DUNG;
//		default:
//			return QD3_TIEPTUC;
//		}
//	}
//
//	//	Naive bayes
//	// Generate train data
//	static List<Document> getTrainData1()
//	{
//		List<Document> trainData = new List<Document> ();
//
//		string path = parentPath + "Test1.txt";
//		StreamReader reader =  new StreamReader(path);
//		string text = reader.ReadLine ();
//		while (text != null) {
//			string[] arr = text.Split (","[0]);
//			if (arr.Length != 7)
//				continue;
//
//			//	Test
//			//			handleDistance(arr [0]);
//			//			handleSpeed(arr [1]);
//
//			// 
//			string props = arr [0] + " " + arr [1] + " " + arr [2] + " " + arr [3] + " " + arr [4] + " " + arr [5];
//			string _class = arr [6];
//			Document doc = new Document (_class, props);
//
//			trainData.Add (doc);
//
//			text = reader.ReadLine ();
//		}
//		return trainData;
//	}
//
//	static List<Document> getTrainData2()
//	{
//		List<Document> trainData = new List<Document> ();
//
//		string path = parentPath + "Test2.txt";
//		StreamReader reader =  new StreamReader(path);
//		string text = reader.ReadLine ();
//		while (text != null) {
//			string[] arr = text.Split (","[0]);
//			if (arr.Length != 3)
//				continue;
//
//			string props = arr [0] + " " + arr [1];
//			string _class = arr [2];
//			Document doc = new Document (_class, props);
//
//			trainData.Add (doc);
//
//			text = reader.ReadLine ();
//		}
//		return trainData;
//	}
//
//	static List<Document> getTrainData3()
//	{
//		List<Document> trainData = new List<Document> ();
//
//		string path = parentPath + "Test3.txt";
//		StreamReader reader =  new StreamReader(path);
//		string text = reader.ReadLine ();
//		while (text != null) {
//			string[] arr = text.Split (","[0]);
//			if (arr.Length != 4)
//				continue;
//
//			string props = arr [0] + " " + arr [1] + " " + arr [2];
//			string _class = arr [3];
//			Document doc = new Document (_class, props);
//
//			trainData.Add (doc);
//
//			text = reader.ReadLine ();
//		}
//		return trainData;
//	}
//
//	//	Helpers
//	static string handleDistance(string dis)
//	{
//		int i;
//		for (i = 0; i < distanceRanges.Count; i++) {
//			if (distanceRanges [i] > Convert.ToInt32(dis))
//				break;
//		}
//		print ("Distance_Train: " + i);
//		return i.ToString();
//	}
//
//	static string handleSpeed(string speed)
//	{
//		int i;
//		for (i = 0; i < speedRanges.Count; i++) {
//			if (speedRanges [i] > Convert.ToInt32(speed))
//				break;
//		}
//		print ("Speed_Train: " + i);
//		return i.ToString();
//	}
//
//	// 
//	class Document
//	{
//		public Document(string @class, string text)
//		{
//			Class = @class;
//			Text = text;
//		}
//		public string Class { get; set; }
//		public string Text { get; set; }
//	}
//
//	public static class Helpers
//	{
//		public static List<String> ExtractFeatures(String text)
//		{
//			return Regex.Replace(text, "\\p{P}+", "").Split(' ').ToList();
//		}
//	}
//
//	class ClassInfo
//	{
//		public ClassInfo(string name, List<String> trainDocs)
//		{
//			Name = name;
//			var features = trainDocs.SelectMany(x => Helpers.ExtractFeatures(x));
//			WordsCount = features.Count();
//			WordCount = 
//				features.GroupBy(x=>x)
//					.ToDictionary(x=>x.Key, x=>x.Count());
//			NumberOfDocs = trainDocs.Count;
//		}
//		public string Name { get; set; }
//		public int WordsCount { get; set; }
//		public Dictionary<string, int> WordCount { get; set; }
//		public int NumberOfDocs { get; set; }
//		public int NumberOfOccurencesInTrainDocs(String word)
//		{
//			if (WordCount.Keys.Contains(word)) return WordCount[word];
//			return 0;
//		}
//	}
//
//	class Classifier
//	{
//		List<ClassInfo> _classes;
//		int _countOfDocs;
//		int _uniqWordsCount;
//		public Classifier(List<Document> train)
//		{
//			_classes = train.GroupBy(x => x.Class).Select(g => new ClassInfo(g.Key, g.Select(x=>x.Text).ToList())).ToList();
//			_countOfDocs = train.Count;
//			_uniqWordsCount = train.SelectMany(x=>x.Text.Split(' ')).GroupBy(x=>x).Count();
//		}
//
//		public double IsInClassProbability(string className, string text)
//		{
//			var words = Helpers.ExtractFeatures(text);
//			var classResults = _classes
//				.Select(x => new
//					{
//						Result = Math.Pow(Math.E, Calc(x.NumberOfDocs, _countOfDocs, words, x.WordsCount, x, _uniqWordsCount)),
//						ClassName = x.Name
//					});
//
//
//			return classResults.Single(x=>x.ClassName == className).Result / classResults.Sum(x=>x.Result);
//		}
//
//		private static double Calc(double dc, double d, List<String> q, double lc, ClassInfo @class, double v)
//		{
//			return Math.Log(dc / d) + q.Sum(x =>Math.Log((@class.NumberOfOccurencesInTrainDocs(x) + 1) / (v + lc))); 
//		}
//	}
//}
