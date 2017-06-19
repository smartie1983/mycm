using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using CloudMagic.Helpers;

namespace CloudMagic.Rotation
{
    public class WarlockAffliction : CombatRoutine
    {
        public override string Name
        {
			get
			{
				return "Affliction Warlock";
			}
		}

        public override string Class
        {
			get
			{
				return "Warlock";
			}
		}

        public override Form SettingsForm { get; set; }
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
        public override void Initialize()
        {
            Log.Write("Welcome to Affliction Warlock", Color.Purple);
		}

        public override void Stop()
        {
        }

        public override void Pulse() // Updated for Legion (tested and working for single target)
        {
            if (WoW.CanCast("Healthstone") && WoW.ItemCount("Healthstone") >= 1 && !WoW.ItemOnCooldown("Healthstone") && WoW.HealthPercent <= 60 && WoW.HealthPercent != 0 && WoW.IsInCombat)
            {
                WoW.CastSpell("Healthstone");
                return;
            }
            if (combatRoutine.Type == RotationType.SingleTarget) // Do Single Target Stuff here
            {
                if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat && !WoW.PlayerIsCasting && !WoW.IsMounted)
                {                   
                    if ((!WoW.TargetHasDebuff("Agony") || WoW.TargetDebuffTimeRemaining("Agony") <= 540)
                        && (!WoW.PlayerIsChanneling || WoW.TargetDebuffTimeRemaining("Agony") <= 150)
                        && WoW.CanCast("Agony")
                        && WoW.IsSpellInRange("Agony"))
                    {
                        WoW.CastSpell("Agony");
                        return;
                    }
                    if ((WoW.CurrentSoulShards >= 3 || (WoW.CurrentSoulShards >= 2 && WoW.WasLastCasted("Unstable Affliction")))
                        && !WoW.IsMoving
                        && WoW.CanCast("Unstable Affliction")
                        && WoW.IsSpellInRange("Agony") && WoW.TargetHasDebuff("Agony") && WoW.TargetHasDebuff("Corruption"))
                    {
                        WoW.CastSpell("Unstable Affliction");
                        Thread.Sleep(200);
                        return;
                    }
                    if ((WoW.TargetHasDebuff("Unstable Affliction1") && WoW.TargetHasDebuff("Unstable Affliction2")
                        || WoW.TargetHasDebuff("Unstable Affliction1") && WoW.TargetHasDebuff("Unstable Affliction3")
                        || WoW.TargetHasDebuff("Unstable Affliction1") && WoW.TargetHasDebuff("Unstable Affliction4")
                        || WoW.TargetHasDebuff("Unstable Affliction1") && WoW.TargetHasDebuff("Unstable Affliction5")
                        || WoW.TargetHasDebuff("Unstable Affliction2") && WoW.TargetHasDebuff("Unstable Affliction3")
                        || WoW.TargetHasDebuff("Unstable Affliction2") && WoW.TargetHasDebuff("Unstable Affliction4")
                        || WoW.TargetHasDebuff("Unstable Affliction2") && WoW.TargetHasDebuff("Unstable Affliction5")
                        || WoW.TargetHasDebuff("Unstable Affliction3") && WoW.TargetHasDebuff("Unstable Affliction4")
                        || WoW.TargetHasDebuff("Unstable Affliction3") && WoW.TargetHasDebuff("Unstable Affliction5")
                        || WoW.TargetHasDebuff("Unstable Affliction4") && WoW.TargetHasDebuff("Unstable Affliction5")
                        || WoW.PlayerBuffStacks("Reap Souls") >= 12)
                        && !WoW.PlayerIsCasting
                        && WoW.CanCast("Reap Souls")
                        && !WoW.PlayerHasBuff("Deadwind Harvester")
                        && WoW.PlayerHasBuff("Tormented Souls") && WoW.TargetHasDebuff("Agony") && WoW.TargetHasDebuff("Corruption"))
                    {
                        WoW.CastSpell("Reap Souls");
                        return;
                    }
                    if (WoW.CanCast("Phantom") && WoW.IsSpellInRange("Agony") && !WoW.PlayerIsChanneling && !WoW.PlayerIsCasting && !WoW.IsMoving && WoW.TargetHasDebuff("Agony") && WoW.TargetHasDebuff("Corruption"))
                    {
                        WoW.CastSpell("Phantom");
                        return;
                    }
					if (WoW.IsSpellInRange("Agony") && WoW.CanCast("Trinket") &&!WoW.ItemOnCooldown("Trinket") && WoW.TargetHasDebuff("Agony") && WoW.TargetHasDebuff("Corruption"))
					{
						WoW.CastSpell("Trinket");
						return;
					}
					if (WoW.CanCast("Berserk") && UseCooldowns && !WoW.IsSpellOnCooldown ("Berserk") && WoW.PlayerRace == "Troll")
                    {
                        WoW.CastSpell("Berserk");
                        return;
                    }
                    if (WoW.CanCast("Life Tap") && !WoW.PlayerIsChanneling && WoW.Talent(2) == 3 && !WoW.PlayerHasBuff("Empowered Life Tap"))
                    {
                        WoW.CastSpell("Life Tap");
                        return;
                    }
                    if ((!WoW.TargetHasDebuff("Corruption") || WoW.TargetDebuffTimeRemaining("Corruption") <= 420)
                        && (!WoW.PlayerIsChanneling || WoW.TargetDebuffTimeRemaining("Corruption") <= 150)
                        && WoW.CanCast("Corruption")
                        && WoW.IsSpellInRange("Agony"))
                    {
                        WoW.CastSpell("Corruption");
                        return;
                    }
                    if ((!WoW.TargetHasDebuff("Siphon Life") || WoW.TargetDebuffTimeRemaining("Siphon Life") <= 420)
                        && (!WoW.PlayerIsChanneling || WoW.TargetDebuffTimeRemaining("Siphon Life") <= 150)
                        && WoW.Talent(4) == 1
                        && WoW.CanCast("Siphon Life")
                        && WoW.IsSpellInRange("Agony") && WoW.TargetHasDebuff("Agony") && WoW.TargetHasDebuff("Corruption"))
                    {
                        WoW.CastSpell("Siphon Life");
                        return;
                    }
                    if (WoW.CanCast("Unstable Affliction") && WoW.TargetHasDebuff("Agony") && WoW.TargetHasDebuff("Corruption") && !WoW.IsMoving && WoW.Talent(2) == 1 && !WoW.IsMoving && WoW.IsSpellInRange("Unstable Affliction") && !WoW.PlayerIsChanneling && WoW.CurrentSoulShards >= 1
                        && (!WoW.TargetHasDebuff("Unstable Affliction1") || !WoW.TargetHasDebuff("Unstable Affliction2") || !WoW.TargetHasDebuff("Unstable Affliction3") || !WoW.TargetHasDebuff("Unstable Affliction4") || !WoW.TargetHasDebuff("Unstable Affliction5")
                        || (WoW.TargetDebuffTimeRemaining("Unstable Affliction1") <= 150) || (WoW.TargetDebuffTimeRemaining("Unstable Affliction2") <= 150) || (WoW.TargetDebuffTimeRemaining("Unstable Affliction3") <= 150)
                        || (WoW.TargetDebuffTimeRemaining("Unstable Affliction4") <= 150) || (WoW.TargetDebuffTimeRemaining("Unstable Affliction5") <= 150)))
                    {
                        WoW.CastSpell("Unstable Affliction");
                        Thread.Sleep(200);
                        return;
                    }
                    if (WoW.IsInCombat && WoW.Mana < 70 && WoW.HealthPercent > 70 && WoW.CanCast("Life Tap"))
                    {
                        WoW.CastSpell("Life Tap");
                        return;
                    }
                    if (WoW.CanCast("Drain Soul") && WoW.IsSpellInRange("Agony") && !WoW.PlayerIsChanneling && !WoW.PlayerIsCasting && !WoW.IsMoving && WoW.TargetHasDebuff("Agony") && WoW.TargetHasDebuff("Corruption"))
                    {
                        WoW.CastSpell("Drain Soul");
                        return;
                    }
                }
            }
            if (combatRoutine.Type == RotationType.AOE)
            {
                if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat && !WoW.PlayerIsChanneling && !WoW.PlayerIsCasting && !WoW.IsMounted) // Do AOE stuff here
                {
                    if (WoW.CanCast("Agony") && WoW.IsSpellInRange("Agony") && WoW.TargetHasDebuff("Seed of Corruption") && (!WoW.TargetHasDebuff("Agony") || (WoW.TargetDebuffTimeRemaining("Agony") <= 540)))
                    {
                        WoW.CastSpell("Agony");
                        return;
                    }

                    if (WoW.CanCast("Corruption") && WoW.IsSpellInRange("Agony") && WoW.TargetHasDebuff("Seed of Corruption") && (!WoW.TargetHasDebuff("Corruption") || (WoW.TargetDebuffTimeRemaining("Corruption") <= 420)))
                    {
                        WoW.CastSpell("Corruption");
                        return;
                    }
                    if (WoW.CanCast("Seed of Corruption") && WoW.IsSpellInRange("Agony") && !WoW.TargetHasDebuff("Seed of Corruption") && !WoW.IsMoving && WoW.CurrentSoulShards >= 1)
                    {
                        WoW.CastSpell("Seed of Corruption");
                        return;
                    }
                    if (WoW.CanCast("Drain Soul") && WoW.IsSpellInRange("Agony") && !WoW.PlayerIsChanneling && !WoW.PlayerIsCasting && !WoW.IsMoving && WoW.TargetHasDebuff("Agony") && WoW.TargetHasDebuff("Corruption"))
                    {
                        WoW.CastSpell("Drain Soul");
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
Spell,980,Agony,NumPad1
Spell,63106,Siphon Life,NumPad2
Spell,172,Corruption,NumPad3
Spell,30108,Unstable Affliction,NumPad4
Spell,216698,Reap Souls,NumPad5
Spell,1454,Life Tap,NumPad7
Spell,198590,Drain Soul,Add
Spell,27243,Seed of Corruption,NumPad0
Spell,5512,Healthstone,NumPad2
Spell,205179,Phantom,D9
Spell,133585,Trinket,D8
Spell,26297,Berserk,D0
Aura,980,Agony
Aura,216698,Reap Souls
Aura,27243,Seed of Corruption
Aura,146739,Corruption
Aura,63106,Siphon Life
Aura,233490,Unstable Affliction1
Aura,233496,Unstable Affliction2
Aura,233497,Unstable Affliction3
Aura,233498,Unstable Affliction4
Aura,233499,Unstable Affliction5
Aura,216708,Deadwind Harvester
Aura,216695,Tormented Souls
Aura,235156,Empowered Life Tap
Item,5512,Healthstone
Item,133585,Trinket
*/
