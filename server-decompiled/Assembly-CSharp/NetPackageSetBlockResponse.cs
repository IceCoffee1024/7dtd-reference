using System;
using UnityEngine.Scripting;

// Token: 0x02000852 RID: 2130
[Preserve]
public class NetPackageSetBlockResponse : NetPackage
{
	// Token: 0x06003DDF RID: 15839 RVA: 0x00189E4F File Offset: 0x0018804F
	public NetPackageSetBlockResponse Setup(eSetBlockResponse _response)
	{
		this.response = _response;
		return this;
	}

	// Token: 0x06003DE0 RID: 15840 RVA: 0x00189E59 File Offset: 0x00188059
	public override void read(PooledBinaryReader _reader)
	{
		this.response = (eSetBlockResponse)_reader.ReadUInt16();
	}

	// Token: 0x06003DE1 RID: 15841 RVA: 0x00189E67 File Offset: 0x00188067
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((ushort)this.response);
	}

	// Token: 0x06003DE2 RID: 15842 RVA: 0x00189E80 File Offset: 0x00188080
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

	// Token: 0x04003268 RID: 12904
	public eSetBlockResponse response;
}
