namespace ITNotionWPF.Model.User;

public record UserDto(User User)
{
    public override string ToString()
    {
        return User.ToString();
    }
}