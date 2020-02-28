// // // //  ** ** ** ** ** ** ** // // // //  ** ** ** ** ** ** ** // // // // ** ** ** ** ** ** **
// * Copyright 2015  All Rights Reserved.
// *
// * Please direct any bugs or questions to vadakuma@gmail.com
// *
// * author vadakuma
// // // //  ** ** ** ** ** ** ** // // // //  ** ** ** ** ** ** ** // // // // ** ** ** ** ** ** **
using UnityEngine;
using System.Collections;

public class TransformRandomizer : MonoBehaviour {


	public enum SearchMethod
	{
		SearchByTag,
		SearchByName,
	};

	public enum WorldSpace
	{
		Local,
		World,
	};
	// generate new values at start game
	public bool 		randAtStart = false;

	// choose search method 
	public SearchMethod	search;
	public string 		objectsName = "None";
	public string 		objectsTag = "Untagged";

	/** scale randomizer
	 */
	public bool 		randScale = true;
	public Vector3 		minScale = new Vector3(0, 0, 0);
	public Vector3 		maxScale = new Vector3(1, 1, 1);
	/** rotation randomizer
	 */
	public WorldSpace 	rotSpace = WorldSpace.World;
	public bool 		randRot = false;
	public Vector3 		minRot = new Vector3(0, 0, 0);
	public Vector3 		maxRot = new Vector3(360, 360, 360);

	/** position randomizer
	 */
	public WorldSpace 	posSpace = WorldSpace.World;
	public bool 		randPos = false;
	public Vector3 		minPos = new Vector3(-10, -10, -10);
	public Vector3 		maxPos = new Vector3(10, 10, 10);

	/** color randomizer
	 */
	public bool 		randColor = false;
	public bool 		randColorFromPalitra = false;

	/* "_Color" is the main color of a material.
	 * "_SpecColor" is the specular color of a material (used in specular/vertexlit shaders). 
	"_Emission" is the emissive color of a material (used in vertexlit shaders). 
	"_ReflectColor" is the reflection color of the material (used in reflective shaders).*/
	public string 		colorName = "_Color";
	public Color  		minColor = new Color(0,0,0,0);
	public Color  		maxColor = new Color(1,1,1,1);
	public Color[]  	Palitra;

	//
	protected GameObject[] objects;


	// Use this for initialization
	void Start () {
		if(randAtStart)
			RandomGenerate(0);
	}

	/** Call from editor script
	* 0 - all, 1 - scale, 2 - rot, 3 - pos, 4 - color
	*/
	public void RandomGenerate(int rnd)
	{
		objects = new GameObject[0];
		// search all object by ...
		if(search == SearchMethod.SearchByTag)
		{
			try
			{
				objects = GameObject.FindGameObjectsWithTag(objectsTag);
			}
			catch
			{
				Debug.LogWarning("There are no objects with Tag: " + objectsTag);
			}
		}
		else if(search == SearchMethod.SearchByName)
		{
			try
			{
				objects = GameObject.FindGameObjectsWithTag(objectsName);
			}
			catch
			{
				Debug.LogWarning("There are no objects with Name: " + objectsName);
			}
		}

		if(objects == null || objects.Length < 1)
		{
			Debug.LogWarning("There are no search results.");
			return;
		}

		switch(rnd)
		{
			default:
			case 0:
				// set new params for all found objects
				if(randScale)
					RandomScale();
				
				if(randRot)
					RandomRot();
				
				if(randPos)
					RandomPos();
				
				if(randColor)
				{
					if(!randColorFromPalitra)
						RandomColor();
					else
						RandomColorFromPalitra();
				}
			break;
			case 1:
				if(randScale)
					RandomScale();
			break;
			case 2:
				if(randRot)
					RandomRot();
			break;
			case 3:
				if(randPos)
					RandomPos();
			break;
			case 4:
				if(randColor)
				{
					if(!randColorFromPalitra)
						RandomColor();
					else
						RandomColorFromPalitra();
				}
			break;
		}
		// free memory array
		objects = new GameObject[0];
	}


	/** Set random range scale to all object*/
	protected void RandomScale()
	{
		int len = objects.Length;
		for(int idx = 0; idx < len; ++idx)
		{
			Vector3 newScale = new Vector3(Random.Range(minScale.x, maxScale.x),
			                               Random.Range(minScale.y, maxScale.y),
			                               Random.Range(minScale.z, maxScale.z));
			objects[idx].transform.localScale = newScale;
		}
		Debug.Log(len + " objects was scaled.");
	}

	/*** Set random range rotatio to all object */
	protected void RandomRot()
	{
		int len = objects.Length;
		for(int idx = 0; idx < len; ++idx)
		{
			Quaternion rot = Quaternion.Euler(Random.Range(minRot.x, maxRot.x),
			                                  Random.Range(minRot.y, maxRot.y),
			                                  Random.Range(minRot.z, maxRot.z));

			if(rotSpace == WorldSpace.World)
				objects[idx].transform.rotation = rot;
			else
				objects[idx].transform.localRotation = rot;
		}
		Debug.Log(len + " objects was rotated.");
	}

	/* Set random range position to all object*/
	protected void RandomPos()
	{
		int len = objects.Length;
		for(int idx=0; idx < len; ++idx)
		{
			Vector3 pos = new Vector3(Random.Range(minPos.x, maxPos.x),
			                                  Random.Range(minPos.y, maxPos.y),
			                                  Random.Range(minPos.z, maxPos.z));
			if(posSpace == WorldSpace.World)
				objects[idx].transform.position = pos;
			else
				objects[idx].transform.localPosition = pos;
		}
		Debug.Log(len + " objects was dislocated.");
	}

	/* Set random range color to all materials of object*/
	protected void RandomColor()
	{
		int len = objects.Length;
		for(int idx=0; idx < len; ++idx)
		{
			Material[] mat = objects[idx].GetComponent<Renderer>().materials;
			//Material[] mat = objects[idx].GetComponent<Renderer>().sharedMaterials;
			if(mat != null)
			{
				int len1 = mat.Length;
				for(int jdx=0; jdx < len1; ++jdx)
				{
					Color newColor = new Color( Random.Range(minColor.r, maxColor.r),
					                            Random.Range(minColor.g, maxColor.g),
					                            Random.Range(minColor.b, maxColor.b),
					                            Random.Range(minColor.a, maxColor.a));
					try
					{
						mat[jdx].SetColor(colorName, newColor);
					}
					catch
					{
						Debug.LogWarning(mat[jdx] + " material was not colored.");
					}
				}
			}
			else
			{
				Debug.LogWarning("There are no materials on " + objects[idx] + ".");
			}
		}
		Debug.Log(objects.Length + " materials was colored.");
	}

	/* Set random range color from Palitra to all materials of object*/
	protected void RandomColorFromPalitra()
	{
		if(Palitra == null || Palitra.Length < 1)
		{
			Debug.LogWarning("There are no color in the array!");
			return;
		}
		int len = objects.Length;
		for(int idx=0; idx < len; ++idx)
		{
			Material[] mat = objects[idx].GetComponent<Renderer>().sharedMaterials;
			if(mat != null)
			{
				int len1 = mat.Length;
				for(int jdx=0; jdx < len1; ++jdx)
				{
					int index = Random.Range(0, Palitra.Length);
					Color newColor = Palitra[index];
					try
					{
						mat[jdx].SetColor(colorName, newColor);
					}
					catch
					{
						Debug.LogWarning(mat[jdx] + " material was not colored.");
					}
				}
			}
			else
			{
				Debug.LogWarning("There are no materials on " + objects[idx] + ".");
			}
		}
		Debug.Log(objects.Length + " materials was colored.");
	}


	// Update is called once per frame
	void Update () {
	
	}
}
