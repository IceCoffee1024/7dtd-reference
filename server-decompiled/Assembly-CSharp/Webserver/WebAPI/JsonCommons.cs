using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Utf8Json;

namespace Webserver.WebAPI
{
	// Token: 0x02001B2B RID: 6955
	public static class JsonCommons
	{
		// Token: 0x0600D11E RID: 53534 RVA: 0x004BBCFC File Offset: 0x004B9EFC
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

		// Token: 0x0600D11F RID: 53535 RVA: 0x004BBD54 File Offset: 0x004B9F54
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

		// Token: 0x0600D120 RID: 53536 RVA: 0x004BBDAC File Offset: 0x004B9FAC
		public static void WriteVector2I(ref JsonWriter _writer, Vector2i _position)
		{
			_writer.WriteRaw(JsonCommons.jsonKeyPositionX);
			_writer.WriteInt32(_position.x);
			_writer.WriteRaw(JsonCommons.jsonKeyPositionY);
			_writer.WriteInt32(_position.y);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D121 RID: 53537 RVA: 0x004BBDE2 File Offset: 0x004B9FE2
		public static void WriteVector2(ref JsonWriter _writer, Vector2 _position)
		{
			_writer.WriteRaw(JsonCommons.jsonKeyPositionX);
			_writer.WriteSingle(_position.x);
			_writer.WriteRaw(JsonCommons.jsonKeyPositionY);
			_writer.WriteSingle(_position.y);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D122 RID: 53538 RVA: 0x004BBE18 File Offset: 0x004BA018
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

		// Token: 0x0600D123 RID: 53539 RVA: 0x004BBE58 File Offset: 0x004BA058
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

		// Token: 0x0600D124 RID: 53540 RVA: 0x004BBEBC File Offset: 0x004BA0BC
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

		// Token: 0x0600D125 RID: 53541 RVA: 0x004BBF1E File Offset: 0x004BA11E
		public static void WriteDateTime(ref JsonWriter _writer, DateTime _dateTime)
		{
			_writer.WriteString(_dateTime.ToString("o"));
		}

		// Token: 0x0600D126 RID: 53542 RVA: 0x004BBF34 File Offset: 0x004BA134
		public static bool TryReadDateTime(IDictionary<string, object> _jsonInput, string _fieldName, out DateTime _result)
		{
			_result = default(DateTime);
			string text;
			return JsonCommons.TryGetJsonField(_jsonInput, _fieldName, out text) && DateTime.TryParse(text, null, DateTimeStyles.RoundtripKind, out _result);
		}

		// Token: 0x0600D127 RID: 53543 RVA: 0x004BBF64 File Offset: 0x004BA164
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

		// Token: 0x0600D128 RID: 53544 RVA: 0x004BBFB4 File Offset: 0x004BA1B4
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

		// Token: 0x0600D129 RID: 53545 RVA: 0x004BC00C File Offset: 0x004BA20C
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

		// Token: 0x0600D12A RID: 53546 RVA: 0x004BC054 File Offset: 0x004BA254
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

		// Token: 0x04009E76 RID: 40566
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPositionX = JsonWriter.GetEncodedPropertyNameWithBeginObject("x");

		// Token: 0x04009E77 RID: 40567
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPositionY = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("y");

		// Token: 0x04009E78 RID: 40568
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPositionZ = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("z");

		// Token: 0x04009E79 RID: 40569
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyDays = JsonWriter.GetEncodedPropertyNameWithBeginObject("days");

		// Token: 0x04009E7A RID: 40570
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyHours = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("hours");

		// Token: 0x04009E7B RID: 40571
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyMinutes = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("minutes");

		// Token: 0x04009E7C RID: 40572
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyCombinedString = JsonWriter.GetEncodedPropertyNameWithBeginObject("combinedString");

		// Token: 0x04009E7D RID: 40573
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPlatformId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("platformId");

		// Token: 0x04009E7E RID: 40574
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId");
	}
}
