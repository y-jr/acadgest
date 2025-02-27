namespace acadgest.Dto.Mark
{
    public class UpdateMiniPautaDto
    {
        public Guid Id { get; set; }
        public Guid PupilId { get; set; }
        public int Trim { get; set; }
        public string Mac { get; set; } = string.Empty;
        public string Pp { get; set; } = string.Empty;
        public string Pt { get; set; } = string.Empty;
    }
}