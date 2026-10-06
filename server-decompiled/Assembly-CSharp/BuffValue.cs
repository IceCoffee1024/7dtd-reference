using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000656 RID: 1622
public class BuffValue
{
	// Token: 0x17000572 RID: 1394
	// (get) Token: 0x060034AC RID: 13484 RVA: 0x0015CC04 File Offset: 0x0015AE04
	public BuffClass BuffClass
	{
		get
		{
			if (this.cachedBuff == null && !BuffManager.Buffs.TryGetValue(this.buffName, out this.cachedBuff))
			{
				Log.Error("Buff Class not found for '{0}'", new object[] { this.buffName });
			}
			return this.cachedBuff;
		}
	}

	// Token: 0x17000573 RID: 1395
	// (get) Token: 0x060034AD RID: 13485 RVA: 0x0015CC50 File Offset: 0x0015AE50
	// (set) Token: 0x060034AE RID: 13486 RVA: 0x0015CC5D File Offset: 0x0015AE5D
	public bool Remove
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.buffFlags & BuffValue.BuffFlags.Remove) > BuffValue.BuffFlags.None;
		}
		set
		{
			if (value)
			{
				this.buffFlags |= BuffValue.BuffFlags.Remove;
				return;
			}
			this.buffFlags &= (BuffValue.BuffFlags)251;
		}
	}

	// Token: 0x17000574 RID: 1396
	// (get) Token: 0x060034AF RID: 13487 RVA: 0x0015CC83 File Offset: 0x0015AE83
	// (set) Token: 0x060034B0 RID: 13488 RVA: 0x0015CC90 File Offset: 0x0015AE90
	public bool Finished
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.buffFlags & BuffValue.BuffFlags.Finished) > BuffValue.BuffFlags.None;
		}
		set
		{
			if (value)
			{
				this.buffFlags |= BuffValue.BuffFlags.Finished;
				return;
			}
			this.buffFlags &= (BuffValue.BuffFlags)253;
		}
	}

	// Token: 0x17000575 RID: 1397
	// (get) Token: 0x060034B1 RID: 13489 RVA: 0x0015CCB6 File Offset: 0x0015AEB6
	// (set) Token: 0x060034B2 RID: 13490 RVA: 0x0015CCC3 File Offset: 0x0015AEC3
	public bool Started
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.buffFlags & BuffValue.BuffFlags.Started) > BuffValue.BuffFlags.None;
		}
		set
		{
			if (value)
			{
				this.buffFlags |= BuffValue.BuffFlags.Started;
				return;
			}
			this.buffFlags &= (BuffValue.BuffFlags)254;
		}
	}

	// Token: 0x17000576 RID: 1398
	// (get) Token: 0x060034B3 RID: 13491 RVA: 0x0015CCE9 File Offset: 0x0015AEE9
	// (set) Token: 0x060034B4 RID: 13492 RVA: 0x0015CCF7 File Offset: 0x0015AEF7
	public bool Invalid
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.buffFlags & BuffValue.BuffFlags.Invalid) > BuffValue.BuffFlags.None;
		}
		set
		{
			if (value)
			{
				this.buffFlags |= BuffValue.BuffFlags.Invalid;
				return;
			}
			this.buffFlags &= (BuffValue.BuffFlags)239;
		}
	}

	// Token: 0x17000577 RID: 1399
	// (get) Token: 0x060034B5 RID: 13493 RVA: 0x0015CD1E File Offset: 0x0015AF1E
	// (set) Token: 0x060034B6 RID: 13494 RVA: 0x0015CD2B File Offset: 0x0015AF2B
	public bool Update
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.buffFlags & BuffValue.BuffFlags.Update) > BuffValue.BuffFlags.None;
		}
		set
		{
			if (value)
			{
				this.buffFlags |= BuffValue.BuffFlags.Update;
				return;
			}
			this.buffFlags &= (BuffValue.BuffFlags)247;
		}
	}

	// Token: 0x17000578 RID: 1400
	// (get) Token: 0x060034B7 RID: 13495 RVA: 0x0015CD51 File Offset: 0x0015AF51
	// (set) Token: 0x060034B8 RID: 13496 RVA: 0x0015CD5F File Offset: 0x0015AF5F
	public bool Paused
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.buffFlags & BuffValue.BuffFlags.Paused) > BuffValue.BuffFlags.None;
		}
		set
		{
			if (value)
			{
				this.buffFlags |= BuffValue.BuffFlags.Paused;
				return;
			}
			this.buffFlags &= (BuffValue.BuffFlags)223;
		}
	}

	// Token: 0x17000579 RID: 1401
	// (get) Token: 0x060034B9 RID: 13497 RVA: 0x0015CD86 File Offset: 0x0015AF86
	// (set) Token: 0x060034BA RID: 13498 RVA: 0x0015CD8E File Offset: 0x0015AF8E
	public int StackEffectMultiplier
	{
		get
		{
			return (int)this.stackEffectMultiplier;
		}
		set
		{
			this.stackEffectMultiplier = (byte)Mathf.Clamp(value, 0, 255);
		}
	}

	// Token: 0x1700057A RID: 1402
	// (get) Token: 0x060034BB RID: 13499 RVA: 0x0015CDA3 File Offset: 0x0015AFA3
	public float DurationInSeconds
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.durationTicks / 20f;
		}
	}

	// Token: 0x1700057B RID: 1403
	// (get) Token: 0x060034BC RID: 13500 RVA: 0x0015CDB3 File Offset: 0x0015AFB3
	// (set) Token: 0x060034BD RID: 13501 RVA: 0x0015CDBB File Offset: 0x0015AFBB
	public uint DurationInTicks
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.durationTicks;
		}
		set
		{
			this.durationTicks = value;
			this.updateTicks = (ushort)value;
		}
	}

	// Token: 0x060034BE RID: 13502 RVA: 0x0015CDCC File Offset: 0x0015AFCC
	public void DurationTick()
	{
		this.durationTicks += 1U;
		ushort num = this.updateTicks + 1;
		this.updateTicks = num;
		if ((int)num >= this.BuffClass.UpdateRateTicks)
		{
			this.Update = true;
			this.updateTicks = 0;
		}
	}

	// Token: 0x060034BF RID: 13503 RVA: 0x0015CE14 File Offset: 0x0015B014
	public void DurationTriggerUpdate()
	{
		this.updateTicks = 32767;
	}

	// Token: 0x1700057C RID: 1404
	// (get) Token: 0x060034C0 RID: 13504 RVA: 0x0015CE21 File Offset: 0x0015B021
	public string BuffName
	{
		get
		{
			return this.buffName;
		}
	}

	// Token: 0x1700057D RID: 1405
	// (get) Token: 0x060034C1 RID: 13505 RVA: 0x0015CE29 File Offset: 0x0015B029
	public int InstigatorId
	{
		get
		{
			return this.instigatorId;
		}
	}

	// Token: 0x1700057E RID: 1406
	// (get) Token: 0x060034C2 RID: 13506 RVA: 0x0015CE31 File Offset: 0x0015B031
	public Vector3i InstigatorPos
	{
		get
		{
			return this.instigatorPos;
		}
	}

	// Token: 0x060034C3 RID: 13507 RVA: 0x0000640C File Offset: 0x0000460C
	public BuffValue()
	{
	}

	// Token: 0x060034C4 RID: 13508 RVA: 0x0015CE3C File Offset: 0x0015B03C
	public BuffValue(string _buffEffectGroupId, Vector3i _instigatorPos, int _instigatorId = -1, BuffClass _buffClass = null)
	{
		this.buffName = _buffEffectGroupId;
		this.stackEffectMultiplier = 1;
		this.durationTicks = 0U;
		this.instigatorId = _instigatorId;
		this.buffFlags = BuffValue.BuffFlags.None;
		this.updateTicks = 0;
		this.instigatorPos = _instigatorPos;
		if (_buffClass == null)
		{
			this.cacheBuffClassPointer();
			return;
		}
		this.cachedBuff = _buffClass;
	}

	// Token: 0x060034C5 RID: 13509 RVA: 0x0015CE93 File Offset: 0x0015B093
	public void ClearBuffClassLink()
	{
		this.cachedBuff = null;
	}

	// Token: 0x060034C6 RID: 13510 RVA: 0x0015CE9C File Offset: 0x0015B09C
	[PublicizedFrom(EAccessModifier.Private)]
	public void cacheBuffClassPointer()
	{
		if (!BuffManager.Buffs.TryGetValue(this.buffName, out this.cachedBuff))
		{
			this.Remove = true;
		}
	}

	// Token: 0x060034C7 RID: 13511 RVA: 0x0015CEC0 File Offset: 0x0015B0C0
	public void Tick()
	{
		BuffClass buffClass = this.BuffClass;
		if (buffClass != null)
		{
			buffClass.Tick(this);
			return;
		}
		this.Remove = true;
	}

	// Token: 0x060034C8 RID: 13512 RVA: 0x0015CEE8 File Offset: 0x0015B0E8
	public void Write(PooledBinaryWriter _bw)
	{
		_bw.Write(this.buffName);
		_bw.Write(this.stackEffectMultiplier);
		_bw.Write(this.durationTicks);
		_bw.Write(this.instigatorId);
		_bw.Write((byte)this.buffFlags);
		_bw.Write(this.updateTicks);
		StreamUtils.Write(_bw, this.instigatorPos);
	}

	// Token: 0x060034C9 RID: 13513 RVA: 0x0015CF4C File Offset: 0x0015B14C
	public void Read(PooledBinaryReader _br, int _version)
	{
		if (_version < 2)
		{
			int num = _br.ReadInt32();
			using (Dictionary<string, BuffClass>.KeyCollection.Enumerator enumerator = BuffManager.Buffs.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string text = enumerator.Current;
					if (text.GetHashCode() == num)
					{
						this.buffName = BuffManager.Buffs[text].Name;
						break;
					}
				}
				goto IL_0070;
			}
		}
		this.buffName = _br.ReadString().ToLower();
		IL_0070:
		this.stackEffectMultiplier = _br.ReadByte();
		this.durationTicks = _br.ReadUInt32();
		this.instigatorId = _br.ReadInt32();
		this.buffFlags = (BuffValue.BuffFlags)_br.ReadByte();
		if (_version == 0)
		{
			this.updateTicks = (ushort)_br.ReadByte();
		}
		else
		{
			this.updateTicks = _br.ReadUInt16();
		}
		if (_version >= 3)
		{
			this.instigatorPos = StreamUtils.ReadVector3i(_br);
		}
		this.cacheBuffClassPointer();
	}

	// Token: 0x040029C3 RID: 10691
	[PublicizedFrom(EAccessModifier.Private)]
	public BuffClass cachedBuff;

	// Token: 0x040029C4 RID: 10692
	[PublicizedFrom(EAccessModifier.Private)]
	public string buffName;

	// Token: 0x040029C5 RID: 10693
	[PublicizedFrom(EAccessModifier.Private)]
	public byte stackEffectMultiplier;

	// Token: 0x040029C6 RID: 10694
	[PublicizedFrom(EAccessModifier.Private)]
	public uint durationTicks;

	// Token: 0x040029C7 RID: 10695
	[PublicizedFrom(EAccessModifier.Private)]
	public int instigatorId;

	// Token: 0x040029C8 RID: 10696
	[PublicizedFrom(EAccessModifier.Private)]
	public BuffValue.BuffFlags buffFlags;

	// Token: 0x040029C9 RID: 10697
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort updateTicks;

	// Token: 0x040029CA RID: 10698
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i instigatorPos;

	// Token: 0x02000657 RID: 1623
	[PublicizedFrom(EAccessModifier.Private)]
	public enum BuffFlags : byte
	{
		// Token: 0x040029CC RID: 10700
		None,
		// Token: 0x040029CD RID: 10701
		Started,
		// Token: 0x040029CE RID: 10702
		Finished,
		// Token: 0x040029CF RID: 10703
		Remove = 4,
		// Token: 0x040029D0 RID: 10704
		Update = 8,
		// Token: 0x040029D1 RID: 10705
		Invalid = 16,
		// Token: 0x040029D2 RID: 10706
		Paused = 32
	}
}
