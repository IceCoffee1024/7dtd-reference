using System;
using System.Net;
using JetBrains.Annotations;
using SandboxOptions;
using UnityEngine.Scripting;
using Utf8Json;
using Utf8Json.Resolvers;

namespace Webserver.WebAPI.APIs.ServerState
{
	// Token: 0x02001AB5 RID: 6837
	[Preserve]
	public class SandboxSettings : AbsRestApi
	{
		// Token: 0x0600CD79 RID: 52601 RVA: 0x004AF989 File Offset: 0x004ADB89
		public SandboxSettings()
			: base("SandboxSettings")
		{
		}

		// Token: 0x0600CD7A RID: 52602 RVA: 0x004AF998 File Offset: 0x004ADB98
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

		// Token: 0x0600CD7B RID: 52603 RVA: 0x004AFA9C File Offset: 0x004ADC9C
		[PublicizedFrom(EAccessModifier.Private)]
		public void logSandbox(ref JsonWriter _writer, SandboxOptionPreset _preset, bool _onlyChanged, bool _detailed)
		{
			JsonSerializer.SetDefaultResolver(StandardResolver.CamelCase);
			bool flag = false;
			for (int i = 0; i < 152; i++)
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
							goto IL_01B5;
						}
						num = defaultIndex;
					}
					string valueTextFromIndex = baseSandboxOption.GetValueTextFromIndex(num);
					string valueTextFromIndex2 = baseSandboxOption.GetValueTextFromIndex(defaultIndex);
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
								this.logValue(ref _writer, j, obj, valueSet.GetDisplayAtIndex(j));
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
				IL_01B5:;
			}
		}

		// Token: 0x0600CD7C RID: 52604 RVA: 0x004AFC70 File Offset: 0x004ADE70
		[PublicizedFrom(EAccessModifier.Private)]
		public void logValue(ref JsonWriter _writer, int _index, object _value, string _localizedValue)
		{
			SandboxSettings.SandboxOptionValue sandboxOptionValue;
			sandboxOptionValue.Index = _index;
			sandboxOptionValue.Value = _value;
			sandboxOptionValue.LocalizedName = _localizedValue;
			JsonSerializer.Serialize<SandboxSettings.SandboxOptionValue>(ref _writer, sandboxOptionValue);
		}

		// Token: 0x04009BDF RID: 39903
		[PublicizedFrom(EAccessModifier.Private)]
		public const string QueryParamCode = "code";

		// Token: 0x04009BE0 RID: 39904
		[PublicizedFrom(EAccessModifier.Private)]
		public const string QueryParamOnlyChanged = "onlyChanged";

		// Token: 0x04009BE1 RID: 39905
		[PublicizedFrom(EAccessModifier.Private)]
		public const string QueryParamDetailed = "detailed";

		// Token: 0x04009BE2 RID: 39906
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonCodeKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("code");

		// Token: 0x04009BE3 RID: 39907
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptionsKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("options");

		// Token: 0x04009BE4 RID: 39908
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptInternalKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("key");

		// Token: 0x04009BE5 RID: 39909
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptDetailsKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("details");

		// Token: 0x04009BE6 RID: 39910
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptTypeKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("type");

		// Token: 0x04009BE7 RID: 39911
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptActiveValueKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("activeValue");

		// Token: 0x04009BE8 RID: 39912
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptDefaultValueKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("defaultValue");

		// Token: 0x04009BE9 RID: 39913
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptValueSetKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("valueSet");

		// Token: 0x02001AB6 RID: 6838
		[PublicizedFrom(EAccessModifier.Private)]
		public struct SandboxOptionInfo
		{
			// Token: 0x04009BEA RID: 39914
			[UsedImplicitly]
			public string CategoryName;

			// Token: 0x04009BEB RID: 39915
			[UsedImplicitly]
			public string InternalName;

			// Token: 0x04009BEC RID: 39916
			[UsedImplicitly]
			public string LocalizedName;

			// Token: 0x04009BED RID: 39917
			[UsedImplicitly]
			public string Description;
		}

		// Token: 0x02001AB7 RID: 6839
		[PublicizedFrom(EAccessModifier.Private)]
		public struct SandboxOptionValue
		{
			// Token: 0x04009BEE RID: 39918
			[UsedImplicitly]
			public int Index;

			// Token: 0x04009BEF RID: 39919
			[UsedImplicitly]
			public object Value;

			// Token: 0x04009BF0 RID: 39920
			[UsedImplicitly]
			public string LocalizedName;
		}
	}
}
