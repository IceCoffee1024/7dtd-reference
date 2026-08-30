using System;
using UnityEngine.Scripting;

// Token: 0x02000847 RID: 2119
[Preserve]
public class NetPackageSetBlockResponse : NetPackage
{
	// Token: 0x06003E11 RID: 15889 RVA: 0x00189280 File Offset: 0x00187480
	public NetPackageSetBlockResponse Setup(eSetBlockResponse _response)
	{
		this.response = _response;
		return this;
	}

	// Token: 0x06003E12 RID: 15890 RVA: 0x0018928A File Offset: 0x0018748A
	public override void read(PooledBinaryReader _reader)
	{
		this.response = (eSetBlockResponse)_reader.ReadUInt16();
	}

	// Token: 0x06003E13 RID: 15891 RVA: 0x00189298 File Offset: 0x00187498
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((ushort)this.response);
	}

	// Token: 0x06003E14 RID: 15892 RVA: 0x001892B0 File Offset: 0x001874B0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		eSetBlockResponse eSetBlockResponse = this.response;
		if (eSetBlockResponse == eSetBlockResponse.PowerBlockLimitExceeded)
		{
			GameManager.ShowTooltip(GameManager.Instance.World.GetPrimaryPlayer(), "uicannotaddpowerblock", false, false, 0f);
			return;
		}
		if (eSetBlockResponse != eSetBlockResponse.StorageBlockLimitExceeded)
		{
			return;
		}
		GameManager.ShowTooltip(GameManager.Instance.World.GetPrimaryPlayer(), "uicannotaddstorageblock", false, false, 0f);
	}

	// Token: 0x06003E15 RID: 15893 RVA: 0x00080550 File Offset: 0x0007E750
	public override int GetLength()
	{
		return 4;
	}

	// Token: 0x04003261 RID: 12897
	public eSetBlockResponse response;
}
