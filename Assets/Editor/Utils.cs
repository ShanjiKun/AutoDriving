using UnityEngine;
using UnityEditor;
namespace Editor
{
	public class Utils
	{
		public static string chooseFile()
		{
			string path = EditorUtility.OpenFilePanel("Choose training data:", "", "txt");
			if (path.Length != 0) {
				return path;
			}
			return "";
		}

		public static void showAlert(string title, string message)
		{
			EditorUtility.DisplayDialog (title,
				message, "OK", "Cancel");
		}

		public static bool showAlertTraining()
		{
			return EditorUtility.DisplayDialog ("Training data success!",
				"Press Run button to start.", "RUN", "Cancel");
		}
	}
}