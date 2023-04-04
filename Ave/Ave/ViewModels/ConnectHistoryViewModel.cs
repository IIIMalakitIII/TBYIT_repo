using System;

namespace Ave.API.ViewModels
{
    public class ConnectHistoryViewModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Description { get; set; }

        public int OrganId { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
