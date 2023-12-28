using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

// Lista todos los usuarios creados
app.MapGet("/users", async (TodoDb db) =>
    await db.Users.ToListAsync());


// Agrega un nuevo usuario, asignando la fecha de creación en tiempo real.
app.MapPost("/users", async (Users user, TodoDb db) =>
{
    user.FechaDeCreación = DateTime.UtcNow; 
    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Created($"/users/{user.Id}", user);

    
});

// /Encrypt, encripta el texto recibido utilizando lógica AES, guardando el valor de IV para su uso posterior.
app.MapPost("/Encrypt", async (EncryptionRequest request) =>
{
    try
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = KeyManager.ObtenerClave();
            aesAlg.GenerateIV(); 
            var iv = aesAlg.IV; 
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(request.Text);

            using (ICryptoTransform encryptor = aesAlg.CreateEncryptor())
            {
                byte[] ciphertextBytes = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);
                byte[] combinedBytes = new byte[iv.Length + ciphertextBytes.Length];
                Array.Copy(iv, 0, combinedBytes, 0, iv.Length);
                Array.Copy(ciphertextBytes, 0, combinedBytes, iv.Length, ciphertextBytes.Length);
                string encryptedTextWithIV = Convert.ToBase64String(combinedBytes);
                var response = new { encrypted = encryptedTextWithIV };
                return Results.Ok(JsonSerializer.Serialize(response));
            }
        }
    }
    catch (CryptographicException ex)
    {
        Console.WriteLine($"Error during encryption: {ex.Message}");
        return Results.BadRequest("Error during encryption");
    }
});


// /Decrypt utiliza la IV guardada en la encriptación, y lo desencripta.
app.MapPost("/Decrypt", async (DecryptionRequest request) =>
{
    try
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = KeyManager.ObtenerClave();
            byte[] combinedBytes = Convert.FromBase64String(request.Encrypted);
            byte[] ivBytes = new byte[aesAlg.IV.Length];
            Array.Copy(combinedBytes, 0, ivBytes, 0, ivBytes.Length);
            aesAlg.IV = ivBytes;
            byte[] ciphertextBytes = new byte[combinedBytes.Length - aesAlg.IV.Length];
            Array.Copy(combinedBytes, aesAlg.IV.Length, ciphertextBytes, 0, ciphertextBytes.Length);
            using (ICryptoTransform decryptor = aesAlg.CreateDecryptor())
            {
                byte[] plaintextBytes = decryptor.TransformFinalBlock(ciphertextBytes, 0, ciphertextBytes.Length);
                string decryptedText = Encoding.UTF8.GetString(plaintextBytes);
                var response = new { decrypted = decryptedText };
                return Results.Ok(JsonSerializer.Serialize(response));
            }
        }
    }
    catch (CryptographicException ex)
    {
        Console.WriteLine($"Error during decryption: {ex.Message}");
        return Results.BadRequest("Error during decryption");
    }
});


app.Run();