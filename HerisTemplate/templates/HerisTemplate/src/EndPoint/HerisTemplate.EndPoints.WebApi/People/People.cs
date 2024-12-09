namespace HerisTemplate.EndPoints.WebApi.People;

public class People : BaseController
{
    [HttpPost("Create")]
    public async Task<IActionResult> CreatePerson() => Ok();
}