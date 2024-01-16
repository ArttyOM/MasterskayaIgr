using MasterServerToolkit.Networking;
using System.Collections.Generic;

namespace MasterServerToolkit.MasterServer
{
    public class PlayersPacket : SerializablePacket
    {
        public List<string> Players = new();

        public override void FromBinaryReader(EndianBinaryReader reader)
        {
            var count = reader.ReadByte();

            for (byte i = 0; i < count; i++) Players.Add(reader.ReadString());
        }

        public override void ToBinaryWriter(EndianBinaryWriter writer)
        {
            writer.Write((byte) Players.Count);

            foreach (var region in Players) writer.Write(region);
        }
    }
}