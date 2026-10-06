using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001230 RID: 4656
public class AssetMappings
{
	// Token: 0x170011DC RID: 4572
	// (get) Token: 0x06009481 RID: 38017 RVA: 0x00383C0C File Offset: 0x00381E0C
	public int Count
	{
		get
		{
			return this.list.Count;
		}
	}

	// Token: 0x06009482 RID: 38018 RVA: 0x00383C19 File Offset: 0x00381E19
	public void Add(string name, string address)
	{
		this.list.Add(new AssetMappings.AssetAddress
		{
			name = name,
			address = address
		});
	}

	// Token: 0x06009483 RID: 38019 RVA: 0x00383C3C File Offset: 0x00381E3C
	public Dictionary<string, string> ToDictionary()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (AssetMappings.AssetAddress assetAddress in this.list)
		{
			dictionary.Add(assetAddress.name, assetAddress.address);
		}
		return dictionary;
	}

	// Token: 0x04006F4B RID: 28491
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AssetMappings.AssetAddress> list = new List<AssetMappings.AssetAddress>();

	// Token: 0x02001231 RID: 4657
	[Serializable]
	public class AssetAddress
	{
		// Token: 0x04006F4C RID: 28492
		public string name;

		// Token: 0x04006F4D RID: 28493
		public string address;
	}
}
