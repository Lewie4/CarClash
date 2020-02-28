// // // //  ** ** ** ** ** ** ** // // // //  ** ** ** ** ** ** ** // // // // ** ** ** ** ** ** **
// * Copyright 2015  All Rights Reserved.
// *
// * Please direct any bugs or questions to vadakuma@gmail.com
// *
// * author vadakuma
// // // //  ** ** ** ** ** ** ** // // // //  ** ** ** ** ** ** ** // // // // ** ** ** ** ** ** **

using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(TransformRandomizer))]
[CanEditMultipleObjects]
[System.Serializable]
public class RandomizerEditor : Editor {

	TransformRandomizer 	myScript;

	SerializedObject 		serObj;
	SerializedProperty 		search;
	SerializedProperty 		objectsName;
	SerializedProperty 		randAtStart;
	SerializedProperty		randScale;
	SerializedProperty		randRot;
	SerializedProperty		randPos;
	SerializedProperty 		rotSpace;
	SerializedProperty 		posSpace;
	SerializedProperty 		minRot;
	SerializedProperty 		maxRot;
	SerializedProperty 		minPos;
	SerializedProperty 		maxPos;
	SerializedProperty 		minScale;
	SerializedProperty 		maxScale;
	SerializedProperty  	colorName;
	SerializedProperty 		minColor;
	SerializedProperty 		maxColor;
	SerializedProperty 		Palitra;
	
	void OnEnable() {

	}

	public override void OnInspectorGUI()
	{
		serObj = new SerializedObject (target);
		myScript = (TransformRandomizer)target;

		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("Previosly set search method and Tag or objects Name.", MessageType.None);
		EditorGUILayout.Space();

		// Main Settings
		search = serObj.FindProperty("search");
		EditorGUILayout.PropertyField (search, new GUIContent("Search method"));


		if(myScript.search == TransformRandomizer.SearchMethod.SearchByTag)
		{
			myScript.objectsTag = EditorGUILayout.TagField("Tag for Objects:", myScript.objectsTag);
		}
		else
		{
			objectsName = serObj.FindProperty("objectsName");
			EditorGUILayout.PropertyField (objectsName, new GUIContent("Name objects:"));
		}
		randAtStart = serObj.FindProperty("randAtStart");
		EditorGUILayout.PropertyField (randAtStart, new GUIContent("Generate at game start"));

		EditorGUILayout.Space();
		if (GUILayout.Button("Generate for All"))
		{
			myScript.RandomGenerate(0);
		}
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		// Scale
		myScript.randScale = EditorGUILayout.BeginToggleGroup ("Scale Randomizer", myScript.randScale);
		if(myScript.randScale)
		{
			minScale = serObj.FindProperty("minScale");
			EditorGUILayout.PropertyField (minScale, new GUIContent("Min Scale"));
			maxScale = serObj.FindProperty("maxScale");
			EditorGUILayout.PropertyField (maxScale, new GUIContent("Max Scale"));
			if (GUILayout.Button("Generate New Scale"))
			{
				myScript.RandomGenerate(1);
			}
			EditorGUILayout.Space();
		}
		EditorGUILayout.EndToggleGroup ();
		// Rotation
		myScript.randRot = EditorGUILayout.BeginToggleGroup ("Rotation Randomizer", myScript.randRot);
		if(myScript.randRot)
		{
			rotSpace = serObj.FindProperty("rotSpace");
			EditorGUILayout.PropertyField (rotSpace, new GUIContent("Rotation Space"));
			minRot = serObj.FindProperty("minRot");
			EditorGUILayout.PropertyField (minRot, new GUIContent("Min Rotation"));
			maxRot = serObj.FindProperty("maxRot");
			EditorGUILayout.PropertyField (maxRot, new GUIContent("Max Rotation"));
			if (GUILayout.Button("Generate New Rotation"))
			{
				myScript.RandomGenerate(2);
			}
			EditorGUILayout.Space();
		}
		EditorGUILayout.EndToggleGroup ();

		// Position
		myScript.randPos = EditorGUILayout.BeginToggleGroup ("Position Randomizer", myScript.randPos);
		if(myScript.randPos)
		{
			posSpace = serObj.FindProperty("posSpace");
			EditorGUILayout.PropertyField (posSpace, new GUIContent("Position Space"));
			minPos = serObj.FindProperty("minPos");
			EditorGUILayout.PropertyField (minPos, new GUIContent("Min Position"));
			maxPos = serObj.FindProperty("maxPos");
			EditorGUILayout.PropertyField (maxPos, new GUIContent("Max Position"));
			if (GUILayout.Button("Generate New Position"))
			{
				myScript.RandomGenerate(3);
			}
			EditorGUILayout.Space();
		}
		EditorGUILayout.EndToggleGroup ();

		// Color
		myScript.randColor = EditorGUILayout.BeginToggleGroup ("Color Randomizer(WIP)", myScript.randColor);
		if(myScript.randColor)
		{
			EditorGUILayout.HelpBox("Common color names used by Unity's builtin shaders:" + "\n" + 
			                        @"""_Color"" is the main color." + "\n" + 
			                        @"""_SpecColor"" is the specular color." + "\n" + 
			                        @"""_Emission"" is the emissive color of a material." + "\n" + 
			                        @"""_ReflectColor"" is the reflection color of the material.",
			                        MessageType.Info);
			colorName = serObj.FindProperty("colorName");
			EditorGUILayout.PropertyField (colorName, new GUIContent("Color Name"));
			minColor = serObj.FindProperty("minColor");
			EditorGUILayout.PropertyField (minColor, new GUIContent("Min Color"));
			maxColor = serObj.FindProperty("maxColor");
			EditorGUILayout.PropertyField (maxColor, new GUIContent("Max Color"));

			EditorGUI.indentLevel += 1;
			myScript.randColorFromPalitra = EditorGUILayout.BeginToggleGroup ("Use Colors Picker", myScript.randColorFromPalitra);
			if(myScript.randColorFromPalitra)
			{
				EditorGUILayout.HelpBox("Randomly selecting color from array for each object", MessageType.Info);
				EditorGUI.indentLevel += 1;
				Palitra = serObj.FindProperty("Palitra");
				EditorGUILayout.PropertyField (Palitra, new GUIContent("Colors Picker"), true);
				EditorGUI.indentLevel -= 1;
			}
			EditorGUILayout.EndToggleGroup ();
			EditorGUI.indentLevel -= 1;

			if (GUILayout.Button("Generate New Color"))
			{
				myScript.RandomGenerate(4);
			}
			EditorGUILayout.Space();
		}
		EditorGUILayout.EndToggleGroup ();

		if (GUI.changed)
			serObj.ApplyModifiedProperties();
	}
}
