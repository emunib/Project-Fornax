    using System.Collections.Generic;
    using Schemas;
    public class LobbyInfo
    {
        public object Id { get; set; }
        public userPublic Host { get; set; }
        public List<userPublic> Players { get; set; }
    }