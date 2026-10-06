using System;
using System.Net;
using UnityEngine.Profiling;
using Utf8Json;

namespace Webserver.WebAPI.APIs.ServerState
{
	// Token: 0x02001B07 RID: 6919
	public abstract class KeyValueListAbs : AbsRestApi
	{
		// Token: 0x0600D049 RID: 53321 RVA: 0x004B9F94 File Offset: 0x004B8194
		[PublicizedFrom(EAccessModifier.Protected)]
		public KeyValueListAbs(string _listName)
			: base(null)
		{
			this.buildSampler = CustomSampler.Create("JSON_" + _listName + "_BuildSampler", false);
		}

		// Token: 0x0600D04A RID: 53322 RVA: 0x004B9FBC File Offset: 0x004B81BC
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

		// Token: 0x0600D04B RID: 53323 RVA: 0x004BA025 File Offset: 0x004B8225
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

		// Token: 0x0600D04C RID: 53324 RVA: 0x004BA059 File Offset: 0x004B8259
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeInt(ref JsonWriter _writer, ref bool _first, string _key, int _value)
		{
			this.writeKeyType(ref _writer, ref _first, _key, "int");
			_writer.WriteRaw(KeyValueListAbs.keyValue);
			_writer.WriteInt32(_value);
		}

		// Token: 0x0600D04D RID: 53325 RVA: 0x004BA07C File Offset: 0x004B827C
		[PublicizedFrom(EAccessModifier.Protected)]
		public void addItem(ref JsonWriter _writer, ref bool _first, string _key, int _value)
		{
			this.writeInt(ref _writer, ref _first, _key, _value);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D04E RID: 53326 RVA: 0x004BA08F File Offset: 0x004B828F
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

		// Token: 0x0600D04F RID: 53327 RVA: 0x004BA0CB File Offset: 0x004B82CB
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeFloat(ref JsonWriter _writer, ref bool _first, string _key, float _value)
		{
			this.writeKeyType(ref _writer, ref _first, _key, "float");
			_writer.WriteRaw(KeyValueListAbs.keyValue);
			_writer.WriteSingle(_value);
		}

		// Token: 0x0600D050 RID: 53328 RVA: 0x004BA0EE File Offset: 0x004B82EE
		[PublicizedFrom(EAccessModifier.Protected)]
		public void addItem(ref JsonWriter _writer, ref bool _first, string _key, float _value)
		{
			this.writeFloat(ref _writer, ref _first, _key, _value);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D051 RID: 53329 RVA: 0x004BA101 File Offset: 0x004B8301
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

		// Token: 0x0600D052 RID: 53330 RVA: 0x004BA13D File Offset: 0x004B833D
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeBool(ref JsonWriter _writer, ref bool _first, string _key, bool _value)
		{
			this.writeKeyType(ref _writer, ref _first, _key, "bool");
			_writer.WriteRaw(KeyValueListAbs.keyValue);
			_writer.WriteBoolean(_value);
		}

		// Token: 0x0600D053 RID: 53331 RVA: 0x004BA160 File Offset: 0x004B8360
		[PublicizedFrom(EAccessModifier.Protected)]
		public void addItem(ref JsonWriter _writer, ref bool _first, string _key, bool _value)
		{
			this.writeBool(ref _writer, ref _first, _key, _value);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D054 RID: 53332 RVA: 0x004BA173 File Offset: 0x004B8373
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

		// Token: 0x0600D055 RID: 53333 RVA: 0x004BA1AF File Offset: 0x004B83AF
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeString(ref JsonWriter _writer, ref bool _first, string _key, string _value)
		{
			this.writeKeyType(ref _writer, ref _first, _key, "string");
			_writer.WriteRaw(KeyValueListAbs.keyValue);
			_writer.WriteString(_value);
		}

		// Token: 0x0600D056 RID: 53334 RVA: 0x004BA1D2 File Offset: 0x004B83D2
		[PublicizedFrom(EAccessModifier.Protected)]
		public void addItem(ref JsonWriter _writer, ref bool _first, string _key, string _value)
		{
			this.writeString(ref _writer, ref _first, _key, _value);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D057 RID: 53335 RVA: 0x004BA1E5 File Offset: 0x004B83E5
		[PublicizedFrom(EAccessModifier.Protected)]
		public void addItem(ref JsonWriter _writer, ref bool _first, string _key, string _value, string _default)
		{
			this.writeString(ref _writer, ref _first, _key, _value);
			_writer.WriteRaw(KeyValueListAbs.keyDefault);
			_writer.WriteString(_default);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D058 RID: 53336
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void iterateList(ref JsonWriter _writer, ref bool _first);

		// Token: 0x04009DD0 RID: 40400
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler buildSampler;

		// Token: 0x04009DD1 RID: 40401
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] keyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009DD2 RID: 40402
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] keyType = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("type");

		// Token: 0x04009DD3 RID: 40403
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] keyValue = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("value");

		// Token: 0x04009DD4 RID: 40404
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] keyDefault = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("default");

		// Token: 0x04009DD5 RID: 40405
		[PublicizedFrom(EAccessModifier.Private)]
		public int largestBuffer;
	}
}
