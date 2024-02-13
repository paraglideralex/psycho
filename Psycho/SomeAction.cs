using Microsoft.AspNetCore.Mvc;

namespace Psycho;

public class SomeAction : IActionResult
{
    public Task ExecuteResultAsync(ActionContext context)
    {
        return Task.CompletedTask;
    }
}
