using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Audio;
using InControl;
using Unity.Profiling;
using UnityEngine;

// Token: 0x02001176 RID: 4470
public abstract class XUiView : IXUiElement
{
	// Token: 0x17001077 RID: 4215
	// (get) Token: 0x06008CDE RID: 36062 RVA: 0x0035A08F File Offset: 0x0035828F
	// (set) Token: 0x06008CDF RID: 36063 RVA: 0x0035A097 File Offset: 0x00358297
	public XUiController Controller
	{
		get
		{
			return this.controller;
		}
		set
		{
			this.controller = value;
			if (this.controller.ViewComponent != this)
			{
				this.controller.ViewComponent = this;
			}
		}
	}

	// Token: 0x17001078 RID: 4216
	// (get) Token: 0x06008CE0 RID: 36064
	public abstract UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x17001079 RID: 4217
	// (get) Token: 0x06008CE1 RID: 36065 RVA: 0x0035A0BA File Offset: 0x003582BA
	public Transform UiTransform
	{
		get
		{
			return this.uiTransform;
		}
	}

	// Token: 0x1700107A RID: 4218
	// (get) Token: 0x06008CE2 RID: 36066 RVA: 0x0035A0C2 File Offset: 0x003582C2
	public bool ColliderEnabled
	{
		get
		{
			return !this.destroyed && this.collider.enabled;
		}
	}

	// Token: 0x1700107B RID: 4219
	// (get) Token: 0x06008CE3 RID: 36067 RVA: 0x0035A0DC File Offset: 0x003582DC
	public Vector3 ColliderCenter
	{
		get
		{
			if (!this.destroyed)
			{
				return this.collider.bounds.center;
			}
			return default(Vector3);
		}
	}

	// Token: 0x1700107C RID: 4220
	// (get) Token: 0x06008CE4 RID: 36068 RVA: 0x0035A110 File Offset: 0x00358310
	public float ColliderHeightExtent
	{
		get
		{
			if (!this.destroyed)
			{
				return this.collider.bounds.size.y / 2f;
			}
			return 0f;
		}
	}

	// Token: 0x1700107D RID: 4221
	// (get) Token: 0x06008CE5 RID: 36069 RVA: 0x0035A14C File Offset: 0x0035834C
	public float ColliderWidthExtent
	{
		get
		{
			if (!this.destroyed)
			{
				return this.collider.bounds.size.x / 2f;
			}
			return 0f;
		}
	}

	// Token: 0x1700107E RID: 4222
	// (get) Token: 0x06008CE6 RID: 36070 RVA: 0x0035A188 File Offset: 0x00358388
	public Bounds ColliderBounds
	{
		get
		{
			if (!this.destroyed)
			{
				return this.collider.bounds;
			}
			return default(Bounds);
		}
	}

	// Token: 0x1700107F RID: 4223
	// (get) Token: 0x06008CE7 RID: 36071 RVA: 0x0035A1B4 File Offset: 0x003583B4
	public virtual Vector3 LocalCenter
	{
		get
		{
			return (this.position + this.size / 2).AsVector2();
		}
	}

	// Token: 0x17001080 RID: 4224
	// (get) Token: 0x06008CE8 RID: 36072
	public abstract Vector3[] WorldCorners { get; }

	// Token: 0x17001081 RID: 4225
	// (get) Token: 0x06008CE9 RID: 36073 RVA: 0x0035A1E5 File Offset: 0x003583E5
	// (set) Token: 0x06008CEA RID: 36074 RVA: 0x0035A1ED File Offset: 0x003583ED
	[XuiXmlAttribute("repeat_content", false)]
	public bool RepeatContent { get; set; }

	// Token: 0x17001082 RID: 4226
	// (get) Token: 0x06008CEB RID: 36075 RVA: 0x0035A1F6 File Offset: 0x003583F6
	// (set) Token: 0x06008CEC RID: 36076 RVA: 0x0035A1FE File Offset: 0x003583FE
	[XuiXmlAttribute("repeat_count", false)]
	public virtual int RepeatCount { get; set; }

	// Token: 0x17001083 RID: 4227
	// (get) Token: 0x06008CED RID: 36077 RVA: 0x0035A207 File Offset: 0x00358407
	public bool UiTransformIsHovered
	{
		get
		{
			return UICamera.hoveredObject == this.uiTransform.gameObject;
		}
	}

	// Token: 0x17001084 RID: 4228
	// (get) Token: 0x06008CEE RID: 36078 RVA: 0x0035A21E File Offset: 0x0035841E
	public bool IsActiveInHierarchy
	{
		get
		{
			if (!(this.uiTransform != null))
			{
				return this.isVisible;
			}
			return this.uiTransform.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x17001085 RID: 4229
	// (get) Token: 0x06008CEF RID: 36079 RVA: 0x0035A245 File Offset: 0x00358445
	public string ID
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x06008CF0 RID: 36080 RVA: 0x0035A24D File Offset: 0x0035844D
	[XuiXmlAttribute("name", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeName(string _value)
	{
		this.id = _value;
	}

	// Token: 0x17001086 RID: 4230
	// (get) Token: 0x06008CF1 RID: 36081 RVA: 0x0035A256 File Offset: 0x00358456
	// (set) Token: 0x06008CF2 RID: 36082 RVA: 0x0035A260 File Offset: 0x00358460
	[XuiXmlAttribute("visible", false)]
	public bool IsVisible
	{
		get
		{
			return this.isVisible;
		}
		set
		{
			if (this.isVisible == value)
			{
				return;
			}
			this.isVisible = value;
			if (this.uiTransform != null && this.isVisible != this.uiTransform.gameObject.activeSelf)
			{
				this.uiTransform.gameObject.SetActive(this.isVisible);
			}
			this.Controller.OnVisibilityChanged(this.IsActiveInHierarchy);
			if (this.xui.playerUI.CursorController.navigationTarget == this)
			{
				this.xui.playerUI.RefreshNavigationTarget();
			}
			this.SetDirty();
		}
	}

	// Token: 0x17001087 RID: 4231
	// (get) Token: 0x06008CF3 RID: 36083 RVA: 0x0035A2F9 File Offset: 0x003584F9
	// (set) Token: 0x06008CF4 RID: 36084 RVA: 0x0035A304 File Offset: 0x00358504
	[XuiXmlAttribute("enabled", false)]
	public virtual bool Enabled
	{
		get
		{
			return this.enabled;
		}
		set
		{
			if (value == this.enabled)
			{
				return;
			}
			this.enabled = value;
			if (!this.enabled && this.xui.playerUI.CursorController.navigationTarget == this)
			{
				this.xui.playerUI.RefreshNavigationTarget();
			}
			if (!value && this.isOver)
			{
				this.OnHover(true);
			}
			this.Controller.OnEnabledChanged(this.enabled);
			this.SetDirty();
		}
	}

	// Token: 0x17001088 RID: 4232
	// (get) Token: 0x06008CF5 RID: 36085 RVA: 0x0035A37B File Offset: 0x0035857B
	// (set) Token: 0x06008CF6 RID: 36086 RVA: 0x0035A383 File Offset: 0x00358583
	[XuiXmlAttribute("collider_scale", false)]
	public float ColliderScale
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	} = 1f;

	// Token: 0x17001089 RID: 4233
	// (get) Token: 0x06008CF7 RID: 36087 RVA: 0x0035A38C File Offset: 0x0035858C
	// (set) Token: 0x06008CF8 RID: 36088 RVA: 0x0035A394 File Offset: 0x00358594
	[XuiXmlAttribute("collider_padding", false)]
	public Vector2i ColliderPadding
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	} = Vector2i.zero;

	// Token: 0x1700108A RID: 4234
	// (get) Token: 0x06008CF9 RID: 36089 RVA: 0x0035A39D File Offset: 0x0035859D
	// (set) Token: 0x06008CFA RID: 36090 RVA: 0x0035A3A5 File Offset: 0x003585A5
	[XuiXmlAttribute("size", false)]
	public Vector2i Size
	{
		get
		{
			return this.size;
		}
		set
		{
			if (this.size == value)
			{
				return;
			}
			this.size = value;
			this.SetDirty();
		}
	}

	// Token: 0x1700108B RID: 4235
	// (get) Token: 0x06008CFB RID: 36091 RVA: 0x0035A3C3 File Offset: 0x003585C3
	// (set) Token: 0x06008CFC RID: 36092 RVA: 0x0035A3D0 File Offset: 0x003585D0
	[XuiXmlAttribute("width", false)]
	public int Width
	{
		get
		{
			return this.Size.x;
		}
		set
		{
			if (this.Width == value)
			{
				return;
			}
			Vector2i vector2i = this.Size;
			vector2i.x = value;
			this.Size = vector2i;
		}
	}

	// Token: 0x1700108C RID: 4236
	// (get) Token: 0x06008CFD RID: 36093 RVA: 0x0035A3FD File Offset: 0x003585FD
	// (set) Token: 0x06008CFE RID: 36094 RVA: 0x0035A40C File Offset: 0x0035860C
	[XuiXmlAttribute("height", false)]
	public int Height
	{
		get
		{
			return this.Size.y;
		}
		set
		{
			if (this.Height == value)
			{
				return;
			}
			Vector2i vector2i = this.Size;
			vector2i.y = value;
			this.Size = vector2i;
		}
	}

	// Token: 0x1700108D RID: 4237
	// (get) Token: 0x06008CFF RID: 36095 RVA: 0x0035A439 File Offset: 0x00358639
	// (set) Token: 0x06008D00 RID: 36096 RVA: 0x0035A441 File Offset: 0x00358641
	[XuiXmlAttribute("pos", false)]
	public Vector2i Position
	{
		get
		{
			return this.position;
		}
		set
		{
			if (this.position == value)
			{
				return;
			}
			this.position = value;
			this.SetDirty();
			this.positionDirty = true;
		}
	}

	// Token: 0x1700108E RID: 4238
	// (get) Token: 0x06008D01 RID: 36097 RVA: 0x0035A466 File Offset: 0x00358666
	// (set) Token: 0x06008D02 RID: 36098 RVA: 0x0035A46E File Offset: 0x0035866E
	[XuiXmlAttribute("ignoreparentpadding", false)]
	public bool IgnoreParentPadding
	{
		get
		{
			return this.ignoreParentPadding;
		}
		set
		{
			if (this.ignoreParentPadding == value)
			{
				return;
			}
			this.ignoreParentPadding = value;
			this.SetDirty();
		}
	}

	// Token: 0x1700108F RID: 4239
	// (get) Token: 0x06008D03 RID: 36099 RVA: 0x0035A488 File Offset: 0x00358688
	public Vector2i PaddedPosition
	{
		get
		{
			Vector2i vector2i = this.position;
			Vector2i vector2i3;
			if (!this.ignoreParentPadding)
			{
				XUiController parent = this.Controller.Parent;
				Vector2i? vector2i2;
				if (parent == null)
				{
					vector2i2 = null;
				}
				else
				{
					XUiView viewComponent = parent.ViewComponent;
					vector2i2 = ((viewComponent != null) ? new Vector2i?(viewComponent.InnerPosition) : null);
				}
				vector2i3 = vector2i2 ?? Vector2i.zero;
			}
			else
			{
				vector2i3 = Vector2i.zero;
			}
			return vector2i + vector2i3;
		}
	}

	// Token: 0x17001090 RID: 4240
	// (get) Token: 0x06008D04 RID: 36100 RVA: 0x0035A4FF File Offset: 0x003586FF
	public virtual Vector2i InnerSize
	{
		get
		{
			return new Vector2i(this.size.x - this.padding.SumLeftRight, this.size.y - this.padding.SumTopBottom);
		}
	}

	// Token: 0x17001091 RID: 4241
	// (get) Token: 0x06008D05 RID: 36101 RVA: 0x0035A534 File Offset: 0x00358734
	public Vector2i InnerPosition
	{
		get
		{
			return new Vector2i(this.padding.Left, -this.padding.Top);
		}
	}

	// Token: 0x17001092 RID: 4242
	// (get) Token: 0x06008D06 RID: 36102 RVA: 0x0035A552 File Offset: 0x00358752
	// (set) Token: 0x06008D07 RID: 36103 RVA: 0x0035A55A File Offset: 0x0035875A
	[XuiXmlAttribute("rotation", false)]
	public float Rotation
	{
		get
		{
			return this.rotation;
		}
		set
		{
			if (Mathf.Approximately(this.rotation, value))
			{
				return;
			}
			this.rotation = value;
			this.SetDirty();
			this.rotationDirty = true;
		}
	}

	// Token: 0x17001093 RID: 4243
	// (get) Token: 0x06008D08 RID: 36104 RVA: 0x0035A57F File Offset: 0x0035877F
	// (set) Token: 0x06008D09 RID: 36105 RVA: 0x0035A587 File Offset: 0x00358787
	public int Depth
	{
		get
		{
			return this.depth;
		}
		set
		{
			if (this.depth == value)
			{
				return;
			}
			this.depth = value;
			this.SetDirty();
		}
	}

	// Token: 0x06008D0A RID: 36106 RVA: 0x0035A5A0 File Offset: 0x003587A0
	[XuiXmlAttribute("depth", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeDepth(int _value)
	{
		XUiView viewComponent = this.controller.Parent.ViewComponent;
		this.Depth = _value + ((viewComponent != null) ? viewComponent.Depth : 0);
	}

	// Token: 0x06008D0B RID: 36107 RVA: 0x0035A5C6 File Offset: 0x003587C6
	[XuiXmlAttribute("padding_left", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributePaddingLeft(int _value)
	{
		this.padding = this.padding.SetLeft(_value);
	}

	// Token: 0x06008D0C RID: 36108 RVA: 0x0035A5DA File Offset: 0x003587DA
	[XuiXmlAttribute("padding_right", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributePaddingRight(int _value)
	{
		this.padding = this.padding.SetRight(_value);
	}

	// Token: 0x06008D0D RID: 36109 RVA: 0x0035A5EE File Offset: 0x003587EE
	[XuiXmlAttribute("padding_top", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributePaddingTop(int _value)
	{
		this.padding = this.padding.SetTop(_value);
	}

	// Token: 0x06008D0E RID: 36110 RVA: 0x0035A602 File Offset: 0x00358802
	[XuiXmlAttribute("padding_bottom", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributePaddingBottom(int _value)
	{
		this.padding = this.padding.SetBottom(_value);
	}

	// Token: 0x06008D0F RID: 36111 RVA: 0x0035A616 File Offset: 0x00358816
	[XuiXmlAttribute("padding", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributePadding(string _value)
	{
		XUiSideSizes.TryParse(_value, out this.padding, "padding");
	}

	// Token: 0x06008D10 RID: 36112 RVA: 0x0035A62A File Offset: 0x0035882A
	[XuiXmlAttribute("sound", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeSound(string _value)
	{
		this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _o)
		{
			this.xuiSound = _o;
		});
	}

	// Token: 0x06008D11 RID: 36113 RVA: 0x0035A644 File Offset: 0x00358844
	[XuiXmlAttribute("sound_play_on_hover", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeSoundOnHover(string _value)
	{
		this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _o)
		{
			this.xuiHoverSound = _o;
		});
	}

	// Token: 0x17001094 RID: 4244
	// (get) Token: 0x06008D12 RID: 36114 RVA: 0x0035A65E File Offset: 0x0035885E
	// (set) Token: 0x06008D13 RID: 36115 RVA: 0x0035A666 File Offset: 0x00358866
	[XuiXmlAttribute("sound_play_on_press", false)]
	public bool SoundPlayOnClick { get; set; }

	// Token: 0x17001095 RID: 4245
	// (get) Token: 0x06008D14 RID: 36116 RVA: 0x0035A66F File Offset: 0x0035886F
	// (set) Token: 0x06008D15 RID: 36117 RVA: 0x0035A677 File Offset: 0x00358877
	public bool SoundPlayOnHover { get; set; }

	// Token: 0x17001096 RID: 4246
	// (get) Token: 0x06008D16 RID: 36118 RVA: 0x0035A680 File Offset: 0x00358880
	// (set) Token: 0x06008D17 RID: 36119 RVA: 0x0035A688 File Offset: 0x00358888
	[XuiXmlAttribute("sound_play_on_open", false)]
	public bool SoundPlayOnOpen { get; set; }

	// Token: 0x17001097 RID: 4247
	// (get) Token: 0x06008D18 RID: 36120 RVA: 0x0035A691 File Offset: 0x00358891
	// (set) Token: 0x06008D19 RID: 36121 RVA: 0x0035A699 File Offset: 0x00358899
	[XuiXmlAttribute("sound_volume", false)]
	public float SoundVolume { get; set; }

	// Token: 0x17001098 RID: 4248
	// (get) Token: 0x06008D1A RID: 36122 RVA: 0x0035A6A2 File Offset: 0x003588A2
	public bool HasHoverSound
	{
		get
		{
			return this.xuiHoverSound != null;
		}
	}

	// Token: 0x17001099 RID: 4249
	// (get) Token: 0x06008D1B RID: 36123 RVA: 0x0035A6B0 File Offset: 0x003588B0
	// (set) Token: 0x06008D1C RID: 36124 RVA: 0x0035A6B8 File Offset: 0x003588B8
	[XuiXmlAttribute("hold_delay", false)]
	public float HoldDelay
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = 0.5f;

	// Token: 0x1700109A RID: 4250
	// (get) Token: 0x06008D1D RID: 36125 RVA: 0x0035A6C1 File Offset: 0x003588C1
	// (set) Token: 0x06008D1E RID: 36126 RVA: 0x0035A6C9 File Offset: 0x003588C9
	[XuiXmlAttribute("hold_timed_initial_interval", false)]
	public float HoldEventIntervalInitial
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = 0.5f;

	// Token: 0x1700109B RID: 4251
	// (get) Token: 0x06008D1F RID: 36127 RVA: 0x0035A6D2 File Offset: 0x003588D2
	// (set) Token: 0x06008D20 RID: 36128 RVA: 0x0035A6DA File Offset: 0x003588DA
	[XuiXmlAttribute("hold_timed_final_interval", false)]
	public float HoldEventIntervalFinal
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = 0.06f;

	// Token: 0x1700109C RID: 4252
	// (get) Token: 0x06008D21 RID: 36129 RVA: 0x0035A6E3 File Offset: 0x003588E3
	// (set) Token: 0x06008D22 RID: 36130 RVA: 0x0035A6EB File Offset: 0x003588EB
	[XuiXmlAttribute("hold_timed_step_acceleration", false)]
	public float HoldEventIntervalAcceleration
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = 0.015f;

	// Token: 0x1700109D RID: 4253
	// (get) Token: 0x06008D23 RID: 36131 RVA: 0x0035A6F4 File Offset: 0x003588F4
	// (set) Token: 0x06008D24 RID: 36132 RVA: 0x0035A6FC File Offset: 0x003588FC
	[XuiXmlAttribute("on_hover", false)]
	public bool EventOnHover
	{
		get
		{
			return this.eventOnHover;
		}
		set
		{
			if (this.eventOnHover == value)
			{
				return;
			}
			this.eventOnHover = value;
			this.SetDirty();
		}
	}

	// Token: 0x1700109E RID: 4254
	// (get) Token: 0x06008D25 RID: 36133 RVA: 0x0035A715 File Offset: 0x00358915
	// (set) Token: 0x06008D26 RID: 36134 RVA: 0x0035A71D File Offset: 0x0035891D
	[XuiXmlAttribute("on_press", false)]
	public bool EventOnPress
	{
		get
		{
			return this.eventOnPress;
		}
		set
		{
			if (this.eventOnPress == value)
			{
				return;
			}
			this.eventOnPress = value;
			this.SetDirty();
		}
	}

	// Token: 0x1700109F RID: 4255
	// (get) Token: 0x06008D27 RID: 36135 RVA: 0x0035A736 File Offset: 0x00358936
	// (set) Token: 0x06008D28 RID: 36136 RVA: 0x0035A73E File Offset: 0x0035893E
	[XuiXmlAttribute("on_doubleclick", false)]
	public bool EventOnDoubleClick
	{
		get
		{
			return this.eventOnDoubleClick;
		}
		set
		{
			if (this.eventOnDoubleClick == value)
			{
				return;
			}
			this.eventOnDoubleClick = value;
			this.SetDirty();
		}
	}

	// Token: 0x170010A0 RID: 4256
	// (get) Token: 0x06008D29 RID: 36137 RVA: 0x0035A757 File Offset: 0x00358957
	// (set) Token: 0x06008D2A RID: 36138 RVA: 0x0035A75F File Offset: 0x0035895F
	[XuiXmlAttribute("on_held", false)]
	public bool EventOnHeld
	{
		get
		{
			return this.eventOnHeld;
		}
		set
		{
			if (this.eventOnHeld == value)
			{
				return;
			}
			this.eventOnHeld = value;
			this.SetDirty();
		}
	}

	// Token: 0x170010A1 RID: 4257
	// (get) Token: 0x06008D2B RID: 36139 RVA: 0x0035A778 File Offset: 0x00358978
	// (set) Token: 0x06008D2C RID: 36140 RVA: 0x0035A780 File Offset: 0x00358980
	[XuiXmlAttribute("on_scroll", false)]
	public bool EventOnScroll
	{
		get
		{
			return this.eventOnScroll;
		}
		set
		{
			if (this.eventOnScroll == value)
			{
				return;
			}
			this.eventOnScroll = value;
			this.SetDirty();
		}
	}

	// Token: 0x170010A2 RID: 4258
	// (get) Token: 0x06008D2D RID: 36141 RVA: 0x0035A799 File Offset: 0x00358999
	// (set) Token: 0x06008D2E RID: 36142 RVA: 0x0035A7A1 File Offset: 0x003589A1
	[XuiXmlAttribute("on_drag", false)]
	public bool EventOnDrag
	{
		get
		{
			return this.eventOnDrag;
		}
		set
		{
			if (this.eventOnDrag == value)
			{
				return;
			}
			this.eventOnDrag = value;
			this.SetDirty();
		}
	}

	// Token: 0x170010A3 RID: 4259
	// (get) Token: 0x06008D2F RID: 36143 RVA: 0x0035A7BA File Offset: 0x003589BA
	// (set) Token: 0x06008D30 RID: 36144 RVA: 0x0035A7C2 File Offset: 0x003589C2
	[XuiXmlAttribute("on_select", false)]
	public bool EventOnSelect
	{
		get
		{
			return this.eventOnSelect;
		}
		set
		{
			if (this.eventOnSelect == value)
			{
				return;
			}
			this.eventOnSelect = value;
			this.SetDirty();
		}
	}

	// Token: 0x170010A4 RID: 4260
	// (get) Token: 0x06008D31 RID: 36145 RVA: 0x0035A7DC File Offset: 0x003589DC
	public virtual bool HasAnyEvent
	{
		get
		{
			return this.EventOnPress || this.EventOnDoubleClick || this.EventOnHover || this.EventOnHeld || this.EventOnDrag || this.EventOnScroll || this.EventOnSelect || !string.IsNullOrEmpty(this.ToolTip);
		}
	}

	// Token: 0x170010A5 RID: 4261
	// (get) Token: 0x06008D32 RID: 36146 RVA: 0x0035A831 File Offset: 0x00358A31
	public bool IsHovered
	{
		get
		{
			return this.isOver;
		}
	}

	// Token: 0x170010A6 RID: 4262
	// (get) Token: 0x06008D33 RID: 36147 RVA: 0x0035A839 File Offset: 0x00358A39
	// (set) Token: 0x06008D34 RID: 36148 RVA: 0x0035A85B File Offset: 0x00358A5B
	public bool IsNavigatable
	{
		get
		{
			return this.isNavigatable && this.Enabled && this.IsVisible && this.IsActiveInHierarchy;
		}
		set
		{
			this.isNavigatable = value;
		}
	}

	// Token: 0x06008D35 RID: 36149 RVA: 0x0035A864 File Offset: 0x00358A64
	[XuiXmlAttribute("gamepad_selectable", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeGamepadSelectable(bool _value)
	{
		this.IsNavigatable = _value;
		this.gamepadSelectableSetFromAttributes = true;
	}

	// Token: 0x170010A7 RID: 4263
	// (get) Token: 0x06008D36 RID: 36150 RVA: 0x0035A874 File Offset: 0x00358A74
	// (set) Token: 0x06008D37 RID: 36151 RVA: 0x0035A87C File Offset: 0x00358A7C
	public bool IsSnappable { get; set; } = true;

	// Token: 0x06008D38 RID: 36152 RVA: 0x0035A885 File Offset: 0x00358A85
	[XuiXmlAttribute("snap", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeGamepadSnappable(bool _value)
	{
		this.IsSnappable = _value;
		this.gamepadSnappableSetFromAttributes = true;
	}

	// Token: 0x170010A8 RID: 4264
	// (get) Token: 0x06008D39 RID: 36153 RVA: 0x0035A895 File Offset: 0x00358A95
	// (set) Token: 0x06008D3A RID: 36154 RVA: 0x0035A89D File Offset: 0x00358A9D
	[XuiXmlAttribute("use_selection_box", false)]
	public bool UseSelectionBox { get; set; } = true;

	// Token: 0x170010A9 RID: 4265
	// (get) Token: 0x06008D3B RID: 36155 RVA: 0x0035A8A6 File Offset: 0x00358AA6
	// (set) Token: 0x06008D3C RID: 36156 RVA: 0x0035A8AE File Offset: 0x00358AAE
	[XuiXmlAttribute("nav_up", false)]
	public string NavUpTargetString
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.navUpTargetString;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.navUpTargetString == value)
			{
				return;
			}
			this.navUpTargetString = value;
			this.SetDirty();
		}
	}

	// Token: 0x170010AA RID: 4266
	// (get) Token: 0x06008D3D RID: 36157 RVA: 0x0035A8CC File Offset: 0x00358ACC
	// (set) Token: 0x06008D3E RID: 36158 RVA: 0x0035A8D4 File Offset: 0x00358AD4
	public XUiView NavUpTarget
	{
		get
		{
			return this.navUpTarget;
		}
		set
		{
			this.navUpTarget = value;
			foreach (XUiController xuiController in this.Controller.Children)
			{
				xuiController.ViewComponent.NavUpTarget = value;
			}
		}
	}

	// Token: 0x170010AB RID: 4267
	// (get) Token: 0x06008D3F RID: 36159 RVA: 0x0035A938 File Offset: 0x00358B38
	// (set) Token: 0x06008D40 RID: 36160 RVA: 0x0035A940 File Offset: 0x00358B40
	[XuiXmlAttribute("nav_down", false)]
	public string NavDownTargetString
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.navDownTargetString;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.navDownTargetString == value)
			{
				return;
			}
			this.navDownTargetString = value;
			this.SetDirty();
		}
	}

	// Token: 0x170010AC RID: 4268
	// (get) Token: 0x06008D41 RID: 36161 RVA: 0x0035A95E File Offset: 0x00358B5E
	// (set) Token: 0x06008D42 RID: 36162 RVA: 0x0035A968 File Offset: 0x00358B68
	public XUiView NavDownTarget
	{
		get
		{
			return this.navDownTarget;
		}
		set
		{
			this.navDownTarget = value;
			foreach (XUiController xuiController in this.Controller.Children)
			{
				xuiController.ViewComponent.navDownTarget = value;
			}
		}
	}

	// Token: 0x170010AD RID: 4269
	// (get) Token: 0x06008D43 RID: 36163 RVA: 0x0035A9CC File Offset: 0x00358BCC
	// (set) Token: 0x06008D44 RID: 36164 RVA: 0x0035A9D4 File Offset: 0x00358BD4
	[XuiXmlAttribute("nav_left", false)]
	public string NavLeftTargetString
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.navLeftTargetString;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.navLeftTargetString == value)
			{
				return;
			}
			this.navLeftTargetString = value;
			this.SetDirty();
		}
	}

	// Token: 0x170010AE RID: 4270
	// (get) Token: 0x06008D45 RID: 36165 RVA: 0x0035A9F2 File Offset: 0x00358BF2
	// (set) Token: 0x06008D46 RID: 36166 RVA: 0x0035A9FC File Offset: 0x00358BFC
	public XUiView NavLeftTarget
	{
		get
		{
			return this.navLeftTarget;
		}
		set
		{
			this.navLeftTarget = value;
			foreach (XUiController xuiController in this.Controller.Children)
			{
				xuiController.ViewComponent.navLeftTarget = value;
			}
		}
	}

	// Token: 0x170010AF RID: 4271
	// (get) Token: 0x06008D47 RID: 36167 RVA: 0x0035AA60 File Offset: 0x00358C60
	// (set) Token: 0x06008D48 RID: 36168 RVA: 0x0035AA68 File Offset: 0x00358C68
	[XuiXmlAttribute("nav_right", false)]
	public string NavRightTargetString
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.navRightTargetString;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.navRightTargetString == value)
			{
				return;
			}
			this.navRightTargetString = value;
			this.SetDirty();
		}
	}

	// Token: 0x170010B0 RID: 4272
	// (get) Token: 0x06008D49 RID: 36169 RVA: 0x0035AA86 File Offset: 0x00358C86
	// (set) Token: 0x06008D4A RID: 36170 RVA: 0x0035AA90 File Offset: 0x00358C90
	public XUiView NavRightTarget
	{
		get
		{
			return this.navRightTarget;
		}
		set
		{
			this.navRightTarget = value;
			foreach (XUiController xuiController in this.Controller.Children)
			{
				xuiController.ViewComponent.navRightTarget = value;
			}
		}
	}

	// Token: 0x170010B1 RID: 4273
	// (get) Token: 0x06008D4B RID: 36171 RVA: 0x0035AAF4 File Offset: 0x00358CF4
	// (set) Token: 0x06008D4C RID: 36172 RVA: 0x0035AAFC File Offset: 0x00358CFC
	[XuiXmlAttribute("tooltip", false)]
	public string ToolTip
	{
		get
		{
			return this.toolTip;
		}
		set
		{
			if (this.toolTip == value)
			{
				return;
			}
			if (GameManager.Instance.GameIsFocused && this.enabled && this.isOver && this.xui.ToolTipWindow != null && this.xui.ToolTipWindow.ToolTip == this.toolTip)
			{
				this.xui.ToolTipWindow.ToolTip = value;
			}
			this.toolTip = value;
		}
	}

	// Token: 0x170010B2 RID: 4274
	// (get) Token: 0x06008D4D RID: 36173 RVA: 0x0035AB76 File Offset: 0x00358D76
	// (set) Token: 0x06008D4E RID: 36174 RVA: 0x0035AB80 File Offset: 0x00358D80
	[XuiXmlAttribute("disabled_tooltip", false)]
	public string DisabledToolTip
	{
		get
		{
			return this.disabledToolTip;
		}
		set
		{
			if (this.disabledToolTip == value)
			{
				return;
			}
			if (GameManager.Instance.GameIsFocused && this.enabled && this.isOver && this.xui.ToolTipWindow != null && this.xui.ToolTipWindow.ToolTip == this.disabledToolTip)
			{
				this.xui.ToolTipWindow.ToolTip = value;
			}
			this.disabledToolTip = value;
		}
	}

	// Token: 0x06008D4F RID: 36175 RVA: 0x0035ABFA File Offset: 0x00358DFA
	[XuiXmlAttribute("tooltip_key", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeTooltipKey(string _value)
	{
		this.ToolTip = Localization.Get(_value, false, null);
	}

	// Token: 0x06008D50 RID: 36176 RVA: 0x0035AC0A File Offset: 0x00358E0A
	[XuiXmlAttribute("disabled_tooltip_key", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeDisabledTooltipKey(string _value)
	{
		this.DisabledToolTip = Localization.Get(_value, false, null);
	}

	// Token: 0x06008D51 RID: 36177 RVA: 0x0035AC1C File Offset: 0x00358E1C
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiView(XUi _xui, string _id)
	{
		this.xui = _xui;
		this.id = _id;
	}

	// Token: 0x06008D52 RID: 36178 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void createComponents(GameObject _go)
	{
	}

	// Token: 0x06008D53 RID: 36179 RVA: 0x0035ACE0 File Offset: 0x00358EE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void buildView()
	{
		Type type = base.GetType();
		GameObject gameObject;
		if (XUiView.componentTemplates.TryGetValue(type, out gameObject))
		{
			this.uiTransform = global::UnityEngine.Object.Instantiate<GameObject>(gameObject).transform;
			return;
		}
		if (XUiView.templatesParent == null)
		{
			Transform transform = this.xui.playerUI.uiCamera.transform;
			GameObject gameObject2 = new GameObject("_ViewTemplates");
			gameObject2.SetActive(false);
			XUiView.templatesParent = gameObject2.transform;
			XUiView.templatesParent.parent = transform;
		}
		GameObject gameObject3 = new GameObject(type.Name)
		{
			layer = 12,
			transform = 
			{
				parent = XUiView.templatesParent
			}
		};
		gameObject3.AddComponent<BoxCollider>().enabled = false;
		UIEventListener.Get(gameObject3);
		this.createComponents(gameObject3);
		XUiView.componentTemplates[type] = gameObject3;
		this.uiTransform = global::UnityEngine.Object.Instantiate<GameObject>(gameObject3).transform;
	}

	// Token: 0x06008D54 RID: 36180 RVA: 0x0035ADB9 File Offset: 0x00358FB9
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void captureComponents()
	{
		this.collider = this.uiTransform.gameObject.GetComponent<BoxCollider>();
	}

	// Token: 0x06008D55 RID: 36181 RVA: 0x0035ADD4 File Offset: 0x00358FD4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void addTweeners()
	{
		for (int i = 0; i < this.Tweeners.Count; i++)
		{
			this.Tweeners[i].CreateTween(this.uiTransform.gameObject);
		}
	}

	// Token: 0x06008D56 RID: 36182 RVA: 0x0035AE14 File Offset: 0x00359014
	public virtual void InitView()
	{
		this.buildView();
		this.uiTransform.name = this.id;
		this.captureComponents();
		this.addTweeners();
		XUiController parent = this.controller.Parent;
		if (((parent != null) ? parent.ViewComponent : null) != null)
		{
			XUiView viewComponent = this.controller.Parent.ViewComponent;
			this.uiTransform.parent = viewComponent.uiTransform;
			this.uiTransform.localScale = Vector3.one;
			this.uiTransform.localPosition = new Vector3((float)this.PaddedPosition.x, (float)this.PaddedPosition.y, 0f);
			this.uiTransform.localEulerAngles = new Vector3(0f, 0f, this.rotation);
		}
		if (this.HasAnyEvent)
		{
			this.collider.enabled = true;
			this.refreshBoxCollider();
			UIEventListener uieventListener = UIEventListener.Get(this.uiTransform.gameObject);
			uieventListener.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onHover, new UIEventListener.BoolDelegate(delegate(GameObject _, bool _state)
			{
				this.OnHover(_state);
			}));
			uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.OnClick));
			uieventListener.onDoubleClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onDoubleClick, new UIEventListener.VoidDelegate(this.OnDoubleClick));
			uieventListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener.onDrag, new UIEventListener.VectorDelegate(this.OnDrag));
			uieventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onPress, new UIEventListener.BoolDelegate(this.OnPress));
			uieventListener.onScroll = (UIEventListener.FloatDelegate)Delegate.Combine(uieventListener.onScroll, new UIEventListener.FloatDelegate(this.OnScroll));
			uieventListener.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onSelect, new UIEventListener.BoolDelegate(this.OnSelect));
			uieventListener.onDragOut = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onDragOut, new UIEventListener.VoidDelegate(this.OnHeldDragOut));
		}
		if (this.uiTransform.gameObject.activeSelf != this.isVisible)
		{
			this.uiTransform.gameObject.SetActive(this.isVisible);
		}
		if (!this.gamepadSelectableSetFromAttributes)
		{
			this.IsNavigatable = this.EventOnPress;
		}
		if (!this.gamepadSnappableSetFromAttributes)
		{
			this.IsSnappable = this.EventOnPress;
		}
	}

	// Token: 0x06008D57 RID: 36183 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetRepeatContentTemplateParams(Dictionary<string, object> _templateParams, int _curRepeatNum)
	{
	}

	// Token: 0x06008D58 RID: 36184 RVA: 0x0035B06E File Offset: 0x0035926E
	public virtual void Cleanup()
	{
		this.destroyed = true;
	}

	// Token: 0x06008D59 RID: 36185 RVA: 0x0035B078 File Offset: 0x00359278
	public virtual void Update(float _dt)
	{
		if (this.isOver && !this.UiTransformIsHovered)
		{
			using (this.pmUpdateHover.Auto())
			{
				this.OnHover(false);
			}
		}
		if (this.isPressed && this.enabled)
		{
			using (this.pmUpdatePress.Auto())
			{
				float unscaledTime = Time.unscaledTime;
				if (!this.isHold)
				{
					this.isHold = unscaledTime - this.pressStartTime >= this.HoldDelay;
					if (this.isHold)
					{
						this.holdStartTime = unscaledTime;
						this.controller.Held(EHoldType.HoldStart, 0f, -1f);
						this.holdEventNextTime = 0f;
						this.holdEventLastTime = unscaledTime;
						this.holdEventIntervalChangeSpeed = 0f;
						this.holdEventIntervalCurrent = this.HoldEventIntervalInitial;
					}
				}
				else
				{
					this.controller.Held(EHoldType.Hold, unscaledTime - this.holdStartTime, -1f);
				}
				if (this.isHold && unscaledTime >= this.holdEventNextTime)
				{
					this.holdEventIntervalCurrent = Mathf.SmoothDamp(this.holdEventIntervalCurrent, this.HoldEventIntervalFinal, ref this.holdEventIntervalChangeSpeed, this.HoldEventIntervalAcceleration, float.PositiveInfinity, _dt);
					this.holdEventNextTime = unscaledTime + this.holdEventIntervalCurrent;
					this.controller.Held(EHoldType.HoldTimed, unscaledTime - this.holdStartTime, unscaledTime - this.holdEventLastTime);
					this.holdEventLastTime = unscaledTime;
				}
			}
		}
		if (this.isDirty)
		{
			using (this.pmUpdateData.Auto())
			{
				this.updateData();
				this.isDirty = false;
			}
		}
	}

	// Token: 0x06008D5A RID: 36186 RVA: 0x0035B240 File Offset: 0x00359440
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void updateData()
	{
		if (this.positionDirty)
		{
			this.uiTransform.localPosition = new Vector3((float)this.PaddedPosition.x, (float)this.PaddedPosition.y, this.uiTransform.localPosition.z);
			this.positionDirty = false;
		}
		if (this.rotationDirty)
		{
			this.uiTransform.localEulerAngles = new Vector3(0f, 0f, this.rotation);
			this.rotationDirty = false;
		}
		this.parseAnchors();
		this.parseNavigationTargets();
	}

	// Token: 0x06008D5B RID: 36187 RVA: 0x0035B2D0 File Offset: 0x003594D0
	public void TryUpdatePosition()
	{
		if (this.positionDirty)
		{
			this.uiTransform.localPosition = new Vector3((float)this.PaddedPosition.x, (float)this.PaddedPosition.y, this.uiTransform.localPosition.z);
		}
	}

	// Token: 0x06008D5C RID: 36188 RVA: 0x0035B320 File Offset: 0x00359520
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void refreshBoxCollider()
	{
		this.collider.center = new Vector3((float)this.size.x * 0.5f, 0f - (float)this.size.y * 0.5f, 0f);
		this.collider.size = new Vector3((float)this.size.x * this.ColliderScale + (float)(2 * this.ColliderPadding.x), (float)this.size.y * this.ColliderScale + (float)(2 * this.ColliderPadding.y), 0f);
	}

	// Token: 0x06008D5D RID: 36189 RVA: 0x0035B3C5 File Offset: 0x003595C5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetDirty()
	{
		this.isDirty = true;
	}

	// Token: 0x170010B3 RID: 4275
	// (get) Token: 0x06008D5E RID: 36190 RVA: 0x0035B3CE File Offset: 0x003595CE
	// (set) Token: 0x06008D5F RID: 36191 RVA: 0x0035B3D6 File Offset: 0x003595D6
	[XuiXmlAttribute("anchor_left", false)]
	public string AnchorLeft
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.anchorLeft;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.anchorLeft == value)
			{
				return;
			}
			this.anchorLeft = value;
			this.SetDirty();
		}
	}

	// Token: 0x170010B4 RID: 4276
	// (get) Token: 0x06008D60 RID: 36192 RVA: 0x0035B3F4 File Offset: 0x003595F4
	// (set) Token: 0x06008D61 RID: 36193 RVA: 0x0035B3FC File Offset: 0x003595FC
	[XuiXmlAttribute("anchor_right", false)]
	public string AnchorRight
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.anchorRight;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.anchorRight == value)
			{
				return;
			}
			this.anchorRight = value;
			this.SetDirty();
		}
	}

	// Token: 0x170010B5 RID: 4277
	// (get) Token: 0x06008D62 RID: 36194 RVA: 0x0035B41A File Offset: 0x0035961A
	// (set) Token: 0x06008D63 RID: 36195 RVA: 0x0035B422 File Offset: 0x00359622
	[XuiXmlAttribute("anchor_bottom", false)]
	public string AnchorBottom
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.anchorBottom;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.anchorBottom == value)
			{
				return;
			}
			this.anchorBottom = value;
			this.SetDirty();
		}
	}

	// Token: 0x170010B6 RID: 4278
	// (get) Token: 0x06008D64 RID: 36196 RVA: 0x0035B440 File Offset: 0x00359640
	// (set) Token: 0x06008D65 RID: 36197 RVA: 0x0035B448 File Offset: 0x00359648
	[XuiXmlAttribute("anchor_top", false)]
	public string AnchorTop
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.anchorTop;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (this.anchorTop == value)
			{
				return;
			}
			this.anchorTop = value;
			this.SetDirty();
		}
	}

	// Token: 0x170010B7 RID: 4279
	// (get) Token: 0x06008D66 RID: 36198 RVA: 0x0035B466 File Offset: 0x00359666
	public bool HasAnchorStringLeft
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return !string.IsNullOrEmpty(this.anchorLeft);
		}
	}

	// Token: 0x170010B8 RID: 4280
	// (get) Token: 0x06008D67 RID: 36199 RVA: 0x0035B476 File Offset: 0x00359676
	public bool HasAnchorStringRight
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return !string.IsNullOrEmpty(this.anchorRight);
		}
	}

	// Token: 0x170010B9 RID: 4281
	// (get) Token: 0x06008D68 RID: 36200 RVA: 0x0035B486 File Offset: 0x00359686
	public bool HasAnchorStringTop
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return !string.IsNullOrEmpty(this.anchorTop);
		}
	}

	// Token: 0x170010BA RID: 4282
	// (get) Token: 0x06008D69 RID: 36201 RVA: 0x0035B496 File Offset: 0x00359696
	public bool HasAnchorStringBottom
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return !string.IsNullOrEmpty(this.anchorBottom);
		}
	}

	// Token: 0x170010BB RID: 4283
	// (get) Token: 0x06008D6A RID: 36202 RVA: 0x0035B4A6 File Offset: 0x003596A6
	public bool HasAnchorStringLeftAndRight
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.HasAnchorStringLeft && this.HasAnchorStringRight;
		}
	}

	// Token: 0x170010BC RID: 4284
	// (get) Token: 0x06008D6B RID: 36203 RVA: 0x0035B4B8 File Offset: 0x003596B8
	public bool HasAnchorStringTopAndBottom
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.HasAnchorStringTop && this.HasAnchorStringBottom;
		}
	}

	// Token: 0x06008D6C RID: 36204 RVA: 0x0035B4CC File Offset: 0x003596CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void parseAnchors()
	{
		UIRect uiRect = this.UiRect;
		if (uiRect == null)
		{
			return;
		}
		if (this.<parseAnchors>g__ParseAnchorString|281_0(this.anchorLeft, ref this.anchorLeftParsed, uiRect, uiRect.leftAnchor) | this.<parseAnchors>g__ParseAnchorString|281_0(this.anchorRight, ref this.anchorRightParsed, uiRect, uiRect.rightAnchor) | this.<parseAnchors>g__ParseAnchorString|281_0(this.anchorBottom, ref this.anchorBottomParsed, uiRect, uiRect.bottomAnchor) | this.<parseAnchors>g__ParseAnchorString|281_0(this.anchorTop, ref this.anchorTopParsed, uiRect, uiRect.topAnchor))
		{
			this.anchoredLeftAndRight = uiRect.leftAnchor.target && uiRect.rightAnchor.target;
			this.anchoredTopAndBottom = uiRect.bottomAnchor.target && uiRect.topAnchor.target;
			this.SetDirty();
			uiRect.ResetAnchors();
		}
		this.anchorsParsed();
	}

	// Token: 0x06008D6D RID: 36205 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void anchorsParsed()
	{
	}

	// Token: 0x06008D6E RID: 36206 RVA: 0x0035B5BB File Offset: 0x003597BB
	public virtual void OnOpen()
	{
		if (this.xuiSound != null && this.SoundPlayOnOpen)
		{
			Manager.PlayXUiSound(this.xuiSound, this.SoundVolume);
		}
		this.isPressed = false;
		this.isHold = false;
	}

	// Token: 0x06008D6F RID: 36207 RVA: 0x0035B5F4 File Offset: 0x003597F4
	public virtual void OnClose()
	{
		this.isPressed = false;
		this.isHold = false;
		if (!GameManager.Instance.IsQuitting)
		{
			CursorControllerAbs cursorController = this.xui.playerUI.CursorController;
			if (cursorController.HoverTarget == this)
			{
				cursorController.HoverTarget = null;
			}
			if (cursorController.navigationTarget == this)
			{
				this.controller.Hovered(false);
				cursorController.SetNavigationTarget(null);
			}
			if (cursorController.lockNavigationToView == this)
			{
				cursorController.SetNavigationLockView(null, null);
			}
		}
	}

	// Token: 0x06008D70 RID: 36208 RVA: 0x0035B669 File Offset: 0x00359869
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnScroll(GameObject _go, float _delta)
	{
		if (this.EventOnScroll)
		{
			this.controller.Scrolled(_delta);
		}
	}

	// Token: 0x06008D71 RID: 36209 RVA: 0x0035B67F File Offset: 0x0035987F
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSelect(GameObject _go, bool _selected)
	{
		if (this.EventOnSelect && this.enabled)
		{
			this.controller.Selected(_selected);
		}
	}

	// Token: 0x06008D72 RID: 36210 RVA: 0x0035B6A0 File Offset: 0x003598A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDrag(GameObject _go, Vector2 _delta)
	{
		if (this.EventOnDrag && this.enabled)
		{
			EDragType edragType = (this.wasDragging ? EDragType.Dragging : EDragType.DragStart);
			this.wasDragging = true;
			this.controller.Dragged(_delta, edragType);
		}
	}

	// Token: 0x06008D73 RID: 36211 RVA: 0x0035B6E0 File Offset: 0x003598E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPress(GameObject _go, bool _pressed)
	{
		if (this.EventOnPress && this.enabled)
		{
			this.controller.MouseUpDown(_pressed);
		}
		if (this.EventOnDrag && this.enabled && !_pressed && this.wasDragging)
		{
			this.wasDragging = false;
			this.controller.Dragged(default(Vector2), EDragType.DragEnd);
		}
		if (this.EventOnHeld)
		{
			if (_pressed && !this.isPressed && this.enabled)
			{
				this.isPressed = true;
				this.isHold = false;
				this.pressStartTime = Time.unscaledTime;
				this.holdStartTime = -1f;
				return;
			}
			if (!_pressed)
			{
				this.isPressed = false;
				bool flag = this.isHold;
				this.isHold = false;
				if (flag)
				{
					this.controller.Held(EHoldType.HoldEnd, Time.unscaledTime - this.holdStartTime, -1f);
				}
			}
		}
	}

	// Token: 0x06008D74 RID: 36212 RVA: 0x0035B7B8 File Offset: 0x003599B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnHeldDragOut(GameObject _go)
	{
		if (this.EventOnHeld && this.isPressed)
		{
			this.isPressed = false;
			bool flag = this.isHold;
			this.isHold = false;
			if (flag)
			{
				this.controller.Held(EHoldType.HoldEnd, Time.unscaledTime - this.holdStartTime, -1f);
			}
		}
	}

	// Token: 0x06008D75 RID: 36213 RVA: 0x0035B808 File Offset: 0x00359A08
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnClick(GameObject _go)
	{
		if (this.EventOnPress && this.enabled)
		{
			if (this.xuiSound != null && this.SoundPlayOnClick && UICamera.currentTouchID == -1)
			{
				Manager.PlayXUiSound(this.xuiSound, this.SoundVolume);
			}
			this.controller.Pressed(UICamera.currentTouchID);
		}
	}

	// Token: 0x06008D76 RID: 36214 RVA: 0x0035B864 File Offset: 0x00359A64
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDoubleClick(GameObject _go)
	{
		if (this.EventOnDoubleClick && this.enabled)
		{
			if (this.xuiSound != null && this.SoundPlayOnClick && UICamera.currentTouchID == -1)
			{
				Manager.PlayXUiSound(this.xuiSound, this.SoundVolume);
			}
			this.controller.DoubleClicked(UICamera.currentTouchID);
		}
	}

	// Token: 0x06008D77 RID: 36215 RVA: 0x0035B8C0 File Offset: 0x00359AC0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnHover(bool _isOver)
	{
		PlayerActionsLocal playerInput = this.xui.playerUI.playerInput;
		if (playerInput != null && playerInput.LastDeviceClass == InputDeviceClass.Keyboard && !Cursor.visible)
		{
			_isOver = false;
		}
		bool flag = _isOver;
		bool flag2 = _isOver && !this.enabled && !string.IsNullOrEmpty(this.DisabledToolTip);
		_isOver &= this.enabled;
		bool flag3 = _isOver && !string.IsNullOrEmpty(this.ToolTip);
		if (_isOver != this.isOver && _isOver)
		{
			this.PlayHoverSound();
		}
		this.isOver = _isOver;
		if (this.EventOnHover)
		{
			this.controller.Hovered(_isOver);
		}
		if (this.xui.ToolTipWindow != null)
		{
			if (flag3)
			{
				this.xui.ToolTipWindow.ToolTip = this.ToolTip;
			}
			else if (flag2)
			{
				this.xui.ToolTipWindow.ToolTip = this.DisabledToolTip;
			}
			else
			{
				this.xui.ToolTipWindow.ToolTip = "";
			}
		}
		this.xui.playerUI.CursorController.HoverTarget = (flag ? this : null);
	}

	// Token: 0x06008D78 RID: 36216 RVA: 0x0035B9DB File Offset: 0x00359BDB
	public void PlayHoverSound()
	{
		if (this.xuiHoverSound != null && this.SoundPlayOnHover && this.enabled && GameManager.Instance.GameIsFocused)
		{
			Manager.PlayXUiSound(this.xuiHoverSound, this.SoundVolume);
		}
	}

	// Token: 0x06008D79 RID: 36217 RVA: 0x0035BA18 File Offset: 0x00359C18
	public void PlayClickSound()
	{
		if (this.xuiSound != null)
		{
			Manager.PlayXUiSound(this.xuiSound, this.SoundVolume);
		}
	}

	// Token: 0x06008D7A RID: 36218 RVA: 0x0035BA39 File Offset: 0x00359C39
	public Rect GetXUiRect()
	{
		return this.GetXUiRect(Vector2.zero);
	}

	// Token: 0x06008D7B RID: 36219 RVA: 0x0035BA48 File Offset: 0x00359C48
	public Rect GetXUiRect(Vector2 _padding)
	{
		XUiController parent = this.controller;
		Vector3 vector = this.uiTransform.localPosition;
		vector.x -= _padding.x;
		vector.y += _padding.y;
		while (parent.Parent != null && parent.Parent.ViewComponent != null)
		{
			parent = parent.Parent;
			vector += parent.ViewComponent.uiTransform.localPosition;
		}
		vector += parent.ViewComponent.uiTransform.parent.localPosition;
		Vector2 vector2 = this.Size.AsVector2() + 2f * _padding;
		XUiV_Window xuiV_Window = parent.ViewComponent as XUiV_Window;
		if (xuiV_Window != null && xuiV_Window.IsInStackPanel)
		{
			Transform parent2 = xuiV_Window.uiTransform.parent.parent;
			vector *= parent2.localScale.x;
			vector2 *= parent2.localScale.x;
			vector += parent2.localPosition;
		}
		return new Rect(new Vector2(vector.x, vector.y - vector2.y), vector2);
	}

	// Token: 0x06008D7C RID: 36220 RVA: 0x0035BB76 File Offset: 0x00359D76
	public string GetXuiHierarchy()
	{
		return XUiUtils.GetXuiHierarchy(this.Controller);
	}

	// Token: 0x06008D7D RID: 36221 RVA: 0x0035BB83 File Offset: 0x00359D83
	public Vector2 GetClosestPoint(Vector3 _point)
	{
		return this.collider.ClosestPointOnBounds(_point);
	}

	// Token: 0x06008D7E RID: 36222 RVA: 0x0035BB98 File Offset: 0x00359D98
	public void ClearNavigationTargets()
	{
		this.NavUpTarget = (this.NavDownTarget = (this.NavLeftTarget = (this.NavRightTarget = null)));
	}

	// Token: 0x06008D7F RID: 36223 RVA: 0x0035BBC8 File Offset: 0x00359DC8
	[PublicizedFrom(EAccessModifier.Private)]
	public void parseNavigationTargets()
	{
		if (!string.IsNullOrEmpty(this.navUpTargetString))
		{
			this.NavUpTarget = this.<parseNavigationTargets>g__FindView|310_0(this.navUpTargetString);
		}
		if (!string.IsNullOrEmpty(this.navDownTargetString))
		{
			this.NavDownTarget = this.<parseNavigationTargets>g__FindView|310_0(this.navDownTargetString);
		}
		if (!string.IsNullOrEmpty(this.navLeftTargetString))
		{
			this.NavLeftTarget = this.<parseNavigationTargets>g__FindView|310_0(this.navLeftTargetString);
		}
		if (!string.IsNullOrEmpty(this.navRightTargetString))
		{
			this.NavRightTarget = this.<parseNavigationTargets>g__FindView|310_0(this.navRightTargetString);
		}
	}

	// Token: 0x06008D80 RID: 36224 RVA: 0x0035BC54 File Offset: 0x00359E54
	public virtual void SetDefaults(XUiController _parent)
	{
		this.RepeatContent = false;
		this.RepeatCount = 1;
		this.Size = Vector2i.min;
		int? num;
		if (_parent == null)
		{
			num = null;
		}
		else
		{
			XUiView viewComponent = _parent.ViewComponent;
			num = ((viewComponent != null) ? new int?(viewComponent.Depth) : null);
		}
		int? num2 = num;
		this.Depth = num2.GetValueOrDefault();
		this.ToolTip = "";
		this.SoundPlayOnClick = true;
		this.SoundPlayOnOpen = false;
		this.SoundPlayOnHover = true;
		this.SoundVolume = 1f;
	}

	// Token: 0x06008D81 RID: 36225 RVA: 0x0035BCE0 File Offset: 0x00359EE0
	public virtual void SetPostParsingDefaults(XUiController _parent)
	{
		XUiView xuiView = ((_parent != null) ? _parent.ViewComponent : null);
		Vector2i vector2i = this.Size;
		if (vector2i.x == -2147483648)
		{
			vector2i.x = (this.ignoreParentPadding ? ((xuiView != null) ? xuiView.Size.x : 0) : ((xuiView != null) ? xuiView.InnerSize.x : 0));
		}
		if (vector2i.y == -2147483648)
		{
			vector2i.y = (this.ignoreParentPadding ? ((xuiView != null) ? xuiView.Size.y : 0) : ((xuiView != null) ? xuiView.InnerSize.y : 0));
		}
		this.Size = vector2i;
	}

	// Token: 0x06008D82 RID: 36226 RVA: 0x0035BD88 File Offset: 0x00359F88
	public void ParseInitialAttributeValue(string _attribute, string _value)
	{
		if (_value.Contains("{"))
		{
			BindingsManager.CreateBinding(this, _attribute, _value);
			return;
		}
		if (ParsingMethodCache.Instance.TryParseDirect(this, _attribute, _value))
		{
			return;
		}
		if (this.Controller != null && ParsingMethodCache.Instance.TryParseDirect(this.Controller, _attribute, _value))
		{
			return;
		}
		this.ParseAttributeViewAndController(_attribute, _value);
	}

	// Token: 0x06008D83 RID: 36227 RVA: 0x0035BDE4 File Offset: 0x00359FE4
	public bool ParseAttributeViewAndController(string _attribute, string _value)
	{
		if (_value.Contains("{") && XUiFromXml.DebugXuiLoading == XUiFromXml.DebugLevel.Verbose)
		{
			Log.Warning(string.Concat(new string[] { "[XUi] Refreshed binding contained '{': ", _attribute, "='", _value, "' on ", this.id }));
		}
		if (this.Controller != null)
		{
			if (!this.Controller.ParseAttribute(_attribute, _value))
			{
				this.Controller.CustomAttributes[_attribute] = _value;
			}
			return true;
		}
		return false;
	}

	// Token: 0x06008D84 RID: 36228 RVA: 0x0035BE6B File Offset: 0x0035A06B
	public virtual void OnVisibilityChanged(bool _visibleInScene)
	{
		if (this.isOver && !_visibleInScene)
		{
			this.OnHover(false);
		}
	}

	// Token: 0x06008D89 RID: 36233 RVA: 0x0035BEA8 File Offset: 0x0035A0A8
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool <parseAnchors>g__ParseAnchorString|281_0(string _anchorString, ref string _parsedString, UIRect _uiRect, UIRect.AnchorPoint _anchor)
	{
		if (string.IsNullOrEmpty(_anchorString))
		{
			return false;
		}
		if (_anchorString == _parsedString && _anchor.target != null)
		{
			return false;
		}
		_parsedString = _anchorString;
		int num = _anchorString.IndexOf(',');
		if (num < 0)
		{
			throw new ArgumentException("Invalid anchor string '" + _anchorString + "', expected '<target>,<relative>,<absolute>'");
		}
		string text = _anchorString.Substring(0, num);
		int num2 = _anchorString.IndexOf(',', num + 1);
		if (num2 < 0)
		{
			throw new ArgumentException("Invalid anchor string '" + _anchorString + "', expected '<target>,<relative>,<absolute>'");
		}
		float num3 = StringParsers.ParseFloat(_anchorString, num + 1, num2 - 1, NumberStyles.Any);
		int num4 = StringParsers.ParseSInt32(_anchorString, num2 + 1, -1, NumberStyles.Integer);
		if (text.Length == 0)
		{
			throw new ArgumentException("Invalid anchor string '" + _anchorString + "', expected '<target>,<relative>,<absolute>'");
		}
		if (text.EqualsCaseInsensitive("#parent"))
		{
			_anchor.target = this.uiTransform.parent;
		}
		else if (text.EqualsCaseInsensitive("#cam"))
		{
			UICamera componentInParent = this.uiTransform.gameObject.GetComponentInParent<UICamera>();
			if (componentInParent == null)
			{
				throw new Exception("UICamera not found");
			}
			_anchor.target = componentInParent.transform;
		}
		else if (text[0] == '#')
		{
			string text2 = text.Substring(1);
			UIAnchor.Side side;
			if (!EnumUtils.TryParse<UIAnchor.Side>(text2, out side, true))
			{
				throw new ArgumentException("Invalid anchor side name '" + text2 + "', expected any of '\tBottomLeft,Left,TopLeft,Top,TopRight,Right,BottomRight,Bottom,Center'");
			}
			_anchor.target = this.xui.GetAnchor(side).transform;
		}
		else
		{
			XUiView xuiView = XUiUtils.FindHierarchyClosestView(this.Controller, text);
			if (xuiView == null)
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Invalid anchor string '",
					_anchorString,
					"', view component with name '",
					text,
					"' not found.\nOn: ",
					this.GetXuiHierarchy()
				}));
			}
			_anchor.target = xuiView.UiTransform;
			XUiV_Grid xuiV_Grid = xuiView as XUiV_Grid;
			if (xuiV_Grid != null)
			{
				xuiV_Grid.OnSizeChanged += delegate(Vector2Int _, Vector2 _)
				{
					_uiRect.UpdateAnchors();
				};
			}
		}
		_anchor.relative = num3;
		_anchor.absolute = num4;
		return true;
	}

	// Token: 0x06008D8A RID: 36234 RVA: 0x0035C0C4 File Offset: 0x0035A2C4
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView <parseNavigationTargets>g__FindView|310_0(string _name)
	{
		if (string.IsNullOrEmpty(_name))
		{
			return null;
		}
		XUiView xuiView = XUiUtils.FindHierarchyClosestView(this.Controller, _name);
		if (xuiView != null)
		{
			return xuiView;
		}
		XUiController childById = this.controller.WindowGroup.Controller.GetChildById(_name);
		xuiView = ((childById != null) ? childById.ViewComponent : null);
		if (xuiView != null)
		{
			return xuiView;
		}
		Log.Error("Invalid navigation target, view component with name '" + _name + "' not found.\nOn: " + this.GetXuiHierarchy());
		return null;
	}

	// Token: 0x0400680E RID: 26638
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<Type, GameObject> componentTemplates = new Dictionary<Type, GameObject>();

	// Token: 0x0400680F RID: 26639
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform templatesParent;

	// Token: 0x04006810 RID: 26640
	public readonly XUi xui;

	// Token: 0x04006811 RID: 26641
	[PublicizedFrom(EAccessModifier.Private)]
	public bool destroyed;

	// Token: 0x04006812 RID: 26642
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController controller;

	// Token: 0x04006813 RID: 26643
	[PublicizedFrom(EAccessModifier.Protected)]
	public Transform uiTransform;

	// Token: 0x04006814 RID: 26644
	[PublicizedFrom(EAccessModifier.Protected)]
	public BoxCollider collider;

	// Token: 0x04006815 RID: 26645
	public readonly List<XUiTweenAbs> Tweeners = new List<XUiTweenAbs>();

	// Token: 0x04006818 RID: 26648
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDirty;

	// Token: 0x04006819 RID: 26649
	[PublicizedFrom(EAccessModifier.Protected)]
	public string id;

	// Token: 0x0400681A RID: 26650
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isVisible = true;

	// Token: 0x0400681B RID: 26651
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool enabled = true;

	// Token: 0x0400681E RID: 26654
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector2i size;

	// Token: 0x0400681F RID: 26655
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector2i position;

	// Token: 0x04006820 RID: 26656
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool positionDirty;

	// Token: 0x04006821 RID: 26657
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool ignoreParentPadding;

	// Token: 0x04006822 RID: 26658
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiSideSizes padding;

	// Token: 0x04006823 RID: 26659
	[PublicizedFrom(EAccessModifier.Protected)]
	public float rotation;

	// Token: 0x04006824 RID: 26660
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool rotationDirty;

	// Token: 0x04006825 RID: 26661
	[PublicizedFrom(EAccessModifier.Protected)]
	public int depth;

	// Token: 0x04006826 RID: 26662
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip xuiSound;

	// Token: 0x04006827 RID: 26663
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip xuiHoverSound;

	// Token: 0x04006830 RID: 26672
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eventOnHover;

	// Token: 0x04006831 RID: 26673
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eventOnPress;

	// Token: 0x04006832 RID: 26674
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eventOnDoubleClick;

	// Token: 0x04006833 RID: 26675
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eventOnHeld;

	// Token: 0x04006834 RID: 26676
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eventOnScroll;

	// Token: 0x04006835 RID: 26677
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eventOnDrag;

	// Token: 0x04006836 RID: 26678
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eventOnSelect;

	// Token: 0x04006837 RID: 26679
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isNavigatable = true;

	// Token: 0x0400683A RID: 26682
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool gamepadSelectableSetFromAttributes;

	// Token: 0x0400683B RID: 26683
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool gamepadSnappableSetFromAttributes;

	// Token: 0x0400683C RID: 26684
	[PublicizedFrom(EAccessModifier.Private)]
	public string navUpTargetString;

	// Token: 0x0400683D RID: 26685
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView navUpTarget;

	// Token: 0x0400683E RID: 26686
	[PublicizedFrom(EAccessModifier.Private)]
	public string navDownTargetString;

	// Token: 0x0400683F RID: 26687
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView navDownTarget;

	// Token: 0x04006840 RID: 26688
	[PublicizedFrom(EAccessModifier.Private)]
	public string navLeftTargetString;

	// Token: 0x04006841 RID: 26689
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView navLeftTarget;

	// Token: 0x04006842 RID: 26690
	[PublicizedFrom(EAccessModifier.Private)]
	public string navRightTargetString;

	// Token: 0x04006843 RID: 26691
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView navRightTarget;

	// Token: 0x04006844 RID: 26692
	[PublicizedFrom(EAccessModifier.Private)]
	public string toolTip;

	// Token: 0x04006845 RID: 26693
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledToolTip;

	// Token: 0x04006846 RID: 26694
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmUpdateHover = new ProfilerMarker("XV.U-Hover");

	// Token: 0x04006847 RID: 26695
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmUpdatePress = new ProfilerMarker("XV.U-Press");

	// Token: 0x04006848 RID: 26696
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmUpdateData = new ProfilerMarker("XV.U-UpdateData");

	// Token: 0x04006849 RID: 26697
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorLeft;

	// Token: 0x0400684A RID: 26698
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorRight;

	// Token: 0x0400684B RID: 26699
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorBottom;

	// Token: 0x0400684C RID: 26700
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorTop;

	// Token: 0x0400684D RID: 26701
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorLeftParsed;

	// Token: 0x0400684E RID: 26702
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorRightParsed;

	// Token: 0x0400684F RID: 26703
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorBottomParsed;

	// Token: 0x04006850 RID: 26704
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorTopParsed;

	// Token: 0x04006851 RID: 26705
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool anchoredLeftAndRight;

	// Token: 0x04006852 RID: 26706
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool anchoredTopAndBottom;

	// Token: 0x04006853 RID: 26707
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasDragging;

	// Token: 0x04006854 RID: 26708
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPressed;

	// Token: 0x04006855 RID: 26709
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHold;

	// Token: 0x04006856 RID: 26710
	[PublicizedFrom(EAccessModifier.Private)]
	public float pressStartTime;

	// Token: 0x04006857 RID: 26711
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdStartTime;

	// Token: 0x04006858 RID: 26712
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdEventIntervalCurrent;

	// Token: 0x04006859 RID: 26713
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdEventIntervalChangeSpeed;

	// Token: 0x0400685A RID: 26714
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdEventNextTime;

	// Token: 0x0400685B RID: 26715
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdEventLastTime;

	// Token: 0x0400685C RID: 26716
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isOver;
}
