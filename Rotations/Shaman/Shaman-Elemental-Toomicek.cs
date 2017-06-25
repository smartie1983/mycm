using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CloudMagic.Helpers;
using System.Threading;

namespace CloudMagic.Rotation
{
    public class Elemental : CombatRoutine
    {
		public override string Name
        {
            get { return "Elemental Toomicek"; }
        }

        public override string Class
        {
            get { return "Shaman"; }
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
            Log.Write("Welcome to Elemental Shaman by Toomicek", Color.Green);
            Log.Write("Suggested build: 3112211", Color.Green);
            Log.Write("CloudMagic Elemental");
        }

        public override void Stop()
        {
        }

        public override void Pulse()
        {
            if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat && WoW.TargetIsVisible && !WoW.IsMounted) //First things go first
            {
                if (WoW.HealthPercent <= 60 && WoW.CanCast ("Heilende Woge") && WoW.Mana >= 48400)
                {
                    WoW.CastSpell("Heilende Woge");
                }
                if (WoW.CanCast("Astral Shift") && WoW.HealthPercent < 40 && !WoW.IsSpellOnCooldown("Astral Shift")) //ASTRAL SHIFT - DMG REDUCTION if we are below 40% of HP
                {
                    WoW.CastSpell("Astral Shift");
                    return;
                }
                if (WoW.IsSpellInRange("Wind Shear") && !WoW.IsSpellOnCooldown("Wind Shear") && WoW.TargetIsCastingAndSpellIsInterruptible && WoW.TargetPercentCast >= 60 && !WoW.PlayerIsChanneling)
                {
                    WoW.CastSpell("Wind Shear");
                    return;
                }
				if (WoW.CanCast("Ascendance") && !WoW.IsSpellOnCooldown("Ascendance") && WoW.TargetHasDebuff("Flame Shock") && UseCooldowns) // && WoW.IsBoss) //use Ascendance on boss
				{
					WoW.CastSpell("Ascendance");
					return;
				}
                if (!WoW.IsSpellOnCooldown("Fire Elemental") && WoW.TargetHasDebuff("Flame Shock") && UseCooldowns) // && WoW.IsBoss) // use Fire Elemental
                {
                    WoW.CastSpell("Fire Elemental");
                    return;
                }
				if (WoW.IsSpellInRange("Wind Shear") && WoW.CanCast("Trinket") && WoW.TargetHasDebuff("Flame Shock") && !WoW.ItemOnCooldown("Trinket"))
				{
					WoW.CastSpell("Trinket");
					return;
				}
				if (WoW.CanCast("Blood Fury") && WoW.PlayerHasBuff("Ascendance") && WoW.TargetHasDebuff("Flame Shock") && !WoW.IsSpellOnCooldown ("Blood Fury") && WoW.PlayerRace == "Orc" && UseCooldowns)
				{
					WoW.CastSpell("Blood Fury");
					return;
				}
				if (WoW.CanCast("Stormkeeper") && !WoW.IsSpellOnCooldown("Stormkeeper") && !WoW.PlayerHasBuff("Ascendance") && WoW.TargetHasDebuff("Flame Shock")) //Stormkeeper after ascendance
				{
					WoW.CastSpell("Stormkeeper");
					return;
				}
				if (combatRoutine.Type == RotationType.SingleTarget) // Do Single Target Stuff here
				{
                    if (WoW.CanCast("Totem Mastery") && !WoW.PlayerHasBuff("Totem Mastery")) //Totem mastery at beggining
                    {
                        WoW.CastSpell("Totem Mastery");
                        return;
                    }
                    if (WoW.CanCast("Flame Shock") && !WoW.TargetHasDebuff("Flame Shock")) //Refresh Flame shock
                    {
                        WoW.CastSpell("Flame Shock");
                        return;
                    }
					if (WoW.CanCast("Earthquake") && WoW.PlayerHasBuff("Free Earthquake") && WoW.TargetHasDebuff("Flame Shock"))
					{
						WoW.CastSpell("Earthquake");
						return;
					}
                    if (WoW.CanCast("Lightning Bolt") && WoW.PlayerHasBuff("Stormkeeper") && WoW.TargetHasDebuff("Flame Shock")) //Filler with stormkeeper
                    {
                        WoW.CastSpell("Lightning Bolt");
						return;
                    }
                    if (WoW.CanCast("Elemental Mastery") && WoW.IsSpellOnCooldown("Elemental Mastery") && WoW.TargetHasDebuff("Flame Shock") && WoW.Talent (4) == 3) //use Elemental Mastery on CD
                    {
                        WoW.CastSpell("Elemental Mastery");
                        return;
                    }
					if (WoW.CanCast("Elemental Blast") && WoW.IsSpellInRange("Wind Shear") && WoW.TargetHasDebuff("Flame Shock") && WoW.Talent (5) == 3)
					{
						WoW.CastSpell("Elemental Blast");
						return;
					}
                    if (WoW.CanCast("Earth Shock") && WoW.Maelstrom > 99) //Earth shock on 100 maelstrom
                    {
                        WoW.CastSpell("Earth Shock");
                        return;
                    }
                    if (WoW.CanCast("Lava Burst") && WoW.TargetHasDebuff("Flame Shock") && WoW.PlayerHasBuff("Lava Surge")) //lava burst when we have lava surge
                    {
                        WoW.CastSpell("Lava Burst");
                        return;
                    }
                    if (WoW.CanCast("Lava Burst") && WoW.TargetHasDebuff("Flame Shock") && !WoW.IsMoving) //lava burst when not moving 
                    {
                        WoW.CastSpell("Lava Burst");
                        return;
                    }
                    if (WoW.CanCast("Lightning Bolt") && !WoW.IsMoving && WoW.TargetHasDebuff("Flame Shock")) //Filler
                    {
                        WoW.CastSpell("Lightning Bolt");
                        return;
                    }
                }
				if (combatRoutine.Type == RotationType.AOE) //cast chain light and earthguake, using CDs without fire elemental   
				{
					if (WoW.CanCast("Totem Mastery") && !WoW.PlayerHasBuff("Totem Mastery")) //Totem mastery at beggining
					{
						WoW.CastSpell("Totem Mastery");
						return;
					}
					if (WoW.CanCast("Flame Shock") && !WoW.TargetHasDebuff("Flame Shock")) //Refresh Flame shock
					{
						WoW.CastSpell("Flame Shock");
						return;
					}
					if (WoW.CanCast("Elemental Blast") && WoW.IsSpellInRange("Wind Shear") && WoW.TargetHasDebuff("Flame Shock") && WoW.Talent (5) == 3)
					{
						WoW.CastSpell("Elemental Blast");
						return;
					}
					if (WoW.CanCast("Earthquake") && WoW.PlayerHasBuff("Free Earthquake") && WoW.TargetHasDebuff("Flame Shock"))
					{
						WoW.CastSpell("Earthquake");
						return;
					}
					if (WoW.CanCast("Lava Beam") && WoW.PlayerHasBuff("Ascendance") && !WoW.IsMoving && WoW.TargetHasDebuff("Flame Shock")) //Filler
					{
						WoW.CastSpell("Lava Beam");
						return;
					}
					if (WoW.CanCast("Lava Burst") && WoW.TargetHasDebuff("Flame Shock") && WoW.PlayerHasBuff("Lava Surge") && WoW.TargetHasDebuff("Flame Shock")) //lava burst when we have lava surge
					{
						WoW.CastSpell("Lava Burst");
						return;
					}
					if (WoW.CanCast("Earthquake") && WoW.Maelstrom > 50 && WoW.TargetHasDebuff("Flame Shock"))
						//Earthquake using this macro #showtooltip Earthquake /cast [@cursor] Earthquake Need to point at location where EQ is cast
					{
						WoW.CastSpell("Earthquake");
						return;
					}
					if (WoW.CanCast("Chain Lightning") && WoW.PlayerHasBuff("Stormkeeper") && WoW.TargetHasDebuff("Flame Shock")) //Chain with stormkeeper
					{
						WoW.CastSpell("Chain Lightning");
						return;
					}
					if (WoW.CanCast("Chain Lightning") && !WoW.IsMoving && WoW.TargetHasDebuff("Flame Shock")) //Filler
					{
						WoW.CastSpell("Chain Lightning");
					}
				}
            }
        }
    }
}

/*
[AddonDetails.db]
AddonAuthor=Toomicek
AddonName=smartie
WoWVersion=Legion - 70100
[SpellBook.db]
Spell,188389,Flame Shock,D3
Spell,108271,Astral Shift,D6
Spell,198067,Fire Elemental,F7
Spell,114074,Lava Beam,E
Spell,188196,Lightning Bolt,D1
Spell,51505,Lava Burst,D2
Spell,198103,Earth Elemental,F8
Spell,188443,Chain Lightning,E
Spell,16166,Elemental Mastery,D4
Spell,114050,Ascendance,D8
Spell,61882,Earthquake,F2
Spell,205495,Stormkeeper,D0
Spell,210643,Totem Mastery,F
Spell,8042,Earth Shock,Q
Spell,51490,Thunderstorm,D5
Spell,57994,Wind Shear,F11
Spell,133585,Trinket,D0
Spell,33697,Blood Fury,NumPad1
Spell,8004,Heilende Woge,F1
Spell,117014,Elemental Blast,D5
Aura,188389,Flame Shock
Aura,210659,Totem Mastery
Aura,114050,Ascendance
Aura,77762,Lava Surge
Aura,205495,Stormkeeper
Aura,208723,Free Earthquake
Item,133585,Trinket
*/
