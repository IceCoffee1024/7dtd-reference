using System;
using System.IO;

// Token: 0x020000CC RID: 204
public struct AnimParamData
{
	// Token: 0x060004B6 RID: 1206 RVA: 0x00020F7F File Offset: 0x0001F17F
	public AnimParamData(int _nameHash, AnimParamData.ValueTypes _valueType, bool _value)
	{
		this.NameHash = _nameHash;
		this.ValueType = _valueType;
		this.FloatValue = 0f;
		this.IntValue = (_value ? 1 : 0);
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x00020FA7 File Offset: 0x0001F1A7
	public AnimParamData(int _nameHash, AnimParamData.ValueTypes _valueType, float _value)
	{
		this.NameHash = _nameHash;
		this.ValueType = _valueType;
		this.FloatValue = _value;
		this.IntValue = 0;
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x00020FC5 File Offset: 0x0001F1C5
	public AnimParamData(int _nameHash, AnimParamData.ValueTypes _valueType, int _value)
	{
		this.NameHash = _nameHash;
		this.ValueType = _valueType;
		this.FloatValue = 0f;
		this.IntValue = _value;
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x00020FE8 File Offset: 0x0001F1E8
	public static AnimParamData CreateFromBinary(BinaryReader _br)
	{
		int num = _br.ReadInt32();
		AnimParamData.ValueTypes valueTypes = (AnimParamData.ValueTypes)_br.ReadByte();
		switch (valueTypes)
		{
		case AnimParamData.ValueTypes.Bool:
		case AnimParamData.ValueTypes.Trigger:
			return new AnimParamData(num, valueTypes, _br.ReadBoolean());
		case AnimParamData.ValueTypes.Float:
		case AnimParamData.ValueTypes.DataFloat:
			return new AnimParamData(num, valueTypes, _br.ReadSingle());
		case AnimParamData.ValueTypes.Int:
			return new AnimParamData(num, valueTypes, _br.ReadInt32());
		default:
		{
			string text = "Invalid Value Type: ";
			byte b = (byte)valueTypes;
			throw new InvalidDataException(text + b.ToString());
		}
		}
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x00021064 File Offset: 0x0001F264
	public void Write(BinaryWriter _bw)
	{
		_bw.Write(this.NameHash);
		_bw.Write((byte)this.ValueType);
		switch (this.ValueType)
		{
		case AnimParamData.ValueTypes.Bool:
		case AnimParamData.ValueTypes.Trigger:
			_bw.Write(this.IntValue != 0);
			return;
		case AnimParamData.ValueTypes.Float:
		case AnimParamData.ValueTypes.DataFloat:
			_bw.Write(this.FloatValue);
			return;
		case AnimParamData.ValueTypes.Int:
			_bw.Write(this.IntValue);
			return;
		default:
			return;
		}
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x000210D4 File Offset: 0x0001F2D4
	public string ToString(AvatarController _controller)
	{
		string parameterName = _controller.GetParameterName(this.NameHash);
		return string.Format("{0} {1}, {2}, f{3}, i{4}", new object[] { parameterName, this.NameHash, this.ValueType, this.FloatValue, this.IntValue });
	}

	// Token: 0x0400054B RID: 1355
	public readonly int NameHash;

	// Token: 0x0400054C RID: 1356
	public readonly AnimParamData.ValueTypes ValueType;

	// Token: 0x0400054D RID: 1357
	public readonly float FloatValue;

	// Token: 0x0400054E RID: 1358
	public readonly int IntValue;

	// Token: 0x020000CD RID: 205
	public enum ValueTypes : byte
	{
		// Token: 0x04000550 RID: 1360
		Bool,
		// Token: 0x04000551 RID: 1361
		Trigger,
		// Token: 0x04000552 RID: 1362
		Float,
		// Token: 0x04000553 RID: 1363
		Int,
		// Token: 0x04000554 RID: 1364
		DataFloat
	}
}
