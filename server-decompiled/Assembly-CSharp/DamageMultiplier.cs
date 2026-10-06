using System;
using System.Collections.Generic;
using System.Globalization;

// Token: 0x02000543 RID: 1347
public class DamageMultiplier
{
	// Token: 0x06002C1C RID: 11292 RVA: 0x00117859 File Offset: 0x00115A59
	public DamageMultiplier()
	{
	}

	// Token: 0x06002C1D RID: 11293 RVA: 0x0011786C File Offset: 0x00115A6C
	public DamageMultiplier(DynamicProperties _properties)
	{
		if (_properties.Classes.ContainsKey("DamageBonus"))
		{
			foreach (KeyValuePair<string, string> keyValuePair in _properties.Classes["DamageBonus"].Values)
			{
				float num = StringParsers.ParseFloat(keyValuePair.Value, 0, -1, NumberStyles.Any);
				this.addMultiplier(keyValuePair.Key, num);
			}
		}
	}

	// Token: 0x06002C1E RID: 11294 RVA: 0x0011790C File Offset: 0x00115B0C
	[PublicizedFrom(EAccessModifier.Private)]
	public void addMultiplier(string _name, float _value)
	{
		this.damageMultiplier[_name] = _value;
	}

	// Token: 0x06002C1F RID: 11295 RVA: 0x0011791B File Offset: 0x00115B1B
	public float Get(string _group)
	{
		if (_group == null || this.damageMultiplier == null || !this.damageMultiplier.ContainsKey(_group))
		{
			return 1f;
		}
		return this.damageMultiplier[_group];
	}

	// Token: 0x06002C20 RID: 11296 RVA: 0x00117948 File Offset: 0x00115B48
	public void Read(PooledBinaryReader _br)
	{
		this.damageMultiplier.Clear();
		int num = (int)_br.ReadInt16();
		for (int i = 0; i < num; i++)
		{
			string text = _br.ReadString();
			float num2 = _br.ReadSingle();
			this.damageMultiplier.Add(text, num2);
		}
	}

	// Token: 0x06002C21 RID: 11297 RVA: 0x00117990 File Offset: 0x00115B90
	public void Write(PooledBinaryWriter _bw)
	{
		_bw.Write((short)this.damageMultiplier.Count);
		foreach (KeyValuePair<string, float> keyValuePair in this.damageMultiplier)
		{
			_bw.Write(keyValuePair.Key);
			_bw.Write(keyValuePair.Value);
		}
	}

	// Token: 0x040021CB RID: 8651
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, float> damageMultiplier = new Dictionary<string, float>();
}
