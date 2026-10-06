using System;
using UnityEngine.Scripting;

// Token: 0x02000852 RID: 2130
[Preserve]
public class NetPackageSingleSignData : NetPackage
{
	// Token: 0x06003DF2 RID: 15858 RVA: 0x00189C27 File Offset: 0x00187E27
	public NetPackageSingleSignData Setup(string _libraryId, byte[] _signBlob)
	{
		this.libraryId = _libraryId;
		this.signBlob = _signBlob;
		return this;
	}

	// Token: 0x06003DF3 RID: 15859 RVA: 0x00189C38 File Offset: 0x00187E38
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

	// Token: 0x06003DF4 RID: 15860 RVA: 0x00189C84 File Offset: 0x00187E84
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

	// Token: 0x06003DF5 RID: 15861 RVA: 0x00189CD0 File Offset: 0x00187ED0
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

	// Token: 0x0400326A RID: 12906
	[PublicizedFrom(EAccessModifier.Private)]
	public string libraryId;

	// Token: 0x0400326B RID: 12907
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] signBlob;
}
