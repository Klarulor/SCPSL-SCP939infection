using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using Subtitles;
using UnityEngine;

namespace SCP939infection
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance;
        public EventHandlers EventHandlers = new EventHandlers();
        public readonly List<Player> Infected = new List<Player>();
        public readonly List<Player> Scps = new List<Player>();
        public readonly List<Player> Eba = new List<Player>();
        internal CoroutineHandle CoroutineHandler;
        public override PluginPriority Priority { get; } = PluginPriority.Last;
        public override string Author { get; } = "Klarulor";
        public override Version Version { get; } = new Version(1, 2, 0);
        public override Version RequiredExiledVersion { get; } = new Version(5, 1, 0);
        public Plugin() => Instance = this;
        public string PluginName => typeof(Plugin).Namespace;
        public override void OnEnabled()
        {
            RegisterEvents(); 
            Log.Info($"Plugin {PluginName} started");
        }
        public override void OnDisabled() => UnregisterEvents();
        private void RegisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnRoundStarted;
            Exiled.Events.Handlers.Player.Hurting += EventHandlers.OnHurting;
            Exiled.Events.Handlers.Player.UsingItem += EventHandlers.OnItemUsing;
            Exiled.Events.Handlers.Server.RoundEnded += EventHandlers.OnRoundEnded;
            Exiled.Events.Handlers.Player.Left += EventHandlers.OnLeft;
            Exiled.Events.Handlers.Player.Died += EventHandlers.Player_Died;
            Exiled.Events.Handlers.Player.ChangingRole += Player_ChangingRole;
            //Exiled.Events.Handlers.Player.ChangingRole += EventHandlers.OnChingingRole;

        }

        private void Player_ChangingRole(Exiled.Events.EventArgs.ChangingRoleEventArgs ev)
        {
            if (!Infected.Contains(ev.Player) && !Scps.Contains(ev.Player))
                ev.Player.Scale = Vector3.one;
            if (Infected.Contains(ev.Player))
                Infected.Remove(ev.Player);
        }

        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundStarted;
            Exiled.Events.Handlers.Player.Hurting -= EventHandlers.OnHurting;
            Exiled.Events.Handlers.Player.UsingItem -= EventHandlers.OnItemUsing;
            Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers.OnRoundEnded;
            Exiled.Events.Handlers.Player.Left -= EventHandlers.OnLeft;
            Exiled.Events.Handlers.Player.Died -= EventHandlers.Player_Died;
            Exiled.Events.Handlers.Player.ChangingRole -= Player_ChangingRole;
            //Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers.OnChingingRole;

        }

        public IEnumerator<float> Coroutine()
        {
            for (;;)
            {
                if (Round.IsStarted)
                {
                    foreach (var infected in Infected)
                    {
                        if (infected.Role == RoleType.Spectator || !infected.IsConnected)
                        {
                            Infected.Remove(infected);
                        }
                        else
                        {
                            if (infected.Health - 4 <= 5f)
                            {
                                EventHandlers.Spawn(infected);
                            }
                            infected.Health -= 4f;
                        }
                    }

                    foreach (var scp in Scps)
                    {
                        if (scp.Health + Config.RecoverySpeed <= scp.MaxHealth)
                            scp.Health += Config.RecoverySpeed;
                    }
                }
                yield return Timing.WaitForSeconds(1);
            }
        }
    }
}
