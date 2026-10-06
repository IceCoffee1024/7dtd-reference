using System;
using UnityEngine.Scripting;

// Token: 0x02000848 RID: 2120
[Preserve]
public class NetPackageSetBlockResponse : NetPackage
{
	// Token: 0x06003DBC RID: 15804 RVA: 0x00188DA8 File Offset: 0x00186FA8
	public NetPackageSetBlockResponse Setup(eSetBlockResponse _response)
	{
		this.response = _response;
		return this;
	}

	// Token: 0x06003DBD RID: 15805 RVA: 0x00188DB2 File Offset: 0x00186FB2
	public override void read(PooledBinaryReader _reader)
	{
		this.response = (eSetBlockResponse)_reader.ReadUInt16();
	}

	// Token: 0x06003DBE RID: 15806 RVA: 0x00188DC0 File Offset: 0x00186FC0
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((ushort)this.response);
	}

	// Token: 0x06003DBF RID: 15807 RVA: 0x00188DD8 File Offset: 0x00186FD8
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

	// Token: 0x04003248 RID: 12872
	public eSetBlockResponse response;
}
