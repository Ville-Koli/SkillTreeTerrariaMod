using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Runeforge.Content.Items.Accessories;
using Runeforge.Content.UI;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Runeforge.Content.Buffs
{
	public class PoisonImbuementApplier : ModPlayer
	{
		public bool ApplyPoison = false;

		public override void ResetEffects()
		{
			base.ResetEffects();
			ApplyPoison = false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			base.OnHitNPC(target, hit, damageDone);
			if (ApplyPoison)
			{
				target.AddBuff(BuffID.Poisoned, 300);
			}
		}
	}		
}