using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Utf8Json;

namespace Webserver.WebAPI
{
	// Token: 0x02001AFA RID: 6906
	public static class JsonCommons
	{
		// Token: 0x0600D00A RID: 53258 RVA: 0x004B8200 File Offset: 0x004B6400
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

		// Token: 0x0600D00B RID: 53259 RVA: 0x004B8258 File Offset: 0x004B6458
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

		// Token: 0x0600D00C RID: 53260 RVA: 0x004B82B0 File Offset: 0x004B64B0
		public static void WriteVector2I(ref JsonWriter _writer, Vector2i _position)
		{
			_writer.WriteRaw(JsonCommons.jsonKeyPositionX);
			_writer.WriteInt32(_position.x);
			_writer.WriteRaw(JsonCommons.jsonKeyPositionY);
			_writer.WriteInt32(_position.y);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D00D RID: 53261 RVA: 0x004B82E6 File Offset: 0x004B64E6
		public static void WriteVector2(ref JsonWriter _writer, Vector2 _position)
		{
			_writer.WriteRaw(JsonCommons.jsonKeyPositionX);
			_writer.WriteSingle(_position.x);
			_writer.WriteRaw(JsonCommons.jsonKeyPositionY);
			_writer.WriteSingle(_position.y);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D00E RID: 53262 RVA: 0x004B831C File Offset: 0x004B651C
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

		// Token: 0x0600D00F RID: 53263 RVA: 0x004B835C File Offset: 0x004B655C
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

		// Token: 0x0600D010 RID: 53264 RVA: 0x004B83C0 File Offset: 0x004B65C0
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

		// Token: 0x0600D011 RID: 53265 RVA: 0x004B8422 File Offset: 0x004B6622
		public static void WriteDateTime(ref JsonWriter _writer, DateTime _dateTime)
		{
			_writer.WriteString(_dateTime.ToString("o"));
		}

		// Token: 0x0600D012 RID: 53266 RVA: 0x004B8438 File Offset: 0x004B6638
		public static bool TryReadDateTime(IDictionary<string, object> _jsonInput, string _fieldName, out DateTime _result)
		{
			_result = default(DateTime);
			string text;
			return JsonCommons.TryGetJsonField(_jsonInput, _fieldName, out text) && DateTime.TryParse(text, null, DateTimeStyles.RoundtripKind, out _result);
		}

		// Token: 0x0600D013 RID: 53267 RVA: 0x004B8468 File Offset: 0x004B6668
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

		// Token: 0x0600D014 RID: 53268 RVA: 0x004B84B8 File Offset: 0x004B66B8
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

		// Token: 0x0600D015 RID: 53269 RVA: 0x004B8510 File Offset: 0x004B6710
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

		// Token: 0x0600D016 RID: 53270 RVA: 0x004B8558 File Offset: 0x004B6758
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

		// Token: 0x04009D8D RID: 40333
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPositionX = JsonWriter.GetEncodedPropertyNameWithBeginObject("x");

		// Token: 0x04009D8E RID: 40334
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPositionY = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("y");

		// Token: 0x04009D8F RID: 40335
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPositionZ = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("z");

		// Token: 0x04009D90 RID: 40336
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyDays = JsonWriter.GetEncodedPropertyNameWithBeginObject("days");

		// Token: 0x04009D91 RID: 40337
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyHours = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("hours");

		// Token: 0x04009D92 RID: 40338
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyMinutes = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("minutes");

		// Token: 0x04009D93 RID: 40339
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyCombinedString = JsonWriter.GetEncodedPropertyNameWithBeginObject("combinedString");

		// Token: 0x04009D94 RID: 40340
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPlatformId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("platformId");

		// Token: 0x04009D95 RID: 40341
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId");
	}
}
