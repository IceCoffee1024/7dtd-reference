using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EF4 RID: 3828
[Preserve]
public class XUiC_CollectedItemList : XUiController
{
	// Token: 0x0600780B RID: 30731 RVA: 0x002D2CF0 File Offset: 0x002D0EF0
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("item");
		this.PrefabItems = childById.ViewComponent.UiTransform;
		this.height = (float)(childById.ViewComponent.Size.y + 2);
		this.xui.CollectedItemList = this;
	}

	// Token: 0x0600780C RID: 30732 RVA: 0x002D2D45 File Offset: 0x002D0F45
	public override void Cleanup()
	{
		base.Cleanup();
		this.xui.CollectedItemList = null;
	}

	// Token: 0x0600780D RID: 30733 RVA: 0x002D2D59 File Offset: 0x002D0F59
	public void AddRemoveItemQueueEntry(ItemStack stack)
	{
		this.removeItemQueue.Add(stack);
	}

	// Token: 0x0600780E RID: 30734 RVA: 0x002D2D68 File Offset: 0x002D0F68
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.localPlayer == null)
		{
			this.localPlayer = this.xui.playerUI.entityPlayer;
		}
		base.ViewComponent.IsVisible = !this.localPlayer.IsDead() && this.xui.playerUI.windowManager.IsHUDEnabled();
		if (this.removeItemQueue.Count > 0)
		{
			for (int i = 0; i < this.removeItemQueue.Count; i++)
			{
				this.RemoveItemStack(this.removeItemQueue[i]);
			}
			this.removeItemQueue.Clear();
		}
		if (this.items.Count > 0)
		{
			float time = Time.time;
			for (int j = 0; j < this.items.Count; j++)
			{
				if (time - this.items[j].TimeAdded > 5f)
				{
					this.removeLastEntry(j);
					j--;
				}
			}
		}
	}

	// Token: 0x0600780F RID: 30735 RVA: 0x002D2E60 File Offset: 0x002D1060
	public override void OnOpen()
	{
		base.OnOpen();
		this.PrefabItems.gameObject.SetActive(false);
	}

	// Token: 0x06007810 RID: 30736 RVA: 0x002D2E7C File Offset: 0x002D107C
	public void AddIconNotification(string iconNotifier, int count = 1, bool _bAddOnlyIfNotExisting = false)
	{
		if (iconNotifier == string.Empty)
		{
			return;
		}
		if (this.items == null)
		{
			return;
		}
		if (count == 0)
		{
			return;
		}
		if (_bAddOnlyIfNotExisting && this.items.Count > 0 && this.items[this.items.Count - 1].uiAtlasIcon.EqualsCaseInsensitive(iconNotifier))
		{
			return;
		}
		Transform transform;
		UILabel uilabel;
		for (int i = this.items.Count - 1; i >= 0; i--)
		{
			if (this.items[i].uiAtlasIcon != string.Empty && this.items[i].uiAtlasIcon.EqualsCaseInsensitive(iconNotifier))
			{
				this.items[i].count += count;
				transform = this.items[i].Item.transform;
				if (transform != null)
				{
					uilabel = transform.GetComponentInChildren<UILabel>();
					if (uilabel != null)
					{
						uilabel.text = ((this.items[i].count > 1) ? ("+" + this.items[i].count.ToString()) : "");
					}
				}
				this.items[i].TimeAdded = Time.time;
				return;
			}
		}
		GameObject gameObject = base.ViewComponent.UiTransform.gameObject.AddChild(this.PrefabItems.gameObject);
		if (gameObject == null)
		{
			return;
		}
		gameObject.SetActive(true);
		transform = gameObject.transform.Find("Negative");
		if (transform != null)
		{
			transform.gameObject.SetActive(false);
		}
		uilabel = gameObject.transform.GetComponentInChildren<UILabel>();
		if (uilabel == null)
		{
			uilabel = gameObject.transform.GetComponent<UILabel>();
		}
		if (uilabel != null)
		{
			uilabel.text = ((count > 0) ? ("+" + count.ToString()) : (count.ToString() ?? ""));
		}
		UISprite component = gameObject.transform.Find("Icon").GetComponent<UISprite>();
		if (component != null)
		{
			component.atlas = this.xui.GetAtlasByName("UIAtlas", iconNotifier);
			component.spriteName = iconNotifier;
			component.color = Color.white;
		}
		gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, (float)this.items.Count * this.height + (float)this.yOffset, gameObject.transform.localPosition.z);
		XUiC_CollectedItemList.Data data = new XUiC_CollectedItemList.Data();
		data.Item = gameObject;
		data.TimeAdded = Time.time;
		data.ItemStack = null;
		data.count = count;
		data.uiAtlasIcon = iconNotifier;
		this.items.Add(data);
		if (this.items.Count > 12)
		{
			this.removeLastEntry(0);
		}
	}

	// Token: 0x06007811 RID: 30737 RVA: 0x002D317C File Offset: 0x002D137C
	public void AddCraftingSkillNotification(ProgressionValue craftingSkill, bool _bAddOnlyIfNotExisting = false)
	{
		if (craftingSkill == null)
		{
			return;
		}
		if (this.items == null)
		{
			return;
		}
		if (_bAddOnlyIfNotExisting && this.items.Count > 0 && this.items[this.items.Count - 1].CraftingSkill == craftingSkill)
		{
			return;
		}
		Transform transform;
		UILabel uilabel;
		for (int i = this.items.Count - 1; i >= 0; i--)
		{
			if (this.items[i].CraftingSkill != null && this.items[i].CraftingSkill == craftingSkill)
			{
				transform = this.items[i].Item.transform;
				if (transform != null)
				{
					uilabel = transform.GetComponentInChildren<UILabel>();
					if (uilabel != null)
					{
						uilabel.text = string.Format("{0}/{1}", this.items[i].CraftingSkill.Level, this.items[i].CraftingSkill.ProgressionClass.MaxLevel);
					}
				}
				this.items[i].TimeAdded = Time.time;
				return;
			}
		}
		GameObject gameObject = base.ViewComponent.UiTransform.gameObject.AddChild(this.PrefabItems.gameObject);
		if (gameObject == null)
		{
			return;
		}
		gameObject.SetActive(true);
		transform = gameObject.transform.Find("Negative");
		if (transform != null)
		{
			transform.gameObject.SetActive(false);
		}
		uilabel = gameObject.transform.GetComponentInChildren<UILabel>();
		if (uilabel == null)
		{
			uilabel = gameObject.transform.GetComponent<UILabel>();
		}
		if (uilabel != null)
		{
			uilabel.text = string.Format("{0}/{1}", craftingSkill.Level, craftingSkill.ProgressionClass.MaxLevel);
		}
		UISprite component = gameObject.transform.Find("Icon").GetComponent<UISprite>();
		if (component != null)
		{
			component.atlas = this.xui.GetAtlasByName("UIAtlas", craftingSkill.ProgressionClass.Icon);
			component.spriteName = craftingSkill.ProgressionClass.Icon;
			component.color = Color.white;
		}
		gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, (float)this.items.Count * this.height + (float)this.yOffset, gameObject.transform.localPosition.z);
		XUiC_CollectedItemList.Data data = new XUiC_CollectedItemList.Data();
		data.Item = gameObject;
		data.TimeAdded = Time.time;
		data.ItemStack = null;
		data.count = -1;
		data.CraftingSkill = craftingSkill;
		data.uiAtlasIcon = craftingSkill.ProgressionClass.Icon;
		this.items.Add(data);
		if (this.items.Count > 12)
		{
			this.removeLastEntry(0);
		}
	}

	// Token: 0x06007812 RID: 30738 RVA: 0x002D346C File Offset: 0x002D166C
	public void AddItemStack(ItemStack _is, bool _bAddOnlyIfNotExisting = false)
	{
		if (_is == null)
		{
			return;
		}
		if (_is.itemValue == null)
		{
			return;
		}
		if (this.items == null)
		{
			return;
		}
		if (_is.itemValue.type == 0)
		{
			return;
		}
		if (_is.count == 0)
		{
			Manager.PlayInsidePlayerHead("missingitemtorepair", -1, 0f, false, false);
		}
		if (_bAddOnlyIfNotExisting && this.items.Count > 0 && this.items[this.items.Count - 1].ItemStack != null && this.items[this.items.Count - 1].ItemStack.itemValue.type == _is.itemValue.type && this.items[this.items.Count - 1].ItemStack.count == _is.count)
		{
			return;
		}
		bool flag = _is.count < 0;
		for (int i = this.items.Count - 1; i >= 0; i--)
		{
			if (this.items[i].ItemStack != null && this.items[i].ItemStack.itemValue.type == _is.itemValue.type)
			{
				if (this.items[i].isNegative == flag)
				{
					if (_is.count != 0 && !this.items[i].isMissingNotifier)
					{
						this.items[i].ItemStack.count += _is.count;
						Transform transform = this.items[i].Item.transform;
						if (transform != null)
						{
							UILabel componentInChildren = transform.GetComponentInChildren<UILabel>();
							if (componentInChildren != null)
							{
								componentInChildren.text = string.Format("{0} ({1})", (this.items[i].ItemStack.count >= 1) ? ("+" + this.items[i].ItemStack.count.ToString()) : this.items[i].ItemStack.count.ToString(), this.xui.PlayerInventory.GetItemCountWithMods(this.items[i].ItemStack.itemValue));
							}
						}
						this.items[i].TimeAdded = Time.time;
						return;
					}
					if (_is.count == 0 && this.items[i].isMissingNotifier)
					{
						this.items[i].TimeAdded = Time.time;
						return;
					}
				}
				else
				{
					this.items[i].TimeAdded = 0f;
				}
			}
		}
		GameObject gameObject = base.ViewComponent.UiTransform.gameObject.AddChild(this.PrefabItems.gameObject);
		if (gameObject == null)
		{
			return;
		}
		gameObject.SetActive(true);
		if (_is.count == 0)
		{
			Transform transform2 = gameObject.transform.Find("Negative");
			if (transform2 != null)
			{
				transform2.gameObject.SetActive(true);
			}
			UILabel uilabel = gameObject.transform.GetComponentInChildren<UILabel>();
			if (uilabel == null)
			{
				uilabel = gameObject.transform.GetComponent<UILabel>();
			}
			if (uilabel != null)
			{
				uilabel.text = "";
			}
		}
		else
		{
			Transform transform3 = gameObject.transform.Find("Negative");
			if (transform3 != null)
			{
				transform3.gameObject.SetActive(false);
			}
			UILabel uilabel2 = gameObject.transform.GetComponentInChildren<UILabel>();
			if (uilabel2 == null)
			{
				uilabel2 = gameObject.transform.GetComponent<UILabel>();
			}
			if (uilabel2 != null)
			{
				uilabel2.text = string.Format("{0} ({1})", (_is.count >= 1) ? ("+" + _is.count.ToString()) : _is.count.ToString(), this.xui.PlayerInventory.GetItemCountWithMods(_is.itemValue));
			}
		}
		UISprite component = gameObject.transform.Find("Icon").GetComponent<UISprite>();
		ItemClass itemClass = _is.itemValue.ItemClass;
		if (component != null && itemClass != null)
		{
			string text = _is.itemValue.GetPropertyOverride("CustomIcon", itemClass.GetIconName());
			text = itemClass.GetIconName();
			component.atlas = this.xui.GetAtlasByName("itemIconAtlas", text);
			component.spriteName = text;
			component.color = itemClass.GetIconTint(_is.itemValue);
		}
		gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, (float)this.items.Count * this.height + (float)this.yOffset, gameObject.transform.localPosition.z);
		XUiC_CollectedItemList.Data data = new XUiC_CollectedItemList.Data();
		data.Item = gameObject;
		data.TimeAdded = Time.time;
		data.ItemStack = _is;
		data.uiAtlasIcon = string.Empty;
		data.isMissingNotifier = _is.count == 0;
		data.isNegative = flag;
		this.items.Add(data);
		if (this.items.Count > 12)
		{
			this.removeLastEntry(0);
		}
	}

	// Token: 0x06007813 RID: 30739 RVA: 0x002D39D8 File Offset: 0x002D1BD8
	public void RemoveItemStack(ItemStack _is)
	{
		if (_is == null || _is.IsEmpty())
		{
			return;
		}
		_is = _is.Clone();
		if (_is.count > 0)
		{
			_is.count *= -1;
		}
		for (int i = this.items.Count - 1; i >= 0; i--)
		{
			if (this.items[i].ItemStack != null && this.items[i].ItemStack.itemValue.type == _is.itemValue.type)
			{
				if (this.items[i].isNegative)
				{
					if (_is.count != 0 && !this.items[i].isMissingNotifier)
					{
						this.items[i].ItemStack.count += _is.count;
						Transform transform = this.items[i].Item.transform;
						if (transform != null)
						{
							UILabel componentInChildren = transform.GetComponentInChildren<UILabel>();
							if (componentInChildren != null)
							{
								componentInChildren.text = string.Format("{0} ({1})", (this.items[i].ItemStack.count >= 1) ? ("+" + this.items[i].ItemStack.count.ToString()) : this.items[i].ItemStack.count.ToString(), this.xui.PlayerInventory.GetItemCountWithMods(this.items[i].ItemStack.itemValue));
							}
						}
						this.items[i].TimeAdded = Time.time;
						return;
					}
					if (_is.count == 0 && this.items[i].isMissingNotifier)
					{
						this.items[i].TimeAdded = Time.time;
						return;
					}
				}
				else
				{
					this.items[i].TimeAdded = 0f;
				}
			}
		}
		GameObject gameObject = base.ViewComponent.UiTransform.gameObject.AddChild(this.PrefabItems.gameObject);
		gameObject.SetActive(true);
		UILabel uilabel = gameObject.transform.GetComponentInChildren<UILabel>();
		if (uilabel == null)
		{
			uilabel = gameObject.transform.GetComponent<UILabel>();
		}
		uilabel.text = string.Format("{0} ({1})", (_is.count >= 0) ? ("-" + _is.count.ToString()) : _is.count.ToString(), this.xui.PlayerInventory.GetItemCountWithMods(_is.itemValue));
		gameObject.transform.Find("Negative").gameObject.SetActive(false);
		UISprite component = gameObject.transform.Find("Icon").GetComponent<UISprite>();
		ItemClass itemClassOrMissing = _is.itemValue.ItemClassOrMissing;
		string propertyOverride = _is.itemValue.GetPropertyOverride("CustomIcon", (itemClassOrMissing.CustomIcon == null) ? itemClassOrMissing.GetIconName() : itemClassOrMissing.CustomIcon.Value);
		component.atlas = this.xui.GetAtlasByName(((global::UnityEngine.Object)component.atlas).name, propertyOverride);
		component.spriteName = propertyOverride;
		component.color = itemClassOrMissing.GetIconTint(_is.itemValue);
		gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, (float)this.items.Count * this.height + (float)this.yOffset, gameObject.transform.localPosition.z);
		XUiC_CollectedItemList.Data data = new XUiC_CollectedItemList.Data();
		data.Item = gameObject;
		data.ItemStack = _is;
		data.TimeAdded = Time.time;
		data.uiAtlasIcon = string.Empty;
		data.isNegative = true;
		this.items.Add(data);
		if (this.items.Count > 12)
		{
			this.removeLastEntry(0);
		}
	}

	// Token: 0x06007814 RID: 30740 RVA: 0x002D3E00 File Offset: 0x002D2000
	[PublicizedFrom(EAccessModifier.Private)]
	public void removeLastEntry(int index = 0)
	{
		GameObject item = this.items[index].Item;
		if (item)
		{
			item.GetOrAddComponent<TemporaryObject>().enabled = true;
			TweenColor orAddComponent = item.GetOrAddComponent<TweenColor>();
			orAddComponent.from = Color.white;
			orAddComponent.to = new Color(1f, 1f, 1f, 0f);
			orAddComponent.enabled = true;
			orAddComponent.duration = 0.4f;
			TweenPosition tweenPosition = item.GetComponent<TweenPosition>();
			if (tweenPosition)
			{
				global::UnityEngine.Object.Destroy(tweenPosition);
			}
			tweenPosition = item.AddComponent<TweenPosition>();
			tweenPosition.from = item.transform.localPosition;
			tweenPosition.to = new Vector3(tweenPosition.from.x + 300f, tweenPosition.from.y, tweenPosition.from.z);
			tweenPosition.enabled = true;
			tweenPosition.duration = 0.4f;
		}
		this.items.RemoveAt(index);
		this.updateEntries();
	}

	// Token: 0x06007815 RID: 30741 RVA: 0x002D3EFC File Offset: 0x002D20FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateEntries()
	{
		float time = Time.time;
		for (int i = 0; i < this.items.Count; i++)
		{
			XUiC_CollectedItemList.Data data = this.items[i];
			if (time - data.TimeAdded <= 5f)
			{
				GameObject item = data.Item;
				TweenPosition tweenPosition = item.GetComponent<TweenPosition>();
				if (tweenPosition)
				{
					global::UnityEngine.Object.Destroy(tweenPosition);
				}
				tweenPosition = item.AddComponent<TweenPosition>();
				tweenPosition.from = item.transform.localPosition;
				Vector3 from = tweenPosition.from;
				from.y = (float)i * this.height + (float)this.yOffset;
				tweenPosition.to = from;
				tweenPosition.enabled = true;
			}
		}
	}

	// Token: 0x06007816 RID: 30742 RVA: 0x002D3FB2 File Offset: 0x002D21B2
	public void SetYOffset(int _yOffset)
	{
		if (_yOffset != this.yOffset)
		{
			this.yOffset = _yOffset;
			this.updateEntries();
		}
	}

	// Token: 0x040058C4 RID: 22724
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer localPlayer;

	// Token: 0x040058C5 RID: 22725
	[PublicizedFrom(EAccessModifier.Private)]
	public float height;

	// Token: 0x040058C6 RID: 22726
	[PublicizedFrom(EAccessModifier.Private)]
	public int yOffset;

	// Token: 0x040058C7 RID: 22727
	public Transform PrefabItems;

	// Token: 0x040058C8 RID: 22728
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cItemMax = 12;

	// Token: 0x040058C9 RID: 22729
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_CollectedItemList.Data> items = new List<XUiC_CollectedItemList.Data>();

	// Token: 0x040058CA RID: 22730
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ItemStack> removeItemQueue = new List<ItemStack>();

	// Token: 0x02000EF5 RID: 3829
	[PublicizedFrom(EAccessModifier.Private)]
	public class Data
	{
		// Token: 0x040058CB RID: 22731
		public GameObject Item;

		// Token: 0x040058CC RID: 22732
		public float TimeAdded;

		// Token: 0x040058CD RID: 22733
		public ItemStack ItemStack;

		// Token: 0x040058CE RID: 22734
		public ProgressionValue CraftingSkill;

		// Token: 0x040058CF RID: 22735
		public string uiAtlasIcon;

		// Token: 0x040058D0 RID: 22736
		public int count;

		// Token: 0x040058D1 RID: 22737
		public bool isNegative;

		// Token: 0x040058D2 RID: 22738
		public bool isMissingNotifier;
	}
}
