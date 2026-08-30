using System;
using System.IO;
using UnityEngine;

// Token: 0x02000522 RID: 1314
public class Faction
{
	// Token: 0x06002B4A RID: 11082 RVA: 0x00111B1F File Offset: 0x0010FD1F
	public Faction()
	{
	}

	// Token: 0x06002B4B RID: 11083 RVA: 0x00111B38 File Offset: 0x0010FD38
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

	// Token: 0x06002B4C RID: 11084 RVA: 0x00111B90 File Offset: 0x0010FD90
	public void ModifyRelationship(byte _factionId, float _modifier)
	{
		float num = this.Relationships[(int)_factionId];
		if (num != 255f)
		{
			num = Mathf.Clamp(num + _modifier, 0f, 1000f);
		}
		this.Relationships[(int)_factionId] = num;
	}

	// Token: 0x06002B4D RID: 11085 RVA: 0x00111BCA File Offset: 0x0010FDCA
	public void SetRelationship(byte _factionId, float _value)
	{
		this.Relationships[(int)_factionId] = (float)((byte)Mathf.Clamp(_value, 0f, 1000f));
	}

	// Token: 0x06002B4E RID: 11086 RVA: 0x00111BE6 File Offset: 0x0010FDE6
	public float GetRelationship(byte _factionId)
	{
		return this.Relationships[(int)_factionId];
	}

	// Token: 0x06002B4F RID: 11087 RVA: 0x00111BF0 File Offset: 0x0010FDF0
	public void SetAlly(byte _factionId)
	{
		this.Relationships[(int)_factionId] = 1000f;
	}

	// Token: 0x06002B50 RID: 11088 RVA: 0x00111C00 File Offset: 0x0010FE00
	public void Write(BinaryWriter bw)
	{
		for (int i = 0; i < 255; i++)
		{
			bw.Write(this.Relationships[i]);
		}
		bw.Write(this.IsPlayerFaction);
	}

	// Token: 0x06002B51 RID: 11089 RVA: 0x00111C38 File Offset: 0x0010FE38
	public void Read(BinaryReader br)
	{
		this.Relationships = new float[255];
		for (int i = 0; i < 255; i++)
		{
			this.Relationships[i] = br.ReadSingle();
		}
		this.IsPlayerFaction = br.ReadBoolean();
	}

	// Token: 0x06002B52 RID: 11090 RVA: 0x00111C80 File Offset: 0x0010FE80
	public override string ToString()
	{
		return string.Format("{0} : {1}", this.Name, string.Join(", ", Array.ConvertAll<float, string>(this.Relationships, (float x) => x.ToCultureInvariantString())));
	}

	// Token: 0x0400211A RID: 8474
	public byte ID;

	// Token: 0x0400211B RID: 8475
	public string Name;

	// Token: 0x0400211C RID: 8476
	public string Icon;

	// Token: 0x0400211D RID: 8477
	public bool IsPlayerFaction;

	// Token: 0x0400211E RID: 8478
	public float[] Relationships = new float[255];
}
