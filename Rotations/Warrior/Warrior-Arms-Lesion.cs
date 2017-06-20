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
					if (WoW.CanCast("Colossus Smash") && !WoW.TargetHasDebuff("Colossus Smash"))
					{
						WoW.CastSpell("Colossus Smash");
						return;
					}
					if (WoW.CanCast("Warbreaker") && !WoW.TargetHasDebuff("Colossus Smash") && !WoW.PlayerHasBuff("Shattered Defenses") && WoW.IsSpellInRange("Mortal Strike"))
					{
						WoW.CastSpell("Warbreaker");
						return;
					}				
					if (WoW.IsSpellInRange("Mortal Strike") && WoW.CanCast("Avatar") && !WoW.IsSpellOnCooldown("Avatar") && WoW.TargetHasDebuff("Colossus Smash") && WoW.Talent (3) == 3)
					{
						WoW.CastSpell("Avatar");
						return;
					}						
					//Slam control
					if (WoW.IsSpellInRange("Mortal Strike") && WoW.CanCast("Slam") && !WoW.CanCast("Colossus Smash") && !WoW.CanCast("Mortal Strike") && WoW.PlayerHasBuff("Shattered Defenses"))
					{
						WoW.CastSpell("Slam");
						return;
					}				
					//Normal ST rotation
					if (!WoW.PlayerHasBuff("Battle Cry") && WoW.IsSpellInRange("Mortal Strike"))
					{					
						if (WoW.TargetHealthPercent >  20)
                        //When targets are above 20%. Not in Execute phase.
						{				
							//CS Control
							if (WoW.CanCast("Colossus Smash") && !WoW.PlayerHasBuff("Shattered Defenses") && !WoW.IsSpellOnCooldown("Colossus Smash") || WoW.IsSpellOverlayed("Colossus Smash"))
							{
								WoW.CastSpell("Colossus Smash");
								return;
							}
							if (WoW.CanCast("Colossus Smash") && WoW.SpellCooldownTimeRemaining("Battle Cry") < 100)
							{
								WoW.CastSpell("Colossus Smash");
								return;
							}
							//MS 
							if (WoW.CanCast("Mortal Strike") && !WoW.IsSpellOnCooldown("Mortal Strike") || WoW.IsSpellOverlayed("Mortal Strike"))
							{
								WoW.CastSpell("Mortal Strike");
								return;
							}
							//Slam
							if (WoW.CanCast("Slam") && WoW.Rage >= 32 && !WoW.CanCast("Colossus Smash") && !WoW.CanCast("Mortal Strike"))
							{
								WoW.CastSpell("Slam");
								return;
							}
						}
						if (WoW.TargetHealthPercent < 20)
						{
							//Non BC ST
							if (WoW.CanCast("Colossus Smash") && !WoW.IsSpellOnCooldown("Colossus Smash") && !WoW.PlayerHasBuff("Shattered Defenses"))
							{
								WoW.CastSpell("Colossus Smash");
								return;
							}                 
							if (WoW.CanCast("Execute") && WoW.Rage >= 18 && WoW.PlayerHasBuff("Shattered Defenses"))
							{
								WoW.CastSpell("Execute");
								return;
							}
							if (WoW.CanCast("Execute") && !WoW.PlayerHasBuff("Shattered Defenses"))
							{
								WoW.CastSpell("Execute");
								return;
							}
						}
					}
					if (WoW.PlayerHasBuff("Battle Cry") && WoW.IsSpellInRange("Mortal Strike"))
					{
						if (WoW.CanCast("Avatar") && WoW.PlayerHasBuff("Battle Cry") && WoW.IsSpellOnCooldown("Battle Cry") && WoW.Talent (3) == 3)
						{
							WoW.CastSpell("Avatar");
							return;
						}
						if (WoW.TargetHealthPercent > 20)
						{               
							//CS on cooldown but not overlapping SD
							if (WoW.CanCast("Colossus Smash") && !WoW.IsSpellOnCooldown("Colossus Smash") && !WoW.PlayerHasBuff("Shattered Defenses"))
							{
								WoW.CastSpell("Colossus Smash");
								return;
							}
							//MS with SD
							if (WoW.CanCast("Mortal Strike") && WoW.PlayerHasBuff("Shattered Defenses") && !WoW.IsSpellOnCooldown("Mortal Strike"))
							{
								WoW.CastSpell("Mortal Strike");
								return;
							}
							//MS on cooldown
							if (WoW.CanCast("Mortal Strike") && !WoW.IsSpellOnCooldown("Mortal Strike") && WoW.IsSpellOnCooldown("Colossus Smash"))
							{
								WoW.CastSpell("Mortal Strike");
							}
							//if all else fails, slam.
							if (WoW.CanCast("Slam") && WoW.IsSpellOnCooldown("Colossus Smash") && WoW.IsSpellOnCooldown("Mortal Strike"))
							{
								WoW.CastSpell("Slam");
								return;
							}
						}
						if (WoW.TargetHealthPercent <= 20)
						{
							if (WoW.CanCast("Colossus Smash") && !WoW.TargetHasDebuff("Colossus Smash") && !WoW.IsSpellOnCooldown("Colossus Smash"))
							{
								WoW.CastSpell("Colossus Smash");
								return;
							}
							if (WoW.CanCast("Execute") && !WoW.CanCast("Mortal Strike"))
							{
								WoW.CastSpell("Execute");
								return;
							}
						}
					}	
				}
				if (combatRoutine.Type == RotationType.AOE)
				{
					if (WoW.TargetHealthPercent > 20)
					{
						if (WoW.CanCast("Bladestorm") && WoW.CountEnemyNPCsInRange > 8 && UseCooldowns)
						{
							WoW.CastSpell("Bladestorm");
							return;
						}
						if (WoW.CanCast("Colossus Smash") && !WoW.TargetHasDebuff("Colossus Smash"))
						{
							WoW.CastSpell("Colossus Smash");
							return;
						}
						if (WoW.CanCast("Warbreaker") && !WoW.TargetHasDebuff("Colossus Smash") && !WoW.PlayerHasBuff("Shattered Defenses") && WoW.IsSpellInRange("Mortal Strike"))
						{
							WoW.CastSpell("Warbreaker");
							return;
						}
						if (WoW.CanCast("Cleave") && !WoW.PlayerHasBuff("Cleave") && !WoW.IsSpellOnCooldown("Cleave"))
						{
							WoW.CastSpell("Cleave");
							return;
						}
						if (WoW.CanCast("Whirlwind") && WoW.Rage > 30 && WoW.IsSpellOnCooldown("Cleave"))
						{
							WoW.CastSpell("Whirlwind");
							return;
						}
						if (WoW.CanCast("Mortal Strike") && WoW.Rage > 28 && WoW.TargetHasDebuff("Colossus Smash") && WoW.IsSpellOnCooldown("Cleave"))
						{
							WoW.CastSpell("Mortal Strike");
							return;
						}
					}
					if (WoW.TargetHealthPercent <= 20)
					{
						if (WoW.CanCast("Colossus Smash") && !WoW.IsSpellOnCooldown("Colossus Smash") && !WoW.PlayerHasBuff("Shattered Defenses"))
						{
							WoW.CastSpell("Colossus Smash");
							return;
						}                 
						if (WoW.CanCast("Execute") && WoW.Rage >= 18 && WoW.PlayerHasBuff("Shattered Defenses"))
						{
							WoW.CastSpell("Execute");
							return;
						}
						if (WoW.CanCast("Execute") && !WoW.PlayerHasBuff("Shattered Defenses"))
						{
							WoW.CastSpell("Execute");
							return;
						}
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
Spell,107574,Avatar,F4
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
Item,133585,Trinket
*/
