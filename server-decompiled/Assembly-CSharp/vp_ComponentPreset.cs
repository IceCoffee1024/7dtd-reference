using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200156C RID: 5484
[Preserve]
public sealed class vp_ComponentPreset
{
	// Token: 0x17001428 RID: 5160
	// (get) Token: 0x0600ABA8 RID: 43944 RVA: 0x00401303 File Offset: 0x003FF503
	// (set) Token: 0x0600ABA9 RID: 43945 RVA: 0x0040130B File Offset: 0x003FF50B
	public Type ComponentType
	{
		get
		{
			return this.m_ComponentType;
		}
		set
		{
			this.m_ComponentType = value;
		}
	}

	// Token: 0x0600ABAA RID: 43946 RVA: 0x00401314 File Offset: 0x003FF514
	public static string Save(Component component, string fullPath)
	{
		vp_ComponentPreset vp_ComponentPreset = new vp_ComponentPreset();
		vp_ComponentPreset.InitFromComponent(component);
		return vp_ComponentPreset.Save(vp_ComponentPreset, fullPath, false);
	}

	// Token: 0x0600ABAB RID: 43947 RVA: 0x0040132C File Offset: 0x003FF52C
	public static string Save(vp_ComponentPreset savePreset, string fullPath, bool isDifference = false)
	{
		vp_ComponentPreset.m_FullPath = fullPath;
		bool logErrors = vp_ComponentPreset.LogErrors;
		vp_ComponentPreset.LogErrors = false;
		vp_ComponentPreset vp_ComponentPreset = new vp_ComponentPreset();
		vp_ComponentPreset.LoadTextStream(vp_ComponentPreset.m_FullPath);
		vp_ComponentPreset.LogErrors = logErrors;
		if (vp_ComponentPreset != null)
		{
			if (vp_ComponentPreset.m_ComponentType != null)
			{
				if (vp_ComponentPreset.ComponentType != savePreset.ComponentType)
				{
					return string.Concat(new string[]
					{
						"'",
						vp_ComponentPreset.ExtractFilenameFromPath(vp_ComponentPreset.m_FullPath),
						"' has the WRONG component type: ",
						vp_ComponentPreset.ComponentType.ToString(),
						".\n\nDo you want to replace it with a ",
						savePreset.ComponentType.ToString(),
						"?"
					});
				}
				if (File.Exists(vp_ComponentPreset.m_FullPath))
				{
					if (isDifference)
					{
						return "This will update '" + vp_ComponentPreset.ExtractFilenameFromPath(vp_ComponentPreset.m_FullPath) + "' with only the values modified since pressing Play or setting a state.\n\nContinue?";
					}
					return "'" + vp_ComponentPreset.ExtractFilenameFromPath(vp_ComponentPreset.m_FullPath) + "' already exists.\n\nDo you want to replace it?";
				}
			}
			if (File.Exists(vp_ComponentPreset.m_FullPath))
			{
				return "'" + vp_ComponentPreset.ExtractFilenameFromPath(vp_ComponentPreset.m_FullPath) + "' has an UNKNOWN component type.\n\nDo you want to replace it?";
			}
		}
		vp_ComponentPreset.ClearTextFile();
		vp_ComponentPreset.Append("///////////////////////////////////////////////////////////");
		vp_ComponentPreset.Append("// Component Preset Script");
		vp_ComponentPreset.Append("///////////////////////////////////////////////////////////\n");
		vp_ComponentPreset.Append("ComponentType " + savePreset.ComponentType.Name);
		foreach (vp_ComponentPreset.Field field in savePreset.m_Fields)
		{
			string text = "";
			FieldInfo fieldFromHandle = FieldInfo.GetFieldFromHandle(field.FieldHandle);
			string text2;
			if (fieldFromHandle.FieldType == typeof(float))
			{
				text2 = ((float)field.Args).ToCultureInvariantString("0.#######");
			}
			else if (fieldFromHandle.FieldType == typeof(Vector4))
			{
				Vector4 vector = (Vector4)field.Args;
				text2 = string.Concat(new string[]
				{
					vector.x.ToCultureInvariantString("0.#######"),
					" ",
					vector.y.ToCultureInvariantString("0.#######"),
					" ",
					vector.z.ToCultureInvariantString("0.#######"),
					" ",
					vector.w.ToCultureInvariantString("0.#######")
				});
			}
			else if (fieldFromHandle.FieldType == typeof(Vector3))
			{
				Vector3 vector2 = (Vector3)field.Args;
				text2 = string.Concat(new string[]
				{
					vector2.x.ToCultureInvariantString("0.#######"),
					" ",
					vector2.y.ToCultureInvariantString("0.#######"),
					" ",
					vector2.z.ToCultureInvariantString("0.#######")
				});
			}
			else if (fieldFromHandle.FieldType == typeof(Vector2))
			{
				Vector2 vector3 = (Vector2)field.Args;
				text2 = vector3.x.ToCultureInvariantString("0.#######") + " " + vector3.y.ToCultureInvariantString("0.#######");
			}
			else if (fieldFromHandle.FieldType == typeof(int))
			{
				text2 = ((int)field.Args).ToString();
			}
			else if (fieldFromHandle.FieldType == typeof(bool))
			{
				text2 = ((bool)field.Args).ToString();
			}
			else if (fieldFromHandle.FieldType == typeof(string))
			{
				text2 = (string)field.Args;
			}
			else
			{
				text = "//";
				text2 = "<NOTE: Type '" + fieldFromHandle.FieldType.Name.ToString() + "' can't be saved to preset.>";
			}
			if (!string.IsNullOrEmpty(text2) && fieldFromHandle.Name != "Persist")
			{
				vp_ComponentPreset.Append(text + fieldFromHandle.Name + " " + text2);
			}
		}
		return null;
	}

	// Token: 0x0600ABAC RID: 43948 RVA: 0x00401788 File Offset: 0x003FF988
	public static string SaveDifference(vp_ComponentPreset initialStatePreset, Component modifiedComponent, string fullPath, vp_ComponentPreset diskPreset)
	{
		if (initialStatePreset.ComponentType != modifiedComponent.GetType())
		{
			vp_ComponentPreset.Error("Tried to save difference between different type components in 'SaveDifference'");
			return null;
		}
		vp_ComponentPreset vp_ComponentPreset = new vp_ComponentPreset();
		vp_ComponentPreset.InitFromComponent(modifiedComponent);
		vp_ComponentPreset vp_ComponentPreset2 = new vp_ComponentPreset();
		vp_ComponentPreset2.m_ComponentType = vp_ComponentPreset.ComponentType;
		for (int i = 0; i < vp_ComponentPreset.m_Fields.Count; i++)
		{
			if (!initialStatePreset.m_Fields[i].Args.Equals(vp_ComponentPreset.m_Fields[i].Args))
			{
				vp_ComponentPreset2.m_Fields.Add(vp_ComponentPreset.m_Fields[i]);
			}
		}
		foreach (vp_ComponentPreset.Field field in diskPreset.m_Fields)
		{
			bool flag = true;
			foreach (vp_ComponentPreset.Field field2 in vp_ComponentPreset2.m_Fields)
			{
				if (field.FieldHandle == field2.FieldHandle)
				{
					flag = false;
				}
			}
			bool flag2 = false;
			foreach (vp_ComponentPreset.Field field3 in vp_ComponentPreset.m_Fields)
			{
				if (field.FieldHandle == field3.FieldHandle)
				{
					flag2 = true;
				}
			}
			if (!flag2)
			{
				flag = false;
			}
			if (flag)
			{
				vp_ComponentPreset2.m_Fields.Add(field);
			}
		}
		return vp_ComponentPreset.Save(vp_ComponentPreset2, fullPath, true);
	}

	// Token: 0x0600ABAD RID: 43949 RVA: 0x00401940 File Offset: 0x003FFB40
	public void InitFromComponent(Component component)
	{
		this.m_ComponentType = component.GetType();
		this.m_Fields.Clear();
		foreach (FieldInfo fieldInfo in this.m_ComponentType.GetFields())
		{
			if (fieldInfo.IsPublic && (fieldInfo.FieldType == typeof(float) || fieldInfo.FieldType == typeof(Vector4) || fieldInfo.FieldType == typeof(Vector3) || fieldInfo.FieldType == typeof(Vector2) || fieldInfo.FieldType == typeof(int) || fieldInfo.FieldType == typeof(bool) || fieldInfo.FieldType == typeof(string)))
			{
				this.m_Fields.Add(new vp_ComponentPreset.Field(fieldInfo.FieldHandle, fieldInfo.GetValue(component)));
			}
		}
	}

	// Token: 0x0600ABAE RID: 43950 RVA: 0x00401A54 File Offset: 0x003FFC54
	public static vp_ComponentPreset CreateFromComponent(Component component)
	{
		vp_ComponentPreset vp_ComponentPreset = new vp_ComponentPreset();
		vp_ComponentPreset.m_ComponentType = component.GetType();
		foreach (FieldInfo fieldInfo in vp_ComponentPreset.m_ComponentType.GetFields())
		{
			if (fieldInfo.IsPublic && (fieldInfo.FieldType == typeof(float) || fieldInfo.FieldType == typeof(Vector4) || fieldInfo.FieldType == typeof(Vector3) || fieldInfo.FieldType == typeof(Vector2) || fieldInfo.FieldType == typeof(int) || fieldInfo.FieldType == typeof(bool) || fieldInfo.FieldType == typeof(string)))
			{
				vp_ComponentPreset.m_Fields.Add(new vp_ComponentPreset.Field(fieldInfo.FieldHandle, fieldInfo.GetValue(component)));
			}
		}
		return vp_ComponentPreset;
	}

	// Token: 0x0600ABAF RID: 43951 RVA: 0x00401B64 File Offset: 0x003FFD64
	public int TryMakeCompatibleWithComponent(vp_Component component)
	{
		this.m_ComponentType = component.GetType();
		List<FieldInfo> list = new List<FieldInfo>(this.m_ComponentType.GetFields());
		int i = this.m_Fields.Count - 1;
		while (i > -1)
		{
			foreach (FieldInfo fieldInfo in list)
			{
				if (fieldInfo.Name.Contains("PositionOffset") || fieldInfo.Name.Contains("RotationOffset"))
				{
					break;
				}
				if (this.m_Fields[i].FieldHandle == fieldInfo.FieldHandle)
				{
					goto IL_00B8;
				}
			}
			goto IL_00A0;
			IL_00B8:
			i--;
			continue;
			IL_00A0:
			this.m_Fields.Remove(this.m_Fields[i]);
			goto IL_00B8;
		}
		return this.m_Fields.Count;
	}

	// Token: 0x0600ABB0 RID: 43952 RVA: 0x00401C50 File Offset: 0x003FFE50
	public bool LoadTextStream(string fullPath)
	{
		vp_ComponentPreset.m_FullPath = fullPath;
		FileInfo fileInfo = new FileInfo(vp_ComponentPreset.m_FullPath);
		if (fileInfo == null || !fileInfo.Exists)
		{
			vp_ComponentPreset.Error("Failed to read file. '" + vp_ComponentPreset.m_FullPath + "'");
			return false;
		}
		TextReader textReader = fileInfo.OpenText();
		List<string> list = new List<string>();
		string text;
		while ((text = textReader.ReadLine()) != null)
		{
			list.Add(text);
		}
		textReader.Close();
		if (list == null)
		{
			vp_ComponentPreset.Error("Preset is empty. '" + vp_ComponentPreset.m_FullPath + "'");
			return false;
		}
		this.ParseLines(list);
		return true;
	}

	// Token: 0x0600ABB1 RID: 43953 RVA: 0x00401CE8 File Offset: 0x003FFEE8
	public static bool Load(vp_Component component, string fullPath)
	{
		vp_ComponentPreset vp_ComponentPreset = new vp_ComponentPreset();
		vp_ComponentPreset.LoadTextStream(fullPath);
		return vp_ComponentPreset.Apply(component, vp_ComponentPreset);
	}

	// Token: 0x0600ABB2 RID: 43954 RVA: 0x00401D0C File Offset: 0x003FFF0C
	public bool LoadFromResources(string resourcePath)
	{
		vp_ComponentPreset.m_FullPath = resourcePath;
		TextAsset textAsset = Resources.Load(vp_ComponentPreset.m_FullPath) as TextAsset;
		if (textAsset == null)
		{
			vp_ComponentPreset.Error("Failed to read file. '" + vp_ComponentPreset.m_FullPath + "'");
			return false;
		}
		return this.LoadFromTextAsset(textAsset);
	}

	// Token: 0x0600ABB3 RID: 43955 RVA: 0x00401D5C File Offset: 0x003FFF5C
	public static vp_ComponentPreset LoadFromResources(vp_Component component, string resourcePath)
	{
		vp_ComponentPreset vp_ComponentPreset = new vp_ComponentPreset();
		vp_ComponentPreset.LoadFromResources(resourcePath);
		vp_ComponentPreset.Apply(component, vp_ComponentPreset);
		return vp_ComponentPreset;
	}

	// Token: 0x0600ABB4 RID: 43956 RVA: 0x00401D80 File Offset: 0x003FFF80
	public bool LoadFromTextAsset(TextAsset file)
	{
		vp_ComponentPreset.m_FullPath = file.name;
		List<string> list = new List<string>();
		foreach (string text in file.text.Split('\n', StringSplitOptions.None))
		{
			list.Add(text);
		}
		if (list == null)
		{
			vp_ComponentPreset.Error("Preset is empty. '" + vp_ComponentPreset.m_FullPath + "'");
			return false;
		}
		this.ParseLines(list);
		return true;
	}

	// Token: 0x0600ABB5 RID: 43957 RVA: 0x00401DEC File Offset: 0x003FFFEC
	public static vp_ComponentPreset LoadFromTextAsset(vp_Component component, TextAsset file)
	{
		vp_ComponentPreset vp_ComponentPreset = new vp_ComponentPreset();
		vp_ComponentPreset.LoadFromTextAsset(file);
		vp_ComponentPreset.Apply(component, vp_ComponentPreset);
		return vp_ComponentPreset;
	}

	// Token: 0x0600ABB6 RID: 43958 RVA: 0x00401E10 File Offset: 0x00400010
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Append(string str)
	{
		str = str.Replace("\n", Environment.NewLine);
		StreamWriter streamWriter = null;
		try
		{
			streamWriter = new StreamWriter(vp_ComponentPreset.m_FullPath, true);
			streamWriter.WriteLine(str);
			if (streamWriter != null)
			{
				streamWriter.Close();
			}
		}
		catch
		{
			vp_ComponentPreset.Error("Failed to write to file: '" + vp_ComponentPreset.m_FullPath + "'");
		}
		if (streamWriter != null)
		{
			streamWriter.Close();
		}
	}

	// Token: 0x0600ABB7 RID: 43959 RVA: 0x00401E84 File Offset: 0x00400084
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ClearTextFile()
	{
		StreamWriter streamWriter = null;
		try
		{
			streamWriter = new StreamWriter(vp_ComponentPreset.m_FullPath, false);
			if (streamWriter != null)
			{
				streamWriter.Close();
			}
		}
		catch
		{
			vp_ComponentPreset.Error("Failed to clear file: '" + vp_ComponentPreset.m_FullPath + "'");
		}
		if (streamWriter != null)
		{
			streamWriter.Close();
		}
	}

	// Token: 0x0600ABB8 RID: 43960 RVA: 0x00401EE0 File Offset: 0x004000E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void ParseLines(List<string> lines)
	{
		vp_ComponentPreset.m_LineNumber = 0;
		foreach (string text in lines)
		{
			vp_ComponentPreset.m_LineNumber++;
			string text2 = vp_ComponentPreset.RemoveComments(text);
			if (!string.IsNullOrEmpty(text2) && !this.Parse(text2))
			{
				return;
			}
		}
		vp_ComponentPreset.m_LineNumber = 0;
	}

	// Token: 0x0600ABB9 RID: 43961 RVA: 0x00401F58 File Offset: 0x00400158
	[PublicizedFrom(EAccessModifier.Private)]
	public bool Parse(string line)
	{
		line = line.Trim();
		if (string.IsNullOrEmpty(line))
		{
			return true;
		}
		string[] array = line.Split(null, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = array[i].Trim();
		}
		if (this.m_ComponentType == null)
		{
			if (!(array[0] == "ComponentType") || array.Length != 2)
			{
				vp_ComponentPreset.PresetError("Unknown ComponentType.");
				return false;
			}
			vp_ComponentPreset.m_Type = Type.GetType(array[1]);
			if (vp_ComponentPreset.m_Type == null)
			{
				vp_ComponentPreset.PresetError("No such ComponentType: '" + array[1] + "'");
				return false;
			}
			this.m_ComponentType = vp_ComponentPreset.m_Type;
			return true;
		}
		else
		{
			FieldInfo fieldInfo = null;
			foreach (FieldInfo fieldInfo2 in vp_ComponentPreset.m_Type.GetFields())
			{
				if (fieldInfo2.Name == array[0])
				{
					fieldInfo = fieldInfo2;
				}
			}
			if (fieldInfo == null)
			{
				if (array[0] != "ComponentType")
				{
					string[] array2 = this.FindMovedParameter(vp_ComponentPreset.m_Type.Name, array[0]);
					if (array2 != null && array2.Length == 2)
					{
						if ((array2[0] == null || (!string.IsNullOrEmpty(array2[0]) && array2[0] == vp_ComponentPreset.m_Type.Name)) && !string.IsNullOrEmpty(array2[1]) && array2[1] != array[0])
						{
							vp_ComponentPreset.PresetWarning(string.Concat(new string[]
							{
								"The parameter '",
								array[0],
								"' has been renamed to '",
								array2[1],
								"'. Please update your presets."
							}));
						}
						else if (array2[0] != null && array2[0] != vp_ComponentPreset.m_Type.Name && (string.IsNullOrEmpty(array2[1]) || array2[1] == array[0]))
						{
							vp_ComponentPreset.PresetWarning(string.Concat(new string[]
							{
								"The parameter '",
								array[0],
								"' has been moved to the '",
								array2[0],
								"' component. Please update your presets."
							}));
						}
						else if (array2[0] != null && array2[0] != vp_ComponentPreset.m_Type.Name && !string.IsNullOrEmpty(array2[1]) && array2[1] != array[0])
						{
							vp_ComponentPreset.PresetWarning(string.Concat(new string[]
							{
								"The parameter '",
								array[0],
								"' has been moved to the '",
								array2[0],
								"' component and renamed to '",
								array2[1],
								"'. Please update your presets."
							}));
						}
						else
						{
							vp_ComponentPreset.PresetWarning(string.Concat(new string[]
							{
								"'",
								vp_ComponentPreset.m_Type.Name,
								"' no longer supports the parameter: '",
								array[0],
								"'. Please update your presets."
							}));
						}
					}
					else
					{
						vp_ComponentPreset.PresetError(string.Concat(new string[]
						{
							"'",
							vp_ComponentPreset.m_Type.Name,
							"' has no such field: '",
							array[0],
							"'"
						}));
					}
				}
				return true;
			}
			vp_ComponentPreset.Field field = new vp_ComponentPreset.Field(fieldInfo.FieldHandle, vp_ComponentPreset.TokensToObject(fieldInfo, array));
			this.m_Fields.Add(field);
			return true;
		}
	}

	// Token: 0x0600ABBA RID: 43962 RVA: 0x00402284 File Offset: 0x00400484
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] FindMovedParameter(string type, string field)
	{
		string[] array;
		if (!this.MovedParameters.TryGetValue(type + "." + field, out array))
		{
			return null;
		}
		return array;
	}

	// Token: 0x0600ABBB RID: 43963 RVA: 0x004022B0 File Offset: 0x004004B0
	public static bool Apply(vp_Component component, vp_ComponentPreset preset)
	{
		if (preset == null)
		{
			vp_ComponentPreset.Error("Tried to apply a preset that was null in '" + vp_Utility.GetErrorLocation(1, false) + "'");
			return false;
		}
		if (preset.m_ComponentType == null)
		{
			vp_ComponentPreset.Error("Preset ComponentType was null in '" + vp_Utility.GetErrorLocation(1, false) + "'");
			return false;
		}
		if (component == null)
		{
			vp_ComponentPreset.Error("Component was null when attempting to apply preset in '" + vp_Utility.GetErrorLocation(1, false) + "'");
			return false;
		}
		if (component.Type != preset.m_ComponentType)
		{
			string text = "a '";
			Type componentType = preset.m_ComponentType;
			string text2 = text + ((componentType != null) ? componentType.ToString() : null) + "' preset";
			if (preset.m_ComponentType == null)
			{
				text2 = "an unknown preset type";
			}
			vp_ComponentPreset.Error(string.Concat(new string[]
			{
				"Applied ",
				text2,
				" to a '",
				component.Type.ToString(),
				"' component in '",
				vp_Utility.GetErrorLocation(1, false),
				"'"
			}));
			return false;
		}
		foreach (vp_ComponentPreset.Field field in preset.m_Fields)
		{
			foreach (FieldInfo fieldInfo in component.Fields)
			{
				if (fieldInfo.FieldHandle == field.FieldHandle)
				{
					fieldInfo.SetValue(component, field.Args);
				}
			}
		}
		return true;
	}

	// Token: 0x0600ABBC RID: 43964 RVA: 0x0040244C File Offset: 0x0040064C
	public static Type GetFileType(string fullPath)
	{
		bool logErrors = vp_ComponentPreset.LogErrors;
		vp_ComponentPreset.LogErrors = false;
		vp_ComponentPreset vp_ComponentPreset = new vp_ComponentPreset();
		vp_ComponentPreset.LoadTextStream(fullPath);
		vp_ComponentPreset.LogErrors = logErrors;
		if (vp_ComponentPreset != null && vp_ComponentPreset.m_ComponentType != null)
		{
			return vp_ComponentPreset.m_ComponentType;
		}
		return null;
	}

	// Token: 0x0600ABBD RID: 43965 RVA: 0x00402490 File Offset: 0x00400690
	public static Type GetFileTypeFromAsset(TextAsset asset)
	{
		bool logErrors = vp_ComponentPreset.LogErrors;
		vp_ComponentPreset.LogErrors = false;
		vp_ComponentPreset vp_ComponentPreset = new vp_ComponentPreset();
		vp_ComponentPreset.LoadFromTextAsset(asset);
		vp_ComponentPreset.LogErrors = logErrors;
		if (vp_ComponentPreset != null && vp_ComponentPreset.m_ComponentType != null)
		{
			return vp_ComponentPreset.m_ComponentType;
		}
		return null;
	}

	// Token: 0x0600ABBE RID: 43966 RVA: 0x004024D4 File Offset: 0x004006D4
	[PublicizedFrom(EAccessModifier.Private)]
	public static object TokensToObject(FieldInfo field, string[] tokens)
	{
		if (field.FieldType == typeof(float))
		{
			return vp_ComponentPreset.ArgsToFloat(tokens);
		}
		if (field.FieldType == typeof(Vector4))
		{
			return vp_ComponentPreset.ArgsToVector4(tokens);
		}
		if (field.FieldType == typeof(Vector3))
		{
			return vp_ComponentPreset.ArgsToVector3(tokens);
		}
		if (field.FieldType == typeof(Vector2))
		{
			return vp_ComponentPreset.ArgsToVector2(tokens);
		}
		if (field.FieldType == typeof(int))
		{
			return vp_ComponentPreset.ArgsToInt(tokens);
		}
		if (field.FieldType == typeof(bool))
		{
			return vp_ComponentPreset.ArgsToBool(tokens);
		}
		if (field.FieldType == typeof(string))
		{
			return vp_ComponentPreset.ArgsToString(tokens);
		}
		return null;
	}

	// Token: 0x0600ABBF RID: 43967 RVA: 0x004025D4 File Offset: 0x004007D4
	[PublicizedFrom(EAccessModifier.Private)]
	public static string RemoveComments(string str)
	{
		string text = "";
		for (int i = 0; i < str.Length; i++)
		{
			switch (vp_ComponentPreset.m_ReadMode)
			{
			case vp_ComponentPreset.ReadMode.Normal:
				if (str[i] == '/' && str[i + 1] == '*')
				{
					vp_ComponentPreset.m_ReadMode = vp_ComponentPreset.ReadMode.BlockComment;
					i++;
				}
				else if (str[i] == '/' && str[i + 1] == '/')
				{
					vp_ComponentPreset.m_ReadMode = vp_ComponentPreset.ReadMode.LineComment;
					i++;
				}
				else
				{
					text += str[i].ToString();
				}
				break;
			case vp_ComponentPreset.ReadMode.LineComment:
				if (i == str.Length - 1)
				{
					vp_ComponentPreset.m_ReadMode = vp_ComponentPreset.ReadMode.Normal;
				}
				break;
			case vp_ComponentPreset.ReadMode.BlockComment:
				if (str[i] == '*' && str[i + 1] == '/')
				{
					vp_ComponentPreset.m_ReadMode = vp_ComponentPreset.ReadMode.Normal;
					i++;
				}
				break;
			}
		}
		return text;
	}

	// Token: 0x0600ABC0 RID: 43968 RVA: 0x004026B4 File Offset: 0x004008B4
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector4 ArgsToVector4(string[] args)
	{
		if (args.Length - 1 != 4)
		{
			vp_ComponentPreset.PresetError("Wrong number of fields for '" + args[0] + "'");
			return Vector4.zero;
		}
		Vector4 vector;
		try
		{
			vector = new Vector4(Convert.ToSingle(args[1], CultureInfo.InvariantCulture), Convert.ToSingle(args[2], CultureInfo.InvariantCulture), Convert.ToSingle(args[3], CultureInfo.InvariantCulture), Convert.ToSingle(args[4], CultureInfo.InvariantCulture));
		}
		catch
		{
			vp_ComponentPreset.PresetError(string.Concat(new string[]
			{
				"Illegal value: '",
				args[1],
				", ",
				args[2],
				", ",
				args[3],
				", ",
				args[4],
				"'"
			}));
			return Vector4.zero;
		}
		return vector;
	}

	// Token: 0x0600ABC1 RID: 43969 RVA: 0x00402790 File Offset: 0x00400990
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector3 ArgsToVector3(string[] args)
	{
		if (args.Length - 1 != 3)
		{
			vp_ComponentPreset.PresetError("Wrong number of fields for '" + args[0] + "'");
			return Vector3.zero;
		}
		Vector3 vector;
		try
		{
			vector = new Vector3(Convert.ToSingle(args[1], CultureInfo.InvariantCulture), Convert.ToSingle(args[2], CultureInfo.InvariantCulture), Convert.ToSingle(args[3], CultureInfo.InvariantCulture));
		}
		catch
		{
			vp_ComponentPreset.PresetError(string.Concat(new string[]
			{
				"Illegal value: '",
				args[1],
				", ",
				args[2],
				", ",
				args[3],
				"'"
			}));
			return Vector3.zero;
		}
		return vector;
	}

	// Token: 0x0600ABC2 RID: 43970 RVA: 0x00402850 File Offset: 0x00400A50
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector2 ArgsToVector2(string[] args)
	{
		if (args.Length - 1 != 2)
		{
			vp_ComponentPreset.PresetError("Wrong number of fields for '" + args[0] + "'");
			return Vector2.zero;
		}
		Vector2 vector;
		try
		{
			vector = new Vector2(Convert.ToSingle(args[1], CultureInfo.InvariantCulture), Convert.ToSingle(args[2], CultureInfo.InvariantCulture));
		}
		catch
		{
			vp_ComponentPreset.PresetError(string.Concat(new string[]
			{
				"Illegal value: '",
				args[1],
				", ",
				args[2],
				"'"
			}));
			return Vector2.zero;
		}
		return vector;
	}

	// Token: 0x0600ABC3 RID: 43971 RVA: 0x004028F8 File Offset: 0x00400AF8
	[PublicizedFrom(EAccessModifier.Private)]
	public static float ArgsToFloat(string[] args)
	{
		if (args.Length - 1 != 1)
		{
			vp_ComponentPreset.PresetError("Wrong number of fields for '" + args[0] + "'");
			return 0f;
		}
		float num;
		try
		{
			num = Convert.ToSingle(args[1], CultureInfo.InvariantCulture);
		}
		catch
		{
			vp_ComponentPreset.PresetError("Illegal value: '" + args[1] + "'");
			return 0f;
		}
		return num;
	}

	// Token: 0x0600ABC4 RID: 43972 RVA: 0x00402970 File Offset: 0x00400B70
	[PublicizedFrom(EAccessModifier.Private)]
	public static int ArgsToInt(string[] args)
	{
		if (args.Length - 1 != 1)
		{
			vp_ComponentPreset.PresetError("Wrong number of fields for '" + args[0] + "'");
			return 0;
		}
		int num;
		try
		{
			num = Convert.ToInt32(args[1], CultureInfo.InvariantCulture);
		}
		catch
		{
			vp_ComponentPreset.PresetError("Illegal value: '" + args[1] + "'");
			return 0;
		}
		return num;
	}

	// Token: 0x0600ABC5 RID: 43973 RVA: 0x004029E0 File Offset: 0x00400BE0
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool ArgsToBool(string[] args)
	{
		if (args.Length - 1 != 1)
		{
			vp_ComponentPreset.PresetError("Wrong number of fields for '" + args[0] + "'");
			return false;
		}
		if (args[1].ToLower() == "true")
		{
			return true;
		}
		if (args[1].ToLower() == "false")
		{
			return false;
		}
		vp_ComponentPreset.PresetError("Illegal value: '" + args[1] + "'");
		return false;
	}

	// Token: 0x0600ABC6 RID: 43974 RVA: 0x00402A54 File Offset: 0x00400C54
	[PublicizedFrom(EAccessModifier.Private)]
	public static string ArgsToString(string[] args)
	{
		string text = "";
		for (int i = 1; i < args.Length; i++)
		{
			text += args[i];
			if (i < args.Length - 1)
			{
				text += " ";
			}
		}
		return text;
	}

	// Token: 0x0600ABC7 RID: 43975 RVA: 0x00402A94 File Offset: 0x00400C94
	public Type GetFieldType(string fieldName)
	{
		Type type = null;
		foreach (vp_ComponentPreset.Field field in this.m_Fields)
		{
			FieldInfo fieldFromHandle = FieldInfo.GetFieldFromHandle(field.FieldHandle);
			if (fieldFromHandle.Name == fieldName)
			{
				type = fieldFromHandle.FieldType;
			}
		}
		return type;
	}

	// Token: 0x0600ABC8 RID: 43976 RVA: 0x00402B04 File Offset: 0x00400D04
	public object GetFieldValue(string fieldName)
	{
		object obj = null;
		foreach (vp_ComponentPreset.Field field in this.m_Fields)
		{
			if (FieldInfo.GetFieldFromHandle(field.FieldHandle).Name == fieldName)
			{
				obj = field.Args;
			}
		}
		return obj;
	}

	// Token: 0x0600ABC9 RID: 43977 RVA: 0x00402B74 File Offset: 0x00400D74
	public void SetFieldValue(string fieldName, object value)
	{
		foreach (vp_ComponentPreset.Field field in this.m_Fields)
		{
			if (FieldInfo.GetFieldFromHandle(field.FieldHandle).Name == fieldName)
			{
				field.Args = value;
				break;
			}
		}
	}

	// Token: 0x0600ABCA RID: 43978 RVA: 0x00402BE4 File Offset: 0x00400DE4
	public static string ExtractFilenameFromPath(string path)
	{
		int num = Math.Max(path.LastIndexOf('/'), path.LastIndexOf('\\'));
		if (num == -1)
		{
			return path;
		}
		if (num == path.Length - 1)
		{
			return "";
		}
		return path.Substring(num + 1, path.Length - num - 1);
	}

	// Token: 0x0600ABCB RID: 43979 RVA: 0x00402C34 File Offset: 0x00400E34
	[PublicizedFrom(EAccessModifier.Private)]
	public static void PresetError(string message)
	{
		if (!vp_ComponentPreset.LogErrors)
		{
			return;
		}
		Debug.LogError(string.Concat(new string[]
		{
			"Preset Error: ",
			vp_ComponentPreset.m_FullPath,
			" (at ",
			vp_ComponentPreset.m_LineNumber.ToString(),
			") ",
			message
		}));
	}

	// Token: 0x0600ABCC RID: 43980 RVA: 0x00402C8C File Offset: 0x00400E8C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void PresetWarning(string message)
	{
		if (!vp_ComponentPreset.LogErrors)
		{
			return;
		}
		Debug.LogWarning(string.Concat(new string[]
		{
			"Preset Warning: ",
			vp_ComponentPreset.m_FullPath,
			" (at ",
			vp_ComponentPreset.m_LineNumber.ToString(),
			") ",
			message
		}));
	}

	// Token: 0x0600ABCD RID: 43981 RVA: 0x00402CE2 File Offset: 0x00400EE2
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Error(string message)
	{
		if (!vp_ComponentPreset.LogErrors)
		{
			return;
		}
		Debug.LogError("Error: " + message);
	}

	// Token: 0x04007F95 RID: 32661
	[PublicizedFrom(EAccessModifier.Private)]
	public static string m_FullPath = null;

	// Token: 0x04007F96 RID: 32662
	[PublicizedFrom(EAccessModifier.Private)]
	public static int m_LineNumber = 0;

	// Token: 0x04007F97 RID: 32663
	[PublicizedFrom(EAccessModifier.Private)]
	public static Type m_Type = null;

	// Token: 0x04007F98 RID: 32664
	public static bool LogErrors = true;

	// Token: 0x04007F99 RID: 32665
	[PublicizedFrom(EAccessModifier.Private)]
	public static vp_ComponentPreset.ReadMode m_ReadMode = vp_ComponentPreset.ReadMode.Normal;

	// Token: 0x04007F9A RID: 32666
	[PublicizedFrom(EAccessModifier.Private)]
	public Type m_ComponentType;

	// Token: 0x04007F9B RID: 32667
	[PublicizedFrom(EAccessModifier.Private)]
	public List<vp_ComponentPreset.Field> m_Fields = new List<vp_ComponentPreset.Field>();

	// Token: 0x04007F9C RID: 32668
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, string[]> MovedParameters = new Dictionary<string, string[]>
	{
		{
			"vp_FPCamera.MouseAcceleration",
			new string[] { "vp_FPInput", "MouseLookAcceleration" }
		},
		{
			"vp_FPCamera.MouseSensitivity",
			new string[] { "vp_FPInput", "MouseLookSensitivity" }
		},
		{
			"vp_FPCamera.MouseSmoothSteps",
			new string[] { "vp_FPInput", "MouseLookSmoothSteps" }
		},
		{
			"vp_FPCamera.MouseSmoothWeight",
			new string[] { "vp_FPInput", "MouseLookSmoothWeight" }
		},
		{
			"vp_FPCamera.MouseAccelerationThreshold",
			new string[] { "vp_FPInput", "MouseLookAccelerationThreshold" }
		},
		{
			"vp_FPInput.ForceCursor",
			new string[] { "vp_FPInput", "MouseCursorForced" }
		}
	};

	// Token: 0x0200156D RID: 5485
	[PublicizedFrom(EAccessModifier.Private)]
	public enum ReadMode
	{
		// Token: 0x04007F9E RID: 32670
		Normal,
		// Token: 0x04007F9F RID: 32671
		LineComment,
		// Token: 0x04007FA0 RID: 32672
		BlockComment
	}

	// Token: 0x0200156E RID: 5486
	[PublicizedFrom(EAccessModifier.Private)]
	public class Field
	{
		// Token: 0x0600ABD0 RID: 43984 RVA: 0x00402E0D File Offset: 0x0040100D
		public Field(RuntimeFieldHandle fieldHandle, object args)
		{
			this.FieldHandle = fieldHandle;
			this.Args = args;
		}

		// Token: 0x04007FA1 RID: 32673
		public RuntimeFieldHandle FieldHandle;

		// Token: 0x04007FA2 RID: 32674
		public object Args;
	}
}
