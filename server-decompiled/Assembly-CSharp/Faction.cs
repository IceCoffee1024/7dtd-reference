using System;
using System.IO;
using UnityEngine;

// Token: 0x0200051D RID: 1309
public class Faction
{
	// Token: 0x06002B20 RID: 11040 RVA: 0x00110AA7 File Offset: 0x0010ECA7
	public Faction()
	{
	}

	// Token: 0x06002B21 RID: 11041 RVA: 0x00110AC0 File Offset: 0x0010ECC0
	public Faction(string _name, bool _playerFaction = false, string _icon = "")
	{
		this.Name = _name;
		this.Icon = _icon;
		this.IsPlayerFaction = _playerFaction;
		for (int i = 0; i < 255; i++)
		{
			this.Relationships[i] = 400f;
		}
	}

	// Token: 0x06002B22 RID: 11042 RVA: 0x00110B18 File Offset: 0x0010ED18
	public void ModifyRelationship(byte _factionId, float _modifier)
	{
		float num = this.Relationships[(int)_factionId];
		if (num != 255f)
		{
			num = Mathf.Clamp(num + _modifier, 0f, 1000f);
		}
		this.Relationships[(int)_factionId] = num;
	}

	// Token: 0x06002B23 RID: 11043 RVA: 0x00110B52 File Offset: 0x0010ED52
	public void SetRelationship(byte _factionId, float _value)
	{
		this.Relationships[(int)_factionId] = (float)((byte)Mathf.Clamp(_value, 0f, 1000f));
	}

	// Token: 0x06002B24 RID: 11044 RVA: 0x00110B6E File Offset: 0x0010ED6E
	public float GetRelationship(byte _factionId)
	{
		return this.Relationships[(int)_factionId];
	}

	// Token: 0x06002B25 RID: 11045 RVA: 0x00110B78 File Offset: 0x0010ED78
	public void SetAlly(byte _factionId)
	{
		this.Relationships[(int)_factionId] = 1000f;
	}

	// Token: 0x06002B26 RID: 11046 RVA: 0x00110B88 File Offset: 0x0010ED88
	public void Write(BinaryWriter bw)
	{
		for (int i = 0; i < 255; i++)
		{
			bw.Write(this.Relationships[i]);
		}
		bw.Write(this.IsPlayerFaction);
	}

	// Token: 0x06002B27 RID: 11047 RVA: 0x00110BC0 File Offset: 0x0010EDC0
	public void Read(BinaryReader br)
	{
		this.Relationships = new float[255];
		for (int i = 0; i < 255; i++)
		{
			this.Relationships[i] = br.ReadSingle();
		}
		this.IsPlayerFaction = br.ReadBoolean();
	}

	// Token: 0x06002B28 RID: 11048 RVA: 0x00110C08 File Offset: 0x0010EE08
	public override string ToString()
	{
		return string.Format("{0} : {1}", this.Name, string.Join(", ", Array.ConvertAll<float, string>(this.Relationships, (float x) => x.ToCultureInvariantString())));
	}

	// Token: 0x040020EB RID: 8427
	public byte ID;

	// Token: 0x040020EC RID: 8428
	public string Name;

	// Token: 0x040020ED RID: 8429
	public string Icon;

	// Token: 0x040020EE RID: 8430
	public bool IsPlayerFaction;

	// Token: 0x040020EF RID: 8431
	public float[] Relationships = new float[255];
}
