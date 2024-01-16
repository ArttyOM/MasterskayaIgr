using MasterServerToolkit.Networking;
using System.Collections.Generic;

namespace MasterServerToolkit.MasterServer
{
    public class NotificationPacket : SerializablePacket
    {
        public int RoomId { get; set; } = -1;
        public string Message { get; set; } = string.Empty;
        public List<int> Recipients { get; set; } = new();
        public List<int> IgnoreRecipients { get; set; } = new();

        public override void FromBinaryReader(EndianBinaryReader reader)
        {
            RoomId = reader.ReadInt32();
            Message = reader.ReadString();

            var recipientsCount = reader.ReadInt32();

            for (var i = 0; i < recipientsCount; i++) Recipients.Add(reader.ReadInt32());

            var ignoreRecipientsCount = reader.ReadInt32();

            for (var i = 0; i < ignoreRecipientsCount; i++) IgnoreRecipients.Add(reader.ReadInt32());
        }

        public override void ToBinaryWriter(EndianBinaryWriter writer)
        {
            writer.Write(RoomId);
            writer.Write(Message);

            writer.Write(Recipients.Count);

            foreach (var recipient in Recipients) writer.Write(recipient);

            writer.Write(IgnoreRecipients.Count);

            foreach (var recipient in IgnoreRecipients) writer.Write(recipient);
        }
    }
}