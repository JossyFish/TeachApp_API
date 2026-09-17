namespace TeachApp.Contracts.Events
{
    public class EmailMessageEvent
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsHtml { get; set; } = true;
        public DateTime ScheduledFor { get; set; }
        public Dictionary<string, string>? Metadata { get; set; }
    }
}
