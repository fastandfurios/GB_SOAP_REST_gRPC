using ClinicService;

var (passwordSalt, passwordHash) = PasswordUtils.CreatePasswordHash("12345");
Console.WriteLine(passwordSalt);
Console.WriteLine(passwordHash);
Console.ReadKey();