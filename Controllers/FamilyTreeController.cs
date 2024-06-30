using FamilyTree.Interfaces;
using FamilyTree.Models;
using FamilyTree.Models.Commands;
using Microsoft.AspNetCore.Mvc;

namespace FamilyTree.Controllers;

[ApiController]
[Route("[controller]/members")]
public class FamilyTreeController(IFamilyTreeService service) : ControllerBase
{
    [HttpPost("without-parent")]
    public async Task<IActionResult> CreateNewMemberWithoutParent([FromBody] NewFamilyMemberCmd cmd)
    {
        await service.CreateNewMemberAsync(cmd);
        return Ok();
    }

    [HttpPost("with-parent/{parentId:int}")]
    public async Task<IActionResult> CreateNewMemberWithParent([FromBody] NewFamilyMemberCmd cmd,
        [FromRoute] int parentId)
    {
        await service.CreateNewMemberAsync(cmd, parentId);
        return Ok();
    }

    [HttpGet("")]
    public async IAsyncEnumerable<FamilyMemberWithChildrenInfo> GetAll()
    {
        await foreach (var member in service.GetAllMembers())
        {
            yield return member;
        }
    }

    [HttpGet("great-grandfather-for/{greatGrandsonId:int}")]
    public async Task<IActionResult> GetGreatGrandfather([FromRoute] int greatGrandsonId)
    {
        var result = await service.GetGreatGrandfather(greatGrandsonId);
        
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
    
    [HttpGet("grandfather-for/{grandsonId:int}")]
    public async Task<IActionResult> GetGrandfather([FromRoute] int grandsonId)
    {
        var result = await service.GetGrandfather(grandsonId);
        
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}