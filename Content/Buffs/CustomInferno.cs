using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Runeforge.Content.Items.Accessories;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Runeforge.Content.Buffs
{
	// This is a basic item template.
	// Please see tModLoader's ExampleMod for every other example:
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
	public class CustomInferno : ModBuff
	{
		public int radius = 150;
		private Player player;
		public override void Update(Player player, ref int buffIndex)
		{
			base.Update(player, ref buffIndex);
			this.player = player;
		}
		public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams)
		{
			if (player == null) return;
			Texture2D texture = ModContent.Request<Texture2D>("Terraria/Images/Projectile_400").Value;
			Vector2 pos = player.Center.ToScreenPosition();
			
			// Optional animation
			float rotation = (float)Main.timeForVisualEffects * 0.05f;

			// Draw multiple flames around a circle
			int segments = 30;

			for (int i = 0; i < segments; i++)
			{
				float angle = MathF.PI * 2 * i / segments + rotation;
				Vector2 offset = radius * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
				Vector2 drawPos = pos + offset;

				spriteBatch.Draw(
					texture,
					drawPos,
					null,
					Color.OrangeRed * 0.8f,
					angle,
					texture.Size() / 2f,
					1f,
					SpriteEffects.None,
					0f
				);
			}
		}
	}		
}