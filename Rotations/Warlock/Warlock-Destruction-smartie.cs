// ##changelog##
// v0.1 initial release
// v0.2 added more Talents

// ##todo##
// -add Shadowburn
// -cleanup
// -add aoe rota

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CloudMagic.Helpers;
using System.Threading;

namespace CloudMagic.Rotation
{
    public class DestructionWarlock : CombatRoutine
    {		
        public override string Name
        {
            get 
			{ 
				return "DestructionWarlock"; 
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
				return 99; 
			} 
		}
        public override void Initialize()
        {
            Log.Write("Welcome to smartie`s Destruction Warlock v0.2", Color.Green);
            Log.Write("All Talents are supported and auto detected", Color.Green);
            Log.Write("Hold down Alt for Havoc cast - You will need a Mouseover macro for that", Color.Red);
			Log.Write("#showtooltip Havoc /cast [@mouseover,harm] Havoc; [harm] Havoc", Color.Red);
        }
        public override void Stop()
        {
        }
        public override void Pulse()
        {
			if (WoW.HasTarget && WoW.TargetIsEnemy && WoW.IsInCombat && !WoW.PlayerIsChanneling && !WoW.PlayerIsCasting && WoW.TargetIsVisible && !WoW.IsMounted)
            {
				if (combatRoutine.Type == RotationType.SingleTarget || combatRoutine.Type == RotationType.SingleTargetCleave)
				{
					if (WoW.CanCast("UnendingResolve") && WoW.HealthPercent <= 20 && WoW.HealthPercent != 0)
					{
						WoW.CastSpell("UnendingResolve");
						return;
					}
					if (WoW.CanCast("Healthstone") && WoW.ItemCount("Healthstone") >= 1 && !WoW.ItemOnCooldown("Healthstone") && WoW.HealthPercent <= 30 && WoW.HealthPercent != 0)
					{
						WoW.CastSpell("Healthstone");
						return;
					}
					if (WoW.CanCast("DoomGuard") && !WoW.IsSpellOnCooldown("DoomGuard") && (WoW.PlayerHasBuff("Soul Harvest") && WoW.Talent (4) == 3 || WoW.Talent (4) != 3) && UseCooldowns)
                    {
                        WoW.CastSpell("DoomGuard");
                        return;
                    }
					if (WoW.CanCast("Soul Harvest") && !WoW.IsMoving && UseCooldowns && !WoW.IsSpellOnCooldown("Soul Harvest") && WoW.CurrentSoulShards >= 4 && WoW.Talent (4) == 3)
					{
                        WoW.CastSpell("Soul Harvest");
                        return;
                    }
					if (WoW.CanCast("Berserk") && UseCooldowns && !WoW.IsSpellOnCooldown ("Berserk") && WoW.PlayerRace == "Troll")
                    {
                        WoW.CastSpell("Berserk");
                        return;
                    }
					if (WoW.CanCast("Havoc") && !WoW.IsSpellOnCooldown("Havoc") && Control.ModifierKeys == Keys.Alt) // you will need a mouseover macro
					{
						WoW.CastSpell("Havoc");
						return;
					}
					if (!WoW.IsMoving)
					{
						if (WoW.CanCast("Life Tap") && !WoW.PlayerHasBuff("Life Tap") && WoW.HealthPercent >= 40 && WoW.Talent (2) == 3)
						{
							WoW.CastSpell("Life Tap");
							return;						
						}
						if ((!WoW.TargetHasDebuff("Immolate") || WoW.TargetDebuffTimeRemaining("Immolate") <= 420) && !WoW.WasLastCasted("Immolate") && WoW.CanCast("Immolate"))
						{
							WoW.CastSpell("Immolate");
							return;
						}
						if (WoW.CanCast("DimRift") && WoW.PlayerSpellCharges("DimRift") == 3)
						{
							WoW.CastSpell("DimRift");
							return;
						}
						if (WoW.CanCast("Channel Demonfire") && !WoW.IsSpellOnCooldown("Channel Demonfire") && WoW.TargetHasDebuff("Immolate") && WoW.TargetDebuffTimeRemaining("Immolate") >= 500 && WoW.Talent (7) == 2 )
						{
							WoW.CastSpell("Channel Demonfire");
							return;	
						}
						if (WoW.TargetDebuffTimeRemaining("Immolate") >= 1000 && WoW.CanCast("Conflagrate"))
						{
							WoW.CastSpell("Conflagrate");
							return;
						}
						if (WoW.TargetDebuffTimeRemaining("Immolate") >= 1000 && WoW.PlayerSpellCharges("Conflagrate") == 1 && WoW.WasLastCasted("Conflagrate") && WoW.CanCast("Conflagrate"))
						{
							WoW.CastSpell("Conflagrate");
							return;
						}
						if (WoW.PlayerHasBuff("Conflagrate") && WoW.TargetHasDebuff("ChaosBolt") && WoW.CanCast("Conflagrate") && WoW.CurrentSoulShards <= 4 && WoW.CanCast("Conflagrate"))
						{
							WoW.CastSpell("Conflagrate");
							return;
						}
						if (WoW.CanCast("Conflagrate") && WoW.PlayerSpellCharges("Conflagrate") == 2 && !WoW.WasLastCasted("Immolate") && WoW.CurrentSoulShards <= 4)
						{
							WoW.CastSpell("Conflagrate");
							return;
						}
						if (WoW.CanCast("ServiceImp") && WoW.CurrentSoulShards >= 1 && (UseCooldowns && WoW.PlayerHasBuff("Soul Harvest") && WoW.Talent (4) == 3 || !UseCooldowns || WoW.Talent (4) != 3 && UseCooldowns))
						{
							WoW.CastSpell("ServiceImp");
							return;
						}
						if (WoW.CanCast("ChaosBolt") && WoW.CurrentSoulShards > 3 && (UseCooldowns && WoW.SpellCooldownTimeRemaining("Soul Harvest") >= 500 && WoW.Talent (4) == 3 || UseCooldowns && WoW.PlayerHasBuff("Soul Harvest") && WoW.Talent (4) == 3 || !UseCooldowns || WoW.Talent (4) != 3 && UseCooldowns))
						{
							WoW.CastSpell("ChaosBolt");
							return;
						}
						if (WoW.CanCast("DimRift") && !WoW.IsSpellOnCooldown("DimRift") && WoW.PlayerSpellCharges("DimRift") <= 2)
						{
							WoW.CastSpell("DimRift");
							return;
						}
						if (WoW.CanCast("ChaosBolt") && WoW.CurrentSoulShards >= 2 && (UseCooldowns && WoW.SpellCooldownTimeRemaining("Soul Harvest") >= 500 && WoW.Talent (4) == 3 || UseCooldowns && WoW.PlayerHasBuff("Soul Harvest") && WoW.Talent (4) == 3 || !UseCooldowns || WoW.Talent (4) != 3 && UseCooldowns))
						{
							WoW.CastSpell("ChaosBolt");
							return;
						}
						if (WoW.CanCast("Incinerate") && WoW.CurrentSoulShards <= 1 || (UseCooldowns && WoW.SpellCooldownTimeRemaining("Soul Harvest") <= 500 && WoW.CurrentSoulShards < 4 && WoW.Talent (4) == 3))
						{
							WoW.CastSpell("Incinerate");
							return;
						}
						if (WoW.CanCast("Incinerate") && WoW.TargetHasDebuff("ChaosBolt") && WoW.TargetDebuffTimeRemaining("ChaosBolt") >= 200 && WoW.CurrentSoulShards <= 3)
						{
							WoW.CastSpell("Incinerate");
							return;
						}
					}
					if (WoW.IsMoving)
					{
						if (WoW.CanCast("DimRift"))
						{
							WoW.CastSpell("DimRift");
							return;
						}
						if (WoW.CanCast("Conflagrate"))
						{
							WoW.CastSpell("Conflagrate");
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
Spell,29722,Incinerate,D1
Spell,348,Immolate,D3
Spell,17962,Conflagrate,D4
Spell,116858,ChaosBolt,D2
Spell,18540,DoomGuard,F1
Spell,196586,DimRift,F2
Spell,111859,ServiceImp,F3
Spell,104773,UnendingResolve,F5
Spell,80240,Havoc,F5
Spell,234153,DrainLife,F2
Spell,26297,Berserk,D0
Spell,5512,Healthstone,NumPad1
Spell,196098,Soul Harvest,D9
Spell,196447,Channel Demonfire,D7
Spell,1454,Life Tap,F6
Aura,235156,Life Tap
Aura,196098,Soul Harvest
Aura,157736,Immolate
Aura,196414,ChaosBolt
Aura,196546,Conflagrate
Item,5512,Healthstone
*/
