namespace ITNOte.me.Model.User;

public record UserDto(User User)
{
    public override string ToString()
    {
        return User.ToString();
    }
}