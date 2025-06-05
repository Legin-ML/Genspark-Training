namespace AccuNotify.Models;

public class Encrypt
{
    public string? Data { get; set; }
    
    public byte[]? EncryptedData { get; set; }
    public byte[]? HashKey {get;set;}
}