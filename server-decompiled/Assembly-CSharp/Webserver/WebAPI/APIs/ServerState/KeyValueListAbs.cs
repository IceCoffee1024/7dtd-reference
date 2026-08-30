using System;
using System.Net;
using UnityEngine.Profiling;
using Utf8Json;

namespace Webserver.WebAPI.APIs.ServerState
{
	// Token: 0x02001ADB RID: 6875
	public abstract class KeyValueListAbs : AbsRestApi
	{
		// Token: 0x0600CEF1 RID: 52977 RVA: 0x004B5AC8 File Offset: 0x004B3CC8
		[PublicizedFrom(EAccessModifier.Protected)]
		public KeyValueListAbs(string _listName)
			: base(null)
		{
			this.buildSampler = CustomSampler.Create("JSON_" + _listName + "_BuildSampler", false);
		}

		// Token: 0x0600CEF2 RID: 52978 RVA: 0x004B5AF0 File Offset: 0x004B3CF0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.EnsureCapacity(this.largestBuffer);
			jsonWriter.WriteBeginArray();
			bool flag = true;
			this.iterateList(ref jsonWriter, ref flag);
			jsonWriter.WriteEndArray();
			int num = jsonWriter.CurrentOffset + 128;
			if (num > this.largestBuffer)
			{
				this.largestBuffer = num;
			}
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x0600CEF3 RID: 52979 RVA: 0x004B5B59 File Offset: 0x004B3D59
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeKeyType(ref JsonWriter _writer, ref bool _first, string _key, string _type)
		{
			if (!_first)
			{
				_writer.WriteValueSeparator();
			}
			_first = false;
			_writer.WriteRaw(KeyValueListAbs.keyName);
			_writer.WriteString(_key);
			_writer.WriteRaw(KeyValueListAbs.keyType);
			_writer.WriteString(_type);
		}

		// Token: 0x0600CEF4 RID: 52980 RVA: 0x004B5B8D File Offset: 0x004B3D8D
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeInt(ref JsonWriter _writer, ref bool _first, string _key, int _value)
		{
			this.writeKeyType(ref _writer, ref _first, _key, "int");
			_writer.WriteRaw(KeyValueListAbs.keyValue);
			_writer.WriteInt32(_value);
		}

		// Token: 0x0600CEF5 RID: 52981 RVA: 0x004B5BB0 File Offset: 0x004B3DB0
		[PublicizedFrom(EAccessModifier.Protected)]
		public void addItem(ref JsonWriter _writer, ref bool _first, string _key, int _value)
		{
			this.writeInt(ref _writer, ref _first, _key, _value);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CEF6 RID: 52982 RVA: 0x004B5BC3 File Offset: 0x004B3DC3
		[PublicizedFrom(EAccessModifier.Protected)]
		public void addItem(ref JsonWriter _writer, ref bool _first, string _key, int _value, int? _default)
		{
			this.writeInt(ref _writer, ref _first, _key, _value);
			_writer.WriteRaw(KeyValueListAbs.keyDefault);
			if (_default != null)
			{
				_writer.WriteInt32(_default.Value);
			}
			else
			{
				_writer.WriteNull();
			}
			_writer.WriteEndObject();
		}

		// Token: 0x0600CEF7 RID: 52983 RVA: 0x004B5BFF File Offset: 0x004B3DFF
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeFloat(ref JsonWriter _writer, ref bool _first, string _key, float _value)
		{
			this.writeKeyType(ref _writer, ref _first, _key, "float");
			_writer.WriteRaw(KeyValueListAbs.keyValue);
			_writer.WriteSingle(_value);
		}

		// Token: 0x0600CEF8 RID: 52984 RVA: 0x004B5C22 File Offset: 0x004B3E22
		[PublicizedFrom(EAccessModifier.Protected)]
		public void addItem(ref JsonWriter _writer, ref bool _first, string _key, float _value)
		{
			this.writeFloat(ref _writer, ref _first, _key, _value);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CEF9 RID: 52985 RVA: 0x004B5C35 File Offset: 0x004B3E35
		[PublicizedFrom(EAccessModifier.Protected)]
		public void addItem(ref JsonWriter _writer, ref bool _first, string _key, float _value, float? _default)
		{
			this.writeFloat(ref _writer, ref _first, _key, _value);
			_writer.WriteRaw(KeyValueListAbs.keyDefault);
			if (_default != null)
			{
				_writer.WriteSingle(_default.Value);
			}
			else
			{
				_writer.WriteNull();
			}
			_writer.WriteEndObject();
		}

		// Token: 0x0600CEFA RID: 52986 RVA: 0x004B5C71 File Offset: 0x004B3E71
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeBool(ref JsonWriter _writer, ref bool _first, string _key, bool _value)
		{
			this.writeKeyType(ref _writer, ref _first, _key, "bool");
			_writer.WriteRaw(KeyValueListAbs.keyValue);
			_writer.WriteBoolean(_value);
		}

		// Token: 0x0600CEFB RID: 52987 RVA: 0x004B5C94 File Offset: 0x004B3E94
		[PublicizedFrom(EAccessModifier.Protected)]
		public void addItem(ref JsonWriter _writer, ref bool _first, string _key, bool _value)
		{
			this.writeBool(ref _writer, ref _first, _key, _value);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CEFC RID: 52988 RVA: 0x004B5CA7 File Offset: 0x004B3EA7
		[PublicizedFrom(EAccessModifier.Protected)]
		public void addItem(ref JsonWriter _writer, ref bool _first, string _key, bool _value, bool? _default)
		{
			this.writeBool(ref _writer, ref _first, _key, _value);
			_writer.WriteRaw(KeyValueListAbs.keyDefault);
			if (_default != null)
			{
				_writer.WriteBoolean(_default.Value);
			}
			else
			{
				_writer.WriteNull();
			}
			_writer.WriteEndObject();
		}

		// Token: 0x0600CEFD RID: 52989 RVA: 0x004B5CE3 File Offset: 0x004B3EE3
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeString(ref JsonWriter _writer, ref bool _first, string _key, string _value)
		{
			this.writeKeyType(ref _writer, ref _first, _key, "string");
			_writer.WriteRaw(KeyValueListAbs.keyValue);
			_writer.WriteString(_value);
		}

		// Token: 0x0600CEFE RID: 52990 RVA: 0x004B5D06 File Offset: 0x004B3F06
		[PublicizedFrom(EAccessModifier.Protected)]
		public void addItem(ref JsonWriter _writer, ref bool _first, string _key, string _value)
		{
			this.writeString(ref _writer, ref _first, _key, _value);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CEFF RID: 52991 RVA: 0x004B5D19 File Offset: 0x004B3F19
		[PublicizedFrom(EAccessModifier.Protected)]
		public void addItem(ref JsonWriter _writer, ref bool _first, string _key, string _value, string _default)
		{
			this.writeString(ref _writer, ref _first, _key, _value);
			_writer.WriteRaw(KeyValueListAbs.keyDefault);
			_writer.WriteString(_default);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CF00 RID: 52992
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void iterateList(ref JsonWriter _writer, ref bool _first);

		// Token: 0x04009D0A RID: 40202
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler buildSampler;

		// Token: 0x04009D0B RID: 40203
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] keyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009D0C RID: 40204
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] keyType = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("type");

		// Token: 0x04009D0D RID: 40205
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] keyValue = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("value");

		// Token: 0x04009D0E RID: 40206
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] keyDefault = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("default");

		// Token: 0x04009D0F RID: 40207
		[PublicizedFrom(EAccessModifier.Private)]
		public int largestBuffer;
	}
}
