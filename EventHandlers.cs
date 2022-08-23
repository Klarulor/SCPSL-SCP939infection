using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using PlayerStatsSystem;
using Subtitles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SCP939infection
{
    public class EventHandlers
    {
        public void OnWaitingForPlayers()
        {

        }

        public void OnRoundStarted()
        {
            Plugin.Instance.CoroutineHandler = MEC.Timing.RunCoroutine(Plugin.Instance.Coroutine());
        }

        public void OnHurting(HurtingEventArgs ev)
        {
            if(ev.Attacker.Role == RoleType.Scp93953 || ev.Attacker.Role == RoleType.Scp93989)
                ev.Amount = 50f;
            if (ev.Attacker.Role == RoleType.Scp93953 || ev.Attacker.Role == RoleType.Scp93989)
            {
                float chance = Random.Range(1, 100);
                Log.Info($"Current number of chance: "+chance);
                Log.Info($"Config chance: { Plugin.Instance.Config.InfectionChance}");
                Log.Info($"Already infected: {Plugin.Instance.Infected.Contains(ev.Target)}");
                if (!Plugin.Instance.Infected.Contains(ev.Target) && chance  <= Plugin.Instance.Config.InfectionChance)
                {
                    Plugin.Instance.Infected.Add(ev.Target);
                    Plugin.Instance.Eba.Add(ev.Target);
                    ev.Target.Broadcast(5, Plugin.Instance.Config.InfectionText);
                }
            }

           
        }
        public void Player_Died(Exiled.Events.EventArgs.DiedEventArgs ev)
        {
            if (ev.Target.Role == RoleType.Scp93989 && Plugin.Instance.Scps.Contains(ev.Target))
            {
                ev.Handler.Base.CassieDeathAnnouncement.Announcement = "";
                ev.Handler.Base.CassieDeathAnnouncement.SubtitleParts = Array.Empty<SubtitlePart>();
            }
            else if (Plugin.Instance.Infected.Contains(ev.Target))
                {
                    Plugin.Instance.Infected.Remove(ev.Target);
                    ev.Target.Scale = Plugin.Instance.Config.Scale;
                }
            
        }
        internal void Spawn(Player target)
        {
            if (OldRoles.ContainsKey(target))
                OldRoles[target] = target.Role;
            else
                OldRoles.Add(target, target.Role);

            Plugin.Instance.Infected.Remove(target);
            Vector3 spawnPosition = target.Position;
            MEC.Timing.CallDelayed(1.5f, () =>
            {
                target.Role.Type = RoleType.Scp93989;
                target.Broadcast(5, Plugin.Instance.Config.BecomeText);
                target.Scale = Plugin.Instance.Config.Scale;
                target.MaxHealth = Plugin.Instance.Config.StartHp;
                target.Health = Plugin.Instance.Config.StartHp;
                target.MaxArtificialHealth = 0;
                target.ArtificialHealth = 0;
            });
        }

        private Dictionary<Player, RoleType> OldRoles = new Dictionary<Player, RoleType>();
        public void OnItemUsing(UsingItemEventArgs ev)
        {
            if (Plugin.Instance.Infected.Contains(ev.Player) /*&& Random.Range(1,100) <= Plugin.Instance.Config.CureChance*/)
            {
                if (Plugin.Instance.Config.ItemsForHealing.Contains(ev.Item.Type))
                {
                    Plugin.Instance.Infected.Remove(ev.Player);
                    ev.Player.Broadcast(5, Plugin.Instance.Config.CureText);
                    ev.Player.SetRole(OldRoles[ev.Player], SpawnReason.None, true);
                    ev.Player.DisableAllEffects();
                }
            }
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            Plugin.Instance.Infected.Clear();
            Plugin.Instance.Scps.Clear();
            Plugin.Instance.Eba.Clear();
            MEC.Timing.KillCoroutines(Plugin.Instance.CoroutineHandler.GetHashCode());
        }

        public void OnLeft(LeftEventArgs ev)
        {
            if (Plugin.Instance.Infected.Contains(ev.Player))
            {
                Plugin.Instance.Infected.Remove(ev.Player);
            }
            if (Plugin.Instance.Scps.Contains(ev.Player))
            {
                Plugin.Instance.Scps.Remove(ev.Player);
            }
        }
    }
}
