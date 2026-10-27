using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Webserver.UrlHandlers;

namespace Webserver.WebAPI
{
	// Token: 0x02001B2D RID: 6957
	public class OpenApiHelpers
	{
		// Token: 0x0600D12E RID: 53550 RVA: 0x004BC18D File Offset: 0x004BA38D
		public OpenApiHelpers()
		{
			this.loadMainSpec();
			Web.ServerInitialized += delegate(Web _)
			{
				this.buildMainSpecRefs();
			};
		}

		// Token: 0x0600D12F RID: 53551 RVA: 0x004BC1B8 File Offset: 0x004BA3B8
		[PublicizedFrom(EAccessModifier.Private)]
		public void loadMainSpec()
		{
			Assembly assembly = base.GetType().Assembly;
			string text = this.loadSpecFileForAssembly(assembly, "openapi.master.yaml");
			if (text == null)
			{
				Log.Warning(string.Format("[Web] Failed loading main OpenAPI spec from assembly '{0}'", assembly));
				return;
			}
			this.specs.Add("openapi.yaml", new OpenApiHelpers.OpenApiSpec(text, null));
		}

		// Token: 0x0600D130 RID: 53552 RVA: 0x004BC20C File Offset: 0x004BA40C
		[PublicizedFrom(EAccessModifier.Private)]
		public void buildMainSpecRefs()
		{
			string text;
			if (!this.TryGetOpenApiSpec(null, out text))
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder(text);
			foreach (KeyValuePair<string, OpenApiHelpers.OpenApiSpec> keyValuePair in this.specs)
			{
				string text2;
				OpenApiHelpers.OpenApiSpec openApiSpec;
				keyValuePair.Deconstruct(out text2, out openApiSpec);
				string text3 = text2;
				OpenApiHelpers.OpenApiSpec openApiSpec2 = openApiSpec;
				if (!text3.Equals("openapi.yaml") && openApiSpec2.ExportedPaths != null && openApiSpec2.ExportedPaths.Count >= 1)
				{
					foreach (KeyValuePair<string, string> keyValuePair2 in openApiSpec2.ExportedPaths)
					{
						string text4;
						keyValuePair2.Deconstruct(out text2, out text4);
						string text5 = text2;
						string text6 = text4;
						this.writePath(stringBuilder, text3, text5, text6);
					}
				}
			}
			this.specs["openapi.yaml"] = new OpenApiHelpers.OpenApiSpec(stringBuilder.ToString(), null);
			Log.Out("[Web] OpenAPI preparation done");
		}

		// Token: 0x0600D131 RID: 53553 RVA: 0x004BC32C File Offset: 0x004BA52C
		[PublicizedFrom(EAccessModifier.Private)]
		public void writePath(StringBuilder _sb, string _apiSpecName, string _exportedPath, string _rebasedPath)
		{
			_sb.AppendLine("  " + (_rebasedPath ?? _exportedPath) + ":");
			_sb.Append("    $ref: './" + _apiSpecName + "#/paths/");
			this.writeJsonPointerEncodedPath(_sb, _exportedPath);
			_sb.AppendLine("'");
		}

		// Token: 0x0600D132 RID: 53554 RVA: 0x004BC381 File Offset: 0x004BA581
		public void LoadOpenApiSpec(AbsWebAPI _api)
		{
			this.loadOpenApiSpec(_api.GetType().Assembly, _api.Name, null);
		}

		// Token: 0x0600D133 RID: 53555 RVA: 0x004BC39C File Offset: 0x004BA59C
		public void LoadOpenApiSpec(AbsHandler _pathHandler)
		{
			Type type = _pathHandler.GetType();
			this.loadOpenApiSpec(type.Assembly, type.Name, _pathHandler.UrlBasePath);
		}

		// Token: 0x0600D134 RID: 53556 RVA: 0x004BC3C8 File Offset: 0x004BA5C8
		public void RegisterCustomSpec(Assembly _assembly, string _apiSpecName, string _replaceBasePath = null)
		{
			this.loadOpenApiSpec(_assembly, _apiSpecName, _replaceBasePath);
		}

		// Token: 0x0600D135 RID: 53557 RVA: 0x004BC3D4 File Offset: 0x004BA5D4
		[PublicizedFrom(EAccessModifier.Private)]
		public void loadOpenApiSpec(Assembly _containingAssembly, string _apiName, string _basePath)
		{
			string text = _apiName + ".openapi.yaml";
			string text2 = this.loadSpecFileForAssembly(_containingAssembly, text);
			if (text2 == null)
			{
				return;
			}
			OpenApiHelpers.OpenApiSpec openApiSpec = new OpenApiHelpers.OpenApiSpec(text2, this.findExportedPaths(text2, _basePath));
			this.specs.Add(text, openApiSpec);
		}

		// Token: 0x0600D136 RID: 53558 RVA: 0x004BC418 File Offset: 0x004BA618
		[PublicizedFrom(EAccessModifier.Private)]
		public string loadSpecFileForAssembly(Assembly _containingAssembly, string _specFileName)
		{
			Assembly assembly = typeof(GameManager).Assembly;
			if (_containingAssembly != assembly)
			{
				return ResourceHelpers.GetManifestResourceText(_containingAssembly, _specFileName, true);
			}
			TextAsset textAsset = (TextAsset)Resources.Load("Data/Webserver/OpenApiSpecs/" + Path.GetFileNameWithoutExtension(_specFileName));
			if (textAsset == null)
			{
				return null;
			}
			return textAsset.text;
		}

		// Token: 0x0600D137 RID: 53559 RVA: 0x004BC46C File Offset: 0x004BA66C
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<string, string> findExportedPaths(string _spec, string _replaceBasePath = null)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Dictionary<string, string> dictionary2;
			using (TextReader textReader = new StringReader(_spec))
			{
				bool flag = false;
				string text;
				while ((text = textReader.ReadLine()) != null)
				{
					if (!flag)
					{
						if (text.StartsWith("paths:"))
						{
							flag = true;
						}
					}
					else
					{
						Match match = OpenApiHelpers.pathMatcher.Match(text);
						if (match.Success)
						{
							string value = match.Groups[1].Value;
							string text2 = null;
							if (_replaceBasePath != null)
							{
								text2 = value.Replace("/BASEPATH/", _replaceBasePath);
							}
							dictionary[value] = text2;
						}
					}
				}
				dictionary2 = dictionary;
			}
			return dictionary2;
		}

		// Token: 0x0600D138 RID: 53560 RVA: 0x004BC514 File Offset: 0x004BA714
		public bool TryGetOpenApiSpec(string _name, out string _specText)
		{
			if (string.IsNullOrEmpty(_name))
			{
				_name = "openapi.yaml";
			}
			OpenApiHelpers.OpenApiSpec openApiSpec;
			if (!this.specs.TryGetValue(_name, out openApiSpec))
			{
				_specText = null;
				return false;
			}
			_specText = openApiSpec.Spec;
			return true;
		}

		// Token: 0x0600D139 RID: 53561 RVA: 0x004BC550 File Offset: 0x004BA750
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeJsonPointerEncodedPath(StringBuilder _targetSb, string _path)
		{
			int i = 0;
			while (i < _path.Length)
			{
				char c = _path[i];
				switch (c)
				{
				case '\0':
					_targetSb.Append("\\u0000");
					break;
				case '\u0001':
					_targetSb.Append("\\u0001");
					break;
				case '\u0002':
					_targetSb.Append("\\u0002");
					break;
				case '\u0003':
					_targetSb.Append("\\u0003");
					break;
				case '\u0004':
					_targetSb.Append("\\u0004");
					break;
				case '\u0005':
					_targetSb.Append("\\u0005");
					break;
				case '\u0006':
					_targetSb.Append("\\u0006");
					break;
				case '\a':
					_targetSb.Append("\\u0007");
					break;
				case '\b':
					_targetSb.Append("\\b");
					break;
				case '\t':
					_targetSb.Append("\\t");
					break;
				case '\n':
					_targetSb.Append("\\n");
					break;
				case '\v':
					_targetSb.Append("\\u000b");
					break;
				case '\f':
					_targetSb.Append("\\f");
					break;
				case '\r':
					_targetSb.Append("\\r");
					break;
				case '\u000e':
					_targetSb.Append("\\u000e");
					break;
				case '\u000f':
					_targetSb.Append("\\u000f");
					break;
				case '\u0010':
					_targetSb.Append("\\u0010");
					break;
				case '\u0011':
					_targetSb.Append("\\u0011");
					break;
				case '\u0012':
					_targetSb.Append("\\u0012");
					break;
				case '\u0013':
					_targetSb.Append("\\u0013");
					break;
				case '\u0014':
					_targetSb.Append("\\u0014");
					break;
				case '\u0015':
					_targetSb.Append("\\u0015");
					break;
				case '\u0016':
					_targetSb.Append("\\u0016");
					break;
				case '\u0017':
					_targetSb.Append("\\u0017");
					break;
				case '\u0018':
					_targetSb.Append("\\u0018");
					break;
				case '\u0019':
					_targetSb.Append("\\u0019");
					break;
				case '\u001a':
					_targetSb.Append("\\u001a");
					break;
				case '\u001b':
					_targetSb.Append("\\u001b");
					break;
				case '\u001c':
					_targetSb.Append("\\u001c");
					break;
				case '\u001d':
					_targetSb.Append("\\u001d");
					break;
				case '\u001e':
					_targetSb.Append("\\u001e");
					break;
				case '\u001f':
					_targetSb.Append("\\u001f");
					break;
				case ' ':
				case '!':
				case '#':
				case '$':
				case '%':
				case '&':
				case '\'':
				case '(':
				case ')':
				case '*':
				case '+':
				case ',':
				case '-':
				case '.':
					goto IL_0330;
				case '"':
					_targetSb.Append("\\\"");
					break;
				case '/':
					_targetSb.Append("~1");
					break;
				default:
					if (c != '\\')
					{
						if (c != '~')
						{
							goto IL_0330;
						}
						_targetSb.Append("~0");
					}
					else
					{
						_targetSb.Append("\\\\");
					}
					break;
				}
				IL_0338:
				i++;
				continue;
				IL_0330:
				_targetSb.Append(c);
				goto IL_0338;
			}
		}

		// Token: 0x04009E7F RID: 40575
		[PublicizedFrom(EAccessModifier.Private)]
		public const string apiSpecResourcesFolder = "Data/Webserver/OpenApiSpecs";

		// Token: 0x04009E80 RID: 40576
		[PublicizedFrom(EAccessModifier.Private)]
		public const string masterResourceName = "openapi.master.yaml";

		// Token: 0x04009E81 RID: 40577
		[PublicizedFrom(EAccessModifier.Private)]
		public const string masterDocName = "openapi.yaml";

		// Token: 0x04009E82 RID: 40578
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, OpenApiHelpers.OpenApiSpec> specs = new CaseInsensitiveStringDictionary<OpenApiHelpers.OpenApiSpec>();

		// Token: 0x04009E83 RID: 40579
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Regex pathMatcher = new Regex("^\\s{1,2}(/\\S+):.*$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

		// Token: 0x02001B2E RID: 6958
		[PublicizedFrom(EAccessModifier.Private)]
		public struct OpenApiSpec
		{
			// Token: 0x0600D13C RID: 53564 RVA: 0x004BC8C3 File Offset: 0x004BAAC3
			public OpenApiSpec(string _spec, Dictionary<string, string> _exportedPaths = null)
			{
				this.ExportedPaths = _exportedPaths;
				this.Spec = _spec;
			}

			// Token: 0x04009E84 RID: 40580
			public readonly Dictionary<string, string> ExportedPaths;

			// Token: 0x04009E85 RID: 40581
			public readonly string Spec;
		}
	}
}
