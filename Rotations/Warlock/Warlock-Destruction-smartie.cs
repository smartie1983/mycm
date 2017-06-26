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
            Log.Write("Welcome to smartie`s Destruction");
            Log.Write("Talents are specced the following: 2,1,1,3,3,2,3");
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
					if (WoW.CanCast("UnendingResolve") && WoW.HealthPercent <= 35)
					{
						WoW.CastSpell("UnendingResolve");
						return;
					}
					if (!WoW.IsMoving && WoW.CanCast("DrainLife") && WoW.HealthPercent <= 20)
					{
						WoW.CastSpell("DrainLife");
						return;
					}
					if (WoW.CanCast("Healthstone") && WoW.ItemCount("Healthstone") >= 1 && !WoW.ItemOnCooldown("Healthstone") && WoW.HealthPercent <= 40 && WoW.HealthPercent != 0)
					{
						WoW.CastSpell("Healthstone");
						return;
					}
					if (WoW.CanCast("DoomGuard") && !WoW.IsSpellOnCooldown("DoomGuard") && WoW.PlayerHasBuff("Soul Harvest") && UseCooldowns)
                    {
                        WoW.CastSpell("DoomGuard");
                        return;
                    }
					if (WoW.CanCast("Soul Harvest") && !WoW.IsMoving && UseCooldowns && !WoW.IsSpellOnCooldown("Soul Harvest") && WoW.CurrentSoulShards >= 4)
					{
                        WoW.CastSpell("Soul Harvest");
                        return;
                    }
					if (WoW.CanCast("Berserk") && UseCooldowns && !WoW.IsSpellOnCooldown ("Berserk") && WoW.PlayerRace == "Troll")
                    {
                        WoW.CastSpell("Berserk");
                        return;
                    }
					if (WoW.CanCast("Havoc") && !WoW.IsSpellOnCooldown("Havoc") && Control.ModifierKeys == Keys.Alt)
                    /* Havoc on mouseover target, create macro to use: #showtooltip /cast [target=mouseover,harm,exists,nodead] Havoc; Havoc */
					{
						WoW.CastSpell("Havoc");
						return;
					}
					if (!WoW.TargetHasDebuff("AuraImmolate") && !WoW.IsMoving)
					{
						if (WoW.HasTarget && WoW.CanCast("DimRift") && WoW.PlayerSpellCharges("DimRift") == 3)
						{
							WoW.CastSpell("DimRift");
							return;
						}
						if (!WoW.WasLastCasted("Immolate") && WoW.CanCast("Immolate"))
						{
							WoW.CastSpell("Immolate");
							return;
						}
					}
					if (WoW.TargetHasDebuff("AuraImmolate") && !WoW.IsMoving)
					{
						if (WoW.CanCast("DimRift") && WoW.PlayerSpellCharges("DimRift") == 3)
						{
							WoW.CastSpell("DimRift");
							return;
						}
						if (WoW.TargetDebuffTimeRemaining("AuraImmolate") >= 1000 && WoW.CanCast("Conflagrate"))
						{
							WoW.CastSpell("Conflagrate");
							return;
						}
						if ((!WoW.TargetHasDebuff("AuraImmolate") || WoW.TargetDebuffTimeRemaining("AuraImmolate") <= 420) && !WoW.WasLastCasted("Immolate") && WoW.CanCast("Immolate"))
						{
							WoW.CastSpell("Immolate");
							return;
						}
						if (WoW.TargetDebuffTimeRemaining("AuraImmolate") >= 1000 && WoW.PlayerSpellCharges("Conflagrate") == 1 && WoW.WasLastCasted("Conflagrate") && WoW.CanCast("Conflagrate"))
						{
							WoW.CastSpell("Conflagrate");
							return;
						}
						if (WoW.PlayerHasBuff("AuraConflagrateBuff") && WoW.TargetHasDebuff("AuraChaosBolt") && WoW.CanCast("Conflagrate") && WoW.CurrentSoulShards <= 4 && WoW.CanCast("Conflagrate"))
						{
							WoW.CastSpell("Conflagrate");
							return;
						}
						if (WoW.CanCast("Conflagrate") && WoW.PlayerSpellCharges("Conflagrate") == 2 && !WoW.WasLastCasted("Immolate") && WoW.CurrentSoulShards <= 4)
						{
							WoW.CastSpell("Conflagrate");
							return;
						}
						if (WoW.CanCast("ServiceImp") && WoW.CurrentSoulShards >= 1 && (UseCooldowns && WoW.PlayerHasBuff("Soul Harvest") || !UseCooldowns))
						{
							WoW.CastSpell("ServiceImp");
							return;
						}
						if (WoW.CanCast("ChaosBolt") && WoW.CurrentSoulShards > 3 && (UseCooldowns && WoW.SpellCooldownTimeRemaining("Soul Harvest") >= 500 || UseCooldowns && WoW.PlayerHasBuff("Soul Harvest") || !UseCooldowns))
						{
							WoW.CastSpell("ChaosBolt");
							return;
						}
						if (WoW.CanCast("DimRift") && !WoW.IsSpellOnCooldown("DimRift") && WoW.PlayerSpellCharges("DimRift") <= 2)
						{
							WoW.CastSpell("DimRift");
							return;
						}
						if (WoW.CanCast("ChaosBolt") && WoW.CurrentSoulShards >= 2 && (UseCooldowns && WoW.SpellCooldownTimeRemaining("Soul Harvest") >= 500 || UseCooldowns && WoW.PlayerHasBuff("Soul Harvest") || !UseCooldowns))
						{
							WoW.CastSpell("ChaosBolt");
							return;
						}
						if (WoW.CanCast("Incinerate") && WoW.CurrentSoulShards <= 1 || (UseCooldowns && WoW.SpellCooldownTimeRemaining("Soul Harvest") <= 500 && WoW.CurrentSoulShards < 4))
						{
							WoW.CastSpell("Incinerate");
							return;
						}
						if (WoW.CanCast("Incinerate") && WoW.TargetHasDebuff("AuraChaosBolt") && WoW.TargetDebuffTimeRemaining("AuraChaosBolt") >= 200 && WoW.CurrentSoulShards <= 3)
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
Aura,196098,Soul Harvest
Aura,157736,AuraImmolate
Aura,196414,AuraChaosBolt
Aura,196546,AuraConflagrateBuff
Item,5512,Healthstone
*/
