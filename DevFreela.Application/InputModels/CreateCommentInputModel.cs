namespace DevFreela.Application.InputModels
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class CreateCommentInputModel
    {
        public string Content { get; set; }
        public int IdUser { get; set; }
    }
}
