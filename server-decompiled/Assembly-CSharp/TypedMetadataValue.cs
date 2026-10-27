using System;

// Token: 0x0200058B RID: 1419
public class TypedMetadataValue
{
	// Token: 0x06002E94 RID: 11924 RVA: 0x001274C8 File Offset: 0x001256C8
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

	// Token: 0x06002E95 RID: 11925 RVA: 0x0012750D File Offset: 0x0012570D
	public TypedMetadataValue.TypeTag GetTypeTag()
	{
		return this.typeTag;
	}

	// Token: 0x06002E96 RID: 11926 RVA: 0x00127515 File Offset: 0x00125715
	[PublicizedFrom(EAccessModifier.Private)]
	public TypedMetadataValue(object val, TypedMetadataValue.TypeTag tag)
	{
		this.typeTag = tag;
		this.value = val;
	}

	// Token: 0x06002E97 RID: 11927 RVA: 0x0012752B File Offset: 0x0012572B
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

	// Token: 0x06002E98 RID: 11928 RVA: 0x00127545 File Offset: 0x00125745
	public object GetValue()
	{
		return this.value;
	}

	// Token: 0x06002E99 RID: 11929 RVA: 0x0012754D File Offset: 0x0012574D
	public bool SetValue(object val)
	{
		if (TypedMetadataValue.ValueMatchesTag(val, this.typeTag))
		{
			this.value = val;
			return true;
		}
		return false;
	}

	// Token: 0x06002E9A RID: 11930 RVA: 0x00127567 File Offset: 0x00125767
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

	// Token: 0x06002E9B RID: 11931 RVA: 0x001275A4 File Offset: 0x001257A4
	public static void Write(TypedMetadataValue tmv, PooledBinaryWriter writer)
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

	// Token: 0x06002E9C RID: 11932 RVA: 0x00127614 File Offset: 0x00125814
	public static TypedMetadataValue Read(PooledBinaryReader reader)
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

	// Token: 0x06002E9D RID: 11933 RVA: 0x0012766C File Offset: 0x0012586C
	public override bool Equals(object other)
	{
		TypedMetadataValue typedMetadataValue = other as TypedMetadataValue;
		return typedMetadataValue != null && this.typeTag == typedMetadataValue.typeTag && this.value.Equals(typedMetadataValue.value);
	}

	// Token: 0x06002E9E RID: 11934 RVA: 0x000111D7 File Offset: 0x0000F3D7
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x06002E9F RID: 11935 RVA: 0x001276A4 File Offset: 0x001258A4
	public TypedMetadataValue Clone()
	{
		TypedMetadataValue.TypeTag typeTag = this.typeTag;
		return new TypedMetadataValue(this.value, this.typeTag);
	}

	// Token: 0x040023FB RID: 9211
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly TypedMetadataValue.TypeTag typeTag;

	// Token: 0x040023FC RID: 9212
	[PublicizedFrom(EAccessModifier.Private)]
	public object value;

	// Token: 0x0200058C RID: 1420
	public enum TypeTag
	{
		// Token: 0x040023FE RID: 9214
		None,
		// Token: 0x040023FF RID: 9215
		Float,
		// Token: 0x04002400 RID: 9216
		Integer,
		// Token: 0x04002401 RID: 9217
		String
	}
}
