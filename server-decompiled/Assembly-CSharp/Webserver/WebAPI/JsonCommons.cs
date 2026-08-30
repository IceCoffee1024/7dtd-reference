using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Utf8Json;

namespace Webserver.WebAPI
{
	// Token: 0x02001ACE RID: 6862
	public static class JsonCommons
	{
		// Token: 0x0600CEB2 RID: 52914 RVA: 0x004B3D34 File Offset: 0x004B1F34
		public static void WriteVector3I(ref JsonWriter _writer, Vector3i _position)
		{
			_writer.WriteRaw(JsonCommons.jsonKeyPositionX);
			_writer.WriteInt32(_position.x);
			_writer.WriteRaw(JsonCommons.jsonKeyPositionY);
			_writer.WriteInt32(_position.y);
			_writer.WriteRaw(JsonCommons.jsonKeyPositionZ);
			_writer.WriteInt32(_position.z);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CEB3 RID: 52915 RVA: 0x004B3D8C File Offset: 0x004B1F8C
		public static void WriteVector3(ref JsonWriter _writer, Vector3 _position)
		{
			_writer.WriteRaw(JsonCommons.jsonKeyPositionX);
			_writer.WriteSingle(_position.x);
			_writer.WriteRaw(JsonCommons.jsonKeyPositionY);
			_writer.WriteSingle(_position.y);
			_writer.WriteRaw(JsonCommons.jsonKeyPositionZ);
			_writer.WriteSingle(_position.z);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CEB4 RID: 52916 RVA: 0x004B3DE4 File Offset: 0x004B1FE4
		public static void WriteVector2I(ref JsonWriter _writer, Vector2i _position)
		{
			_writer.WriteRaw(JsonCommons.jsonKeyPositionX);
			_writer.WriteInt32(_position.x);
			_writer.WriteRaw(JsonCommons.jsonKeyPositionY);
			_writer.WriteInt32(_position.y);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CEB5 RID: 52917 RVA: 0x004B3E1A File Offset: 0x004B201A
		public static void WriteVector2(ref JsonWriter _writer, Vector2 _position)
		{
			_writer.WriteRaw(JsonCommons.jsonKeyPositionX);
			_writer.WriteSingle(_position.x);
			_writer.WriteRaw(JsonCommons.jsonKeyPositionY);
			_writer.WriteSingle(_position.y);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CEB6 RID: 52918 RVA: 0x004B3E50 File Offset: 0x004B2050
		public static void WriteGameTimeObject(ref JsonWriter _writer, int _days, int _hours, int _minutes)
		{
			_writer.WriteRaw(JsonCommons.jsonKeyDays);
			_writer.WriteInt32(_days);
			_writer.WriteRaw(JsonCommons.jsonKeyHours);
			_writer.WriteInt32(_hours);
			_writer.WriteRaw(JsonCommons.jsonKeyMinutes);
			_writer.WriteInt32(_minutes);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CEB7 RID: 52919 RVA: 0x004B3E90 File Offset: 0x004B2090
		public static void WritePlatformUserIdentifier(ref JsonWriter _writer, PlatformUserIdentifierAbs _userIdentifier)
		{
			if (_userIdentifier == null)
			{
				_writer.WriteNull();
				return;
			}
			_writer.WriteRaw(JsonCommons.jsonKeyCombinedString);
			_writer.WriteString(_userIdentifier.CombinedString);
			_writer.WriteRaw(JsonCommons.jsonKeyPlatformId);
			_writer.WriteString(_userIdentifier.PlatformIdentifierString);
			_writer.WriteRaw(JsonCommons.jsonKeyUserId);
			_writer.WriteString(_userIdentifier.ReadablePlatformUserIdentifier);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CEB8 RID: 52920 RVA: 0x004B3EF4 File Offset: 0x004B20F4
		public static bool TryReadPlatformUserIdentifier(IDictionary<string, object> _jsonInput, out PlatformUserIdentifierAbs _userIdentifier)
		{
			string text;
			if (JsonCommons.TryGetJsonField(_jsonInput, "combinedString", out text))
			{
				_userIdentifier = PlatformUserIdentifierAbs.FromCombinedString(text, false);
				if (_userIdentifier != null)
				{
					return true;
				}
			}
			string text2;
			if (!JsonCommons.TryGetJsonField(_jsonInput, "platformId", out text2))
			{
				_userIdentifier = null;
				return false;
			}
			string text3;
			if (!JsonCommons.TryGetJsonField(_jsonInput, "userId", out text3))
			{
				_userIdentifier = null;
				return false;
			}
			_userIdentifier = PlatformUserIdentifierAbs.FromPlatformAndId(text2, text3, false);
			return _userIdentifier != null;
		}

		// Token: 0x0600CEB9 RID: 52921 RVA: 0x004B3F56 File Offset: 0x004B2156
		public static void WriteDateTime(ref JsonWriter _writer, DateTime _dateTime)
		{
			_writer.WriteString(_dateTime.ToString("o"));
		}

		// Token: 0x0600CEBA RID: 52922 RVA: 0x004B3F6C File Offset: 0x004B216C
		public static bool TryReadDateTime(IDictionary<string, object> _jsonInput, string _fieldName, out DateTime _result)
		{
			_result = default(DateTime);
			string text;
			return JsonCommons.TryGetJsonField(_jsonInput, _fieldName, out text) && DateTime.TryParse(text, null, DateTimeStyles.RoundtripKind, out _result);
		}

		// Token: 0x0600CEBB RID: 52923 RVA: 0x004B3F9C File Offset: 0x004B219C
		public static bool TryGetJsonField(IDictionary<string, object> _jsonObject, string _fieldName, out int _value)
		{
			_value = 0;
			object obj;
			if (!_jsonObject.TryGetValue(_fieldName, out obj))
			{
				return false;
			}
			if (obj is double)
			{
				double num = (double)obj;
				bool flag;
				try
				{
					_value = (int)num;
					flag = true;
				}
				catch (Exception)
				{
					flag = false;
				}
				return flag;
			}
			return false;
		}

		// Token: 0x0600CEBC RID: 52924 RVA: 0x004B3FEC File Offset: 0x004B21EC
		public static bool TryGetJsonField(IDictionary<string, object> _jsonObject, string _fieldName, out double _value)
		{
			_value = 0.0;
			object obj;
			if (!_jsonObject.TryGetValue(_fieldName, out obj))
			{
				return false;
			}
			if (obj is double)
			{
				double num = (double)obj;
				bool flag;
				try
				{
					_value = num;
					flag = true;
				}
				catch (Exception)
				{
					flag = false;
				}
				return flag;
			}
			return false;
		}

		// Token: 0x0600CEBD RID: 52925 RVA: 0x004B4044 File Offset: 0x004B2244
		public static bool TryGetJsonField(IDictionary<string, object> _jsonObject, string _fieldName, out string _value)
		{
			_value = null;
			object obj;
			if (!_jsonObject.TryGetValue(_fieldName, out obj))
			{
				return false;
			}
			string text = obj as string;
			if (text == null)
			{
				return false;
			}
			bool flag;
			try
			{
				_value = text;
				flag = true;
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600CEBE RID: 52926 RVA: 0x004B408C File Offset: 0x004B228C
		public static bool TryGetJsonField(IDictionary<string, object> _jsonObject, string _fieldName, out IDictionary<string, object> _value)
		{
			_value = null;
			object obj;
			if (!_jsonObject.TryGetValue(_fieldName, out obj))
			{
				return false;
			}
			IDictionary<string, object> dictionary = obj as IDictionary<string, object>;
			if (dictionary == null)
			{
				return false;
			}
			bool flag;
			try
			{
				_value = dictionary;
				flag = true;
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x04009CC7 RID: 40135
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPositionX = JsonWriter.GetEncodedPropertyNameWithBeginObject("x");

		// Token: 0x04009CC8 RID: 40136
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPositionY = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("y");

		// Token: 0x04009CC9 RID: 40137
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPositionZ = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("z");

		// Token: 0x04009CCA RID: 40138
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyDays = JsonWriter.GetEncodedPropertyNameWithBeginObject("days");

		// Token: 0x04009CCB RID: 40139
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyHours = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("hours");

		// Token: 0x04009CCC RID: 40140
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyMinutes = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("minutes");

		// Token: 0x04009CCD RID: 40141
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyCombinedString = JsonWriter.GetEncodedPropertyNameWithBeginObject("combinedString");

		// Token: 0x04009CCE RID: 40142
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPlatformId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("platformId");

		// Token: 0x04009CCF RID: 40143
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId");
	}
}
