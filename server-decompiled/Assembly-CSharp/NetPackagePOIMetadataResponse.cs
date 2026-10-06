using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000835 RID: 2101
[Preserve]
public class NetPackagePOIMetadataResponse : NetPackage
{
	// Token: 0x1700068F RID: 1679
	// (get) Token: 0x06003D61 RID: 15713 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x17000690 RID: 1680
	// (get) Token: 0x06003D62 RID: 15714 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003D63 RID: 15715 RVA: 0x001866A4 File Offset: 0x001848A4
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

	// Token: 0x06003D64 RID: 15716 RVA: 0x001866FC File Offset: 0x001848FC
	public override void read(PooledBinaryReader _br)
	{
		int num = _br.ReadInt32();
		this.metadatas = new List<PrefabInstance.POIMetadata>(num);
		for (int i = 0; i < num; i++)
		{
			this.metadatas.Add(new PrefabInstance.POIMetadata(_br));
		}
	}

	// Token: 0x06003D65 RID: 15717 RVA: 0x0018673C File Offset: 0x0018493C
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

	// Token: 0x06003D66 RID: 15718 RVA: 0x00186781 File Offset: 0x00184981
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		GameManager.Instance.GetDynamicPrefabDecorator().ProcessPOIMetadataReceived(this.metadatas);
	}

	// Token: 0x040031CD RID: 12749
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PrefabInstance.POIMetadata> metadatas;
}
