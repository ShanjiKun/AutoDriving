using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace AssemblyCSharp
{
	public class CustomNaiveBayes
	{
		//	Define List properties and BClass
		public List<List<Properties>> properties = new List<List<Properties>>();
		public List<BClass> bclasses = new List<BClass> ();
		private int numberOfClass;
		private int numberOfElemInLine;

		//	Setup Single Ton for dicision1
		static CustomNaiveBayes decision1;
		public static CustomNaiveBayes sharedDecision1()
		{
			if (decision1 == null) {
				decision1 = new CustomNaiveBayes ();
			}
			return decision1;
		}

		//	Setup Single Ton for dicision2
		static CustomNaiveBayes decision2;
		public static CustomNaiveBayes sharedDecision2()
		{
			if (decision2 == null) {
				decision2 = new CustomNaiveBayes ();
			}
			return decision2;
		}

		//	Setup Single Ton for dicision3
		static CustomNaiveBayes decision3;
		public static CustomNaiveBayes sharedDecision3()
		{
			if (decision3 == null) {
				decision3 = new CustomNaiveBayes ();
			}
			return decision3;
		}

		//	Get decision with test data
		//	Format test string "prop1,prop2,prop3"
		public string getDecisionWithTest(string testString)
		{
			//	Seperate testString
			string[] arrTest = testString.Split (","[0]);

			//	Check Number of prop.
			if(arrTest.Length != properties.Count) return "";

			//	Generate decision
			string decision = "";
			float PMax = 0;
			foreach (BClass bclass in bclasses) {
				
				float P = bclass.P;

				for (int i=0; i<properties.Count; i++) {
					
					List<Properties> childProps = properties[i];

					int j;
					for (j=0; j<childProps.Count; j++) {
						
						Properties prop = childProps[j];
						if (prop.name == arrTest [i]) {
							P *= prop.getPOfClass (bclass.name);
							break;
						}
					}

					//	Error prop.
					if (j == childProps.Count)
						return "Error prop";
				}

				if (P > PMax) {
					PMax = P;
					decision = bclass.name;
				}
			}

			return decision;
		}

		//	Train data handle
		//	True: success training
		//	False: fail training
		public bool trainSystemWithData(List<string> data)
		{
			bool isSuccess = generateData (data);
			if (!isSuccess)
				return false;

			//	Calculate P class
			calculatePOfClass ();

			//	Calculate P properties
			bool isLaplace = calculatePOfProp ();

			//	Apply Laplace
			if(isLaplace){
				applyLaplace ();
			}

			return true;
		}

		bool generateData(List<string> data)
		{

			if (data.Count == 0)
				return false;

			//	Generate numberOfElemInLine to check correct of data
			numberOfElemInLine = data[0].Split(","[0]).Length;
			if (numberOfElemInLine <= 1)
				return false; 

			//	Separate properties and class
			for(int i=0; i<data.Count; i++) {
				//	Get a line
				string line = data [i];

				//	Separate line to elements
				string[] elems = data [i].Split(","[0]);

				//	Check a valid line
				//	If elems contain just one element or length of lines isn't same, it's an error line.
				if(elems.Length <= 1 || elems.Length != numberOfElemInLine) {
					return false;
				}

				//	Define postition of class at last index.
				string @class = elems[elems.Length-1];

				//	Add class to list
				addClass(@class);
				numberOfClass += 1;

				//	Add prop to list
				//	elems.Length-1 because last index is class
				for(int index = 0; index < elems.Length-1; index++){
					string prop = elems [index];
					addProperties (prop, @class, index);
				}
			}
			return true;
		}

		//	
		void calculatePOfClass()
		{
			for(int i=0; i<bclasses.Count; i++){
				BClass bclass = bclasses [i];
				bclass.calculateP (numberOfClass);
			}
		}

		bool calculatePOfProp()
		{
			bool isLaplace = false;
			for(int i=0; i<properties.Count; i++){
				List<Properties> childProp = properties [i];
				foreach (Properties prop in childProp) {
					foreach(BClass bclass in bclasses) {
						int sumOfPInClass = calculateSumOfPInClass (bclass.name, childProp);
						bool temp = prop.calculateP (bclass, sumOfPInClass);
						isLaplace = isLaplace ? true : temp;
					}
				}
			}

			return isLaplace;
		}

		void applyLaplace()
		{
			//	Hanle class
			numberOfClass += bclasses.Count;
			foreach (BClass bclass in bclasses) {
				bclass.count += 1;
				bclass.calculateP (numberOfClass);
			}

			//	Handle Prop
			for(int i=0; i<properties.Count; i++){
				List<Properties> childProp = properties [i];
				foreach (Properties prop in childProp) {
					foreach(BClass bclass in bclasses) {
						if (!prop.dictClass.ContainsKey (bclass.name)) {
							prop.dictClass.Add (bclass.name, 1);	
						} else {
							int count = prop.dictClass [bclass.name];
							count += 1;
							prop.dictClass [bclass.name] = count;
						}
					}
				}
			}

			for(int i=0; i<properties.Count; i++){
				List<Properties> childProp = properties [i];
				foreach (Properties prop in childProp) {
					prop.clearP ();
					foreach(BClass bclass in bclasses) {
						int sumOfPInClass = calculateSumOfPInClass (bclass.name, childProp);
						prop.calculateP (bclass, sumOfPInClass);
					}
				}
			}

		}

		int calculateSumOfPInClass(string @class, List<Properties> childProps)
		{
			int sum = 0;
			foreach (Properties prop in childProps) {
				if (prop.dictClass.ContainsKey (@class)) {
					int count = prop.dictClass [@class];
					sum += count;
				}
			}

			return sum;
		}

		void addClass(string @class)
		{
			//	Check exist class
			int i;
			for (i = 0; i < bclasses.Count; i++) {
				BClass existClass = bclasses [i];
				if (existClass.name == @class) {
					existClass.count+=1;
					break;
				}
			}

			//	Check to add new class
			if (i == bclasses.Count) {
				BClass newClass = new BClass (@class);
				bclasses.Add (newClass);
			}
		}

		void addProperties(string prop, string @class, int index)
		{
			//	Initial new Child Prop
			if (properties.Count == index) {
				List<Properties> newChildProp = new List<Properties> {
					new Properties (prop, @class),
				};
				properties.Add (newChildProp);
				return;
			}

			//	Check exist prop
			List<Properties> childProp = properties[index];
			int i;
			for (i = 0; i < childProp.Count; i++) {
				Properties existProp = childProp[i];
				if(existProp.name == prop){
					existProp.addPropPerClass (@class);
					break;
				}
			}

			//	Check add new prop
			if(i == childProp.Count){
				Properties newProp = new Properties (prop, @class);
				childProp.Add (newProp);
			}
		}

		List<string> getTempData()
		{
			return new List<string> {
				"P1_1,P2_1,P3_1,C1",
				"P1_1,P2_1,P3_1,C1",
				"P1_2,P2_2,P3_1,C2",
				"P1_2,P2_2,P3_1,C2",
				"P1_3,P2_3,P3_2,C1",
			};
		}

		List<string> getListStringFromFile (string path) {
			List<string> trainData = new List<string> ();

			StreamReader reader =  new StreamReader(path);
			string text = reader.ReadLine ();
			while (text != null) {
				trainData.Add (text);
				text = reader.ReadLine ();
			}
			return trainData;
		}
	}

	public class Properties {
		public string name { get; set ;}
		public Dictionary<string, int> dictClass = new Dictionary<string, int>();		// Key: className, value: count(number of prop in this class)
		public Dictionary<string, float> dictPClass = new Dictionary<string, float>(); 	// Key: className, value: P(P of prop in this class)
		public int count { get; set;}

		public Properties(string name, string @class)
		{
			this.name = name;
			this.count = 1;
			this.dictClass.Add (@class, 1);
		}

		public void addPropPerClass(string @class)
		{
			//	Increment count
			this.count += 1;
			
			//	Add class
			if (!dictClass.ContainsKey(@class)) {
				this.dictClass.Add (@class, 1);
			} else {
				int num = dictClass[@class];
				num += 1;
				dictClass [@class] = num;
			}
		}

		//	True: for normal
		//	False: for need to use Laplace
		public bool calculateP(BClass bclass, int sumOfPInClass)
		{
			if (dictClass.ContainsKey (bclass.name)) {
				int num = dictClass [bclass.name];
				float P = (float)num / (float)sumOfPInClass;
				dictPClass.Add (bclass.name, P);
				return true;
			} else {
				dictPClass.Add (bclass.name, 0f);
				return false;
			}
		}

		public void clearP()
		{
			dictPClass.Clear ();
		}

		public float getPOfClass(string @class)
		{
			return dictPClass [@class];
		}
	}

	public class BClass {
		public string name { get; set;}
		public int count { get; set;}
		public float P { get; set;}

		public BClass(string name)
		{
			this.name = name;
			count = 1;
		}

		public void calculateP (int sum)
		{
			this.P = (float)count / (float)sum;
		}
	}
}

