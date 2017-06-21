// winifix@gmail.com
// ReSharper disable UnusedMember.Global
// ReSharper disable ConvertPropertyToExpressionBody

using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using CloudMagic.Helpers;
using CloudMagic.GUI;

namespace CloudMagic.Rotation
{
    public class ArmsWarriorLe : CombatRoutine
    {
        public override string Name
        {
            get { return "Arms Warrior"; }
        }

        public override string Class
        {
            get { return "Warrior"; }
        }

        public override Form SettingsForm { get; set; }
		
		private readonly Stopwatch Openingwatch = new Stopwatch();

        public override void Initialize()
        {
            Log.Write("Welcome to Arms Warrior", Color.Green);
            Log.Write("Suggested build: 1332311", Color.Green);
        }
        public override int SINGLE 
		{
			get 
			{ 
				return 1; 
			} 
		}
		public override int CLEAVE 
		{ 
			get 
			{ 
				return 99;
			} 
		}
        public override int AOE 
		{ 
			get 
			{ 
				return 3; 
			} 
		}
        public override void Stop()
        {
        }

        public override void Pulse()
        {
            if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat && WoW.TargetIsVisible && !WoW.IsMounted)
            {
                if (!WoW.IsSpellOnCooldown("Diebythesword") && WoW.HealthPercent < 20 && !WoW.IsSpellOnCooldown("Diebythesword"))
                {
                    WoW.CastSpell("Diebythesword");
					return;
                }
				if (WoW.IsSpellInRange("Mortal Strike") && WoW.HealthPercent <= 70 && WoW.PlayerHasBuff("Free Siegensrausch"))
				{
					WoW.CastSpell("Siegensrausch");
					return;
				}
                if (WoW.CanCast("Pummel") && WoW.IsSpellInRange("Mortal Strike") && WoW.TargetIsCastingAndSpellIsInterruptible && WoW.TargetPercentCast >= 60 && !WoW.IsSpellOnCooldown("Pummel") && !WoW.PlayerIsChanneling)
                {
                    WoW.CastSpell("Pummel");						
                    return;
                }
				//Cooldowns
				if (WoW.IsSpellInRange("Mortal Strike") && WoW.CanCast("Battle Cry") && !WoW.IsSpellOnCooldown("Battle Cry") && WoW.TargetHasDebuff("Colossus Smash"))
				{
					WoW.CastSpell("Battle Cry");
					return;
				}
				if (WoW.CanCast("Bladestorm") && UseCooldowns && !WoW.PlayerHasBuff("Battle Cry") && WoW.IsSpellInRange("Mortal Strike"))
				{
					WoW.CastSpell("Bladestorm");
					return;
				}
				/*if (WoW.IsSpellInRange("Mortal Strike") && WoW.CanCast("Trinket") &&!WoW.ItemOnCooldown("Trinket") && WoW.TargetHasDebuff("Colossus Smash") && WoW.PlayerHasBuff("Battle Cry"))
				{
					WoW.CastSpell("Trinket");
					return;
				}*/
				if (WoW.CanCast("Blood Fury") && WoW.PlayerHasBuff("Battle Cry") && !WoW.IsSpellOnCooldown ("Blood Fury") && WoW.PlayerRace == "Orc" && UseCooldowns)
                {
					WoW.CastSpell("Blood Fury");
                    return;
                }
				if (combatRoutine.Type == RotationType.SingleTarget || combatRoutine.Type == RotationType.SingleTargetCleave)
				{
					//Colossus smash control
					if (WoW.CanCast("Colossus Smash") && !WoW.TargetHasDebuff("Colossus Smash") && WoW.IsSpellInRange("Mortal Strike"))
					{
						WoW.CastSpell("Colossus Smash");
						return;
					}
					if (WoW.CanCast("Warbreaker") && !WoW.TargetHasDebuff("Colossus Smash") && !WoW.PlayerHasBuff("Shattered Defenses") && WoW.IsSpellInRange("Mortal Strike"))
					{
						WoW.CastSpell("Warbreaker");
						return;
					}														
					if (WoW.CanCast("Colossus Smash") && !WoW.PlayerHasBuff("Shattered Defenses") && !WoW.IsSpellOnCooldown("Colossus Smash") && WoW.IsSpellInRange("Mortal Strike"))
					{
						WoW.CastSpell("Colossus Smash");
						return;
					}
					if (WoW.CanCast("Mortal Strike") && WoW.Rage >= 20 && WoW.PlayerHasBuff("Shattered Defenses") && WoW.TargetDebuffStacks ("Executioner's Precision") == 2 && WoW.IsSpellInRange("Mortal Strike"))
					{
						WoW.CastSpell("Mortal Strike");
						return;
					}
					if (WoW.CanCast("Execute") && WoW.Rage >= 10 && WoW.TargetHealthPercent <= 20 && WoW.IsSpellInRange("Mortal Strike"))
					{
						WoW.CastSpell("Execute");
						return;
					}
					if (WoW.CanCast("Colossus Smash") && WoW.SpellCooldownTimeRemaining("Battle Cry") < 100 && WoW.IsSpellInRange("Mortal Strike"))
					{
						WoW.CastSpell("Colossus Smash");
						return;
					}
					if (WoW.CanCast("Mortal Strike") && WoW.Rage >= 20 && !WoW.IsSpellOnCooldown("Mortal Strike") && WoW.IsSpellInRange("Mortal Strike"))
					{
						WoW.CastSpell("Mortal Strike");
						return;
					}
					if (WoW.CanCast("Whirlwind") && WoW.Rage >= 30 && !WoW.CanCast("Colossus Smash") && !WoW.CanCast("Mortal Strike") && WoW.IsSpellInRange("Mortal Strike"))
					{
						WoW.CastSpell("Whirlwind");
						return;
					}
				}	
				if (combatRoutine.Type == RotationType.AOE)
				{
					if (WoW.CanCast("Colossus Smash") && !WoW.TargetHasDebuff("Colossus Smash") && WoW.IsSpellInRange("Mortal Strike"))
					{
						WoW.CastSpell("Colossus Smash");
						return;
					}
					if (WoW.CanCast("Execute") && WoW.Rage >= 10 && WoW.TargetHealthPercent <= 20 && WoW.IsSpellInRange("Mortal Strike"))
					{
						WoW.CastSpell("Execute");
						return;
					}
					if (WoW.CanCast("Warbreaker") && !WoW.TargetHasDebuff("Colossus Smash") && !WoW.PlayerHasBuff("Shattered Defenses") && WoW.IsSpellInRange("Mortal Strike"))
					{
						WoW.CastSpell("Warbreaker");
						return;
					}
					if (WoW.CanCast("Cleave") && WoW.Rage >= 10 && !WoW.PlayerHasBuff("Cleave") && !WoW.IsSpellOnCooldown("Cleave") && WoW.IsSpellInRange("Mortal Strike"))
					{
						WoW.CastSpell("Cleave");
						return;
					}
					if (WoW.CanCast("Whirlwind") && WoW.Rage > 30 && WoW.IsSpellOnCooldown("Cleave") && WoW.IsSpellInRange("Mortal Strike"))
					{
						WoW.CastSpell("Whirlwind");
						return;
					}
					if (WoW.CanCast("Mortal Strike") && WoW.Rage > 20 && WoW.TargetHasDebuff("Colossus Smash") && WoW.IsSpellOnCooldown("Cleave") && WoW.IsSpellInRange("Mortal Strike"))
					{
						WoW.CastSpell("Mortal Strike");
						return;
					}
				}
			}
		}
	}
}
/*
[AddonDetails.db]
AddonAuthor=smartie
AddonName=smartie
WoWVersion=Legion - 70200
[SpellBook.db]
Spell,12294,Mortal Strike,D1
Spell,167105,Colossus Smash,D2
Spell,1464,Slam,D3
Spell,1719,Battle Cry,F2
Spell,163201,Execute,D5
Spell,100,Charge,E
Spell,209577,Warbreaker,F1
Spell,845,Cleave,F3
Spell,1680,Whirlwind,D8
Spell,227847,Bladestorm,D9
Spell,34428,Siegensrausch,F3
Spell,118038,Diebythesword,F2
Spell,6552,Pummel,F11
Spell,133585,Trinket,D0
Spell,20572,Blood Fury,NumPad1
Aura,227847,Bladestorm
Aura,107574,Avatar
Aura,208086,Colossus Smash
Aura,209706,Shattered Defenses
Aura,1719,Battle Cry
Aura,845,Cleave
Aura,32216,Free Siegensrausch
Aura,242188,Executioner's Precision
Item,133585,Trinket
*/
