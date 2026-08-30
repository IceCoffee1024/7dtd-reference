using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Audio;
using InControl;
using Unity.Profiling;
using UnityEngine;

// Token: 0x02001135 RID: 4405
public abstract class XUiView : IXUiElement
{
	// Token: 0x17000FFE RID: 4094
	// (get) Token: 0x06008B2D RID: 35629 RVA: 0x0034F38F File Offset: 0x0034D58F
	// (set) Token: 0x06008B2E RID: 35630 RVA: 0x0034F397 File Offset: 0x0034D597
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

	// Token: 0x17000FFF RID: 4095
	// (get) Token: 0x06008B2F RID: 35631
	public abstract UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
	}

	// Token: 0x17001000 RID: 4096
	// (get) Token: 0x06008B30 RID: 35632 RVA: 0x0034F3BA File Offset: 0x0034D5BA
	public Transform UiTransform
	{
		get
		{
			return this.uiTransform;
		}
	}

	// Token: 0x17001001 RID: 4097
	// (get) Token: 0x06008B31 RID: 35633 RVA: 0x0034F3C2 File Offset: 0x0034D5C2
	public bool ColliderEnabled
	{
		get
		{
			return !this.destroyed && this.collider.enabled;
		}
	}

	// Token: 0x17001002 RID: 4098
	// (get) Token: 0x06008B32 RID: 35634 RVA: 0x0034F3DC File Offset: 0x0034D5DC
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

	// Token: 0x17001003 RID: 4099
	// (get) Token: 0x06008B33 RID: 35635 RVA: 0x0034F410 File Offset: 0x0034D610
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

	// Token: 0x17001004 RID: 4100
	// (get) Token: 0x06008B34 RID: 35636 RVA: 0x0034F44C File Offset: 0x0034D64C
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

	// Token: 0x17001005 RID: 4101
	// (get) Token: 0x06008B35 RID: 35637 RVA: 0x0034F488 File Offset: 0x0034D688
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

	// Token: 0x17001006 RID: 4102
	// (get) Token: 0x06008B36 RID: 35638 RVA: 0x0034F4B4 File Offset: 0x0034D6B4
	public virtual Vector3 LocalCenter
	{
		get
		{
			return (this.position + this.size / 2).AsVector2();
		}
	}

	// Token: 0x17001007 RID: 4103
	// (get) Token: 0x06008B37 RID: 35639
	public abstract Vector3[] WorldCorners { get; }

	// Token: 0x17001008 RID: 4104
	// (get) Token: 0x06008B38 RID: 35640 RVA: 0x0034F4E5 File Offset: 0x0034D6E5
	// (set) Token: 0x06008B39 RID: 35641 RVA: 0x0034F4ED File Offset: 0x0034D6ED
	[XuiXmlAttribute("repeat_content", false)]
	public bool RepeatContent { get; set; }

	// Token: 0x17001009 RID: 4105
	// (get) Token: 0x06008B3A RID: 35642 RVA: 0x0034F4F6 File Offset: 0x0034D6F6
	// (set) Token: 0x06008B3B RID: 35643 RVA: 0x0034F4FE File Offset: 0x0034D6FE
	[XuiXmlAttribute("repeat_count", false)]
	public virtual int RepeatCount { get; set; }

	// Token: 0x1700100A RID: 4106
	// (get) Token: 0x06008B3C RID: 35644 RVA: 0x0034F507 File Offset: 0x0034D707
	public bool UiTransformIsHovered
	{
		get
		{
			return UICamera.hoveredObject == this.uiTransform.gameObject;
		}
	}

	// Token: 0x1700100B RID: 4107
	// (get) Token: 0x06008B3D RID: 35645 RVA: 0x0034F51E File Offset: 0x0034D71E
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

	// Token: 0x1700100C RID: 4108
	// (get) Token: 0x06008B3E RID: 35646 RVA: 0x0034F545 File Offset: 0x0034D745
	public string ID
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x06008B3F RID: 35647 RVA: 0x0034F54D File Offset: 0x0034D74D
	[XuiXmlAttribute("name", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeName(string _value)
	{
		this.id = _value;
	}

	// Token: 0x1700100D RID: 4109
	// (get) Token: 0x06008B40 RID: 35648 RVA: 0x0034F556 File Offset: 0x0034D756
	// (set) Token: 0x06008B41 RID: 35649 RVA: 0x0034F560 File Offset: 0x0034D760
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

	// Token: 0x1700100E RID: 4110
	// (get) Token: 0x06008B42 RID: 35650 RVA: 0x0034F5F9 File Offset: 0x0034D7F9
	// (set) Token: 0x06008B43 RID: 35651 RVA: 0x0034F604 File Offset: 0x0034D804
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

	// Token: 0x1700100F RID: 4111
	// (get) Token: 0x06008B44 RID: 35652 RVA: 0x0034F67B File Offset: 0x0034D87B
	// (set) Token: 0x06008B45 RID: 35653 RVA: 0x0034F683 File Offset: 0x0034D883
	[XuiXmlAttribute("collider_scale", false)]
	public float ColliderScale
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	} = 1f;

	// Token: 0x17001010 RID: 4112
	// (get) Token: 0x06008B46 RID: 35654 RVA: 0x0034F68C File Offset: 0x0034D88C
	// (set) Token: 0x06008B47 RID: 35655 RVA: 0x0034F694 File Offset: 0x0034D894
	[XuiXmlAttribute("collider_padding", false)]
	public Vector2i ColliderPadding
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get;
		[PublicizedFrom(EAccessModifier.Protected)]
		set;
	} = Vector2i.zero;

	// Token: 0x17001011 RID: 4113
	// (get) Token: 0x06008B48 RID: 35656 RVA: 0x0034F69D File Offset: 0x0034D89D
	// (set) Token: 0x06008B49 RID: 35657 RVA: 0x0034F6A5 File Offset: 0x0034D8A5
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

	// Token: 0x17001012 RID: 4114
	// (get) Token: 0x06008B4A RID: 35658 RVA: 0x0034F6C3 File Offset: 0x0034D8C3
	// (set) Token: 0x06008B4B RID: 35659 RVA: 0x0034F6D0 File Offset: 0x0034D8D0
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

	// Token: 0x17001013 RID: 4115
	// (get) Token: 0x06008B4C RID: 35660 RVA: 0x0034F6FD File Offset: 0x0034D8FD
	// (set) Token: 0x06008B4D RID: 35661 RVA: 0x0034F70C File Offset: 0x0034D90C
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

	// Token: 0x17001014 RID: 4116
	// (get) Token: 0x06008B4E RID: 35662 RVA: 0x0034F739 File Offset: 0x0034D939
	// (set) Token: 0x06008B4F RID: 35663 RVA: 0x0034F741 File Offset: 0x0034D941
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

	// Token: 0x17001015 RID: 4117
	// (get) Token: 0x06008B50 RID: 35664 RVA: 0x0034F766 File Offset: 0x0034D966
	// (set) Token: 0x06008B51 RID: 35665 RVA: 0x0034F76E File Offset: 0x0034D96E
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

	// Token: 0x17001016 RID: 4118
	// (get) Token: 0x06008B52 RID: 35666 RVA: 0x0034F788 File Offset: 0x0034D988
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

	// Token: 0x17001017 RID: 4119
	// (get) Token: 0x06008B53 RID: 35667 RVA: 0x0034F7FF File Offset: 0x0034D9FF
	public virtual Vector2i InnerSize
	{
		get
		{
			return new Vector2i(this.size.x - this.padding.SumLeftRight, this.size.y - this.padding.SumTopBottom);
		}
	}

	// Token: 0x17001018 RID: 4120
	// (get) Token: 0x06008B54 RID: 35668 RVA: 0x0034F834 File Offset: 0x0034DA34
	public Vector2i InnerPosition
	{
		get
		{
			return new Vector2i(this.padding.Left, -this.padding.Top);
		}
	}

	// Token: 0x17001019 RID: 4121
	// (get) Token: 0x06008B55 RID: 35669 RVA: 0x0034F852 File Offset: 0x0034DA52
	// (set) Token: 0x06008B56 RID: 35670 RVA: 0x0034F85A File Offset: 0x0034DA5A
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

	// Token: 0x1700101A RID: 4122
	// (get) Token: 0x06008B57 RID: 35671 RVA: 0x0034F87F File Offset: 0x0034DA7F
	// (set) Token: 0x06008B58 RID: 35672 RVA: 0x0034F887 File Offset: 0x0034DA87
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

	// Token: 0x06008B59 RID: 35673 RVA: 0x0034F8A0 File Offset: 0x0034DAA0
	[XuiXmlAttribute("depth", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeDepth(int _value)
	{
		XUiView viewComponent = this.controller.Parent.ViewComponent;
		this.Depth = _value + ((viewComponent != null) ? viewComponent.Depth : 0);
	}

	// Token: 0x06008B5A RID: 35674 RVA: 0x0034F8C6 File Offset: 0x0034DAC6
	[XuiXmlAttribute("padding_left", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributePaddingLeft(int _value)
	{
		this.padding = this.padding.SetLeft(_value);
	}

	// Token: 0x06008B5B RID: 35675 RVA: 0x0034F8DA File Offset: 0x0034DADA
	[XuiXmlAttribute("padding_right", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributePaddingRight(int _value)
	{
		this.padding = this.padding.SetRight(_value);
	}

	// Token: 0x06008B5C RID: 35676 RVA: 0x0034F8EE File Offset: 0x0034DAEE
	[XuiXmlAttribute("padding_top", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributePaddingTop(int _value)
	{
		this.padding = this.padding.SetTop(_value);
	}

	// Token: 0x06008B5D RID: 35677 RVA: 0x0034F902 File Offset: 0x0034DB02
	[XuiXmlAttribute("padding_bottom", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributePaddingBottom(int _value)
	{
		this.padding = this.padding.SetBottom(_value);
	}

	// Token: 0x06008B5E RID: 35678 RVA: 0x0034F916 File Offset: 0x0034DB16
	[XuiXmlAttribute("padding", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributePadding(string _value)
	{
		XUiSideSizes.TryParse(_value, out this.padding, "padding");
	}

	// Token: 0x06008B5F RID: 35679 RVA: 0x0034F92A File Offset: 0x0034DB2A
	[XuiXmlAttribute("sound", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeSound(string _value)
	{
		this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _o)
		{
			this.xuiSound = _o;
		});
	}

	// Token: 0x06008B60 RID: 35680 RVA: 0x0034F944 File Offset: 0x0034DB44
	[XuiXmlAttribute("sound_play_on_hover", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeSoundOnHover(string _value)
	{
		this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _o)
		{
			this.xuiHoverSound = _o;
		});
	}

	// Token: 0x1700101B RID: 4123
	// (get) Token: 0x06008B61 RID: 35681 RVA: 0x0034F95E File Offset: 0x0034DB5E
	// (set) Token: 0x06008B62 RID: 35682 RVA: 0x0034F966 File Offset: 0x0034DB66
	[XuiXmlAttribute("sound_play_on_press", false)]
	public bool SoundPlayOnClick { get; set; }

	// Token: 0x1700101C RID: 4124
	// (get) Token: 0x06008B63 RID: 35683 RVA: 0x0034F96F File Offset: 0x0034DB6F
	// (set) Token: 0x06008B64 RID: 35684 RVA: 0x0034F977 File Offset: 0x0034DB77
	public bool SoundPlayOnHover { get; set; }

	// Token: 0x1700101D RID: 4125
	// (get) Token: 0x06008B65 RID: 35685 RVA: 0x0034F980 File Offset: 0x0034DB80
	// (set) Token: 0x06008B66 RID: 35686 RVA: 0x0034F988 File Offset: 0x0034DB88
	[XuiXmlAttribute("sound_play_on_open", false)]
	public bool SoundPlayOnOpen { get; set; }

	// Token: 0x1700101E RID: 4126
	// (get) Token: 0x06008B67 RID: 35687 RVA: 0x0034F991 File Offset: 0x0034DB91
	// (set) Token: 0x06008B68 RID: 35688 RVA: 0x0034F999 File Offset: 0x0034DB99
	[XuiXmlAttribute("sound_volume", false)]
	public float SoundVolume { get; set; }

	// Token: 0x1700101F RID: 4127
	// (get) Token: 0x06008B69 RID: 35689 RVA: 0x0034F9A2 File Offset: 0x0034DBA2
	public bool HasHoverSound
	{
		get
		{
			return this.xuiHoverSound != null;
		}
	}

	// Token: 0x17001020 RID: 4128
	// (get) Token: 0x06008B6A RID: 35690 RVA: 0x0034F9B0 File Offset: 0x0034DBB0
	// (set) Token: 0x06008B6B RID: 35691 RVA: 0x0034F9B8 File Offset: 0x0034DBB8
	[XuiXmlAttribute("hold_delay", false)]
	public float HoldDelay
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = 0.5f;

	// Token: 0x17001021 RID: 4129
	// (get) Token: 0x06008B6C RID: 35692 RVA: 0x0034F9C1 File Offset: 0x0034DBC1
	// (set) Token: 0x06008B6D RID: 35693 RVA: 0x0034F9C9 File Offset: 0x0034DBC9
	[XuiXmlAttribute("hold_timed_initial_interval", false)]
	public float HoldEventIntervalInitial
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = 0.5f;

	// Token: 0x17001022 RID: 4130
	// (get) Token: 0x06008B6E RID: 35694 RVA: 0x0034F9D2 File Offset: 0x0034DBD2
	// (set) Token: 0x06008B6F RID: 35695 RVA: 0x0034F9DA File Offset: 0x0034DBDA
	[XuiXmlAttribute("hold_timed_final_interval", false)]
	public float HoldEventIntervalFinal
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = 0.06f;

	// Token: 0x17001023 RID: 4131
	// (get) Token: 0x06008B70 RID: 35696 RVA: 0x0034F9E3 File Offset: 0x0034DBE3
	// (set) Token: 0x06008B71 RID: 35697 RVA: 0x0034F9EB File Offset: 0x0034DBEB
	[XuiXmlAttribute("hold_timed_step_acceleration", false)]
	public float HoldEventIntervalAcceleration
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	} = 0.015f;

	// Token: 0x17001024 RID: 4132
	// (get) Token: 0x06008B72 RID: 35698 RVA: 0x0034F9F4 File Offset: 0x0034DBF4
	// (set) Token: 0x06008B73 RID: 35699 RVA: 0x0034F9FC File Offset: 0x0034DBFC
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

	// Token: 0x17001025 RID: 4133
	// (get) Token: 0x06008B74 RID: 35700 RVA: 0x0034FA15 File Offset: 0x0034DC15
	// (set) Token: 0x06008B75 RID: 35701 RVA: 0x0034FA1D File Offset: 0x0034DC1D
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

	// Token: 0x17001026 RID: 4134
	// (get) Token: 0x06008B76 RID: 35702 RVA: 0x0034FA36 File Offset: 0x0034DC36
	// (set) Token: 0x06008B77 RID: 35703 RVA: 0x0034FA3E File Offset: 0x0034DC3E
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

	// Token: 0x17001027 RID: 4135
	// (get) Token: 0x06008B78 RID: 35704 RVA: 0x0034FA57 File Offset: 0x0034DC57
	// (set) Token: 0x06008B79 RID: 35705 RVA: 0x0034FA5F File Offset: 0x0034DC5F
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

	// Token: 0x17001028 RID: 4136
	// (get) Token: 0x06008B7A RID: 35706 RVA: 0x0034FA78 File Offset: 0x0034DC78
	// (set) Token: 0x06008B7B RID: 35707 RVA: 0x0034FA80 File Offset: 0x0034DC80
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

	// Token: 0x17001029 RID: 4137
	// (get) Token: 0x06008B7C RID: 35708 RVA: 0x0034FA99 File Offset: 0x0034DC99
	// (set) Token: 0x06008B7D RID: 35709 RVA: 0x0034FAA1 File Offset: 0x0034DCA1
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

	// Token: 0x1700102A RID: 4138
	// (get) Token: 0x06008B7E RID: 35710 RVA: 0x0034FABA File Offset: 0x0034DCBA
	// (set) Token: 0x06008B7F RID: 35711 RVA: 0x0034FAC2 File Offset: 0x0034DCC2
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

	// Token: 0x1700102B RID: 4139
	// (get) Token: 0x06008B80 RID: 35712 RVA: 0x0034FADC File Offset: 0x0034DCDC
	public virtual bool HasAnyEvent
	{
		get
		{
			return this.EventOnPress || this.EventOnDoubleClick || this.EventOnHover || this.EventOnHeld || this.EventOnDrag || this.EventOnScroll || this.EventOnSelect || !string.IsNullOrEmpty(this.ToolTip);
		}
	}

	// Token: 0x1700102C RID: 4140
	// (get) Token: 0x06008B81 RID: 35713 RVA: 0x0034FB31 File Offset: 0x0034DD31
	public bool IsHovered
	{
		get
		{
			return this.isOver;
		}
	}

	// Token: 0x1700102D RID: 4141
	// (get) Token: 0x06008B82 RID: 35714 RVA: 0x0034FB39 File Offset: 0x0034DD39
	// (set) Token: 0x06008B83 RID: 35715 RVA: 0x0034FB5B File Offset: 0x0034DD5B
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

	// Token: 0x06008B84 RID: 35716 RVA: 0x0034FB64 File Offset: 0x0034DD64
	[XuiXmlAttribute("gamepad_selectable", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeGamepadSelectable(bool _value)
	{
		this.IsNavigatable = _value;
		this.gamepadSelectableSetFromAttributes = true;
	}

	// Token: 0x1700102E RID: 4142
	// (get) Token: 0x06008B85 RID: 35717 RVA: 0x0034FB74 File Offset: 0x0034DD74
	// (set) Token: 0x06008B86 RID: 35718 RVA: 0x0034FB7C File Offset: 0x0034DD7C
	public bool IsSnappable { get; set; } = true;

	// Token: 0x06008B87 RID: 35719 RVA: 0x0034FB85 File Offset: 0x0034DD85
	[XuiXmlAttribute("snap", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeGamepadSnappable(bool _value)
	{
		this.IsSnappable = _value;
		this.gamepadSnappableSetFromAttributes = true;
	}

	// Token: 0x1700102F RID: 4143
	// (get) Token: 0x06008B88 RID: 35720 RVA: 0x0034FB95 File Offset: 0x0034DD95
	// (set) Token: 0x06008B89 RID: 35721 RVA: 0x0034FB9D File Offset: 0x0034DD9D
	[XuiXmlAttribute("use_selection_box", false)]
	public bool UseSelectionBox { get; set; } = true;

	// Token: 0x17001030 RID: 4144
	// (get) Token: 0x06008B8A RID: 35722 RVA: 0x0034FBA6 File Offset: 0x0034DDA6
	// (set) Token: 0x06008B8B RID: 35723 RVA: 0x0034FBAE File Offset: 0x0034DDAE
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

	// Token: 0x17001031 RID: 4145
	// (get) Token: 0x06008B8C RID: 35724 RVA: 0x0034FBCC File Offset: 0x0034DDCC
	// (set) Token: 0x06008B8D RID: 35725 RVA: 0x0034FBD4 File Offset: 0x0034DDD4
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

	// Token: 0x17001032 RID: 4146
	// (get) Token: 0x06008B8E RID: 35726 RVA: 0x0034FC38 File Offset: 0x0034DE38
	// (set) Token: 0x06008B8F RID: 35727 RVA: 0x0034FC40 File Offset: 0x0034DE40
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

	// Token: 0x17001033 RID: 4147
	// (get) Token: 0x06008B90 RID: 35728 RVA: 0x0034FC5E File Offset: 0x0034DE5E
	// (set) Token: 0x06008B91 RID: 35729 RVA: 0x0034FC68 File Offset: 0x0034DE68
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

	// Token: 0x17001034 RID: 4148
	// (get) Token: 0x06008B92 RID: 35730 RVA: 0x0034FCCC File Offset: 0x0034DECC
	// (set) Token: 0x06008B93 RID: 35731 RVA: 0x0034FCD4 File Offset: 0x0034DED4
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

	// Token: 0x17001035 RID: 4149
	// (get) Token: 0x06008B94 RID: 35732 RVA: 0x0034FCF2 File Offset: 0x0034DEF2
	// (set) Token: 0x06008B95 RID: 35733 RVA: 0x0034FCFC File Offset: 0x0034DEFC
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

	// Token: 0x17001036 RID: 4150
	// (get) Token: 0x06008B96 RID: 35734 RVA: 0x0034FD60 File Offset: 0x0034DF60
	// (set) Token: 0x06008B97 RID: 35735 RVA: 0x0034FD68 File Offset: 0x0034DF68
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

	// Token: 0x17001037 RID: 4151
	// (get) Token: 0x06008B98 RID: 35736 RVA: 0x0034FD86 File Offset: 0x0034DF86
	// (set) Token: 0x06008B99 RID: 35737 RVA: 0x0034FD90 File Offset: 0x0034DF90
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

	// Token: 0x17001038 RID: 4152
	// (get) Token: 0x06008B9A RID: 35738 RVA: 0x0034FDF4 File Offset: 0x0034DFF4
	// (set) Token: 0x06008B9B RID: 35739 RVA: 0x0034FDFC File Offset: 0x0034DFFC
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

	// Token: 0x17001039 RID: 4153
	// (get) Token: 0x06008B9C RID: 35740 RVA: 0x0034FE76 File Offset: 0x0034E076
	// (set) Token: 0x06008B9D RID: 35741 RVA: 0x0034FE80 File Offset: 0x0034E080
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

	// Token: 0x06008B9E RID: 35742 RVA: 0x0034FEFA File Offset: 0x0034E0FA
	[XuiXmlAttribute("tooltip_key", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeTooltipKey(string _value)
	{
		this.ToolTip = Localization.Get(_value, false, null);
	}

	// Token: 0x06008B9F RID: 35743 RVA: 0x0034FF0A File Offset: 0x0034E10A
	[XuiXmlAttribute("disabled_tooltip_key", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeDisabledTooltipKey(string _value)
	{
		this.DisabledToolTip = Localization.Get(_value, false, null);
	}

	// Token: 0x06008BA0 RID: 35744 RVA: 0x0034FF1C File Offset: 0x0034E11C
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiView(XUi _xui, string _id)
	{
		this.xui = _xui;
		this.id = _id;
	}

	// Token: 0x06008BA1 RID: 35745 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void createComponents(GameObject _go)
	{
	}

	// Token: 0x06008BA2 RID: 35746 RVA: 0x0034FFE0 File Offset: 0x0034E1E0
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

	// Token: 0x06008BA3 RID: 35747 RVA: 0x003500B9 File Offset: 0x0034E2B9
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void captureComponents()
	{
		this.collider = this.uiTransform.gameObject.GetComponent<BoxCollider>();
	}

	// Token: 0x06008BA4 RID: 35748 RVA: 0x003500D4 File Offset: 0x0034E2D4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void addTweeners()
	{
		for (int i = 0; i < this.Tweeners.Count; i++)
		{
			this.Tweeners[i].CreateTween(this.uiTransform.gameObject);
		}
	}

	// Token: 0x06008BA5 RID: 35749 RVA: 0x00350114 File Offset: 0x0034E314
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

	// Token: 0x06008BA6 RID: 35750 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetRepeatContentTemplateParams(Dictionary<string, object> _templateParams, int _curRepeatNum)
	{
	}

	// Token: 0x06008BA7 RID: 35751 RVA: 0x0035036E File Offset: 0x0034E56E
	public virtual void Cleanup()
	{
		this.destroyed = true;
	}

	// Token: 0x06008BA8 RID: 35752 RVA: 0x00350378 File Offset: 0x0034E578
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

	// Token: 0x06008BA9 RID: 35753 RVA: 0x00350540 File Offset: 0x0034E740
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

	// Token: 0x06008BAA RID: 35754 RVA: 0x003505D0 File Offset: 0x0034E7D0
	public void TryUpdatePosition()
	{
		if (this.positionDirty)
		{
			this.uiTransform.localPosition = new Vector3((float)this.PaddedPosition.x, (float)this.PaddedPosition.y, this.uiTransform.localPosition.z);
		}
	}

	// Token: 0x06008BAB RID: 35755 RVA: 0x00350620 File Offset: 0x0034E820
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void refreshBoxCollider()
	{
		this.collider.center = new Vector3((float)this.size.x * 0.5f, 0f - (float)this.size.y * 0.5f, 0f);
		this.collider.size = new Vector3((float)this.size.x * this.ColliderScale + (float)(2 * this.ColliderPadding.x), (float)this.size.y * this.ColliderScale + (float)(2 * this.ColliderPadding.y), 0f);
	}

	// Token: 0x06008BAC RID: 35756 RVA: 0x003506C5 File Offset: 0x0034E8C5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetDirty()
	{
		this.isDirty = true;
	}

	// Token: 0x1700103A RID: 4154
	// (get) Token: 0x06008BAD RID: 35757 RVA: 0x003506CE File Offset: 0x0034E8CE
	// (set) Token: 0x06008BAE RID: 35758 RVA: 0x003506D6 File Offset: 0x0034E8D6
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

	// Token: 0x1700103B RID: 4155
	// (get) Token: 0x06008BAF RID: 35759 RVA: 0x003506F4 File Offset: 0x0034E8F4
	// (set) Token: 0x06008BB0 RID: 35760 RVA: 0x003506FC File Offset: 0x0034E8FC
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

	// Token: 0x1700103C RID: 4156
	// (get) Token: 0x06008BB1 RID: 35761 RVA: 0x0035071A File Offset: 0x0034E91A
	// (set) Token: 0x06008BB2 RID: 35762 RVA: 0x00350722 File Offset: 0x0034E922
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

	// Token: 0x1700103D RID: 4157
	// (get) Token: 0x06008BB3 RID: 35763 RVA: 0x00350740 File Offset: 0x0034E940
	// (set) Token: 0x06008BB4 RID: 35764 RVA: 0x00350748 File Offset: 0x0034E948
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

	// Token: 0x1700103E RID: 4158
	// (get) Token: 0x06008BB5 RID: 35765 RVA: 0x00350766 File Offset: 0x0034E966
	public bool HasAnchorStringLeft
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return !string.IsNullOrEmpty(this.anchorLeft);
		}
	}

	// Token: 0x1700103F RID: 4159
	// (get) Token: 0x06008BB6 RID: 35766 RVA: 0x00350776 File Offset: 0x0034E976
	public bool HasAnchorStringRight
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return !string.IsNullOrEmpty(this.anchorRight);
		}
	}

	// Token: 0x17001040 RID: 4160
	// (get) Token: 0x06008BB7 RID: 35767 RVA: 0x00350786 File Offset: 0x0034E986
	public bool HasAnchorStringTop
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return !string.IsNullOrEmpty(this.anchorTop);
		}
	}

	// Token: 0x17001041 RID: 4161
	// (get) Token: 0x06008BB8 RID: 35768 RVA: 0x00350796 File Offset: 0x0034E996
	public bool HasAnchorStringBottom
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return !string.IsNullOrEmpty(this.anchorBottom);
		}
	}

	// Token: 0x17001042 RID: 4162
	// (get) Token: 0x06008BB9 RID: 35769 RVA: 0x003507A6 File Offset: 0x0034E9A6
	public bool HasAnchorStringLeftAndRight
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.HasAnchorStringLeft && this.HasAnchorStringRight;
		}
	}

	// Token: 0x17001043 RID: 4163
	// (get) Token: 0x06008BBA RID: 35770 RVA: 0x003507B8 File Offset: 0x0034E9B8
	public bool HasAnchorStringTopAndBottom
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.HasAnchorStringTop && this.HasAnchorStringBottom;
		}
	}

	// Token: 0x06008BBB RID: 35771 RVA: 0x003507CC File Offset: 0x0034E9CC
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

	// Token: 0x06008BBC RID: 35772 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void anchorsParsed()
	{
	}

	// Token: 0x06008BBD RID: 35773 RVA: 0x003508BB File Offset: 0x0034EABB
	public virtual void OnOpen()
	{
		if (this.xuiSound != null && this.SoundPlayOnOpen)
		{
			Manager.PlayXUiSound(this.xuiSound, this.SoundVolume);
		}
		this.isPressed = false;
		this.isHold = false;
	}

	// Token: 0x06008BBE RID: 35774 RVA: 0x003508F4 File Offset: 0x0034EAF4
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

	// Token: 0x06008BBF RID: 35775 RVA: 0x00350969 File Offset: 0x0034EB69
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnScroll(GameObject _go, float _delta)
	{
		if (this.EventOnScroll)
		{
			this.controller.Scrolled(_delta);
		}
	}

	// Token: 0x06008BC0 RID: 35776 RVA: 0x0035097F File Offset: 0x0034EB7F
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSelect(GameObject _go, bool _selected)
	{
		if (this.EventOnSelect && this.enabled)
		{
			this.controller.Selected(_selected);
		}
	}

	// Token: 0x06008BC1 RID: 35777 RVA: 0x003509A0 File Offset: 0x0034EBA0
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

	// Token: 0x06008BC2 RID: 35778 RVA: 0x003509E0 File Offset: 0x0034EBE0
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

	// Token: 0x06008BC3 RID: 35779 RVA: 0x00350AB8 File Offset: 0x0034ECB8
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

	// Token: 0x06008BC4 RID: 35780 RVA: 0x00350B08 File Offset: 0x0034ED08
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

	// Token: 0x06008BC5 RID: 35781 RVA: 0x00350B64 File Offset: 0x0034ED64
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

	// Token: 0x06008BC6 RID: 35782 RVA: 0x00350BC0 File Offset: 0x0034EDC0
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

	// Token: 0x06008BC7 RID: 35783 RVA: 0x00350CDB File Offset: 0x0034EEDB
	public void PlayHoverSound()
	{
		if (this.xuiHoverSound != null && this.SoundPlayOnHover && this.enabled && GameManager.Instance.GameIsFocused)
		{
			Manager.PlayXUiSound(this.xuiHoverSound, this.SoundVolume);
		}
	}

	// Token: 0x06008BC8 RID: 35784 RVA: 0x00350D18 File Offset: 0x0034EF18
	public void PlayClickSound()
	{
		if (this.xuiSound != null)
		{
			Manager.PlayXUiSound(this.xuiSound, this.SoundVolume);
		}
	}

	// Token: 0x06008BC9 RID: 35785 RVA: 0x00350D39 File Offset: 0x0034EF39
	public Rect GetXUiRect()
	{
		return this.GetXUiRect(Vector2.zero);
	}

	// Token: 0x06008BCA RID: 35786 RVA: 0x00350D48 File Offset: 0x0034EF48
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

	// Token: 0x06008BCB RID: 35787 RVA: 0x00350E76 File Offset: 0x0034F076
	public string GetXuiHierarchy()
	{
		return XUiUtils.GetXuiHierarchy(this.Controller);
	}

	// Token: 0x06008BCC RID: 35788 RVA: 0x00350E83 File Offset: 0x0034F083
	public Vector2 GetClosestPoint(Vector3 _point)
	{
		return this.collider.ClosestPointOnBounds(_point);
	}

	// Token: 0x06008BCD RID: 35789 RVA: 0x00350E98 File Offset: 0x0034F098
	public void ClearNavigationTargets()
	{
		this.NavUpTarget = (this.NavDownTarget = (this.NavLeftTarget = (this.NavRightTarget = null)));
	}

	// Token: 0x06008BCE RID: 35790 RVA: 0x00350EC8 File Offset: 0x0034F0C8
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

	// Token: 0x06008BCF RID: 35791 RVA: 0x00350F54 File Offset: 0x0034F154
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

	// Token: 0x06008BD0 RID: 35792 RVA: 0x00350FE0 File Offset: 0x0034F1E0
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

	// Token: 0x06008BD1 RID: 35793 RVA: 0x00351088 File Offset: 0x0034F288
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

	// Token: 0x06008BD2 RID: 35794 RVA: 0x003510E4 File Offset: 0x0034F2E4
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

	// Token: 0x06008BD3 RID: 35795 RVA: 0x0035116B File Offset: 0x0034F36B
	public virtual void OnVisibilityChanged(bool _visibleInScene)
	{
		if (this.isOver && !_visibleInScene)
		{
			this.OnHover(false);
		}
	}

	// Token: 0x06008BD8 RID: 35800 RVA: 0x003511A8 File Offset: 0x0034F3A8
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

	// Token: 0x06008BD9 RID: 35801 RVA: 0x003513C4 File Offset: 0x0034F5C4
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

	// Token: 0x040066AF RID: 26287
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<Type, GameObject> componentTemplates = new Dictionary<Type, GameObject>();

	// Token: 0x040066B0 RID: 26288
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform templatesParent;

	// Token: 0x040066B1 RID: 26289
	public readonly XUi xui;

	// Token: 0x040066B2 RID: 26290
	[PublicizedFrom(EAccessModifier.Private)]
	public bool destroyed;

	// Token: 0x040066B3 RID: 26291
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController controller;

	// Token: 0x040066B4 RID: 26292
	[PublicizedFrom(EAccessModifier.Protected)]
	public Transform uiTransform;

	// Token: 0x040066B5 RID: 26293
	[PublicizedFrom(EAccessModifier.Protected)]
	public BoxCollider collider;

	// Token: 0x040066B6 RID: 26294
	public readonly List<XUiTweenAbs> Tweeners = new List<XUiTweenAbs>();

	// Token: 0x040066B9 RID: 26297
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDirty;

	// Token: 0x040066BA RID: 26298
	[PublicizedFrom(EAccessModifier.Protected)]
	public string id;

	// Token: 0x040066BB RID: 26299
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isVisible = true;

	// Token: 0x040066BC RID: 26300
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool enabled = true;

	// Token: 0x040066BF RID: 26303
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector2i size;

	// Token: 0x040066C0 RID: 26304
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector2i position;

	// Token: 0x040066C1 RID: 26305
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool positionDirty;

	// Token: 0x040066C2 RID: 26306
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool ignoreParentPadding;

	// Token: 0x040066C3 RID: 26307
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiSideSizes padding;

	// Token: 0x040066C4 RID: 26308
	[PublicizedFrom(EAccessModifier.Protected)]
	public float rotation;

	// Token: 0x040066C5 RID: 26309
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool rotationDirty;

	// Token: 0x040066C6 RID: 26310
	[PublicizedFrom(EAccessModifier.Protected)]
	public int depth;

	// Token: 0x040066C7 RID: 26311
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip xuiSound;

	// Token: 0x040066C8 RID: 26312
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip xuiHoverSound;

	// Token: 0x040066D1 RID: 26321
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eventOnHover;

	// Token: 0x040066D2 RID: 26322
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eventOnPress;

	// Token: 0x040066D3 RID: 26323
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eventOnDoubleClick;

	// Token: 0x040066D4 RID: 26324
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eventOnHeld;

	// Token: 0x040066D5 RID: 26325
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eventOnScroll;

	// Token: 0x040066D6 RID: 26326
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eventOnDrag;

	// Token: 0x040066D7 RID: 26327
	[PublicizedFrom(EAccessModifier.Private)]
	public bool eventOnSelect;

	// Token: 0x040066D8 RID: 26328
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isNavigatable = true;

	// Token: 0x040066DB RID: 26331
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool gamepadSelectableSetFromAttributes;

	// Token: 0x040066DC RID: 26332
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool gamepadSnappableSetFromAttributes;

	// Token: 0x040066DD RID: 26333
	[PublicizedFrom(EAccessModifier.Private)]
	public string navUpTargetString;

	// Token: 0x040066DE RID: 26334
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView navUpTarget;

	// Token: 0x040066DF RID: 26335
	[PublicizedFrom(EAccessModifier.Private)]
	public string navDownTargetString;

	// Token: 0x040066E0 RID: 26336
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView navDownTarget;

	// Token: 0x040066E1 RID: 26337
	[PublicizedFrom(EAccessModifier.Private)]
	public string navLeftTargetString;

	// Token: 0x040066E2 RID: 26338
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView navLeftTarget;

	// Token: 0x040066E3 RID: 26339
	[PublicizedFrom(EAccessModifier.Private)]
	public string navRightTargetString;

	// Token: 0x040066E4 RID: 26340
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView navRightTarget;

	// Token: 0x040066E5 RID: 26341
	[PublicizedFrom(EAccessModifier.Private)]
	public string toolTip;

	// Token: 0x040066E6 RID: 26342
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledToolTip;

	// Token: 0x040066E7 RID: 26343
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmUpdateHover = new ProfilerMarker("XV.U-Hover");

	// Token: 0x040066E8 RID: 26344
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmUpdatePress = new ProfilerMarker("XV.U-Press");

	// Token: 0x040066E9 RID: 26345
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmUpdateData = new ProfilerMarker("XV.U-UpdateData");

	// Token: 0x040066EA RID: 26346
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorLeft;

	// Token: 0x040066EB RID: 26347
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorRight;

	// Token: 0x040066EC RID: 26348
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorBottom;

	// Token: 0x040066ED RID: 26349
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorTop;

	// Token: 0x040066EE RID: 26350
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorLeftParsed;

	// Token: 0x040066EF RID: 26351
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorRightParsed;

	// Token: 0x040066F0 RID: 26352
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorBottomParsed;

	// Token: 0x040066F1 RID: 26353
	[PublicizedFrom(EAccessModifier.Private)]
	public string anchorTopParsed;

	// Token: 0x040066F2 RID: 26354
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool anchoredLeftAndRight;

	// Token: 0x040066F3 RID: 26355
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool anchoredTopAndBottom;

	// Token: 0x040066F4 RID: 26356
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasDragging;

	// Token: 0x040066F5 RID: 26357
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPressed;

	// Token: 0x040066F6 RID: 26358
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHold;

	// Token: 0x040066F7 RID: 26359
	[PublicizedFrom(EAccessModifier.Private)]
	public float pressStartTime;

	// Token: 0x040066F8 RID: 26360
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdStartTime;

	// Token: 0x040066F9 RID: 26361
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdEventIntervalCurrent;

	// Token: 0x040066FA RID: 26362
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdEventIntervalChangeSpeed;

	// Token: 0x040066FB RID: 26363
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdEventNextTime;

	// Token: 0x040066FC RID: 26364
	[PublicizedFrom(EAccessModifier.Private)]
	public float holdEventLastTime;

	// Token: 0x040066FD RID: 26365
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isOver;
}
