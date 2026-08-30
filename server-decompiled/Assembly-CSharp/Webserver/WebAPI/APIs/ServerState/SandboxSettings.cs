using System;
using System.Net;
using JetBrains.Annotations;
using SandboxOptions;
using UnityEngine.Scripting;
using Utf8Json;
using Utf8Json.Resolvers;

namespace Webserver.WebAPI.APIs.ServerState
{
	// Token: 0x02001ADC RID: 6876
	[Preserve]
	public class SandboxSettings : AbsRestApi
	{
		// Token: 0x0600CF02 RID: 52994 RVA: 0x004B5D7D File Offset: 0x004B3F7D
		public SandboxSettings()
			: base("SandboxSettings")
		{
		}

		// Token: 0x0600CF03 RID: 52995 RVA: 0x004B5D8C File Offset: 0x004B3F8C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			string text = _context.QueryParameters["code"] ?? GameStats.GetString(EnumGameStats.SandboxCode);
			bool flag;
			if (_context.QueryParameters["onlyChanged"] == null || !bool.TryParse(_context.QueryParameters["onlyChanged"], out flag))
			{
				flag = false;
			}
			bool flag2;
			if (_context.QueryParameters["detailed"] == null || !bool.TryParse(_context.QueryParameters["detailed"], out flag2))
			{
				flag2 = false;
			}
			SandboxOptionPreset sandboxOptionPreset = new SandboxOptionPreset();
			if (!SandboxOptionManager.Current.LoadOptionsFromCode(text, sandboxOptionPreset))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.InternalServerError, null, EApiErrorCode.NO_SANDBOX_DATA, null);
				return;
			}
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(SandboxSettings.jsonCodeKey);
			jsonWriter.WriteString(sandboxOptionPreset.SandboxCode);
			jsonWriter.WriteRaw(SandboxSettings.jsonOptionsKey);
			jsonWriter.WriteBeginArray();
			this.logSandbox(ref jsonWriter, sandboxOptionPreset, flag, flag2);
			jsonWriter.WriteEndArray();
			jsonWriter.WriteEndObject();
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x0600CF04 RID: 52996 RVA: 0x004B5E90 File Offset: 0x004B4090
		[PublicizedFrom(EAccessModifier.Private)]
		public void logSandbox(ref JsonWriter _writer, SandboxOptionPreset _preset, bool _onlyChanged, bool _detailed)
		{
			JsonSerializer.SetDefaultResolver(StandardResolver.CamelCase);
			bool flag = false;
			for (int i = 0; i < 165; i++)
			{
				global::SandboxOptions.SandboxOptions sandboxOptions = (global::SandboxOptions.SandboxOptions)i;
				BaseSandboxOption baseSandboxOption;
				if (SandboxOptionManager.Current.SandboxOptionsDict.TryGetValue(sandboxOptions, out baseSandboxOption))
				{
					int defaultIndex = baseSandboxOption.GetDefaultIndex();
					int num;
					if (!_preset.PresetValues.TryGetValue(sandboxOptions, out num))
					{
						if (_onlyChanged)
						{
							goto IL_01B8;
						}
						num = defaultIndex;
					}
					string valueTextFromIndex = baseSandboxOption.GetValueTextFromIndex(num, null);
					string valueTextFromIndex2 = baseSandboxOption.GetValueTextFromIndex(defaultIndex, null);
					if (flag)
					{
						_writer.WriteValueSeparator();
					}
					flag = true;
					_writer.WriteRaw(SandboxSettings.jsonOptInternalKey);
					_writer.WriteString(baseSandboxOption.Option.ToStringCached<global::SandboxOptions.SandboxOptions>());
					_writer.WriteRaw(SandboxSettings.jsonOptTypeKey);
					_writer.WriteString(baseSandboxOption.OptionType.ToStringCached<BaseSandboxOption.OptionTypes>());
					_writer.WriteRaw(SandboxSettings.jsonOptActiveValueKey);
					this.logValue(ref _writer, num, baseSandboxOption.GetValue(), valueTextFromIndex);
					_writer.WriteRaw(SandboxSettings.jsonOptDefaultValueKey);
					this.logValue(ref _writer, defaultIndex, baseSandboxOption.GetDefaultValue(), valueTextFromIndex2);
					if (_detailed)
					{
						_writer.WriteRaw(SandboxSettings.jsonOptValueSetKey);
						_writer.WriteBeginArray();
						SandboxOptionValueSet valueSet = baseSandboxOption.GetValueSet();
						for (int j = 0; j < valueSet.GetValueCount(); j++)
						{
							if (j > 0)
							{
								_writer.WriteValueSeparator();
							}
							object obj;
							if (!valueSet.GetValue(j, out obj))
							{
								Log.Error(string.Format("[Web] In {0}: Could not get value set element at index {1} for option {2}", "SandboxSettings", j, sandboxOptions.ToStringCached<global::SandboxOptions.SandboxOptions>()));
							}
							else
							{
								this.logValue(ref _writer, j, obj, valueSet.GetDisplayAtIndex(j, null));
							}
						}
						_writer.WriteEndArray();
						SandboxSettings.SandboxOptionInfo sandboxOptionInfo;
						sandboxOptionInfo.CategoryName = baseSandboxOption.CategoryName;
						sandboxOptionInfo.InternalName = baseSandboxOption.OptionName;
						sandboxOptionInfo.LocalizedName = baseSandboxOption.OptionNameText;
						sandboxOptionInfo.Description = baseSandboxOption.DescriptionText;
						_writer.WriteRaw(SandboxSettings.jsonOptDetailsKey);
						JsonSerializer.Serialize<SandboxSettings.SandboxOptionInfo>(ref _writer, sandboxOptionInfo);
					}
					_writer.WriteEndObject();
				}
				IL_01B8:;
			}
		}

		// Token: 0x0600CF05 RID: 52997 RVA: 0x004B6064 File Offset: 0x004B4264
		[PublicizedFrom(EAccessModifier.Private)]
		public void logValue(ref JsonWriter _writer, int _index, object _value, string _localizedValue)
		{
			SandboxSettings.SandboxOptionValue sandboxOptionValue;
			sandboxOptionValue.Index = _index;
			sandboxOptionValue.Value = _value;
			sandboxOptionValue.LocalizedName = _localizedValue;
			JsonSerializer.Serialize<SandboxSettings.SandboxOptionValue>(ref _writer, sandboxOptionValue);
		}

		// Token: 0x04009D10 RID: 40208
		[PublicizedFrom(EAccessModifier.Private)]
		public const string QueryParamCode = "code";

		// Token: 0x04009D11 RID: 40209
		[PublicizedFrom(EAccessModifier.Private)]
		public const string QueryParamOnlyChanged = "onlyChanged";

		// Token: 0x04009D12 RID: 40210
		[PublicizedFrom(EAccessModifier.Private)]
		public const string QueryParamDetailed = "detailed";

		// Token: 0x04009D13 RID: 40211
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonCodeKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("code");

		// Token: 0x04009D14 RID: 40212
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptionsKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("options");

		// Token: 0x04009D15 RID: 40213
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptInternalKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("key");

		// Token: 0x04009D16 RID: 40214
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptDetailsKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("details");

		// Token: 0x04009D17 RID: 40215
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptTypeKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("type");

		// Token: 0x04009D18 RID: 40216
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptActiveValueKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("activeValue");

		// Token: 0x04009D19 RID: 40217
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptDefaultValueKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("defaultValue");

		// Token: 0x04009D1A RID: 40218
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptValueSetKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("valueSet");

		// Token: 0x02001ADD RID: 6877
		[PublicizedFrom(EAccessModifier.Private)]
		public struct SandboxOptionInfo
		{
			// Token: 0x04009D1B RID: 40219
			[UsedImplicitly]
			public string CategoryName;

			// Token: 0x04009D1C RID: 40220
			[UsedImplicitly]
			public string InternalName;

			// Token: 0x04009D1D RID: 40221
			[UsedImplicitly]
			public string LocalizedName;

			// Token: 0x04009D1E RID: 40222
			[UsedImplicitly]
			public string Description;
		}

		// Token: 0x02001ADE RID: 6878
		[PublicizedFrom(EAccessModifier.Private)]
		public struct SandboxOptionValue
		{
			// Token: 0x04009D1F RID: 40223
			[UsedImplicitly]
			public int Index;

			// Token: 0x04009D20 RID: 40224
			[UsedImplicitly]
			public object Value;

			// Token: 0x04009D21 RID: 40225
			[UsedImplicitly]
			public string LocalizedName;
		}
	}
}
