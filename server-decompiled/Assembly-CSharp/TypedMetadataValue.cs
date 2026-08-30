using System;
using System.IO;

// Token: 0x02000580 RID: 1408
public class TypedMetadataValue
{
	// Token: 0x06002E2F RID: 11823 RVA: 0x001257D4 File Offset: 0x001239D4
	public static TypedMetadataValue.TypeTag StringToTag(string str)
	{
		TypedMetadataValue.TypeTag typeTag;
		if (!(str == "float"))
		{
			if (!(str == "int"))
			{
				if (!(str == "string"))
				{
					typeTag = TypedMetadataValue.TypeTag.None;
				}
				else
				{
					typeTag = TypedMetadataValue.TypeTag.String;
				}
			}
			else
			{
				typeTag = TypedMetadataValue.TypeTag.Integer;
			}
		}
		else
		{
			typeTag = TypedMetadataValue.TypeTag.Float;
		}
		return typeTag;
	}

	// Token: 0x06002E30 RID: 11824 RVA: 0x00125819 File Offset: 0x00123A19
	public TypedMetadataValue.TypeTag GetTypeTag()
	{
		return this.typeTag;
	}

	// Token: 0x06002E31 RID: 11825 RVA: 0x00125821 File Offset: 0x00123A21
	[PublicizedFrom(EAccessModifier.Private)]
	public TypedMetadataValue(object val, TypedMetadataValue.TypeTag tag)
	{
		this.typeTag = tag;
		this.value = val;
	}

	// Token: 0x06002E32 RID: 11826 RVA: 0x00125837 File Offset: 0x00123A37
	public static bool TryCreate(object val, TypedMetadataValue.TypeTag tag, out TypedMetadataValue result)
	{
		if (!TypedMetadataValue.ValueMatchesTag(val, tag))
		{
			result = null;
			return false;
		}
		result = new TypedMetadataValue(val, tag);
		return true;
	}

	// Token: 0x06002E33 RID: 11827 RVA: 0x00125851 File Offset: 0x00123A51
	public object GetValue()
	{
		return this.value;
	}

	// Token: 0x06002E34 RID: 11828 RVA: 0x00125859 File Offset: 0x00123A59
	public bool SetValue(object val)
	{
		if (TypedMetadataValue.ValueMatchesTag(val, this.typeTag))
		{
			this.value = val;
			return true;
		}
		return false;
	}

	// Token: 0x06002E35 RID: 11829 RVA: 0x00125873 File Offset: 0x00123A73
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool ValueMatchesTag(object val, TypedMetadataValue.TypeTag tag)
	{
		if (val == null)
		{
			return false;
		}
		switch (tag)
		{
		case TypedMetadataValue.TypeTag.Float:
			return val is float;
		case TypedMetadataValue.TypeTag.Integer:
			return val is int;
		case TypedMetadataValue.TypeTag.String:
			return val is string;
		default:
			return false;
		}
	}

	// Token: 0x06002E36 RID: 11830 RVA: 0x001258B0 File Offset: 0x00123AB0
	public static void Write(TypedMetadataValue tmv, BinaryWriter writer)
	{
		if (tmv == null)
		{
			return;
		}
		writer.Write((int)tmv.typeTag);
		switch (tmv.typeTag)
		{
		case TypedMetadataValue.TypeTag.Float:
			writer.Write((float)tmv.value);
			return;
		case TypedMetadataValue.TypeTag.Integer:
			writer.Write((int)tmv.value);
			return;
		case TypedMetadataValue.TypeTag.String:
			writer.Write((string)tmv.value);
			return;
		default:
			return;
		}
	}

	// Token: 0x06002E37 RID: 11831 RVA: 0x00125920 File Offset: 0x00123B20
	public static TypedMetadataValue Read(BinaryReader reader)
	{
		object obj = null;
		TypedMetadataValue.TypeTag typeTag = (TypedMetadataValue.TypeTag)reader.ReadInt32();
		switch (typeTag)
		{
		case TypedMetadataValue.TypeTag.Float:
			obj = reader.ReadSingle();
			break;
		case TypedMetadataValue.TypeTag.Integer:
			obj = reader.ReadInt32();
			break;
		case TypedMetadataValue.TypeTag.String:
			obj = reader.ReadString();
			break;
		}
		return new TypedMetadataValue(obj, typeTag);
	}

	// Token: 0x06002E38 RID: 11832 RVA: 0x00125978 File Offset: 0x00123B78
	public override bool Equals(object other)
	{
		TypedMetadataValue typedMetadataValue = other as TypedMetadataValue;
		return typedMetadataValue != null && this.typeTag == typedMetadataValue.typeTag && this.value.Equals(typedMetadataValue.value);
	}

	// Token: 0x06002E39 RID: 11833 RVA: 0x000111D7 File Offset: 0x0000F3D7
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x06002E3A RID: 11834 RVA: 0x001259B0 File Offset: 0x00123BB0
	public TypedMetadataValue Clone()
	{
		TypedMetadataValue.TypeTag typeTag = this.typeTag;
		return new TypedMetadataValue(this.value, this.typeTag);
	}

	// Token: 0x040023DE RID: 9182
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly TypedMetadataValue.TypeTag typeTag;

	// Token: 0x040023DF RID: 9183
	[PublicizedFrom(EAccessModifier.Private)]
	public object value;

	// Token: 0x02000581 RID: 1409
	public enum TypeTag
	{
		// Token: 0x040023E1 RID: 9185
		None,
		// Token: 0x040023E2 RID: 9186
		Float,
		// Token: 0x040023E3 RID: 9187
		Integer,
		// Token: 0x040023E4 RID: 9188
		String
	}
}
