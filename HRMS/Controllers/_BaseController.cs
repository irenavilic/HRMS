﻿using HRMS.Core.Interfaces.Repositories;
using HRMS.Core.Models.Responses;
using HRMS.Core.Models.Searches;
using HRMS.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HRMS.Controllers;

//[Authorize]
[ApiController]
[ExceptionFilter]
[Route("[controller]")]
public abstract class BaseController<T, TSearch> : ControllerBase
    where T : class
    where TSearch : BaseSearch
{
    private readonly IBaseRepository<T, TSearch> BaseRepository;

    protected BaseController(IBaseRepository<T, TSearch> baseRepository)
    {
        BaseRepository = baseRepository;
    }

    [HttpGet("{id}")]
    public virtual async Task<T> Get(int id)
        => await BaseRepository.GetAsync(id);

    [HttpGet]
    public virtual async Task<PagedResult<T>> GetAll([FromQuery] TSearch search)
        => await BaseRepository.GetAllAsync(search);
}