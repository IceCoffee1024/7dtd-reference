using System;
using UnityEngine.Scripting;

// Token: 0x0200085C RID: 2140
[Preserve]
public class NetPackageSingleSignData : NetPackage
{
	// Token: 0x06003E15 RID: 15893 RVA: 0x0018ACCF File Offset: 0x00188ECF
	public NetPackageSingleSignData Setup(string _libraryId, byte[] _signBlob)
	{
		this.libraryId = _libraryId;
		this.signBlob = _signBlob;
		return this;
	}

	// Token: 0x06003E16 RID: 15894 RVA: 0x0018ACE0 File Offset: 0x00188EE0
	public override void read(PooledBinaryReader _br)
	{
		this.libraryId = _br.ReadString();
		int num = _br.ReadInt32();
		if (num > 0)
		{
			this.signBlob = new byte[num];
			_br.ReadExact(this.signBlob);
			return;
		}
		this.signBlob = null;
	}

	// Token: 0x06003E17 RID: 15895 RVA: 0x0018AD2C File Offset: 0x00188F2C
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.libraryId);
		int num = ((this.signBlob != null) ? this.signBlob.Length : 0);
		_bw.Write(num);
		if (num > 0)
		{
			_bw.Write(this.signBlob);
		}
	}

	// Token: 0x06003E18 RID: 15896 RVA: 0x0018AD78 File Offset: 0x00188F78
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (this.signBlob == null)
		{
			Log.Warning("[NetPackageSingleSignData] Received an empty sign blob for library '" + this.libraryId + "'; ignoring.");
			return;
		}
		SignDataManager.Instance.ProcessSignReceived(this.libraryId, this.signBlob);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(this, false, -1, base.Sender.entityId, -1, null, 192, false);
		}
	}

	// Token: 0x0400328A RID: 12938
	[PublicizedFrom(EAccessModifier.Private)]
	public string libraryId;

	// Token: 0x0400328B RID: 12939
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] signBlob;
}
