using System;
using UnityEngine.Scripting;

// Token: 0x0200083B RID: 2107
[Preserve]
public class NetPackageSetBlockResponse : NetPackage
{
	// Token: 0x06003DAC RID: 15788 RVA: 0x00187260 File Offset: 0x00185460
	public NetPackageSetBlockResponse Setup(eSetBlockResponse _response)
	{
		this.response = _response;
		return this;
	}

	// Token: 0x06003DAD RID: 15789 RVA: 0x0018726A File Offset: 0x0018546A
	public override void read(PooledBinaryReader _reader)
	{
		this.response = (eSetBlockResponse)_reader.ReadUInt16();
	}

	// Token: 0x06003DAE RID: 15790 RVA: 0x00187278 File Offset: 0x00185478
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((ushort)this.response);
	}

	// Token: 0x06003DAF RID: 15791 RVA: 0x00187290 File Offset: 0x00185490
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

	// Token: 0x06003DB0 RID: 15792 RVA: 0x0007FE9C File Offset: 0x0007E09C
	public override int GetLength()
	{
		return 4;
	}

	// Token: 0x040031E2 RID: 12770
	public eSetBlockResponse response;
}
