namespace ITNOteAPI;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class TokenService
{
    private const string SecretKey = "PfxtvRhbxfnmRjulfYbrnjYtCksibnJXtvVsUjdjhbvVytRf;tnczXnjVsLfdyjYt:bdsPf;ukbcmBGj-nb[jymreLjujhbv"; // Секретный ключ
    private const string Issuer = "BaraGodLike";
    private const string Audience = "ItnoteMeUser";

    public static string GenerateToken(string username, string role, int expireMinutes = 60)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(expireMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
