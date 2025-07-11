﻿using FGC.Application.Common.Ports;

namespace FGC.Infrastructure.Services.Hashing;

public class BCryptPasswordHasher : IPasswordHasher
{
    public string Hash(string senha)
        => BCrypt.Net.BCrypt.HashPassword(senha);

    public bool Verify(string senha, string hash)
        => BCrypt.Net.BCrypt.Verify(senha, hash);
}
