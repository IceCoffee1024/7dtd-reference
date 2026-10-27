using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001242 RID: 4674
public class AssetMappings
{
	// Token: 0x170011E7 RID: 4583
	// (get) Token: 0x060094D8 RID: 38104 RVA: 0x00386008 File Offset: 0x00384208
	public int Count
	{
		get
		{
			return this.list.Count;
		}
	}

	// Token: 0x060094D9 RID: 38105 RVA: 0x00386015 File Offset: 0x00384215
	public void Add(string name, string address)
	{
		this.list.Add(new AssetMappings.AssetAddress
		{
			name = name,
			address = address
		});
	}

	// Token: 0x060094DA RID: 38106 RVA: 0x00386038 File Offset: 0x00384238
	public Dictionary<string, string> ToDictionary()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (AssetMappings.AssetAddress assetAddress in this.list)
		{
			dictionary.Add(assetAddress.name, assetAddress.address);
		}
		return dictionary;
	}

	// Token: 0x04006F8C RID: 28556
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AssetMappings.AssetAddress> list = new List<AssetMappings.AssetAddress>();

	// Token: 0x02001243 RID: 4675
	[Serializable]
	public class AssetAddress
	{
		// Token: 0x04006F8D RID: 28557
		public string name;

		// Token: 0x04006F8E RID: 28558
		public string address;
	}
}
