using System;
using System.IO;
using UnityEngine;

// Token: 0x02000AF2 RID: 2802
public class ProgressionValue
{
	// Token: 0x0600533D RID: 21309 RVA: 0x001FF7DC File Offset: 0x001FD9DC
	public float GetCalculatedLevel(EntityAlive _ea)
	{
		if (this.calculatedFrame == Time.frameCount)
		{
			return this.calculatedLevel;
		}
		ProgressionClass progressionClass = this.ProgressionClass;
		if (progressionClass == null)
		{
			return 0f;
		}
		float num = (float)this.Level;
		PassiveEffects passiveEffects;
		switch (progressionClass.Type)
		{
		case ProgressionType.Attribute:
			passiveEffects = PassiveEffects.AttributeLevel;
			break;
		case ProgressionType.Skill:
			passiveEffects = PassiveEffects.SkillLevel;
			break;
		case ProgressionType.Perk:
			passiveEffects = PassiveEffects.PerkLevel;
			break;
		default:
			passiveEffects = PassiveEffects.None;
			break;
		}
		PassiveEffects passiveEffects2 = passiveEffects;
		if (passiveEffects2 != PassiveEffects.None)
		{
			num = EffectManager.GetValue(passiveEffects2, null, num, _ea, null, progressionClass.NameTag, true, true, true, true, true, 1, true, false);
		}
		num = Mathf.Min(num, (float)ProgressionClass.GetCalculatedMaxLevel(_ea, this));
		num = Mathf.Max(num, (float)progressionClass.MinLevel);
		this.calculatedFrame = Time.frameCount;
		this.calculatedLevel = num;
		return num;
	}

	// Token: 0x0600533E RID: 21310 RVA: 0x0000640C File Offset: 0x0000460C
	public ProgressionValue()
	{
	}

	// Token: 0x0600533F RID: 21311 RVA: 0x001FF893 File Offset: 0x001FDA93
	public ProgressionValue(string _name)
	{
		this.name = _name;
	}

	// Token: 0x17000928 RID: 2344
	// (get) Token: 0x06005340 RID: 21312 RVA: 0x001FF8A2 File Offset: 0x001FDAA2
	public string Name
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x17000929 RID: 2345
	// (get) Token: 0x06005341 RID: 21313 RVA: 0x001FF8AC File Offset: 0x001FDAAC
	public ProgressionClass ProgressionClass
	{
		get
		{
			if (this.cachedProgressionClass == null && !Progression.ProgressionClasses.TryGetValue(this.name, out this.cachedProgressionClass))
			{
				Log.Error("ProgressionValue ProgressionClasses missing {0}", new object[] { this.name });
			}
			return this.cachedProgressionClass;
		}
	}

	// Token: 0x1700092A RID: 2346
	// (get) Token: 0x06005342 RID: 21314 RVA: 0x001FF8F8 File Offset: 0x001FDAF8
	// (set) Token: 0x06005343 RID: 21315 RVA: 0x001FF922 File Offset: 0x001FDB22
	public int CostForNextLevel
	{
		get
		{
			if (this.ProgressionClass.CurrencyType == ProgressionCurrencyType.SP)
			{
				return this.ProgressionClass.CalculatedCostForLevel(this.Level + 1);
			}
			return this.costForNextLevel;
		}
		set
		{
			if (this.ProgressionClass.CurrencyType != ProgressionCurrencyType.SP)
			{
				this.costForNextLevel = value;
			}
		}
	}

	// Token: 0x1700092B RID: 2347
	// (get) Token: 0x06005344 RID: 21316 RVA: 0x001FF93C File Offset: 0x001FDB3C
	// (set) Token: 0x06005345 RID: 21317 RVA: 0x001FF96F File Offset: 0x001FDB6F
	public int Level
	{
		get
		{
			ProgressionClass progressionClass = this.ProgressionClass;
			if (progressionClass == null)
			{
				return this.level;
			}
			if (progressionClass.IsSkill)
			{
				return progressionClass.MaxLevel;
			}
			return this.level;
		}
		set
		{
			this.calculatedFrame = -1;
			if (this.ProgressionClass == null)
			{
				this.level = value;
				return;
			}
			if (this.ProgressionClass.IsSkill)
			{
				this.level = this.ProgressionClass.MaxLevel;
				return;
			}
			this.level = value;
		}
	}

	// Token: 0x06005346 RID: 21318 RVA: 0x001FF9AE File Offset: 0x001FDBAE
	public int CalculatedLevel(EntityAlive _ea)
	{
		return (int)this.GetCalculatedLevel(_ea);
	}

	// Token: 0x06005347 RID: 21319 RVA: 0x001FF9B8 File Offset: 0x001FDBB8
	public int CalculatedMaxLevel(EntityAlive _ea)
	{
		return ProgressionClass.GetCalculatedMaxLevel(_ea, this);
	}

	// Token: 0x06005348 RID: 21320 RVA: 0x001FF9C1 File Offset: 0x001FDBC1
	public bool IsLocked(EntityAlive _ea)
	{
		return ProgressionClass.GetCalculatedMaxLevel(_ea, this) == 0;
	}

	// Token: 0x1700092C RID: 2348
	// (get) Token: 0x06005349 RID: 21321 RVA: 0x001FF9CD File Offset: 0x001FDBCD
	public float PercToNextLevel
	{
		get
		{
			return 1f - (float)this.CostForNextLevel / (float)this.ProgressionClass.CalculatedCostForLevel(this.level + 1);
		}
	}

	// Token: 0x0600534A RID: 21322 RVA: 0x001FF9F1 File Offset: 0x001FDBF1
	public void ClearProgressionClassLink()
	{
		this.cachedProgressionClass = null;
	}

	// Token: 0x0600534B RID: 21323 RVA: 0x001FF9FA File Offset: 0x001FDBFA
	public bool CanPurchase(EntityAlive _ea, int _level)
	{
		return _level <= this.ProgressionClass.MaxLevel;
	}

	// Token: 0x0600534C RID: 21324 RVA: 0x001FFA0D File Offset: 0x001FDC0D
	public void CopyFrom(ProgressionValue _pv)
	{
		this.name = _pv.name;
		this.level = _pv.level;
		this.costForNextLevel = _pv.costForNextLevel;
	}

	// Token: 0x0600534D RID: 21325 RVA: 0x001FFA33 File Offset: 0x001FDC33
	public void Read(BinaryReader _reader)
	{
		_reader.ReadByte();
		this.name = _reader.ReadString();
		this.level = (int)_reader.ReadByte();
		this.costForNextLevel = _reader.ReadInt32();
	}

	// Token: 0x0600534E RID: 21326 RVA: 0x001FFA60 File Offset: 0x001FDC60
	public void Write(BinaryWriter _writer, bool _IsNetwork)
	{
		_writer.Write(1);
		_writer.Write(this.name);
		_writer.Write((byte)this.level);
		_writer.Write(this.costForNextLevel);
	}

	// Token: 0x04004119 RID: 16665
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte Version = 1;

	// Token: 0x0400411A RID: 16666
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x0400411B RID: 16667
	[PublicizedFrom(EAccessModifier.Private)]
	public int level;

	// Token: 0x0400411C RID: 16668
	[PublicizedFrom(EAccessModifier.Private)]
	public int costForNextLevel;

	// Token: 0x0400411D RID: 16669
	[PublicizedFrom(EAccessModifier.Private)]
	public int calculatedFrame;

	// Token: 0x0400411E RID: 16670
	[PublicizedFrom(EAccessModifier.Private)]
	public float calculatedLevel;

	// Token: 0x0400411F RID: 16671
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionClass cachedProgressionClass;
}
