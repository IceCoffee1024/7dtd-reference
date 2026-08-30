using System;
using UnityEngine.Scripting;

// Token: 0x0200080E RID: 2062
[Preserve]
public class NetPackageLockResponse : NetPackage
{
	// Token: 0x17000654 RID: 1620
	// (get) Token: 0x06003CBF RID: 15551 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003CC0 RID: 15552 RVA: 0x001834D8 File Offset: 0x001816D8
	public unsafe NetPackageLockResponse Setup(bool _success, string _errorMsg, ReadOnlySpan<ILockTarget> _targets, ILockContext _context, ushort _channel = 0)
	{
		if (_targets == null || _targets.Length == 0)
		{
			Log.Error("[NetPackageLockResponse] No lock targets supplied.");
			return null;
		}
		this.locking = true;
		this.success = _success;
		this.errorMsg = _errorMsg ?? string.Empty;
		this.targets = new ILockTarget[_targets.Length];
		for (int i = 0; i < _targets.Length; i++)
		{
			this.targets[i] = *_targets[i];
		}
		this.context = _context;
		this.channel = _channel;
		return this;
	}

	// Token: 0x06003CC1 RID: 15553 RVA: 0x0018356C File Offset: 0x0018176C
	public NetPackageLockResponse Setup(bool _success, string _errorMsg, bool _isForceUnlocked)
	{
		this.locking = false;
		this.success = _success;
		this.errorMsg = _errorMsg ?? string.Empty;
		this.isForceUnlocked = _isForceUnlocked;
		this.targets = null;
		this.context = null;
		this.channel = 0;
		return this;
	}

	// Token: 0x06003CC2 RID: 15554 RVA: 0x001835AC File Offset: 0x001817AC
	public override void read(PooledBinaryReader _br)
	{
		this.locking = _br.ReadBoolean();
		this.success = _br.ReadBoolean();
		this.errorMsg = _br.ReadString();
		this.isForceUnlocked = _br.ReadBoolean();
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
			Log.Error("[NetPackageLockResponse] Unknown context type: " + text);
			this.context = null;
			return;
		}
		this.context = (ILockContext)Activator.CreateInstance(type);
		this.context.Read(_br);
	}

	// Token: 0x06003CC3 RID: 15555 RVA: 0x00183684 File Offset: 0x00181884
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.locking);
		_bw.Write(this.success);
		_bw.Write(this.errorMsg);
		_bw.Write(this.isForceUnlocked);
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

	// Token: 0x06003CC4 RID: 15556 RVA: 0x00183748 File Offset: 0x00181948
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!this.locking)
		{
			LockManager.Instance.UnlockResponse(this.success, this.errorMsg, this.isForceUnlocked);
			return;
		}
		LockManager.Instance.LockResponse(this.success, this.errorMsg, this.targets.AsSpan<ILockTarget>(), this.context, this.channel);
	}

	// Token: 0x06003CC5 RID: 15557 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x0400314A RID: 12618
	[PublicizedFrom(EAccessModifier.Private)]
	public bool locking;

	// Token: 0x0400314B RID: 12619
	[PublicizedFrom(EAccessModifier.Private)]
	public bool success;

	// Token: 0x0400314C RID: 12620
	[PublicizedFrom(EAccessModifier.Private)]
	public string errorMsg = string.Empty;

	// Token: 0x0400314D RID: 12621
	[PublicizedFrom(EAccessModifier.Private)]
	public ILockTarget[] targets;

	// Token: 0x0400314E RID: 12622
	[PublicizedFrom(EAccessModifier.Private)]
	public ILockContext context;

	// Token: 0x0400314F RID: 12623
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort channel;

	// Token: 0x04003150 RID: 12624
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isForceUnlocked;
}
