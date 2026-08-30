using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F61 RID: 3937
[Preserve]
public class XUiC_Location : XUiController
{
	// Token: 0x06007C1B RID: 31771 RVA: 0x002D97B0 File Offset: 0x002D79B0
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x06007C1C RID: 31772 RVA: 0x002EF4A4 File Offset: 0x002ED6A4
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.LocalPlayer == null && this.xui != null && this.xui.playerUI != null && this.xui.playerUI.entityPlayer != null)
		{
			this.LocalPlayer = this.xui.playerUI.entityPlayer;
		}
		if (this.LocalPlayer == null)
		{
			return;
		}
		if (XUiC_Location.ShowLocation == XUiC_Location.ShowLocationInfoTypes.No)
		{
			if (base.ViewComponent.IsVisible)
			{
				base.ViewComponent.IsVisible = false;
			}
			return;
		}
		GUIWindowManager windowManager = this.xui.playerUI.windowManager;
		if (windowManager.IsHUDEnabled() || (this.xui.DragAndDropWindow.InMenu && windowManager.IsHUDPartialHidden()))
		{
			if (base.ViewComponent.IsVisible && this.LocalPlayer.IsDead())
			{
				base.ViewComponent.IsVisible = false;
			}
			else if (!base.ViewComponent.IsVisible && !this.LocalPlayer.IsDead())
			{
				base.ViewComponent.IsVisible = true;
			}
		}
		else
		{
			base.ViewComponent.IsVisible = false;
		}
		if (!this.LocalPlayer.IsAlive())
		{
			base.ViewComponent.IsVisible = false;
			return;
		}
		PrefabInstance prefab = this.LocalPlayer.prefab;
		if (prefab != this.lastPrefabInstance || this.LocalPlayer.biomeStandingOn != this.lastBiome)
		{
			if (prefab != null && prefab.IsWithinInfoArea(this.LocalPlayer.position))
			{
				if (prefab.prefab.Tags.Test_AnySet(this.partTag) || prefab.prefab.Tags.Test_AnySet(this.streetTileTag) || prefab.prefab.Tags.Test_AnySet(this.navOnlyTileTag) || prefab.prefab.Tags.Test_AnySet(this.hideUITag))
				{
					this.lastPrefabInstance = null;
				}
				else
				{
					this.lastPrefabInstance = prefab;
				}
			}
			else
			{
				this.lastPrefabInstance = null;
			}
			this.lastPrefab = ((this.lastPrefabInstance != null) ? this.lastPrefabInstance.prefab : null);
			if (this.lastPrefab != null)
			{
				this.prefabDiff = (int)this.lastPrefab.DifficultyTier;
			}
			else
			{
				this.prefabDiff = 0;
			}
			this.lastBiome = this.LocalPlayer.biomeStandingOn;
			if (this.lastBiome != null)
			{
				float num = (float)(this.lastBiome.Difficulty - 1) * 0.5f;
				this.biomeDiff = (int)Mathf.Floor(num);
				this.showBiomeHalf = num - (float)this.biomeDiff == 0.5f;
			}
			else
			{
				this.biomeDiff = 0;
				this.showBiomeHalf = false;
			}
			base.RefreshBindings();
		}
	}

	// Token: 0x06007C1D RID: 31773 RVA: 0x002EF75C File Offset: 0x002ED95C
	public override void OnClose()
	{
		base.OnClose();
		base.RefreshBindings();
	}

	// Token: 0x06007C1E RID: 31774 RVA: 0x002EF76C File Offset: 0x002ED96C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_bindingName);
		if (num <= 1576332949U)
		{
			if (num <= 1518322684U)
			{
				if (num <= 1484767446U)
				{
					if (num != 1451212208U)
					{
						if (num == 1484767446U)
						{
							if (_bindingName == "visible3")
							{
								_value = (XUiC_Location.ShowLocation == XUiC_Location.ShowLocationInfoTypes.Yes && this.prefabDiff + this.biomeDiff >= 3).ToString();
								return true;
							}
						}
					}
					else if (_bindingName == "visible1")
					{
						_value = (XUiC_Location.ShowLocation == XUiC_Location.ShowLocationInfoTypes.Yes && this.prefabDiff + this.biomeDiff >= 1).ToString();
						return true;
					}
				}
				else if (num != 1501545065U)
				{
					if (num == 1518322684U)
					{
						if (_bindingName == "visible5")
						{
							_value = (XUiC_Location.ShowLocation == XUiC_Location.ShowLocationInfoTypes.Yes && this.prefabDiff + this.biomeDiff >= 5).ToString();
							return true;
						}
					}
				}
				else if (_bindingName == "visible2")
				{
					_value = (XUiC_Location.ShowLocation == XUiC_Location.ShowLocationInfoTypes.Yes && this.prefabDiff + this.biomeDiff >= 2).ToString();
					return true;
				}
			}
			else if (num <= 1551877922U)
			{
				if (num != 1535100303U)
				{
					if (num == 1551877922U)
					{
						if (_bindingName == "visible7")
						{
							_value = (XUiC_Location.ShowLocation == XUiC_Location.ShowLocationInfoTypes.Yes && this.prefabDiff + this.biomeDiff >= 7).ToString();
							return true;
						}
					}
				}
				else if (_bindingName == "visible4")
				{
					_value = (XUiC_Location.ShowLocation == XUiC_Location.ShowLocationInfoTypes.Yes && this.prefabDiff + this.biomeDiff >= 4).ToString();
					return true;
				}
			}
			else if (num != 1568655541U)
			{
				if (num == 1576332949U)
				{
					if (_bindingName == "visible_loot_max")
					{
						if (this.LocalPlayer == null)
						{
							_value = "false";
							return true;
						}
						_value = this.LocalPlayer.LootAtMax.ToString();
						return true;
					}
				}
			}
			else if (_bindingName == "visible6")
			{
				_value = (XUiC_Location.ShowLocation == XUiC_Location.ShowLocationInfoTypes.Yes && this.prefabDiff + this.biomeDiff >= 6).ToString();
				return true;
			}
		}
		else if (num <= 2327937225U)
		{
			if (num <= 2294381987U)
			{
				if (num != 2277604368U)
				{
					if (num == 2294381987U)
					{
						if (_bindingName == "difficultycolor6")
						{
							_value = this.inactiveColorFormatter.Format(this.inactiveColor);
							if (this.prefabDiff >= 6)
							{
								_value = this.activeColorFormatter.Format(this.difficultyActiveColor);
							}
							else if (this.prefabDiff + this.biomeDiff >= 6)
							{
								_value = this.activeColorFormatter.Format(this.biomeActiveColor);
							}
							return true;
						}
					}
				}
				else if (_bindingName == "difficultycolor7")
				{
					_value = this.inactiveColorFormatter.Format(this.inactiveColor);
					if (this.prefabDiff >= 7)
					{
						_value = this.activeColorFormatter.Format(this.difficultyActiveColor);
					}
					else if (this.prefabDiff + this.biomeDiff >= 7)
					{
						_value = this.activeColorFormatter.Format(this.biomeActiveColor);
					}
					return true;
				}
			}
			else if (num != 2311159606U)
			{
				if (num == 2327937225U)
				{
					if (_bindingName == "difficultycolor4")
					{
						_value = this.inactiveColorFormatter.Format(this.inactiveColor);
						if (this.prefabDiff >= 4)
						{
							_value = this.activeColorFormatter.Format(this.difficultyActiveColor);
						}
						else if (this.prefabDiff + this.biomeDiff >= 4)
						{
							_value = this.activeColorFormatter.Format(this.biomeActiveColor);
						}
						return true;
					}
				}
			}
			else if (_bindingName == "difficultycolor5")
			{
				_value = this.inactiveColorFormatter.Format(this.inactiveColor);
				if (this.prefabDiff >= 5)
				{
					_value = this.activeColorFormatter.Format(this.difficultyActiveColor);
				}
				else if (this.prefabDiff + this.biomeDiff >= 5)
				{
					_value = this.activeColorFormatter.Format(this.biomeActiveColor);
				}
				return true;
			}
		}
		else if (num <= 2361492463U)
		{
			if (num != 2344714844U)
			{
				if (num == 2361492463U)
				{
					if (_bindingName == "difficultycolor2")
					{
						_value = this.inactiveColorFormatter.Format(this.inactiveColor);
						if (this.prefabDiff >= 2)
						{
							_value = this.activeColorFormatter.Format(this.difficultyActiveColor);
						}
						else if (this.prefabDiff + this.biomeDiff >= 2)
						{
							_value = this.activeColorFormatter.Format(this.biomeActiveColor);
						}
						return true;
					}
				}
			}
			else if (_bindingName == "difficultycolor3")
			{
				_value = this.inactiveColorFormatter.Format(this.inactiveColor);
				if (this.prefabDiff >= 3)
				{
					_value = this.activeColorFormatter.Format(this.difficultyActiveColor);
				}
				else if (this.prefabDiff + this.biomeDiff >= 3)
				{
					_value = this.activeColorFormatter.Format(this.biomeActiveColor);
				}
				return true;
			}
		}
		else if (num != 2378270082U)
		{
			if (num != 4063875975U)
			{
				if (num == 4157284869U)
				{
					if (_bindingName == "locationname")
					{
						if (this.lastPrefab != null)
						{
							_value = this.lastPrefab.LocalizedName;
						}
						else if (this.lastBiome != null)
						{
							_value = this.lastBiome.LocalizedName;
						}
						else
						{
							_value = "";
						}
						return true;
					}
				}
			}
			else if (_bindingName == "visible_half")
			{
				_value = (XUiC_Location.ShowLocation == XUiC_Location.ShowLocationInfoTypes.Yes && this.showBiomeHalf).ToString();
				return true;
			}
		}
		else if (_bindingName == "difficultycolor1")
		{
			_value = this.inactiveColorFormatter.Format(this.inactiveColor);
			if (this.prefabDiff >= 1)
			{
				_value = this.activeColorFormatter.Format(this.difficultyActiveColor);
			}
			else if (this.prefabDiff + this.biomeDiff >= 1)
			{
				_value = this.activeColorFormatter.Format(this.biomeActiveColor);
			}
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x06007C1F RID: 31775 RVA: 0x002EFE68 File Offset: 0x002EE068
	public override bool ParseAttribute(string name, string value)
	{
		bool flag = base.ParseAttribute(name, value);
		if (!flag)
		{
			if (!(name == "active_color"))
			{
				if (!(name == "inactive_color"))
				{
					return false;
				}
				this.inactiveColor = StringParsers.ParseColor32(value);
			}
			else
			{
				this.difficultyActiveColor = StringParsers.ParseColor32(value);
			}
			return true;
		}
		return flag;
	}

	// Token: 0x04005BD1 RID: 23505
	[PublicizedFrom(EAccessModifier.Private)]
	public Prefab lastPrefab;

	// Token: 0x04005BD2 RID: 23506
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabInstance lastPrefabInstance;

	// Token: 0x04005BD3 RID: 23507
	[PublicizedFrom(EAccessModifier.Private)]
	public BiomeDefinition lastBiome;

	// Token: 0x04005BD4 RID: 23508
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal LocalPlayer;

	// Token: 0x04005BD5 RID: 23509
	[PublicizedFrom(EAccessModifier.Private)]
	public int prefabDiff;

	// Token: 0x04005BD6 RID: 23510
	[PublicizedFrom(EAccessModifier.Private)]
	public int biomeDiff;

	// Token: 0x04005BD7 RID: 23511
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showBiomeHalf;

	// Token: 0x04005BD8 RID: 23512
	[PublicizedFrom(EAccessModifier.Private)]
	public Color difficultyActiveColor = Color.red;

	// Token: 0x04005BD9 RID: 23513
	[PublicizedFrom(EAccessModifier.Private)]
	public Color biomeActiveColor = new Color(1f, 0.5f, 0f);

	// Token: 0x04005BDA RID: 23514
	[PublicizedFrom(EAccessModifier.Private)]
	public Color inactiveColor = Color.grey;

	// Token: 0x04005BDB RID: 23515
	public static XUiC_Location.ShowLocationInfoTypes ShowLocation = XUiC_Location.ShowLocationInfoTypes.Yes;

	// Token: 0x04005BDC RID: 23516
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Poi> partTag = FastTags<TagGroup.Poi>.Parse("part");

	// Token: 0x04005BDD RID: 23517
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Poi> streetTileTag = FastTags<TagGroup.Poi>.Parse("streettile");

	// Token: 0x04005BDE RID: 23518
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Poi> navOnlyTileTag = FastTags<TagGroup.Poi>.Parse("navonly");

	// Token: 0x04005BDF RID: 23519
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Poi> hideUITag = FastTags<TagGroup.Poi>.Parse("hideui");

	// Token: 0x04005BE0 RID: 23520
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor activeColorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005BE1 RID: 23521
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor inactiveColorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x02000F62 RID: 3938
	public enum ShowLocationInfoTypes
	{
		// Token: 0x04005BE3 RID: 23523
		No,
		// Token: 0x04005BE4 RID: 23524
		Yes,
		// Token: 0x04005BE5 RID: 23525
		NameOnly
	}
}
