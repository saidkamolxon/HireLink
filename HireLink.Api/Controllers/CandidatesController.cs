using HireLink.Service.DTOs;
using HireLink.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HireLink.Api.Controllers;

public class CandidatesController(ICandidateService service) : BaseController
{
    private readonly ICandidateService service = service;

    [HttpPost]
    public async Task<ActionResult<CandidateResultDto>> UpsertAsync(CandidateUpsertDto dto)
        => await this.service.UpsertAsync(dto);
}
