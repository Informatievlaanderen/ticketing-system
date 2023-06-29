namespace TicketingService.Extensions;

using System;
using System.Security.Cryptography;

public static class GuidExtensions
{
    public static ulong ToULong(this Guid guid)
    {
        var guidBytes = guid.ToByteArray();

        // Use SHA-256 hash algorithm
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(guidBytes);

        // Take the first 8 bytes (64 bits) of the hash
        var ulongBytes = new byte[8];
        Array.Copy(hashBytes, ulongBytes, 8);

        // Convert the byte array to a ulong integer
        var ulongValue = BitConverter.ToUInt64(ulongBytes, 0);

        return ulongValue;
    }
}
