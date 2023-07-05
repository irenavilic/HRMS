﻿using HRMS.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HRMS.IdentityServer.Controllers;

[ApiController]
[Route("[action]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration Configuration;
    private readonly IEmployeeRepository EmployeeRepository;

    public AuthController(
        IConfiguration configuration,
        IEmployeeRepository employeeRepository)
    {
        Configuration = configuration;
        EmployeeRepository = employeeRepository;
    }

    [HttpGet]
    public async Task<string> Login(string email, string password)
    {
        var employee = await EmployeeRepository.GetByEmailAndPasswordAsync(email, password);

        if (employee is null) return String.Empty;

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JWTSecret")!));

        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, employee.Email),
            new Claim(ClaimTypes.Name, employee.FirstName + ' ' + employee.LastName),
            new Claim(ClaimTypes.Email, employee.Email)
        };

        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}