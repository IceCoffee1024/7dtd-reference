using System;
using UnityEngine.Scripting;

// Token: 0x02000802 RID: 2050
[Preserve]
public class NetPackageLockRequest : NetPackage
{
	// Token: 0x17000648 RID: 1608
	// (get) Token: 0x06003C58 RID: 15448 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003C59 RID: 15449 RVA: 0x00180EB0 File Offset: 0x0017F0B0
	public unsafe NetPackageLockRequest Setup(ReadOnlySpan<ILockTarget> _targets, ILockContext _context, ushort _channel = 0)
	{
		if (_targets == null || _targets.Length == 0)
		{
			Log.Error("[NetPackageLockRequest] No lock targets supplied.");
			return null;
		}
		this.locking = true;
		this.targets = new ILockTarget[_targets.Length];
		for (int i = 0; i < _targets.Length; i++)
		{
			this.targets[i] = *_targets[i];
		}
		this.context = _context;
		this.channel = _channel;
		return this;
	}

	// Token: 0x06003C5A RID: 15450 RVA: 0x00180F2B File Offset: 0x0017F12B
	public NetPackageLockRequest Setup()
	{
		this.locking = false;
		this.targets = null;
		this.context = null;
		this.channel = 0;
		return this;
	}

	// Token: 0x06003C5B RID: 15451 RVA: 0x00180F4C File Offset: 0x0017F14C
	public override void read(PooledBinaryReader _br)
	{
		this.locking = _br.ReadBoolean();
		this.channel = _br.ReadUInt16();
		int num = _br.ReadInt32();
		if (num > 0)
		{
			this.targets = new ILockTarget[num];
			for (int i = 0; i < num; i++)
			{
				this.targets[i] = ILockTarget.ReadIdentifyingInfo(_br);
			}
		}
		string text = _br.ReadString();
		if (string.IsNullOrEmpty(text))
		{
			this.context = null;
			return;
		}
		Type type = Type.GetType(text);
		if (type == null)
		{
			Log.Error("[NetPackageLockRequest] Unknown context type: " + text);
			this.context = null;
			return;
		}
		this.context = (ILockContext)Activator.CreateInstance(type);
		this.context.Read(_br);
	}

	// Token: 0x06003C5C RID: 15452 RVA: 0x00181000 File Offset: 0x0017F200
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.locking);
		_bw.Write(this.channel);
		ILockTarget[] array = this.targets;
		_bw.Write((array != null) ? array.Length : 0);
		if (this.targets != null)
		{
			for (int i = 0; i < this.targets.Length; i++)
			{
				ILockTarget.WriteIdentifyingInfo(this.targets[i], _bw);
			}
		}
		if (this.context == null)
		{
			_bw.Write(string.Empty);
			return;
		}
		_bw.Write(this.context.GetType().FullName);
		this.context.Write(_bw);
	}

	// Token: 0x06003C5D RID: 15453 RVA: 0x001810A0 File Offset: 0x0017F2A0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (this.locking)
		{
			LockManager.Instance.LockRequestServer(this.targets.AsSpan<ILockTarget>(), base.Sender.entityId, this.context, this.channel);
			return;
		}
		LockManager.Instance.UnlockRequestServer(base.Sender.entityId, false);
	}

	// Token: 0x06003C5E RID: 15454 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x040030C7 RID: 12487
	[PublicizedFrom(EAccessModifier.Private)]
	public bool locking;

	// Token: 0x040030C8 RID: 12488
	[PublicizedFrom(EAccessModifier.Private)]
	public ILockTarget[] targets;

	// Token: 0x040030C9 RID: 12489
	[PublicizedFrom(EAccessModifier.Private)]
	public ILockContext context;

	// Token: 0x040030CA RID: 12490
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort channel;
}
