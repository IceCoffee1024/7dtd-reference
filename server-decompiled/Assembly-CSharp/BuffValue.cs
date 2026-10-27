using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200065D RID: 1629
public class BuffValue
{
	// Token: 0x17000572 RID: 1394
	// (get) Token: 0x060034BA RID: 13498 RVA: 0x0015D7F8 File Offset: 0x0015B9F8
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
	// (get) Token: 0x060034BB RID: 13499 RVA: 0x0015D844 File Offset: 0x0015BA44
	// (set) Token: 0x060034BC RID: 13500 RVA: 0x0015D851 File Offset: 0x0015BA51
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
	// (get) Token: 0x060034BD RID: 13501 RVA: 0x0015D877 File Offset: 0x0015BA77
	// (set) Token: 0x060034BE RID: 13502 RVA: 0x0015D884 File Offset: 0x0015BA84
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
	// (get) Token: 0x060034BF RID: 13503 RVA: 0x0015D8AA File Offset: 0x0015BAAA
	// (set) Token: 0x060034C0 RID: 13504 RVA: 0x0015D8B7 File Offset: 0x0015BAB7
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
	// (get) Token: 0x060034C1 RID: 13505 RVA: 0x0015D8DD File Offset: 0x0015BADD
	// (set) Token: 0x060034C2 RID: 13506 RVA: 0x0015D8EB File Offset: 0x0015BAEB
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
	// (get) Token: 0x060034C3 RID: 13507 RVA: 0x0015D912 File Offset: 0x0015BB12
	// (set) Token: 0x060034C4 RID: 13508 RVA: 0x0015D91F File Offset: 0x0015BB1F
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
	// (get) Token: 0x060034C5 RID: 13509 RVA: 0x0015D945 File Offset: 0x0015BB45
	// (set) Token: 0x060034C6 RID: 13510 RVA: 0x0015D953 File Offset: 0x0015BB53
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
	// (get) Token: 0x060034C7 RID: 13511 RVA: 0x0015D97A File Offset: 0x0015BB7A
	// (set) Token: 0x060034C8 RID: 13512 RVA: 0x0015D982 File Offset: 0x0015BB82
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
	// (get) Token: 0x060034C9 RID: 13513 RVA: 0x0015D997 File Offset: 0x0015BB97
	public float DurationInSeconds
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.durationTicks / 20f;
		}
	}

	// Token: 0x1700057B RID: 1403
	// (get) Token: 0x060034CA RID: 13514 RVA: 0x0015D9A7 File Offset: 0x0015BBA7
	// (set) Token: 0x060034CB RID: 13515 RVA: 0x0015D9AF File Offset: 0x0015BBAF
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

	// Token: 0x060034CC RID: 13516 RVA: 0x0015D9C0 File Offset: 0x0015BBC0
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

	// Token: 0x060034CD RID: 13517 RVA: 0x0015DA08 File Offset: 0x0015BC08
	public void DurationTriggerUpdate()
	{
		this.updateTicks = 32767;
	}

	// Token: 0x1700057C RID: 1404
	// (get) Token: 0x060034CE RID: 13518 RVA: 0x0015DA15 File Offset: 0x0015BC15
	public string BuffName
	{
		get
		{
			return this.buffName;
		}
	}

	// Token: 0x1700057D RID: 1405
	// (get) Token: 0x060034CF RID: 13519 RVA: 0x0015DA1D File Offset: 0x0015BC1D
	public int InstigatorId
	{
		get
		{
			return this.instigatorId;
		}
	}

	// Token: 0x1700057E RID: 1406
	// (get) Token: 0x060034D0 RID: 13520 RVA: 0x0015DA25 File Offset: 0x0015BC25
	public Vector3i InstigatorPos
	{
		get
		{
			return this.instigatorPos;
		}
	}

	// Token: 0x060034D1 RID: 13521 RVA: 0x0000640C File Offset: 0x0000460C
	public BuffValue()
	{
	}

	// Token: 0x060034D2 RID: 13522 RVA: 0x0015DA30 File Offset: 0x0015BC30
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

	// Token: 0x060034D3 RID: 13523 RVA: 0x0015DA87 File Offset: 0x0015BC87
	public void ClearBuffClassLink()
	{
		this.cachedBuff = null;
	}

	// Token: 0x060034D4 RID: 13524 RVA: 0x0015DA90 File Offset: 0x0015BC90
	[PublicizedFrom(EAccessModifier.Private)]
	public void cacheBuffClassPointer()
	{
		if (!BuffManager.Buffs.TryGetValue(this.buffName, out this.cachedBuff))
		{
			this.Remove = true;
		}
	}

	// Token: 0x060034D5 RID: 13525 RVA: 0x0015DAB4 File Offset: 0x0015BCB4
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

	// Token: 0x060034D6 RID: 13526 RVA: 0x0015DADC File Offset: 0x0015BCDC
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

	// Token: 0x060034D7 RID: 13527 RVA: 0x0015DB40 File Offset: 0x0015BD40
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

	// Token: 0x040029DE RID: 10718
	[PublicizedFrom(EAccessModifier.Private)]
	public BuffClass cachedBuff;

	// Token: 0x040029DF RID: 10719
	[PublicizedFrom(EAccessModifier.Private)]
	public string buffName;

	// Token: 0x040029E0 RID: 10720
	[PublicizedFrom(EAccessModifier.Private)]
	public byte stackEffectMultiplier;

	// Token: 0x040029E1 RID: 10721
	[PublicizedFrom(EAccessModifier.Private)]
	public uint durationTicks;

	// Token: 0x040029E2 RID: 10722
	[PublicizedFrom(EAccessModifier.Private)]
	public int instigatorId;

	// Token: 0x040029E3 RID: 10723
	[PublicizedFrom(EAccessModifier.Private)]
	public BuffValue.BuffFlags buffFlags;

	// Token: 0x040029E4 RID: 10724
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort updateTicks;

	// Token: 0x040029E5 RID: 10725
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i instigatorPos;

	// Token: 0x0200065E RID: 1630
	[PublicizedFrom(EAccessModifier.Private)]
	public enum BuffFlags : byte
	{
		// Token: 0x040029E7 RID: 10727
		None,
		// Token: 0x040029E8 RID: 10728
		Started,
		// Token: 0x040029E9 RID: 10729
		Finished,
		// Token: 0x040029EA RID: 10730
		Remove = 4,
		// Token: 0x040029EB RID: 10731
		Update = 8,
		// Token: 0x040029EC RID: 10732
		Invalid = 16,
		// Token: 0x040029ED RID: 10733
		Paused = 32
	}
}
