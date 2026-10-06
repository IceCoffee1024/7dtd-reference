using System;
using System.Net;
using JetBrains.Annotations;
using SandboxOptions;
using UnityEngine.Scripting;
using Utf8Json;
using Utf8Json.Resolvers;

namespace Webserver.WebAPI.APIs.ServerState
{
	// Token: 0x02001B08 RID: 6920
	[Preserve]
	public class SandboxSettings : AbsRestApi
	{
		// Token: 0x0600D05A RID: 53338 RVA: 0x004BA249 File Offset: 0x004B8449
		public SandboxSettings()
			: base("SandboxSettings")
		{
		}

		// Token: 0x0600D05B RID: 53339 RVA: 0x004BA258 File Offset: 0x004B8458
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

		// Token: 0x0600D05C RID: 53340 RVA: 0x004BA35C File Offset: 0x004B855C
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

		// Token: 0x0600D05D RID: 53341 RVA: 0x004BA530 File Offset: 0x004B8730
		[PublicizedFrom(EAccessModifier.Private)]
		public void logValue(ref JsonWriter _writer, int _index, object _value, string _localizedValue)
		{
			SandboxSettings.SandboxOptionValue sandboxOptionValue;
			sandboxOptionValue.Index = _index;
			sandboxOptionValue.Value = _value;
			sandboxOptionValue.LocalizedName = _localizedValue;
			JsonSerializer.Serialize<SandboxSettings.SandboxOptionValue>(ref _writer, sandboxOptionValue);
		}

		// Token: 0x04009DD6 RID: 40406
		[PublicizedFrom(EAccessModifier.Private)]
		public const string QueryParamCode = "code";

		// Token: 0x04009DD7 RID: 40407
		[PublicizedFrom(EAccessModifier.Private)]
		public const string QueryParamOnlyChanged = "onlyChanged";

		// Token: 0x04009DD8 RID: 40408
		[PublicizedFrom(EAccessModifier.Private)]
		public const string QueryParamDetailed = "detailed";

		// Token: 0x04009DD9 RID: 40409
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonCodeKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("code");

		// Token: 0x04009DDA RID: 40410
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptionsKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("options");

		// Token: 0x04009DDB RID: 40411
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptInternalKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("key");

		// Token: 0x04009DDC RID: 40412
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptDetailsKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("details");

		// Token: 0x04009DDD RID: 40413
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptTypeKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("type");

		// Token: 0x04009DDE RID: 40414
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptActiveValueKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("activeValue");

		// Token: 0x04009DDF RID: 40415
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptDefaultValueKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("defaultValue");

		// Token: 0x04009DE0 RID: 40416
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonOptValueSetKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("valueSet");

		// Token: 0x02001B09 RID: 6921
		[PublicizedFrom(EAccessModifier.Private)]
		public struct SandboxOptionInfo
		{
			// Token: 0x04009DE1 RID: 40417
			[UsedImplicitly]
			public string CategoryName;

			// Token: 0x04009DE2 RID: 40418
			[UsedImplicitly]
			public string InternalName;

			// Token: 0x04009DE3 RID: 40419
			[UsedImplicitly]
			public string LocalizedName;

			// Token: 0x04009DE4 RID: 40420
			[UsedImplicitly]
			public string Description;
		}

		// Token: 0x02001B0A RID: 6922
		[PublicizedFrom(EAccessModifier.Private)]
		public struct SandboxOptionValue
		{
			// Token: 0x04009DE5 RID: 40421
			[UsedImplicitly]
			public int Index;

			// Token: 0x04009DE6 RID: 40422
			[UsedImplicitly]
			public object Value;

			// Token: 0x04009DE7 RID: 40423
			[UsedImplicitly]
			public string LocalizedName;
		}
	}
}
