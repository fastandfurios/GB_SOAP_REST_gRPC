using ClinicService;

var (passwordSalt, passwordHash) = PasswordUtils.CreatePasswordHash("54321");
Console.WriteLine(passwordSalt);
Console.WriteLine(passwordHash);
Console.ReadKey();