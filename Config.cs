using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace SCP939infection
{
    public sealed class Config : IConfig
    {
        [Description("Enable")]
        public bool IsEnabled { get; set; } = true;

        public int StartHp { get; set; } = 500;
        public SerializableVector3 Scale { get; set; } = new SerializableVector3(0.5f, 0.5f, 0.5f);
        public SerializableVector3 newRoleScale { get; set; } = new SerializableVector3(0.5f, 0.5f, 0.5f);
        public float RecoverySpeed { get; set; } = 0.5f;
        public int InfectionChance { get; set; } = 25;
        public int CureChance { get; set; } = 100;
        public ItemType[] ItemsForHealing { get; set; } = new ItemType[] {ItemType.Adrenaline, ItemType.SCP500, ItemType.Painkillers, ItemType.SCP207, ItemType.Medkit};
        public string InfectionText { get; set; } = "</size=25>YOU infect 939 virus find cure anything</size>";
        public string BecomeText { get; set; } = "You are SCP-939 infected";
        public string CureText { get; set; } = "You successfully cured. Congratulations!";
    }
}
