namespace Service.Tasks.UserHelpers.Helpers;

public interface IBCryptPasswordHasher
{
    string Hash(
        string data);

    bool Verify(
        string data,
        string hashedData);
}
