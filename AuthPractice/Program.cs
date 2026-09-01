using AuthPractice.Entities;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Member> members = [];

app.MapPost("/auth/register", async (MemberResgisterDto memberDto) =>
{
    var hasher = new PasswordHasher<Member>();
    var member = new Member
    {
        Email = memberDto.email
    };
    member.PasswordHash = hasher.HashPassword(member, memberDto.password);

    members.Add(member);
    return Results.Created($"/auth/{member.Id}", new{member.Id, member.Email});
});

app.MapPost("/auth/login", async (MemberLoginDto memberDto) =>
{
    var member = members.FirstOrDefault(m => m.Email == memberDto.email);
    if(member is null)
    {
        return Results.Unauthorized();
    }
    var hasher = new PasswordHasher<Member>();
    var result = hasher.VerifyHashedPassword(member, member.PasswordHash, memberDto.password);

    if(result == PasswordVerificationResult.Failed)
    {
        return Results.Unauthorized();
    }
    return Results.Ok(new{member.Id, member.Email});
});

app.Run();
