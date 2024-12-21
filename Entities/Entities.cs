namespace filmizleme.Entities
{
    public class Film
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Director { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

    public class User
    {
        public int Id {get; set;}
        public required string Email {get; set;}
        public required string Username {get; set;}
        public required string PasswordHash {get; set;}
    }
    
    
public class WatchedFilm
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int FilmId { get; set; }
    public DateTime WatchedOn { get; set; }
}

}
