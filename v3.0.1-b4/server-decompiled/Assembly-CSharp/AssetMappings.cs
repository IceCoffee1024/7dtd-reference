using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020011FC RID: 4604
public class AssetMappings
{
	// Token: 0x1700115D RID: 4445
	// (get) Token: 0x060092EF RID: 37615 RVA: 0x0037A0E8 File Offset: 0x003782E8
	public int Count
	{
		get
		{
			return this.list.Count;
		}
	}

	// Token: 0x060092F0 RID: 37616 RVA: 0x0037A0F5 File Offset: 0x003782F5
	public void Add(string name, string address)
	{
		this.list.Add(new AssetMappings.AssetAddress
		{
			name = name,
			address = address
		});
	}

	// Token: 0x060092F1 RID: 37617 RVA: 0x0037A118 File Offset: 0x00378318
	public Dictionary<string, string> ToDictionary()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (AssetMappings.AssetAddress assetAddress in this.list)
		{
			dictionary.Add(assetAddress.name, assetAddress.address);
		}
		return dictionary;
	}

	// Token: 0x04006E08 RID: 28168
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AssetMappings.AssetAddress> list = new List<AssetMappings.AssetAddress>();

	// Token: 0x020011FD RID: 4605
	[Serializable]
	public class AssetAddress
	{
		// Token: 0x04006E09 RID: 28169
		public string name;

		// Token: 0x04006E0A RID: 28170
		public string address;
	}
}
