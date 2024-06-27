using FamilyTree.Interfaces;
using FamilyTree.Models.Commands;
using Microsoft.AspNetCore.Mvc;

namespace FamilyTree.Controllers;

[ApiController]
[Route("[controller]")]
public class FamilyTreeController(IFamilyTreeService service) : ControllerBase
{
    [HttpPost("")]
    public async Task<IActionResult> CreateNewMemberWithoutParent([FromBody] NewFamilyMemberCmd cmd)
    {
        await service.CreateNewMemberAsync(cmd);
        return Ok();
    }

    [HttpPost("{parentId:int}")]
    public async Task<IActionResult> CreateNewMemberWithParent([FromBody] NewFamilyMemberCmd cmd,
        [FromRoute] int parentId)
    {
        await service.CreateNewMemberAsync(cmd, parentId);
        return Ok();
    }
}