namespace AuthPractice.Entities;

public class Member
{
    public int Id {set; get;}
    public string Email{set; get;} = string.Empty;
    public string PasswordHash{set; get;} = string.Empty;
}

public record MemberResgisterDto(string email, string password);
public record MemberLoginDto(string email, string password);