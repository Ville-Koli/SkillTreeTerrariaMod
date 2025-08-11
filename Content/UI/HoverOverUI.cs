using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Runeforge.Content.SkillTree;
using ReLogic.Content;
using Terraria;

namespace Runeforge.Content.UI
{
	public class HoverOverUI : UIElement
	{
		public UIImage hoverOverUIImage;
		public Asset<Texture2D> activeConnectionImage;
		public Asset<Texture2D> inActiveConnectionImage;
		public Asset<Texture2D> transparentImage;
		public UIText description;
		public bool active;
		public HoverOverUI(TextureManager textureManager)
		{
			transparentImage = textureManager.transparentBox;
			(Asset<Texture2D> active, Asset<Texture2D> inactive) asset = textureManager.GetUI(UIType.HoverOver);
			hoverOverUIImage = new UIImage(transparentImage);
			description = new UIText("test", 0.7f); // DO NOT SET "test" to "" when changing font size, if font size is 1 then it can be ""
			inActiveConnectionImage = asset.inactive;
			activeConnectionImage = asset.active;
		}

		/**
		<summary>
		Changes the description of the hover over ui
		</summary>

		<returns> returns a boolean if new description was set or not  </returns>
		**/
		public bool ChangeDescription(string newDescription)
		{
			if (description != null)
			{
				description.SetText(newDescription);
				return true;
			}
			return false;
		}

		/**
		<summary>
		Changes hover over ui to be active
		</summary>
		**/
		public void SetActive()
		{
			hoverOverUIImage.SetImage(activeConnectionImage);
			active = true;
		}

		/**
		<summary>
		Changes hover over ui to be inactive
		</summary>
		**/
		public void SetInActive()
		{
			hoverOverUIImage.SetImage(inActiveConnectionImage);
			active = false;
		}

		/**
		<summary>
		Changes hover over ui to be invisible
		</summary>
		**/
		public void SetTransparent()
		{
			hoverOverUIImage.SetImage(transparentImage);
		}

		/**
		<summary>
		Initializes the hover over ui element
		</summary>
		**/
		public override void OnInitialize()
		{
			base.OnInitialize();
			Width.Set(inActiveConnectionImage.Width(), 0f);
			Height.Set(inActiveConnectionImage.Height(), 0f);
			description.PaddingTop = 15;
			description.PaddingLeft = 15;
			description.SetText("");
			Append(hoverOverUIImage);
			hoverOverUIImage.Append(description);
		}

		/**
		<summary>
		Changes hover over ui element in-game tick while ui is open
		</summary>
		**/
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

        public override int GetHashCode()
        {
			return hoverOverUIImage.GetHashCode();
        }
	}
}