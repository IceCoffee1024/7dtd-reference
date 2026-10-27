using System;
using System.Net;
using JetBrains.Annotations;
using SandboxOptions;
using UnityEngine.Scripting;
using Utf8Json;
using Utf8Json.Resolvers;

namespace Webserver.WebAPI.APIs.ServerState
{
	// Token: 0x02001B39 RID: 6969
	[Preserve]
	public class SandboxSettings : AbsRestApi
	{
		// Token: 0x0600D16E RID: 53614 RVA: 0x004BDD45 File Offset: 0x004BBF45
		public SandboxSettings()
			: base("SandboxSettings")
		{
		}

		// Token: 0x0600D16F RID: 53615 RVA: 0x004BDD54 File Offset: 0x004BBF54
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

		// Token: 0x0600D170 RID: 53616 RVA: 0x004BDE58 File Offset: 0x004BC058
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

		// Token: 0x0600D171 RID: 53617 RVA: 0x004BE02C File Offset: 0x004BC22C
		[PublicizedFrom(EAccessModifier.Private)]
		public void logValue(ref JsonWriter _writer, int _index, object _value, string _localizedValue)
		{
			SandboxSettings.SandboxOptionValue sandboxOptionValue;
			sandboxOptionValue.Index = _index;
			sandboxOptionValue.Value = _value;
			sandboxOptionValue.LocalizedName = _localizedValue;
			JsonSerializer.Serialize<SandboxSettings.SandboxOptionValue>(ref _writer, sandboxOptionValue);
		}

		// Token: 0x04009EBF RID: 40639
		[PublicizedFrom(EAccessModifier.Private)]
		public const string QueryParamCode = "code";

		// Token: 0x04009EC0 RID: 40640
		[PublicizedFrom(EAccessModifier.Private)]
		public const string QueryParamOnlyChanged = "onlyChanged";

		// Token: 0x04009EC1 RID: 40641
		[PublicizedFrom(EAccessModifier.Private)]
		public const string QueryParamDetailed = "detailed";

		// Token: 0x04009EC2 RID: 40642
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonCodeKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("code");

		// Token: 0x04009EC3 RID: 40643
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptionsKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("options");

		// Token: 0x04009EC4 RID: 40644
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptInternalKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("key");

		// Token: 0x04009EC5 RID: 40645
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptDetailsKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("details");

		// Token: 0x04009EC6 RID: 40646
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptTypeKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("type");

		// Token: 0x04009EC7 RID: 40647
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptActiveValueKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("activeValue");

		// Token: 0x04009EC8 RID: 40648
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptDefaultValueKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("defaultValue");

		// Token: 0x04009EC9 RID: 40649
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptValueSetKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("valueSet");

		// Token: 0x02001B3A RID: 6970
		[PublicizedFrom(EAccessModifier.Private)]
		public struct SandboxOptionInfo
		{
			// Token: 0x04009ECA RID: 40650
			[UsedImplicitly]
			public string CategoryName;

			// Token: 0x04009ECB RID: 40651
			[UsedImplicitly]
			public string InternalName;

			// Token: 0x04009ECC RID: 40652
			[UsedImplicitly]
			public string LocalizedName;

			// Token: 0x04009ECD RID: 40653
			[UsedImplicitly]
			public string Description;
		}

		// Token: 0x02001B3B RID: 6971
		[PublicizedFrom(EAccessModifier.Private)]
		public struct SandboxOptionValue
		{
			// Token: 0x04009ECE RID: 40654
			[UsedImplicitly]
			public int Index;

			// Token: 0x04009ECF RID: 40655
			[UsedImplicitly]
			public object Value;

			// Token: 0x04009ED0 RID: 40656
			[UsedImplicitly]
			public string LocalizedName;
		}
	}
}
