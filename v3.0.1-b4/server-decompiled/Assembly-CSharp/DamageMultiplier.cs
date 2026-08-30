using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

// Token: 0x02000538 RID: 1336
public class DamageMultiplier
{
	// Token: 0x06002BFC RID: 11260 RVA: 0x00115A11 File Offset: 0x00113C11
	public DamageMultiplier()
	{
	}

	// Token: 0x06002BFD RID: 11261 RVA: 0x00115A24 File Offset: 0x00113C24
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

	// Token: 0x06002BFE RID: 11262 RVA: 0x00115AC4 File Offset: 0x00113CC4
	[PublicizedFrom(EAccessModifier.Private)]
	public void addMultiplier(string _name, float _value)
	{
		this.damageMultiplier[_name] = _value;
	}

	// Token: 0x06002BFF RID: 11263 RVA: 0x00115AD3 File Offset: 0x00113CD3
	public float Get(string _group)
	{
		if (_group == null || this.damageMultiplier == null || !this.damageMultiplier.ContainsKey(_group))
		{
			return 1f;
		}
		return this.damageMultiplier[_group];
	}

	// Token: 0x06002C00 RID: 11264 RVA: 0x00115B00 File Offset: 0x00113D00
	public void Read(BinaryReader _br)
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

	// Token: 0x06002C01 RID: 11265 RVA: 0x00115B48 File Offset: 0x00113D48
	public void Write(BinaryWriter _bw)
	{
		_bw.Write((short)this.damageMultiplier.Count);
		foreach (KeyValuePair<string, float> keyValuePair in this.damageMultiplier)
		{
			_bw.Write(keyValuePair.Key);
			_bw.Write(keyValuePair.Value);
		}
	}

	// Token: 0x04002194 RID: 8596
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, float> damageMultiplier = new Dictionary<string, float>();
}
