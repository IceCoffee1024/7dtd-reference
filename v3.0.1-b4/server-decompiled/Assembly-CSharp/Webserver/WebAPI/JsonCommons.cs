using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Utf8Json;

namespace Webserver.WebAPI
{
	// Token: 0x02001AA7 RID: 6823
	public static class JsonCommons
	{
		// Token: 0x0600CD29 RID: 52521 RVA: 0x004AD940 File Offset: 0x004ABB40
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

		// Token: 0x0600CD2A RID: 52522 RVA: 0x004AD998 File Offset: 0x004ABB98
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

		// Token: 0x0600CD2B RID: 52523 RVA: 0x004AD9F0 File Offset: 0x004ABBF0
		public static void WriteVector2I(ref JsonWriter _writer, Vector2i _position)
		{
			_writer.WriteRaw(JsonCommons.jsonKeyPositionX);
			_writer.WriteInt32(_position.x);
			_writer.WriteRaw(JsonCommons.jsonKeyPositionY);
			_writer.WriteInt32(_position.y);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CD2C RID: 52524 RVA: 0x004ADA26 File Offset: 0x004ABC26
		public static void WriteVector2(ref JsonWriter _writer, Vector2 _position)
		{
			_writer.WriteRaw(JsonCommons.jsonKeyPositionX);
			_writer.WriteSingle(_position.x);
			_writer.WriteRaw(JsonCommons.jsonKeyPositionY);
			_writer.WriteSingle(_position.y);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CD2D RID: 52525 RVA: 0x004ADA5C File Offset: 0x004ABC5C
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

		// Token: 0x0600CD2E RID: 52526 RVA: 0x004ADA9C File Offset: 0x004ABC9C
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

		// Token: 0x0600CD2F RID: 52527 RVA: 0x004ADB00 File Offset: 0x004ABD00
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

		// Token: 0x0600CD30 RID: 52528 RVA: 0x004ADB62 File Offset: 0x004ABD62
		public static void WriteDateTime(ref JsonWriter _writer, DateTime _dateTime)
		{
			_writer.WriteString(_dateTime.ToString("o"));
		}

		// Token: 0x0600CD31 RID: 52529 RVA: 0x004ADB78 File Offset: 0x004ABD78
		public static bool TryReadDateTime(IDictionary<string, object> _jsonInput, string _fieldName, out DateTime _result)
		{
			_result = default(DateTime);
			string text;
			return JsonCommons.TryGetJsonField(_jsonInput, _fieldName, out text) && DateTime.TryParse(text, null, DateTimeStyles.RoundtripKind, out _result);
		}

		// Token: 0x0600CD32 RID: 52530 RVA: 0x004ADBA8 File Offset: 0x004ABDA8
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

		// Token: 0x0600CD33 RID: 52531 RVA: 0x004ADBF8 File Offset: 0x004ABDF8
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

		// Token: 0x0600CD34 RID: 52532 RVA: 0x004ADC50 File Offset: 0x004ABE50
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

		// Token: 0x0600CD35 RID: 52533 RVA: 0x004ADC98 File Offset: 0x004ABE98
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

		// Token: 0x04009B96 RID: 39830
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPositionX = JsonWriter.GetEncodedPropertyNameWithBeginObject("x");

		// Token: 0x04009B97 RID: 39831
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPositionY = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("y");

		// Token: 0x04009B98 RID: 39832
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPositionZ = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("z");

		// Token: 0x04009B99 RID: 39833
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyDays = JsonWriter.GetEncodedPropertyNameWithBeginObject("days");

		// Token: 0x04009B9A RID: 39834
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyHours = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("hours");

		// Token: 0x04009B9B RID: 39835
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyMinutes = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("minutes");

		// Token: 0x04009B9C RID: 39836
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyCombinedString = JsonWriter.GetEncodedPropertyNameWithBeginObject("combinedString");

		// Token: 0x04009B9D RID: 39837
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPlatformId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("platformId");

		// Token: 0x04009B9E RID: 39838
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId");
	}
}
