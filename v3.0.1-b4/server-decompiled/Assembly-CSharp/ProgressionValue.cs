using System;
using System.IO;
using UnityEngine;

// Token: 0x02000AC4 RID: 2756
public class ProgressionValue
{
	// Token: 0x06005279 RID: 21113 RVA: 0x001F8560 File Offset: 0x001F6760
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

	// Token: 0x0600527A RID: 21114 RVA: 0x0000640C File Offset: 0x0000460C
	public ProgressionValue()
	{
	}

	// Token: 0x0600527B RID: 21115 RVA: 0x001F8617 File Offset: 0x001F6817
	public ProgressionValue(string _name)
	{
		this.name = _name;
	}

	// Token: 0x170008EE RID: 2286
	// (get) Token: 0x0600527C RID: 21116 RVA: 0x001F8626 File Offset: 0x001F6826
	public string Name
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x170008EF RID: 2287
	// (get) Token: 0x0600527D RID: 21117 RVA: 0x001F8630 File Offset: 0x001F6830
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

	// Token: 0x170008F0 RID: 2288
	// (get) Token: 0x0600527E RID: 21118 RVA: 0x001F867C File Offset: 0x001F687C
	// (set) Token: 0x0600527F RID: 21119 RVA: 0x001F86A6 File Offset: 0x001F68A6
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

	// Token: 0x170008F1 RID: 2289
	// (get) Token: 0x06005280 RID: 21120 RVA: 0x001F86C0 File Offset: 0x001F68C0
	// (set) Token: 0x06005281 RID: 21121 RVA: 0x001F86F3 File Offset: 0x001F68F3
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

	// Token: 0x06005282 RID: 21122 RVA: 0x001F8732 File Offset: 0x001F6932
	public int CalculatedLevel(EntityAlive _ea)
	{
		return (int)this.GetCalculatedLevel(_ea);
	}

	// Token: 0x06005283 RID: 21123 RVA: 0x001F873C File Offset: 0x001F693C
	public int CalculatedMaxLevel(EntityAlive _ea)
	{
		return ProgressionClass.GetCalculatedMaxLevel(_ea, this);
	}

	// Token: 0x06005284 RID: 21124 RVA: 0x001F8745 File Offset: 0x001F6945
	public bool IsLocked(EntityAlive _ea)
	{
		return ProgressionClass.GetCalculatedMaxLevel(_ea, this) == 0;
	}

	// Token: 0x170008F2 RID: 2290
	// (get) Token: 0x06005285 RID: 21125 RVA: 0x001F8751 File Offset: 0x001F6951
	public float PercToNextLevel
	{
		get
		{
			return 1f - (float)this.CostForNextLevel / (float)this.ProgressionClass.CalculatedCostForLevel(this.level + 1);
		}
	}

	// Token: 0x06005286 RID: 21126 RVA: 0x001F8775 File Offset: 0x001F6975
	public void ClearProgressionClassLink()
	{
		this.cachedProgressionClass = null;
	}

	// Token: 0x06005287 RID: 21127 RVA: 0x001F877E File Offset: 0x001F697E
	public bool CanPurchase(EntityAlive _ea, int _level)
	{
		return _level <= this.ProgressionClass.MaxLevel;
	}

	// Token: 0x06005288 RID: 21128 RVA: 0x001F8791 File Offset: 0x001F6991
	public void CopyFrom(ProgressionValue _pv)
	{
		this.name = _pv.name;
		this.level = _pv.level;
		this.costForNextLevel = _pv.costForNextLevel;
	}

	// Token: 0x06005289 RID: 21129 RVA: 0x001F87B7 File Offset: 0x001F69B7
	public void Read(BinaryReader _reader)
	{
		_reader.ReadByte();
		this.name = _reader.ReadString();
		this.level = (int)_reader.ReadByte();
		this.costForNextLevel = _reader.ReadInt32();
	}

	// Token: 0x0600528A RID: 21130 RVA: 0x001F87E4 File Offset: 0x001F69E4
	public void Write(BinaryWriter _writer, bool _IsNetwork)
	{
		_writer.Write(1);
		_writer.Write(this.name);
		_writer.Write((byte)this.level);
		_writer.Write(this.costForNextLevel);
	}

	// Token: 0x04004026 RID: 16422
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte Version = 1;

	// Token: 0x04004027 RID: 16423
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x04004028 RID: 16424
	[PublicizedFrom(EAccessModifier.Private)]
	public int level;

	// Token: 0x04004029 RID: 16425
	[PublicizedFrom(EAccessModifier.Private)]
	public int costForNextLevel;

	// Token: 0x0400402A RID: 16426
	[PublicizedFrom(EAccessModifier.Private)]
	public int calculatedFrame;

	// Token: 0x0400402B RID: 16427
	[PublicizedFrom(EAccessModifier.Private)]
	public float calculatedLevel;

	// Token: 0x0400402C RID: 16428
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionClass cachedProgressionClass;
}
