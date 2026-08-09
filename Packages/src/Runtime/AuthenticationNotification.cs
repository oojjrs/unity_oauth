namespace oojjrs.oauth
{
    public readonly struct AuthenticationNotification
    {
        public string CaseId { get; }
        public string CreatedAt { get; }
        public string Id { get; }
        public string Message { get; }
        public string PlayerId { get; }
        public string ProjectId { get; }
        public string Type { get; }

        internal AuthenticationNotification(string caseId, string createdAt, string id, string message,
            string playerId, string projectId, string type)
        {
            CaseId = caseId;
            CreatedAt = createdAt;
            Id = id;
            Message = message;
            PlayerId = playerId;
            ProjectId = projectId;
            Type = type;
        }
    }
}
