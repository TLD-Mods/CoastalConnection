using UnityEngine;
using MelonLoader;

namespace CoastalConnection;

public class CoastalConnectionMod : MelonMod
{
	public override void OnSceneWasLoaded(int buildIndex, string sceneName)
	{
		switch (sceneName)
		{
			case "CoastalRegion":
				InstantiateBundle("coastal_highway.bundle");
				break;
			case "CanneryRegion":
				InstantiateBundle("bleak_inlet.bundle");
				break;
		}
	}

	private static void InstantiateBundle(string fileName)
	{
		byte[] data = EmbeddedFiles.Load(fileName);

		string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), fileName);

		using (System.IO.FileStream fs = System.IO.File.Create(tempPath))
		{
			fs.Write(data, 0, data.Length);
		}

		AssetBundle bundle = AssetBundle.LoadFromFile(tempPath);

		try { System.IO.File.Delete(tempPath); } catch { }

		foreach (UnityEngine.Object asset in bundle.LoadAllAssets())
		{
			UnityEngine.Object.Instantiate(asset);
		}

		bundle.Unload(true);
	}
}
