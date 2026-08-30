using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EF7 RID: 3831
[Preserve]
public class XUiC_CreatePoi : XUiController
{
	// Token: 0x17000E09 RID: 3593
	// (get) Token: 0x0600782A RID: 30762 RVA: 0x002D5DC4 File Offset: 0x002D3FC4
	public bool IsCustomSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxList<XUiC_CreatePoi.PoiSizeInfo> xuiC_ComboBoxList = this.cmbSize;
			bool? flag;
			if (xuiC_ComboBoxList == null)
			{
				flag = null;
			}
			else
			{
				XUiC_CreatePoi.PoiSizeInfo value = xuiC_ComboBoxList.Value;
				flag = ((value != null) ? new bool?(value.IsCustom) : null);
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}
	}

	// Token: 0x17000E0A RID: 3594
	// (get) Token: 0x0600782B RID: 30763 RVA: 0x002D5E0C File Offset: 0x002D400C
	public Vector3i SelectedSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (!this.IsCustomSize)
			{
				return new Vector3i(this.cmbSize.Value.Size.x, this.depth, this.cmbSize.Value.Size.z);
			}
			return new Vector3i(this.customSizeX, this.depth, this.customSizeZ);
		}
	}

	// Token: 0x17000E0B RID: 3595
	// (get) Token: 0x0600782C RID: 30764 RVA: 0x002D5E6E File Offset: 0x002D406E
	public PathAbstractions.EAbstractedLocationType SelectedLocation
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_ComboBoxEnum<PathAbstractions.EAbstractedLocationType> xuiC_ComboBoxEnum = this.cmbLocation;
			if (xuiC_ComboBoxEnum == null)
			{
				return PathAbstractions.EAbstractedLocationType.UserDataPath;
			}
			return xuiC_ComboBoxEnum.Value;
		}
	}

	// Token: 0x17000E0C RID: 3596
	// (get) Token: 0x0600782D RID: 30765 RVA: 0x002D5E81 File Offset: 0x002D4081
	public bool ValidCustomSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.customSizeX > 0 && this.customSizeX <= 300 && this.customSizeZ > 0 && this.customSizeZ <= 300;
		}
	}

	// Token: 0x17000E0D RID: 3597
	// (get) Token: 0x0600782E RID: 30766 RVA: 0x002D5EB4 File Offset: 0x002D40B4
	public bool ValidSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !this.IsCustomSize || this.ValidCustomSize;
		}
	}

	// Token: 0x17000E0E RID: 3598
	// (get) Token: 0x0600782F RID: 30767 RVA: 0x002D5EC6 File Offset: 0x002D40C6
	public bool ValidDepth
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.depth > 0 && this.depth <= 30;
		}
	}

	// Token: 0x06007830 RID: 30768 RVA: 0x002D5EE0 File Offset: 0x002D40E0
	public override void Init()
	{
		base.Init();
		XUiC_CreatePoi.ID = base.WindowGroup.Id;
		XUiC_TextInput xuiC_TextInput = base.GetChildById("txtName") as XUiC_TextInput;
		if (xuiC_TextInput != null)
		{
			this.txtName = xuiC_TextInput;
			this.txtName.OnChangeHandler += delegate(XUiController _, string _text, bool _)
			{
				this.name = _text;
				this.validateName();
			};
		}
		XUiC_ComboBoxList<XUiC_CreatePoi.PoiSizeInfo> xuiC_ComboBoxList = base.GetChildById("cmbSize") as XUiC_ComboBoxList<XUiC_CreatePoi.PoiSizeInfo>;
		if (xuiC_ComboBoxList != null)
		{
			this.cmbSize = xuiC_ComboBoxList;
			this.cmbSize.OnValueChanged += delegate(XUiController _, XUiC_CreatePoi.PoiSizeInfo _, XUiC_CreatePoi.PoiSizeInfo _newSize)
			{
				this.autoSelectFolder(_newSize);
				this.IsDirty = true;
			};
		}
		XUiC_TextInput xuiC_TextInput2 = base.GetChildById("txtSizeX") as XUiC_TextInput;
		if (xuiC_TextInput2 != null)
		{
			this.txtSizeX = xuiC_TextInput2;
			this.txtSizeX.OnChangeHandler += delegate(XUiController _, string _text, bool _)
			{
				this.sizeTextChanged(_text, false);
			};
		}
		XUiC_TextInput xuiC_TextInput3 = base.GetChildById("txtSizeZ") as XUiC_TextInput;
		if (xuiC_TextInput3 != null)
		{
			this.txtSizeZ = xuiC_TextInput3;
			this.txtSizeZ.OnChangeHandler += delegate(XUiController _, string _text, bool _)
			{
				this.sizeTextChanged(_text, true);
			};
		}
		XUiC_TextInput xuiC_TextInput4 = base.GetChildById("txtDepth") as XUiC_TextInput;
		if (xuiC_TextInput4 != null)
		{
			this.txtDepth = xuiC_TextInput4;
			this.txtDepth.OnChangeHandler += this.depthTextChanged;
		}
		XUiC_ComboBoxEnum<PathAbstractions.EAbstractedLocationType> xuiC_ComboBoxEnum = base.GetChildById("cmbLocation") as XUiC_ComboBoxEnum<PathAbstractions.EAbstractedLocationType>;
		if (xuiC_ComboBoxEnum != null)
		{
			this.cmbLocation = xuiC_ComboBoxEnum;
			this.cmbLocation.Elements.Remove(PathAbstractions.EAbstractedLocationType.GameData);
			this.cmbLocation.Value = PathAbstractions.EAbstractedLocationType.UserDataPath;
			this.cmbLocation.TriggerValueChangedEvent(PathAbstractions.EAbstractedLocationType.GameData);
			this.cmbLocation.OnValueChanged += delegate(XUiController _, PathAbstractions.EAbstractedLocationType _, PathAbstractions.EAbstractedLocationType _)
			{
				this.updateFolders();
				this.IsDirty = true;
			};
		}
		XUiC_ComboBoxList<Mod> xuiC_ComboBoxList2 = base.GetChildById("cmbMod") as XUiC_ComboBoxList<Mod>;
		if (xuiC_ComboBoxList2 != null)
		{
			this.cmbMod = xuiC_ComboBoxList2;
			this.cmbMod.OnValueChanged += delegate(XUiController _, Mod _, Mod _)
			{
				this.updateFolders();
				this.IsDirty = true;
			};
		}
		XUiC_PrefabFolderList xuiC_PrefabFolderList = base.GetChildById("folders") as XUiC_PrefabFolderList;
		if (xuiC_PrefabFolderList != null)
		{
			this.folderList = xuiC_PrefabFolderList;
			this.folderList.SelectionChanged += delegate(XUiC_List<XUiC_PrefabFolderList.PrefabFolderEntry> _, XUiC_PrefabFolderList.PrefabFolderEntry _, XUiC_PrefabFolderList.PrefabFolderEntry _)
			{
				this.validateName();
			};
			this.folderList.SelectedEntryIndex = 0;
			this.validateName();
		}
		XUiC_SimpleButton xuiC_SimpleButton = base.GetChildById("btnCancel") as XUiC_SimpleButton;
		if (xuiC_SimpleButton != null)
		{
			xuiC_SimpleButton.OnPressed += this.BtnBack_OnPressed;
		}
		XUiC_SimpleButton xuiC_SimpleButton2 = base.GetChildById("btnOk") as XUiC_SimpleButton;
		if (xuiC_SimpleButton2 != null)
		{
			xuiC_SimpleButton2.OnPressed += this.BtnOk_OnPressed;
		}
	}

	// Token: 0x06007831 RID: 30769 RVA: 0x002D612D File Offset: 0x002D432D
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.txtDepth != null)
		{
			this.depthTextChanged(this.txtDepth, this.txtDepth.Text, false);
		}
		this.updateMods();
		this.updateFolders();
		this.IsDirty = true;
	}

	// Token: 0x06007832 RID: 30770 RVA: 0x002D6168 File Offset: 0x002D4368
	[PublicizedFrom(EAccessModifier.Private)]
	public void sizeTextChanged(string _text, bool _isZ)
	{
		int num;
		StringParsers.TryParseSInt32(_text, out num);
		if (_isZ)
		{
			this.customSizeZ = num;
		}
		else
		{
			this.customSizeX = num;
		}
		this.IsDirty = true;
	}

	// Token: 0x06007833 RID: 30771 RVA: 0x002D6198 File Offset: 0x002D4398
	[PublicizedFrom(EAccessModifier.Private)]
	public void depthTextChanged(XUiController _sender, string _text, bool _changeFromCode)
	{
		StringParsers.TryParseSInt32(_text, out this.depth);
		this.IsDirty = true;
	}

	// Token: 0x06007834 RID: 30772 RVA: 0x002D61AE File Offset: 0x002D43AE
	[PublicizedFrom(EAccessModifier.Private)]
	public void autoSelectFolder(XUiC_CreatePoi.PoiSizeInfo _newValue)
	{
		if (this.folderList == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(_newValue.DefaultFolder))
		{
			return;
		}
		this.folderList.SelectByName(_newValue.DefaultFolder);
	}

	// Token: 0x06007835 RID: 30773 RVA: 0x002D61DC File Offset: 0x002D43DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void validateName()
	{
		bool flag = this.name != null && this.name.Length > 0 && !this.name.Contains(" ") && GameUtils.ValidateGameName(this.name) && !this.name.EndsWith("_signs", StringComparison.OrdinalIgnoreCase);
		this.nameExistsInLocation = false;
		if (flag)
		{
			Regex regex = new Regex("^" + this.name + "$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
			List<PathAbstractions.AbstractedLocation> availablePathsList = PathAbstractions.PrefabsSearchPaths.GetAvailablePathsList(regex, false, new PathAbstractions.EAbstractedLocationType?(this.SelectedLocation));
			if (this.SelectedLocation == PathAbstractions.EAbstractedLocationType.Mods)
			{
				using (List<PathAbstractions.AbstractedLocation>.Enumerator enumerator = availablePathsList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.ContainingMod == this.cmbMod.Value)
						{
							this.nameExistsInLocation = true;
							break;
						}
					}
					goto IL_00EE;
				}
			}
			this.nameExistsInLocation = availablePathsList.Count > 0;
		}
		IL_00EE:
		this.validName = flag && !this.nameExistsInLocation;
		XUiC_PrefabFolderList.PrefabFolderEntry selectedEntryData = this.folderList.SelectedEntryData;
		this.validFolder = !string.IsNullOrEmpty((selectedEntryData != null) ? selectedEntryData.AbsolutePath : null);
		this.IsDirty = true;
	}

	// Token: 0x06007836 RID: 30774 RVA: 0x002D6328 File Offset: 0x002D4528
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateMods()
	{
		List<Mod> loadedMods = ModManager.GetLoadedMods();
		if (this.cmbLocation != null)
		{
			if (loadedMods.Count == 0)
			{
				this.cmbLocation.Elements.Remove(PathAbstractions.EAbstractedLocationType.Mods);
			}
			else if (!this.cmbLocation.Elements.Contains(PathAbstractions.EAbstractedLocationType.Mods))
			{
				this.cmbLocation.Elements.Add(PathAbstractions.EAbstractedLocationType.Mods);
			}
		}
		if (this.cmbMod != null)
		{
			this.cmbMod.Elements.Clear();
			loadedMods.Sort((Mod _a, Mod _b) => string.Compare(_a.DisplayName, _b.DisplayName, StringComparison.OrdinalIgnoreCase));
			foreach (Mod mod in loadedMods)
			{
				this.cmbMod.Elements.Add(mod);
			}
			if (loadedMods.Count > 0)
			{
				this.cmbMod.SelectedIndex = 0;
				this.cmbMod.TriggerValueChangedEvent(null);
			}
		}
	}

	// Token: 0x06007837 RID: 30775 RVA: 0x002D6430 File Offset: 0x002D4630
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateFolders()
	{
		if (this.folderList != null)
		{
			XUiC_PrefabFolderList xuiC_PrefabFolderList = this.folderList;
			XUiC_ComboBoxList<Mod> xuiC_ComboBoxList = this.cmbMod;
			xuiC_PrefabFolderList.Mod = ((xuiC_ComboBoxList != null) ? xuiC_ComboBoxList.Value : null);
			XUiC_PrefabFolderList xuiC_PrefabFolderList2 = this.folderList;
			XUiC_ComboBoxEnum<PathAbstractions.EAbstractedLocationType> xuiC_ComboBoxEnum = this.cmbLocation;
			xuiC_PrefabFolderList2.LocationType = ((xuiC_ComboBoxEnum != null) ? xuiC_ComboBoxEnum.Value : PathAbstractions.EAbstractedLocationType.UserDataPath);
		}
		XUiC_ComboBoxList<XUiC_CreatePoi.PoiSizeInfo> xuiC_ComboBoxList2 = this.cmbSize;
		if (xuiC_ComboBoxList2 == null)
		{
			return;
		}
		xuiC_ComboBoxList2.TriggerValueChangedEvent(null);
	}

	// Token: 0x06007838 RID: 30776 RVA: 0x002D6490 File Offset: 0x002D4690
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (!this.IsDirty)
		{
			return;
		}
		base.RefreshBindings();
		this.IsDirty = false;
	}

	// Token: 0x06007839 RID: 30777 RVA: 0x002D64AF File Offset: 0x002D46AF
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_InGameMenuWindow.ID, true);
	}

	// Token: 0x0600783A RID: 30778 RVA: 0x002D64E8 File Offset: 0x002D46E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnOk_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		if (XUiC_LevelToolsHelpers.IsShowImposter())
		{
			XUiC_LevelToolsHelpers.SetShowImposter();
		}
		PrefabEditModeManager.Instance.NewVoxelPrefab();
		Vector3i vector3i = -1 * (this.SelectedSize / 2);
		vector3i.y = 1;
		Vector3i vector3i2 = vector3i + this.SelectedSize - Vector3i.one;
		BlockValue blockValue = Block.GetBlockValue(Constants.cTerrainFillerBlockName, false);
		Block block = blockValue.Block;
		BlockPlacement.Result result = new BlockPlacement.Result(BlockPlacement.EnumPlacement.Voxel, Vector3.one * 0.5f, Vector3i.zero, BlockFace.None, blockValue, PropTransform.identity);
		block.OnBlockPlaceBefore(GameManager.Instance.World, ref result, this.xui.playerUI.entityPlayer, GameManager.Instance.World.GetGameRandom());
		blockValue = result.blockValue;
		blockValue.rotation = blockValue.rotation;
		BlockTools.CubeRPC(GameManager.Instance, vector3i, vector3i2, blockValue, blockValue.Block.shape.IsTerrain() ? MarchingCubes.DensityTerrain : MarchingCubes.DensityAir, 0, TextureFullArray.Default);
		PrefabEditModeManager.Instance.SetGroundLevel(-this.SelectedSize.y);
		XUiC_PrefabFolderList.PrefabFolderEntry selectedEntryData = this.folderList.SelectedEntryData;
		string text = ((selectedEntryData != null) ? selectedEntryData.RelativePath : null);
		PathAbstractions.SearchDefinition prefabsSearchPaths = PathAbstractions.PrefabsSearchPaths;
		PathAbstractions.EAbstractedLocationType selectedLocation = this.SelectedLocation;
		string text2 = text;
		string text3 = this.name;
		XUiC_ComboBoxList<Mod> xuiC_ComboBoxList = this.cmbMod;
		PathAbstractions.AbstractedLocation? abstractedLocation = prefabsSearchPaths.BuildLocation(selectedLocation, text2, text3, (xuiC_ComboBoxList != null) ? xuiC_ComboBoxList.Value : null, null);
		PrefabEditModeManager.Instance.VoxelPrefab.location = abstractedLocation.Value;
		if (PrefabEditModeManager.Instance.SaveVoxelPrefab())
		{
			GameManager.ShowTooltip(this.xui.playerUI.entityPlayer, string.Format(Localization.Get("xuiPrefabsPrefabSaved", false, null), PrefabEditModeManager.Instance.LoadedPrefab.Name), false, false, 0f);
		}
		else
		{
			GameManager.ShowTooltip(this.xui.playerUI.entityPlayer, Localization.Get("xuiPrefabsPrefabSavingError", false, null), false, false, 0f);
		}
		this.xui.playerUI.windowManager.Open(XUiC_InGameMenuWindow.ID, true);
	}

	// Token: 0x0600783B RID: 30779 RVA: 0x002D6713 File Offset: 0x002D4913
	public override bool ParseAttribute(string _name, string _value)
	{
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x0600783C RID: 30780 RVA: 0x002D6720 File Offset: 0x002D4920
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_bindingName);
		if (num <= 1682061105U)
		{
			if (num <= 1015985003U)
			{
				if (num != 200649126U)
				{
					if (num == 1015985003U)
					{
						if (_bindingName == "name_exists")
						{
							_value = this.nameExistsInLocation.ToString();
							return true;
						}
					}
				}
				else if (_bindingName == "location")
				{
					_value = this.SelectedLocation.ToStringCached<PathAbstractions.EAbstractedLocationType>();
					return true;
				}
			}
			else if (num != 1432300115U)
			{
				if (num == 1682061105U)
				{
					if (_bindingName == "valid_name")
					{
						_value = this.validName.ToString();
						return true;
					}
				}
			}
			else if (_bindingName == "valid_size")
			{
				_value = this.ValidSize.ToString();
				return true;
			}
		}
		else if (num <= 2664466067U)
		{
			if (num != 2354714994U)
			{
				if (num == 2664466067U)
				{
					if (_bindingName == "valid_custom_size")
					{
						_value = this.ValidCustomSize.ToString();
						return true;
					}
				}
			}
			else if (_bindingName == "valid_folder")
			{
				_value = this.validFolder.ToString();
				return true;
			}
		}
		else if (num != 3214380611U)
		{
			if (num == 3850360370U)
			{
				if (_bindingName == "size_custom")
				{
					_value = this.IsCustomSize.ToString();
					return true;
				}
			}
		}
		else if (_bindingName == "valid_depth")
		{
			_value = this.ValidDepth.ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x040058BE RID: 22718
	public static string ID = "";

	// Token: 0x040058BF RID: 22719
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtName;

	// Token: 0x040058C0 RID: 22720
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<XUiC_CreatePoi.PoiSizeInfo> cmbSize;

	// Token: 0x040058C1 RID: 22721
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtSizeX;

	// Token: 0x040058C2 RID: 22722
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtSizeZ;

	// Token: 0x040058C3 RID: 22723
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtDepth;

	// Token: 0x040058C4 RID: 22724
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<PathAbstractions.EAbstractedLocationType> cmbLocation;

	// Token: 0x040058C5 RID: 22725
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<Mod> cmbMod;

	// Token: 0x040058C6 RID: 22726
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PrefabFolderList folderList;

	// Token: 0x040058C7 RID: 22727
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x040058C8 RID: 22728
	[PublicizedFrom(EAccessModifier.Private)]
	public int customSizeX;

	// Token: 0x040058C9 RID: 22729
	[PublicizedFrom(EAccessModifier.Private)]
	public int customSizeZ;

	// Token: 0x040058CA RID: 22730
	[PublicizedFrom(EAccessModifier.Private)]
	public int depth = 1;

	// Token: 0x040058CB RID: 22731
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validName;

	// Token: 0x040058CC RID: 22732
	[PublicizedFrom(EAccessModifier.Private)]
	public bool nameExistsInLocation;

	// Token: 0x040058CD RID: 22733
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validFolder;

	// Token: 0x02000EF8 RID: 3832
	[Preserve]
	public class PoiSizeInfo
	{
		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x06007846 RID: 30790 RVA: 0x002D6934 File Offset: 0x002D4B34
		public Vector3i Size
		{
			get
			{
				return new Vector3i(this.X, 1, this.Z);
			}
		}

		// Token: 0x06007847 RID: 30791 RVA: 0x002D6948 File Offset: 0x002D4B48
		public PoiSizeInfo(int _x, int _z, string _label, string _defaultFolder, bool _isCustom = false)
		{
			this.X = _x;
			this.Z = _z;
			this.Label = _label;
			this.DefaultFolder = _defaultFolder;
			this.IsCustom = _isCustom;
		}

		// Token: 0x06007848 RID: 30792 RVA: 0x002D6978 File Offset: 0x002D4B78
		public PoiSizeInfo(string _input)
		{
			if (_input.StartsWith("custom", StringComparison.OrdinalIgnoreCase))
			{
				this.IsCustom = true;
				int num = _input.IndexOf('/');
				if (num > 0)
				{
					this.DefaultFolder = _input.Substring(num + 1);
				}
				return;
			}
			Match match = XUiC_CreatePoi.PoiSizeInfo.infoMatcher.Match(_input);
			if (!match.Success)
			{
				throw new FormatException("PoiSizeInfo: Input ('" + _input + "') in invalid form. Needs to be \"<width>x<height>\" or \"<width>x<height>/<label>\" or \"Custom\".");
			}
			if (!StringParsers.TryParseSInt32(match.Groups[1].Value, out this.X))
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"PoiSizeInfo: Input ('",
					_input,
					"') in invalid form, first part ('",
					match.Groups[1].Value,
					"') is not a valid integer. Needs to be \"<width>x<height>\" or \"<width>x<height>/<label>\" or \"Custom\"."
				}));
			}
			if (!StringParsers.TryParseSInt32(match.Groups[2].Value, out this.Z))
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"PoiSizeInfo: Input ('",
					_input,
					"') in invalid form, first part ('",
					match.Groups[2].Value,
					"') is not a valid integer. Needs to be \"<width>x<height>\" or \"<width>x<height>/<label>\" or \"Custom\"."
				}));
			}
			if (match.Groups[3].Success)
			{
				this.Label = match.Groups[3].Value;
			}
			if (match.Groups[4].Success)
			{
				this.DefaultFolder = match.Groups[4].Value;
			}
		}

		// Token: 0x06007849 RID: 30793 RVA: 0x002D6AF9 File Offset: 0x002D4CF9
		public override string ToString()
		{
			if (!this.IsCustom)
			{
				return string.Format("{0}x{1} ({2})", this.X, this.Z, this.Label);
			}
			return Localization.Get("xuiCustom", false, null);
		}

		// Token: 0x040058CE RID: 22734
		public readonly int X;

		// Token: 0x040058CF RID: 22735
		public readonly int Z;

		// Token: 0x040058D0 RID: 22736
		public readonly bool IsCustom;

		// Token: 0x040058D1 RID: 22737
		public readonly string Label;

		// Token: 0x040058D2 RID: 22738
		public readonly string DefaultFolder;

		// Token: 0x040058D3 RID: 22739
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Regex infoMatcher = new Regex("^(\\d+)x(\\d+)(?:\\/([^/]*))(?:\\/([^/]*))$", RegexOptions.Compiled | RegexOptions.Singleline);
	}
}
