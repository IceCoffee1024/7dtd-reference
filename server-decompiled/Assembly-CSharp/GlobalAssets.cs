using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02001217 RID: 4631
public static class GlobalAssets
{
	// Token: 0x060093F1 RID: 37873 RVA: 0x0037F35D File Offset: 0x0037D55D
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, string> LoadShaderMappings()
	{
		return JsonUtility.FromJson<AssetMappings>(File.ReadAllText(Path.Combine(Addressables.RuntimePath, "shaders.json"))).ToDictionary();
	}

	// Token: 0x060093F2 RID: 37874 RVA: 0x0037F380 File Offset: 0x0037D580
	public static Shader FindShader(string name)
	{
		if (GlobalAssets.shaders == null)
		{
			GlobalAssets.shaders = GlobalAssets.LoadShaderMappings();
		}
		string text;
		if (GlobalAssets.shaders.TryGetValue(name, out text))
		{
			return LoadManager.LoadAssetFromAddressables<Shader>(text, null, null, false, true, false).Asset;
		}
		return Shader.Find(name);
	}

	// Token: 0x04006EDF RID: 28383
	public const string ShaderMappingFile = "shaders.json";

	// Token: 0x04006EE0 RID: 28384
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, string> shaders;
}
