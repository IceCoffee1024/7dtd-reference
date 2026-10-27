using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200083F RID: 2111
[Preserve]
public class NetPackagePOIMetadataResponse : NetPackage
{
	// Token: 0x17000695 RID: 1685
	// (get) Token: 0x06003D84 RID: 15748 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x17000696 RID: 1686
	// (get) Token: 0x06003D85 RID: 15749 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003D86 RID: 15750 RVA: 0x001877A8 File Offset: 0x001859A8
	public NetPackagePOIMetadataResponse Setup(List<PrefabInstance> _pois)
	{
		this.metadatas = new List<PrefabInstance.POIMetadata>(_pois.Count);
		for (int i = 0; i < _pois.Count; i++)
		{
			PrefabInstance prefabInstance = _pois[i];
			if (prefabInstance != null && prefabInstance.prefab != null)
			{
				PrefabInstance.POIMetadata poimetadata = prefabInstance.GetPOIMetadata();
				this.metadatas.Add(poimetadata);
			}
		}
		return this;
	}

	// Token: 0x06003D87 RID: 15751 RVA: 0x00187800 File Offset: 0x00185A00
	public override void read(PooledBinaryReader _br)
	{
		int num = _br.ReadInt32();
		this.metadatas = new List<PrefabInstance.POIMetadata>(num);
		for (int i = 0; i < num; i++)
		{
			this.metadatas.Add(new PrefabInstance.POIMetadata(_br));
		}
	}

	// Token: 0x06003D88 RID: 15752 RVA: 0x00187840 File Offset: 0x00185A40
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		int count = this.metadatas.Count;
		_bw.Write(count);
		for (int i = 0; i < count; i++)
		{
			this.metadatas[i].Write(_bw);
		}
	}

	// Token: 0x06003D89 RID: 15753 RVA: 0x00187885 File Offset: 0x00185A85
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		GameManager.Instance.GetDynamicPrefabDecorator().ProcessPOIMetadataReceived(this.metadatas);
	}

	// Token: 0x040031ED RID: 12781
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PrefabInstance.POIMetadata> metadatas;
}
