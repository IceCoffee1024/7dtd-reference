using System;
using System.Net;
using UnityEngine.Profiling;
using Utf8Json;

namespace Webserver.WebAPI.APIs.ServerState
{
	// Token: 0x02001B38 RID: 6968
	public abstract class KeyValueListAbs : AbsRestApi
	{
		// Token: 0x0600D15D RID: 53597 RVA: 0x004BDA90 File Offset: 0x004BBC90
		[PublicizedFrom(EAccessModifier.Protected)]
		public KeyValueListAbs(string _listName)
			: base(null)
		{
			this.buildSampler = CustomSampler.Create("JSON_" + _listName + "_BuildSampler", false);
		}

		// Token: 0x0600D15E RID: 53598 RVA: 0x004BDAB8 File Offset: 0x004BBCB8
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

		// Token: 0x0600D15F RID: 53599 RVA: 0x004BDB21 File Offset: 0x004BBD21
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

		// Token: 0x0600D160 RID: 53600 RVA: 0x004BDB55 File Offset: 0x004BBD55
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeInt(ref JsonWriter _writer, ref bool _first, string _key, int _value)
		{
			this.writeKeyType(ref _writer, ref _first, _key, "int");
			_writer.WriteRaw(KeyValueListAbs.keyValue);
			_writer.WriteInt32(_value);
		}

		// Token: 0x0600D161 RID: 53601 RVA: 0x004BDB78 File Offset: 0x004BBD78
		[PublicizedFrom(EAccessModifier.Protected)]
		public void addItem(ref JsonWriter _writer, ref bool _first, string _key, int _value)
		{
			this.writeInt(ref _writer, ref _first, _key, _value);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D162 RID: 53602 RVA: 0x004BDB8B File Offset: 0x004BBD8B
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

		// Token: 0x0600D163 RID: 53603 RVA: 0x004BDBC7 File Offset: 0x004BBDC7
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeFloat(ref JsonWriter _writer, ref bool _first, string _key, float _value)
		{
			this.writeKeyType(ref _writer, ref _first, _key, "float");
			_writer.WriteRaw(KeyValueListAbs.keyValue);
			_writer.WriteSingle(_value);
		}

		// Token: 0x0600D164 RID: 53604 RVA: 0x004BDBEA File Offset: 0x004BBDEA
		[PublicizedFrom(EAccessModifier.Protected)]
		public void addItem(ref JsonWriter _writer, ref bool _first, string _key, float _value)
		{
			this.writeFloat(ref _writer, ref _first, _key, _value);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D165 RID: 53605 RVA: 0x004BDBFD File Offset: 0x004BBDFD
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

		// Token: 0x0600D166 RID: 53606 RVA: 0x004BDC39 File Offset: 0x004BBE39
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeBool(ref JsonWriter _writer, ref bool _first, string _key, bool _value)
		{
			this.writeKeyType(ref _writer, ref _first, _key, "bool");
			_writer.WriteRaw(KeyValueListAbs.keyValue);
			_writer.WriteBoolean(_value);
		}

		// Token: 0x0600D167 RID: 53607 RVA: 0x004BDC5C File Offset: 0x004BBE5C
		[PublicizedFrom(EAccessModifier.Protected)]
		public void addItem(ref JsonWriter _writer, ref bool _first, string _key, bool _value)
		{
			this.writeBool(ref _writer, ref _first, _key, _value);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D168 RID: 53608 RVA: 0x004BDC6F File Offset: 0x004BBE6F
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

		// Token: 0x0600D169 RID: 53609 RVA: 0x004BDCAB File Offset: 0x004BBEAB
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeString(ref JsonWriter _writer, ref bool _first, string _key, string _value)
		{
			this.writeKeyType(ref _writer, ref _first, _key, "string");
			_writer.WriteRaw(KeyValueListAbs.keyValue);
			_writer.WriteString(_value);
		}

		// Token: 0x0600D16A RID: 53610 RVA: 0x004BDCCE File Offset: 0x004BBECE
		[PublicizedFrom(EAccessModifier.Protected)]
		public void addItem(ref JsonWriter _writer, ref bool _first, string _key, string _value)
		{
			this.writeString(ref _writer, ref _first, _key, _value);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D16B RID: 53611 RVA: 0x004BDCE1 File Offset: 0x004BBEE1
		[PublicizedFrom(EAccessModifier.Protected)]
		public void addItem(ref JsonWriter _writer, ref bool _first, string _key, string _value, string _default)
		{
			this.writeString(ref _writer, ref _first, _key, _value);
			_writer.WriteRaw(KeyValueListAbs.keyDefault);
			_writer.WriteString(_default);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D16C RID: 53612
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void iterateList(ref JsonWriter _writer, ref bool _first);

		// Token: 0x04009EB9 RID: 40633
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler buildSampler;

		// Token: 0x04009EBA RID: 40634
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] keyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009EBB RID: 40635
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] keyType = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("type");

		// Token: 0x04009EBC RID: 40636
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] keyValue = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("value");

		// Token: 0x04009EBD RID: 40637
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] keyDefault = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("default");

		// Token: 0x04009EBE RID: 40638
		[PublicizedFrom(EAccessModifier.Private)]
		public int largestBuffer;
	}
}
