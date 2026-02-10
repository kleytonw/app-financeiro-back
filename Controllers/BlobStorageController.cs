using ERP_API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
[Authorize]
public class BlobStorageController : ControllerBase
{
    private readonly IBlobStorageService _blobStorageService;

    public BlobStorageController(IBlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }

    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        try
        {
            var fileName = await _blobStorageService.UploadAsync(file);
            return Ok(fileName);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("baixar")]
    public async Task<IActionResult> Baixar(string fileName)
    {
        try
        {
            var (stream, contentType) = await _blobStorageService.DownloadAsync(fileName);
            return File(stream, contentType, fileName);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao baixar arquivo: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("deletar")]
    public async Task<IActionResult> Deletar(string fileName)
    {
        try
        {
            var success = await _blobStorageService.DeleteAsync(fileName);
            return Ok(success ? "Arquivo deletado com sucesso" : "Arquivo não encontrado");
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao deletar arquivo: {ex.Message}");
        }
    }
}