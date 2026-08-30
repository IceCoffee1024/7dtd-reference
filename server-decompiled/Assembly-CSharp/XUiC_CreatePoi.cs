using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F0E RID: 3854
[Preserve]
public class XUiC_CreatePoi : XUiController
{
	// Token: 0x17000E23 RID: 3619
	// (get) Token: 0x060078E2 RID: 30946 RVA: 0x002D9D50 File Offset: 0x002D7F50
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

	// Token: 0x17000E24 RID: 3620
	// (get) Token: 0x060078E3 RID: 30947 RVA: 0x002D9D98 File Offset: 0x002D7F98
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

	// Token: 0x17000E25 RID: 3621
	// (get) Token: 0x060078E4 RID: 30948 RVA: 0x002D9DFA File Offset: 0x002D7FFA
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

	// Token: 0x17000E26 RID: 3622
	// (get) Token: 0x060078E5 RID: 30949 RVA: 0x002D9E0D File Offset: 0x002D800D
	public bool ValidCustomSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.customSizeX > 0 && this.customSizeX <= 300 && this.customSizeZ > 0 && this.customSizeZ <= 300;
		}
	}

	// Token: 0x17000E27 RID: 3623
	// (get) Token: 0x060078E6 RID: 30950 RVA: 0x002D9E40 File Offset: 0x002D8040
	public bool ValidSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return !this.IsCustomSize || this.ValidCustomSize;
		}
	}

	// Token: 0x17000E28 RID: 3624
	// (get) Token: 0x060078E7 RID: 30951 RVA: 0x002D9E52 File Offset: 0x002D8052
	public bool ValidDepth
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.depth > 0 && this.depth <= 30;
		}
	}

	// Token: 0x060078E8 RID: 30952 RVA: 0x002D9E6C File Offset: 0x002D806C
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

	// Token: 0x060078E9 RID: 30953 RVA: 0x002DA0B9 File Offset: 0x002D82B9
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

	// Token: 0x060078EA RID: 30954 RVA: 0x002DA0F4 File Offset: 0x002D82F4
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

	// Token: 0x060078EB RID: 30955 RVA: 0x002DA124 File Offset: 0x002D8324
	[PublicizedFrom(EAccessModifier.Private)]
	public void depthTextChanged(XUiController _sender, string _text, bool _changeFromCode)
	{
		StringParsers.TryParseSInt32(_text, out this.depth);
		this.IsDirty = true;
	}

	// Token: 0x060078EC RID: 30956 RVA: 0x002DA13A File Offset: 0x002D833A
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

	// Token: 0x060078ED RID: 30957 RVA: 0x002DA168 File Offset: 0x002D8368
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

	// Token: 0x060078EE RID: 30958 RVA: 0x002DA2B4 File Offset: 0x002D84B4
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

	// Token: 0x060078EF RID: 30959 RVA: 0x002DA3BC File Offset: 0x002D85BC
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

	// Token: 0x060078F0 RID: 30960 RVA: 0x002DA41C File Offset: 0x002D861C
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

	// Token: 0x060078F1 RID: 30961 RVA: 0x002DA43B File Offset: 0x002D863B
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_InGameMenuWindow.ID, true);
	}

	// Token: 0x060078F2 RID: 30962 RVA: 0x002DA474 File Offset: 0x002D8674
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

	// Token: 0x060078F3 RID: 30963 RVA: 0x002DA69F File Offset: 0x002D889F
	public override bool ParseAttribute(string _name, string _value)
	{
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x060078F4 RID: 30964 RVA: 0x002DA6AC File Offset: 0x002D88AC
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

	// Token: 0x04005970 RID: 22896
	public static string ID = "";

	// Token: 0x04005971 RID: 22897
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtName;

	// Token: 0x04005972 RID: 22898
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<XUiC_CreatePoi.PoiSizeInfo> cmbSize;

	// Token: 0x04005973 RID: 22899
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtSizeX;

	// Token: 0x04005974 RID: 22900
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtSizeZ;

	// Token: 0x04005975 RID: 22901
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtDepth;

	// Token: 0x04005976 RID: 22902
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<PathAbstractions.EAbstractedLocationType> cmbLocation;

	// Token: 0x04005977 RID: 22903
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<Mod> cmbMod;

	// Token: 0x04005978 RID: 22904
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PrefabFolderList folderList;

	// Token: 0x04005979 RID: 22905
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x0400597A RID: 22906
	[PublicizedFrom(EAccessModifier.Private)]
	public int customSizeX;

	// Token: 0x0400597B RID: 22907
	[PublicizedFrom(EAccessModifier.Private)]
	public int customSizeZ;

	// Token: 0x0400597C RID: 22908
	[PublicizedFrom(EAccessModifier.Private)]
	public int depth = 1;

	// Token: 0x0400597D RID: 22909
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validName;

	// Token: 0x0400597E RID: 22910
	[PublicizedFrom(EAccessModifier.Private)]
	public bool nameExistsInLocation;

	// Token: 0x0400597F RID: 22911
	[PublicizedFrom(EAccessModifier.Private)]
	public bool validFolder;

	// Token: 0x02000F0F RID: 3855
	[Preserve]
	public class PoiSizeInfo
	{
		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x060078FE RID: 30974 RVA: 0x002DA8C0 File Offset: 0x002D8AC0
		public Vector3i Size
		{
			get
			{
				return new Vector3i(this.X, 1, this.Z);
			}
		}

		// Token: 0x060078FF RID: 30975 RVA: 0x002DA8D4 File Offset: 0x002D8AD4
		public PoiSizeInfo(int _x, int _z, string _label, string _defaultFolder, bool _isCustom = false)
		{
			this.X = _x;
			this.Z = _z;
			this.Label = _label;
			this.DefaultFolder = _defaultFolder;
			this.IsCustom = _isCustom;
		}

		// Token: 0x06007900 RID: 30976 RVA: 0x002DA904 File Offset: 0x002D8B04
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

		// Token: 0x06007901 RID: 30977 RVA: 0x002DAA85 File Offset: 0x002D8C85
		public override string ToString()
		{
			if (!this.IsCustom)
			{
				return string.Format("{0}x{1} ({2})", this.X, this.Z, this.Label);
			}
			return Localization.Get("xuiCustom", false, null);
		}

		// Token: 0x04005980 RID: 22912
		public readonly int X;

		// Token: 0x04005981 RID: 22913
		public readonly int Z;

		// Token: 0x04005982 RID: 22914
		public readonly bool IsCustom;

		// Token: 0x04005983 RID: 22915
		public readonly string Label;

		// Token: 0x04005984 RID: 22916
		public readonly string DefaultFolder;

		// Token: 0x04005985 RID: 22917
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Regex infoMatcher = new Regex("^(\\d+)x(\\d+)(?:\\/([^/]*))(?:\\/([^/]*))$", RegexOptions.Compiled | RegexOptions.Singleline);
	}
}
