using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200151D RID: 5405
[Preserve]
public sealed class vp_ComponentPreset
{
	// Token: 0x1700139E RID: 5022
	// (get) Token: 0x0600A9A0 RID: 43424 RVA: 0x003F4D6B File Offset: 0x003F2F6B
	// (set) Token: 0x0600A9A1 RID: 43425 RVA: 0x003F4D73 File Offset: 0x003F2F73
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

	// Token: 0x0600A9A2 RID: 43426 RVA: 0x003F4D7C File Offset: 0x003F2F7C
	public static string Save(Component component, string fullPath)
	{
		vp_ComponentPreset vp_ComponentPreset = new vp_ComponentPreset();
		vp_ComponentPreset.InitFromComponent(component);
		return vp_ComponentPreset.Save(vp_ComponentPreset, fullPath, false);
	}

	// Token: 0x0600A9A3 RID: 43427 RVA: 0x003F4D94 File Offset: 0x003F2F94
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

	// Token: 0x0600A9A4 RID: 43428 RVA: 0x003F51F0 File Offset: 0x003F33F0
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

	// Token: 0x0600A9A5 RID: 43429 RVA: 0x003F53A8 File Offset: 0x003F35A8
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

	// Token: 0x0600A9A6 RID: 43430 RVA: 0x003F54BC File Offset: 0x003F36BC
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

	// Token: 0x0600A9A7 RID: 43431 RVA: 0x003F55CC File Offset: 0x003F37CC
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

	// Token: 0x0600A9A8 RID: 43432 RVA: 0x003F56B8 File Offset: 0x003F38B8
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

	// Token: 0x0600A9A9 RID: 43433 RVA: 0x003F5750 File Offset: 0x003F3950
	public static bool Load(vp_Component component, string fullPath)
	{
		vp_ComponentPreset vp_ComponentPreset = new vp_ComponentPreset();
		vp_ComponentPreset.LoadTextStream(fullPath);
		return vp_ComponentPreset.Apply(component, vp_ComponentPreset);
	}

	// Token: 0x0600A9AA RID: 43434 RVA: 0x003F5774 File Offset: 0x003F3974
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

	// Token: 0x0600A9AB RID: 43435 RVA: 0x003F57C4 File Offset: 0x003F39C4
	public static vp_ComponentPreset LoadFromResources(vp_Component component, string resourcePath)
	{
		vp_ComponentPreset vp_ComponentPreset = new vp_ComponentPreset();
		vp_ComponentPreset.LoadFromResources(resourcePath);
		vp_ComponentPreset.Apply(component, vp_ComponentPreset);
		return vp_ComponentPreset;
	}

	// Token: 0x0600A9AC RID: 43436 RVA: 0x003F57E8 File Offset: 0x003F39E8
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

	// Token: 0x0600A9AD RID: 43437 RVA: 0x003F5854 File Offset: 0x003F3A54
	public static vp_ComponentPreset LoadFromTextAsset(vp_Component component, TextAsset file)
	{
		vp_ComponentPreset vp_ComponentPreset = new vp_ComponentPreset();
		vp_ComponentPreset.LoadFromTextAsset(file);
		vp_ComponentPreset.Apply(component, vp_ComponentPreset);
		return vp_ComponentPreset;
	}

	// Token: 0x0600A9AE RID: 43438 RVA: 0x003F5878 File Offset: 0x003F3A78
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

	// Token: 0x0600A9AF RID: 43439 RVA: 0x003F58EC File Offset: 0x003F3AEC
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

	// Token: 0x0600A9B0 RID: 43440 RVA: 0x003F5948 File Offset: 0x003F3B48
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

	// Token: 0x0600A9B1 RID: 43441 RVA: 0x003F59C0 File Offset: 0x003F3BC0
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

	// Token: 0x0600A9B2 RID: 43442 RVA: 0x003F5CEC File Offset: 0x003F3EEC
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

	// Token: 0x0600A9B3 RID: 43443 RVA: 0x003F5D18 File Offset: 0x003F3F18
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

	// Token: 0x0600A9B4 RID: 43444 RVA: 0x003F5EB4 File Offset: 0x003F40B4
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

	// Token: 0x0600A9B5 RID: 43445 RVA: 0x003F5EF8 File Offset: 0x003F40F8
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

	// Token: 0x0600A9B6 RID: 43446 RVA: 0x003F5F3C File Offset: 0x003F413C
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

	// Token: 0x0600A9B7 RID: 43447 RVA: 0x003F603C File Offset: 0x003F423C
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

	// Token: 0x0600A9B8 RID: 43448 RVA: 0x003F611C File Offset: 0x003F431C
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

	// Token: 0x0600A9B9 RID: 43449 RVA: 0x003F61F8 File Offset: 0x003F43F8
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

	// Token: 0x0600A9BA RID: 43450 RVA: 0x003F62B8 File Offset: 0x003F44B8
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

	// Token: 0x0600A9BB RID: 43451 RVA: 0x003F6360 File Offset: 0x003F4560
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

	// Token: 0x0600A9BC RID: 43452 RVA: 0x003F63D8 File Offset: 0x003F45D8
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

	// Token: 0x0600A9BD RID: 43453 RVA: 0x003F6448 File Offset: 0x003F4648
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

	// Token: 0x0600A9BE RID: 43454 RVA: 0x003F64BC File Offset: 0x003F46BC
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

	// Token: 0x0600A9BF RID: 43455 RVA: 0x003F64FC File Offset: 0x003F46FC
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

	// Token: 0x0600A9C0 RID: 43456 RVA: 0x003F656C File Offset: 0x003F476C
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

	// Token: 0x0600A9C1 RID: 43457 RVA: 0x003F65DC File Offset: 0x003F47DC
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

	// Token: 0x0600A9C2 RID: 43458 RVA: 0x003F664C File Offset: 0x003F484C
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

	// Token: 0x0600A9C3 RID: 43459 RVA: 0x003F669C File Offset: 0x003F489C
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

	// Token: 0x0600A9C4 RID: 43460 RVA: 0x003F66F4 File Offset: 0x003F48F4
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

	// Token: 0x0600A9C5 RID: 43461 RVA: 0x003F674A File Offset: 0x003F494A
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Error(string message)
	{
		if (!vp_ComponentPreset.LogErrors)
		{
			return;
		}
		Debug.LogError("Error: " + message);
	}

	// Token: 0x04007DF8 RID: 32248
	[PublicizedFrom(EAccessModifier.Private)]
	public static string m_FullPath = null;

	// Token: 0x04007DF9 RID: 32249
	[PublicizedFrom(EAccessModifier.Private)]
	public static int m_LineNumber = 0;

	// Token: 0x04007DFA RID: 32250
	[PublicizedFrom(EAccessModifier.Private)]
	public static Type m_Type = null;

	// Token: 0x04007DFB RID: 32251
	public static bool LogErrors = true;

	// Token: 0x04007DFC RID: 32252
	[PublicizedFrom(EAccessModifier.Private)]
	public static vp_ComponentPreset.ReadMode m_ReadMode = vp_ComponentPreset.ReadMode.Normal;

	// Token: 0x04007DFD RID: 32253
	[PublicizedFrom(EAccessModifier.Private)]
	public Type m_ComponentType;

	// Token: 0x04007DFE RID: 32254
	[PublicizedFrom(EAccessModifier.Private)]
	public List<vp_ComponentPreset.Field> m_Fields = new List<vp_ComponentPreset.Field>();

	// Token: 0x04007DFF RID: 32255
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

	// Token: 0x0200151E RID: 5406
	[PublicizedFrom(EAccessModifier.Private)]
	public enum ReadMode
	{
		// Token: 0x04007E01 RID: 32257
		Normal,
		// Token: 0x04007E02 RID: 32258
		LineComment,
		// Token: 0x04007E03 RID: 32259
		BlockComment
	}

	// Token: 0x0200151F RID: 5407
	[PublicizedFrom(EAccessModifier.Private)]
	public class Field
	{
		// Token: 0x0600A9C8 RID: 43464 RVA: 0x003F6875 File Offset: 0x003F4A75
		public Field(RuntimeFieldHandle fieldHandle, object args)
		{
			this.FieldHandle = fieldHandle;
			this.Args = args;
		}

		// Token: 0x04007E04 RID: 32260
		public RuntimeFieldHandle FieldHandle;

		// Token: 0x04007E05 RID: 32261
		public object Args;
	}
}
