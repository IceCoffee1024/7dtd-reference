using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000834 RID: 2100
[Preserve]
public class NetPackagePOIMetadataResponse : NetPackage
{
	// Token: 0x17000670 RID: 1648
	// (get) Token: 0x06003DA9 RID: 15785 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x17000671 RID: 1649
	// (get) Token: 0x06003DAA RID: 15786 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003DAB RID: 15787 RVA: 0x00186B6C File Offset: 0x00184D6C
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

	// Token: 0x06003DAC RID: 15788 RVA: 0x00186BC4 File Offset: 0x00184DC4
	public override void read(PooledBinaryReader _br)
	{
		int num = _br.ReadInt32();
		this.metadatas = new List<PrefabInstance.POIMetadata>(num);
		for (int i = 0; i < num; i++)
		{
			this.metadatas.Add(new PrefabInstance.POIMetadata(_br));
		}
	}

	// Token: 0x06003DAD RID: 15789 RVA: 0x00186C04 File Offset: 0x00184E04
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

	// Token: 0x06003DAE RID: 15790 RVA: 0x00186C49 File Offset: 0x00184E49
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		GameManager.Instance.GetDynamicPrefabDecorator().ProcessPOIMetadataReceived(this.metadatas);
	}

	// Token: 0x06003DAF RID: 15791 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x040031E6 RID: 12774
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PrefabInstance.POIMetadata> metadatas;
}
